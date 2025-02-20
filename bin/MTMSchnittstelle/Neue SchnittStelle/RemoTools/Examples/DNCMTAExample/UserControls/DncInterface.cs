// ------------------------------------------------------------------------------------------------
// <copyright file="DncInterface.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.
// </copyright>
// <author>Marco Hayler</author>
// <date>07.10.2015</date>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Provides control elements for operating with the HeidenhainDNC interfaces
    /// </summary>
    public partial class DncInterface : UserControl
    {
        #region "fields"
        /// <summary>
        /// Reference to the main communication object.
        /// Use this to get the interfaces.
        /// </summary>
#if MACHINE_IN_PROCESS
        private HeidenhainDNCLib.JHMachineInProcess machine = null;
#else
        private HeidenhainDNCLib.JHMachine machine = null;
#endif

        /// <summary>
        /// Object of HeidenhainDNC IJHFileSystem interface.
        /// </summary>
        private HeidenhainDNCLib.JHDataAccess dataAccess = null;

        /// <summary>
        /// Object of HeidenhainDNC IJHAutomatic interface.
        /// </summary>
        private HeidenhainDNCLib.JHAutomatic automatic = null;

        /// <summary>
        /// Type of the connected CNC control.
        /// </summary>
        private HeidenhainDNCLib.DNC_CNC_TYPE cncType;

        /// <summary>
        /// Information if the connected CNC control NCK based.
        /// </summary>
        private bool isNck;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// Thread A, used for file transfer.
        /// </summary>
        private BackgroundWorker threadA = null;

        /// <summary>
        /// The timer to realize polling for the thread b example.
        /// Do not use System.Windows.Form.Timer.
        /// To simplify synchronization, these timers run in the gui thread. This can cause a lot
        /// of jitter in the polling rate. Prefer System.Timers.Timer or System.Threading.Timer.
        /// These timers are executed in a separate thread. If you want to write to the gui,
        /// you need the same mechanisms as for example with the background worker or other thread types.
        /// </summary>
        private System.Timers.Timer pollingTimer = null;

        /// <summary>
        /// Time stamp of the start time of thread A.
        /// </summary>
        private DateTime startTimeThreadA = DateTime.Now;

        /// <summary>
        /// Time stamp of the last polling cycle of thread B.
        /// </summary>
        private DateTime lastCycleThreadB = DateTime.Now;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DncInterface"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form</param>
        public DncInterface(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.Disposed += new EventHandler(this.DncInterface_Disposed);

            this.cncType = parentForm.CncType;
            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "delegate"
        /// <summary>
        /// Initializes the column header.
        /// </summary>
        /// <param name="header">The column headers from the tool tables of the connected control.</param>
        private delegate void InitColumnHeaderHandler(ColumnHeader[] header);

        /// <summary>
        /// Initializes the tool count.
        /// </summary>
        /// <param name="toolCount">The max value of the progress bar. Set this to the line count of the tool table.</param>
        private delegate void InitToolCountHandler(int toolCount);

        /// <summary>
        /// Delegate for preventing cross thread access to GUI.
        /// </summary>
        /// <param name="message">Message without newline.</param>
        private delegate void LogOvrValueHandler(string message);
        #endregion

        #region "public methods"
        /// <summary>
        /// Get all interfaces and subscribe for all events here.
        /// </summary>
        /// <returns>Initialization successful.</returns>
        public bool Init()
        {
            if (!this.isNck)
            {
                this.initOkay = false;
                return this.initOkay;
            }

            try
            {
                // --- Get the interface Object(s) ------------------------------------------------
                // Both interfaces (used in different threads) are got from one machine object, so only one connection is used.
                this.dataAccess = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHDATAACCESS);
                this.automatic = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAUTOMATIC);

                //// --- Subscribe for the event(s) -------------------------------------------------

                this.initOkay = true;
            }
            catch
            {
                this.initOkay = false;
            }

            return this.initOkay;
        }

        /// <summary>
        /// The timer, background worker and in general threads that uses the Invoke ore BeginInvoke mechanism
        /// has to be stopped before the gui becomes disposed. Otherwise the try to write to a non existing gui.
        /// </summary>
        private void Stop()
        {
            if (this.pollingTimer != null)
            {
                // Stop thread B
                this.pollingTimer.Stop();
                this.pollingTimer.Dispose();

            }

            if (this.threadA != null)
            {
                // Stop thread A
                this.threadA.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.ThreadA_RunWorkerCompleted);
                this.threadA.DoWork -= new DoWorkEventHandler(this.ThreadA_DoWork);
                this.threadA.CancelAsync();

                // Whait until thread is finished.
                BackgroundWorkerBusyForm busyFormThreadA = new BackgroundWorkerBusyForm(this.threadA);
                if (this.threadA.IsBusy)
                {
                    DialogResult result = busyFormThreadA.ShowDialog();
                }

                this.threadA.Dispose();
            }
        }
        #endregion

        #region "event handler gui"
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DncInterface_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            // Init thread A (background worker)
            this.threadA = new BackgroundWorker();
            this.threadA.WorkerReportsProgress = false;
            this.threadA.WorkerSupportsCancellation = true;

            this.threadA.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ThreadA_RunWorkerCompleted);
            this.threadA.DoWork += new DoWorkEventHandler(this.ThreadA_DoWork);

            // Init "thread B" (System.Timers.Timer)
            this.pollingTimer = new System.Timers.Timer(Properties.Settings.Default.pollingIntervall);
            this.pollingTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.PollingTimer_Tick);
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DncInterface_Disposed(object sender, EventArgs e)
        {
            if (this.dataAccess != null)
            {
                Marshal.ReleaseComObject(this.dataAccess);
            }

            if (this.automatic != null)
            {
                Marshal.ReleaseComObject(this.automatic);
            }
        }

        /// <summary>
        /// Start to read the tool table asynchronous to the gui thread.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadToolTableButton_Click(object sender, EventArgs e)
        {
            ToolTableListView.Clear();
            this.startTimeThreadA = DateTime.Now;
            this.threadA.RunWorkerAsync();
            ThreadAStateLabel.BackColor = Color.Green;
            ThreadAStateLabel.Text = "running...";
            ThreadALoggingTextBox.Text += string.Format(
                                                "{0}: Thread A started ...",
                                                this.DateTimeWithMilliseconds()) + Environment.NewLine;
        }

        /// <summary>
        /// Start polling in thread B
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StartPollingButton_Click(object sender, EventArgs e)
        {
            this.pollingTimer.Start();
            ThreadBStateLabel.BackColor = Color.Green;
            ThreadBStateLabel.Text = "running...";
            StopPollingButton.Enabled = true;
            StartPollingButton.Enabled = false;

            this.lastCycleThreadB = DateTime.Now;
        }

        /// <summary>
        /// Stop polling in thread B (timer based)
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StopPollingButton_Click(object sender, EventArgs e)
        {
            this.pollingTimer.Stop();
            ThreadBStateLabel.BackColor = Color.Yellow;
            ThreadBStateLabel.Text = "stopped...";
            StopPollingButton.Enabled = false;
            StartPollingButton.Enabled = true;
        }
        #endregion

        #region "Timer & Backroundworker events"
        /// <summary>
        /// Does the polling in another thread.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void PollingTimer_Tick(object sender, EventArgs e)
        {
            object speed = null;
            object feed = null;
            object rapid = null;
            string message = string.Empty;

            this.automatic.GetOverrideInfo(ref feed, ref speed, ref rapid);
            message = string.Format(
                            "Polling-Time: {0}ms - Feed: {1}, Speed: {2}, Rapid: {3}",
                            (DateTime.Now - this.lastCycleThreadB).TotalMilliseconds.ToString("0000") + " - " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("000"),
                            feed,
                            speed,
                            rapid);

            this.BeginInvoke(new LogOvrValueHandler(this.LogOvrValueImpl), message);
            this.lastCycleThreadB = DateTime.Now;
        }

        /// <summary>
        /// Task of thread A.
        /// </summary>
        /// <param name="sender">Contains the event sender.</param>
        /// <param name="e">Contains the event arguments.</param>
        private void ThreadA_DoWork(object sender, DoWorkEventArgs e)
        {
            // Both methods reused from the example DNC_CSharp_ReadToolTable
            this.ReadToolColumnNamesWithDataAccess3();
            this.ReadOnTheWholeUsingDataAccess3(sender, e);
        }

        /// <summary>
        /// If thread A has finished.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">Contains the worker result.</param>
        private void ThreadA_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ThreadAStateLabel.BackColor = Color.Yellow;
            ThreadAStateLabel.Text = "stopped...";

            ThreadALoggingTextBox.Text += string.Format(
                                                 "{0}: Thread A finished in {1}ms..." + Environment.NewLine + Environment.NewLine,
                                                 this.DateTimeWithMilliseconds(),
                                                 Math.Round((DateTime.Now - this.startTimeThreadA).TotalMilliseconds));

            ToolTableListView.Items.AddRange((ListViewItem[])e.Result);
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// This is the implementation of the "write value to text box" delegate.
        /// </summary>
        /// <param name="message">This message is written to the textbox.</param>
        private void LogOvrValueImpl(string message)
        {
            ThreadBLoggingTextBox.Text += message + Environment.NewLine;

            // Autoscroll to the last line
            ThreadBLoggingTextBox.SelectionStart = ThreadBLoggingTextBox.TextLength;
            ThreadBLoggingTextBox.ScrollToCaret();
        }

        /// <summary>
        ///  Generate a date time string including milliseconds.
        /// </summary>
        /// <returns>Returns the date time including milliseconds string.</returns>
        private string DateTimeWithMilliseconds()
        {
            return DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("000");
        }

        /// <summary>
        /// Read the tool table column names using the DataAccess3 interface.
        /// </summary>
        private void ReadToolColumnNamesWithDataAccess3()
        {
            HeidenhainDNCLib.IJHDataEntry2 toolTable = null;
            HeidenhainDNCLib.IJHDataEntry2List toolColumnNames = null;
            HeidenhainDNCLib.IJHDataEntry2 toolColumnName = null;
            try
            {
                toolTable = this.dataAccess.GetDataEntry2(@"\TABLE\TOOL", HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                toolColumnNames = toolTable.GetChildList();
                int toolColumnsCount = toolColumnNames.Count;

                ColumnHeader[] columnHeaderList = new ColumnHeader[toolColumnsCount];
                for (int i = 0; i < toolColumnsCount; i++)
                {
                    toolColumnName = toolColumnNames[i];

                    ColumnHeader columnHeader = new ColumnHeader(toolColumnName.bstrName);
                    columnHeader.Text = toolColumnName.bstrName;
                    columnHeaderList[i] = columnHeader;

                    if (toolColumnName != null)
                    {
                        Marshal.ReleaseComObject(toolColumnName);
                    }
                }

                this.Invoke(new InitColumnHeaderHandler(this.InitColumnHeader), columnHeaderList.Clone());
            }
            catch (COMException cex)
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
            }
            catch (Exception ex)
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
            finally
            {
                if (toolTable != null)
                {
                    Marshal.ReleaseComObject(toolTable);
                }

                if (toolColumnNames != null)
                {
                    Marshal.ReleaseComObject(toolColumnNames);
                }

                if (toolColumnName != null)
                {
                    Marshal.ReleaseComObject(toolColumnName);
                }
            }
        }

        /// <summary>
        /// Read the whole tool table at once using the DataAccess3 interface.
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadOnTheWholeUsingDataAccess3(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (worker == null)
            {
                return;
            }

            HeidenhainDNCLib.IJHDataEntry2 toolTable = null;
            HeidenhainDNCLib.IJHDataEntry2List toolTableWithIndexList = null;
            HeidenhainDNCLib.IJHDataEntry2 toolTableWithIndex = null;
            HeidenhainDNCLib.IJHDataEntry2List toolLines = null;
            HeidenhainDNCLib.IJHDataEntry2 toolLine = null;
            HeidenhainDNCLib.IJHDataEntry2List toolCells = null;
            HeidenhainDNCLib.IJHDataEntry2 toolCell = null;
            object cellValue = null;
            string cellValueString = null;

            try
            {
                toolTable = this.dataAccess.GetDataEntry2(@"\TABLE\TOOL", HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                toolTable.ReadTreeValues(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                toolTableWithIndexList = toolTable.childList2;
                toolTableWithIndex = toolTableWithIndexList[0];
                toolLines = toolTableWithIndex.childList2;
                int toolLinesCount = toolLines.Count;
                this.BeginInvoke(new InitToolCountHandler(this.InitToolCount), toolLinesCount);

                ListViewItem[] listViewToolLines = new ListViewItem[toolLinesCount];
                for (int i = 0; i < toolLinesCount; i++)
                {
                    // check if worker has to be stoped
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    toolLine = toolLines[i];
                    toolCells = toolLine.childList2;

                    ListViewItem listViewToolLine = null;

                    for (int j = 0; j < toolCells.Count; j++)
                    {
                        toolCell = toolCells[j];

                        // get local property value
                        cellValue = toolCell.propertyValue;

                        if (cellValue != null)
                        {
                            cellValueString = cellValue.ToString();
                            if (toolCell.bstrName == "T")
                            {
                                cellValueString = cellValueString.Replace(",", ".");
                            }
                        }
                        else
                        {
                            cellValueString = "<undefined>";
                        }

                        if (listViewToolLine == null)
                        {
                            listViewToolLine = new ListViewItem(cellValueString);

                            /* MSDN INFO:
                             * The Name property corresponds to the key for a ListViewItem in the ListView.ListViewItemCollection.
                             * The key comparison is not case-sensitive. If the key parameter is null or an empty string, IndexOfKey returns -1.
                             * --> We need the key to remove the item from list in a handy way
                             */
                            listViewToolLine.Name = cellValueString;
                        }
                        else
                        {
                            listViewToolLine.SubItems.Add(cellValueString);
                        }

                        if (toolCell != null)
                        {
                            Marshal.ReleaseComObject(toolCell);
                        }
                    }

                    listViewToolLines[i] = listViewToolLine;

                    if (toolLine != null)
                    {
                        Marshal.ReleaseComObject(toolLine);
                    }

                    if (toolCells != null)
                    {
                        Marshal.ReleaseComObject(toolCells);
                    }
                }

                e.Result = listViewToolLines;
            }
            catch (COMException cex)
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
            }
            catch (Exception ex)
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
            finally
            {
                if (toolTable != null)
                {
                    Marshal.ReleaseComObject(toolTable);
                }

                if (toolLines != null)
                {
                    Marshal.ReleaseComObject(toolLines);
                }

                if (toolLine != null)
                {
                    Marshal.ReleaseComObject(toolLine);
                }

                if (toolCells != null)
                {
                    Marshal.ReleaseComObject(toolCells);
                }

                if (toolCell != null)
                {
                    Marshal.ReleaseComObject(toolCell);
                }

                if (toolTableWithIndexList != null)
                {
                    Marshal.ReleaseComObject(toolTableWithIndexList);
                }

                if (toolTableWithIndex != null)
                {
                    Marshal.ReleaseComObject(toolTableWithIndex);
                }
            }
        }

        /// <summary>
        /// Initializes the tool count in the combo box header.
        /// If you want to do this from a separate thread, you have to use the delegate.
        /// </summary>
        /// <param name="toolCount">Amount of tool in tool table.</param>
        private void InitToolCount(int toolCount)
        {
            ToolTableGroupBox.Text = "Tool table (" + toolCount + " tools found)";
        }

        /// <summary>
        /// Implementation of the initialization routine for the column headers (tool table),
        /// </summary>
        /// <param name="header">The column header for the tool table.</param>
        private void InitColumnHeader(ColumnHeader[] header)
        {
            ToolTableListView.Columns.AddRange(header);
        }
        #endregion
    }
}
