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
// <summary>
// This file contains a class which shows how to handle the tool table using
// the IJHDataAccess2 and IJHDataAccess3 interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using HeidenhainDNCLib;
    using AppSettings = DNC_CSharp_Demo.Properties.Settings;

    /// <summary>
    /// Main control to operate the tool table 
    /// </summary>
    public partial class DncInterface : UserControl
    {
        #region "field"
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
        /// Object of HeidenhainDNC IJHDataAccess interface.
        /// This can be used to access to PLC data, config data, tables like the tool table and much more.
        /// </summary>
        private HeidenhainDNCLib.JHDataAccess dataAccess = null;

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
        /// Controls has to be locked, while reading tool data.
        /// </summary>
        private bool lockControl = false;

        /// <summary>
        /// The representation of the selected row (ListView)
        /// </summary>
        private ListViewItem selectedRow = null;

        // --- read column names ----------------------------------------------------------------------

        /// <summary>
        /// Set this to true if you want to read the column names of the tool table using the DataAccess2 interface
        /// </summary>
        private bool readTableColumnNamesWithDataAccess2 = false;

        /// <summary>
        /// Set this to true if you want to read the column names of the tool table using the DataAccess3 interface.
        /// Gets read in a different thread (BackgroundWorker). 
        /// </summary>
        private bool readTableColumnNamesWithDataAccess3 = false;

        // --- read whole table -----------------------------------------------------------------------

        /// <summary>
        /// Set this to read the tool table cell by cell using the DataAccess2 interface.
        /// Gets read in a different thread (BackgroundWorker). 
        /// </summary>
        private bool readCellByCellUsingDataAccess2 = false;

        /// <summary>
        /// Set this to read the tool table cell by cell using the DataAccess3 interface.
        /// Gets read in a different thread (BackgroundWorker). 
        /// </summary>
        private bool readCellByCellUsingDataAccess3 = false;

        /// <summary>
        /// Set this to read the tool table line by line using the DataAccess3 interface.
        /// Gets read in a different thread (BackgroundWorker). 
        /// </summary>
        private bool readLineByLineUsingDataAccess3 = false;

        /// <summary>
        /// Set this to read the tool table on the whole using the DataAccess3 interface.
        /// Gets read in a different thread (BackgroundWorker). 
        /// </summary>
        private bool readOnTheWholeUsingDataAccess3 = false;

        /// <summary>
        /// Set this if you do not want to show the measured times in the statistic control
        /// </summary>
        private bool dontShowStatistic = false;

        /// <summary>
        /// The BackgroundWorker which is used to read the tool data 
        /// </summary>
        private BackgroundWorker worker = null;
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
            this.Disposed += new EventHandler(this.ReadTableUserControl_Disposed);

            this.machine = parentForm.Machine;
            this.cncType = parentForm.CncType;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "delegates"
        /// <summary>
        /// Add a line to statistic list view.
        /// </summary>
        /// <param name="message">The message to show in the statistics list view.</param>
        /// <param name="duration">The duration to show in the statistics list view</param>
        private delegate void AddStatisticLineHandler(string message, TimeSpan duration);

        /// <summary>
        /// Initializes the max value of progress bar.
        /// </summary>
        /// <param name="maxProgress">The max value of the progress bar. Set this to the line count of the tool table.</param>
        private delegate void InitProgressBarMaxHandler(int maxProgress);

        /// <summary>
        /// Initializes the tool count.
        /// </summary>
        /// <param name="toolCount">The max value of the progress bar. Set this to the line count of the tool table.</param>
        private delegate void InitToolCountHandler(int toolCount);
        #endregion

        #region "properties"
        /// <summary>
        /// Gets or sets a value indicating whether the controls are locked while reading data.
        /// </summary>
        private bool LockControl
        {
            get
            {
                return this.lockControl;
            }

            set
            {
                this.lockControl = value;
                if (value)
                {
                    ReadColumnNamesButton.Enabled = false;
                    ReadTableButton.Enabled = false;
                    ReadEverythingButton.Enabled = this.isNck;
                    DataAccessTypeGroupBox.Enabled = false;
                    ReadingTypeGroupBox.Enabled = false;
                }
                else
                {
                    ReadColumnNamesButton.Enabled = true;
                    ReadTableButton.Enabled = true;
                    ReadEverythingButton.Enabled = this.isNck;
                    DataAccessTypeGroupBox.Enabled = true;
                    ReadingTypeGroupBox.Enabled = true;
                }
            }
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// Get all interfaces and subscribe for all events here.
        /// </summary>
        /// <returns>Was initialization successful</returns>
        public bool Init()
        {
            try
            {
                // --- Get Interface Object -------------------------------------------------------
                this.dataAccess = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHDATAACCESS);
                if (this.isNck)
                {
                    this.dataAccess.SetAccessMode(DNC_ACCESS_MODE.DNC_ACCESS_MODE_TABLEDATAACCESS, string.Empty);
                }

                //// --- Subscribe for events -------------------------------------------------------

                this.initOkay = true;
                return this.initOkay;
            }
            catch
            {
                this.initOkay = false;
                return this.initOkay;
            }
        }
        #endregion

        #region "event handler"
        // --- BackgroundWorker -------------------------------------------------------------------

        /// <summary>
        /// Update GUI and reset job markers at the and of a worker job.
        /// </summary>
        /// <param name="sender">The reference to the background worker.</param>
        /// <param name="e">Event arguments of the background worker. Use this to get the result.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.LockControl = false;

            if (e.Cancelled)
            {
                return;
            }

            if (sender == null || e.Result == null)
            {
                return;
            }

            if (e.Result.GetType() == typeof(ColumnHeader[]))
            {
                TableListView.Columns.AddRange((ColumnHeader[])e.Result);
            }

            if (e.Result.GetType() == typeof(ListViewItem[]))
            {
                TableListView.Items.AddRange((ListViewItem[])e.Result);
            }

            TableListView.EndUpdate();

            // --- reset jobs ---------------------------------------------------------------------
            this.readTableColumnNamesWithDataAccess2 = false;
            this.readTableColumnNamesWithDataAccess3 = false;
            this.readCellByCellUsingDataAccess2 = false;
            this.readCellByCellUsingDataAccess3 = false;
            this.readLineByLineUsingDataAccess3 = false;
            this.readOnTheWholeUsingDataAccess3 = false;
            this.dontShowStatistic = false;
        }

        /// <summary>
        /// Informs the GUI, that the progress has changed.
        /// </summary>
        /// <param name="sender">The reference to the background worker.</param>
        /// <param name="e">Event arguments of the background worker. Use this to get the new progress value.</param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadingProgressBar.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// The background worker thread job dispatcher.
        /// Checks the jobs requests and starts the according tasks.
        /// </summary>
        /// <param name="sender">The reference to the background worker.</param>
        /// <param name="e">Event arguments of the background worker.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.readTableColumnNamesWithDataAccess2)
            {
                this.ReadTableColumnNamesWithDataAccess2(sender, e);
            }

            if (this.readTableColumnNamesWithDataAccess3)
            {
                this.ReadTableColumnNamesWithDataAccess3(sender, e);
            }

            if (this.readCellByCellUsingDataAccess2)
            {
                this.ReadCellByCellUsingDataAccess2(sender, e);
            }

            if (this.readCellByCellUsingDataAccess3)
            {
                this.ReadCellByCellUsingDataAccess3(sender, e);
            }

            if (this.readLineByLineUsingDataAccess3)
            {
                this.ReadLineByLineUsingDataAccess3(sender, e);
            }

            if (this.readOnTheWholeUsingDataAccess3)
            {
                this.ReadOnTheWholeUsingDataAccess3(sender, e);
            }
        }

        // --- WinForms Events --------------------------------------------------------------------

        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadTableUserControl_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            // --- create BackgoundWorker Thread --------------------------------------------------------
            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;

            this.worker.ProgressChanged += new ProgressChangedEventHandler(this.Worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
            this.worker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);

            // Show selected table
            TableGroupBox.Text += TableAccessors.WholeTable;

            // Preset the GUI depending on the connected control and its supported features
            if (this.isNck)
            {
                ReadEverythingButton.Enabled = true;
            }
            else
            {
                ReadEverythingButton.Enabled = false;

                // DataAccessType
                DataAccess2RadioButton.Checked = true;
                DataAccess3RadioButton.Enabled = false;

                // Reading Type
                CellByCellRadioButton.Checked = true;
                LineByLineRadioButton.Enabled = false;
                OnTheWholeRadioButton.Enabled = false;
            }

            string selectedTable = AppSettings.Default.TableAccessor;
            // Unlock the support for tables with indexed primary keys
            /* In this example indexed primary keys are only supported for the tool table.
             * In general, NCK based controls can also handle user defined tables with indexed primary keys.
             * --> Non indexed primary keys are usually from type "System.Int32"
             * --> Indexed primary keys are usually from type "System.Double" */
            if (!(selectedTable == "TOOL"))
            {
                // Remove "add index" from ToolListContextMenuStrip
                ToolListContextMenuStrip.Items[3].Enabled = false;
            }

            // It is not allowed to add or remove rows from this table (TOOL_P),
            // because it corresponds to the physical tool magazin.
            if (selectedTable == "TOOL_P")
            {
                // Edit row for the pocket table is not yet implemented 
                ToolListContextMenuStrip.Items[0].Enabled = false;

                // Remove "add row" and "delete" from ToolListContextMenuStrip
                ToolListContextMenuStrip.Items[1].Enabled = false;
                ToolListContextMenuStrip.Items[2].Enabled = false;
            }

            // Initializes TableListView with the column names of the selected table
            TableListView.BeginUpdate();
            this.readTableColumnNamesWithDataAccess2 = true;
            this.dontShowStatistic = true;
            this.worker.RunWorkerAsync();

            // Initializes the StatisticsListView
            StatisticsListView.Items.Clear();
            StatisticsListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadTableUserControl_Disposed(object sender, EventArgs e)
        {
            this.StopWorker();
            this.worker.ProgressChanged += new ProgressChangedEventHandler(this.Worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
            this.worker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);

            if (this.dataAccess != null)
            {
                Marshal.ReleaseComObject(this.dataAccess);
            }
        }

        /// <summary>
        /// Sets the selected job to read the column names for the worker thread.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadColumnNamesButton_Click(object sender, EventArgs e)
        {
            this.LockControl = true;
            TableListView.Items.Clear();
            TableListView.Columns.Clear();

            if (DataAccess2RadioButton.Checked)
            {
                this.readTableColumnNamesWithDataAccess2 = true;
            }
            else if (DataAccess3RadioButton.Checked)
            {
                this.readTableColumnNamesWithDataAccess3 = true;
            }

            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// Sets the selected job to read the tool table for the worker thread.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadToolTableButton_Click(object sender, EventArgs e)
        {
            this.LockControl = true;
            TableListView.Items.Clear();

            if (DataAccess2RadioButton.Checked)
            {
                if (CellByCellRadioButton.Checked)
                {
                    this.readCellByCellUsingDataAccess2 = true;
                }
            }
            else if (DataAccess3RadioButton.Checked)
            {
                if (CellByCellRadioButton.Checked)
                {
                    this.readCellByCellUsingDataAccess3 = true;
                }
                else if (LineByLineRadioButton.Checked)
                {
                    this.readLineByLineUsingDataAccess3 = true;
                }
                else if (OnTheWholeRadioButton.Checked)
                {
                    this.readOnTheWholeUsingDataAccess3 = true;
                }
            }

            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// Sets the selected jobs to read the column names and the tool table for the worker thread.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadEverythingButton_Click(object sender, EventArgs e)
        {
            this.LockControl = true;
            this.readTableColumnNamesWithDataAccess2 = true;
            this.readTableColumnNamesWithDataAccess3 = true;
            this.readCellByCellUsingDataAccess2 = true;
            this.readCellByCellUsingDataAccess3 = true;
            this.readLineByLineUsingDataAccess3 = true;
            this.readOnTheWholeUsingDataAccess3 = true;

            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// Stops the processing of the active job in the worker thread and resets the job markers.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StopButton_Click(object sender, EventArgs e)
        {
            this.StopWorker();
        }

        /// <summary>
        /// Clears the tool table list view.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ClearToolTableButton_Click(object sender, EventArgs e)
        {
            TableListView.Items.Clear();
        }

        /// <summary>
        /// Clears the statistic list view.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ClearStatisticsButton_Click(object sender, EventArgs e)
        {
            StatisticsListView.Items.Clear();
        }

        /// <summary>
        /// Clears the tool table and the statistic list views.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ClearAllButton_Click(object sender, EventArgs e)
        {
            TableListView.Items.Clear();
            StatisticsListView.Items.Clear();
            LoadingProgressBar.Value = 0;
        }

        /// <summary>
        /// Enables the advanced data access if the radio button is set to IJHDataAccess3.
        /// </summary>
        /// <param name="sender">The selected radio button.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DataAccess2Button_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            if (radio != null)
            {
                bool enableAdvancedDataAccess = radio.Checked ? false : true;
                if (!enableAdvancedDataAccess)
                {
                    CellByCellRadioButton.Checked = true;
                }

                LineByLineRadioButton.Enabled = enableAdvancedDataAccess;
                OnTheWholeRadioButton.Enabled = enableAdvancedDataAccess;
            }
        }

        /// <summary>
        /// Shows context menu on right click on a tool in the tool list view.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">Used to find out which mouse button was clicked.</param>
        private void TableListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.isNck)
            {
                // Don't show kontext menu, because the manipulation functions are implemented
                // using the more performant IJHDataAccess3 interface.
                // IJHDataAccess3 is not available for non NCK controls.
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (TableListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    this.selectedRow = TableListView.FocusedItem;
                    ToolListContextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        // --- Context menu commands --------------------------------------------------------------

        /// <summary>
        /// Context menu strip --> edit selected tool on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void EditRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // --- 1. Edit Tool on tool table
            AddOrEditForm editForm = new AddOrEditForm(JobType.edit, this.selectedRow.Text, this.dataAccess);

            // --- 2. Alter data on list view
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                ListViewItem modifiedToolData = this.ReadSingleRowUsingDataAccess2(editForm.RowNumber);
                int index = TableListView.Items.IndexOfKey(editForm.RowNumber);
                if (index >= 0)
                {
                    TableListView.Items[index] = modifiedToolData;
                }
            }
        }

        /// <summary>
        /// Context menu strip --> delete selected tool on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DeleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.selectedRow != null)
                {
                    // --- 1. Delete row
                    string selectedRow = this.selectedRow.Text;
                    List<string> deletedTools = this.DeleteRows(selectedRow);

                    // --- 2. remove deleted rows from list
                    foreach (string row in deletedTools)
                    {
                        TableListView.Items.RemoveByKey(row);
                    }
                }
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
        }

        /// <summary>
        /// Context menu strip --> add index to selected tool (on control)
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AddRowIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.selectedRow == null)
                {
                    return;
                }

                string rowNumberWithIndex = this.selectedRow.Text;

                // --- 1. Add new index to tool table
                AddOrEditForm addIndex = new AddOrEditForm(JobType.addIndex, rowNumberWithIndex, this.dataAccess);
                DialogResult dr = addIndex.ShowDialog();

                // --- 2. Add new index to ListView
                if (dr == DialogResult.OK)
                {
                    string rowNumber = addIndex.RowNumber;
                    ListViewItem newRow = this.ReadSingleRowUsingDataAccess2(rowNumber);
                    if (newRow != null)
                    {
                        newRow.Name = rowNumber;
                        TableListView.Items.Add(newRow);

                        // Select the new item in the list view.
                        int index = TableListView.Items.IndexOf(newRow);
                        TableListView.Items[index].Selected = true;
                        TableListView.EnsureVisible(index);
                        TableListView.Select();
                    }
                }
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
        }

        /// <summary>
        /// Context menu strip --> add tool an control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AddRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRow == null)
            {
                return;
            }

            string rowNumberWithIndex = this.selectedRow.Text;

            // --- 1. Add new row to table
            AddOrEditForm addRow = new AddOrEditForm(JobType.addRow, rowNumberWithIndex, this.dataAccess);
            DialogResult dr = addRow.ShowDialog();

            // --- 2. Add new tool to ListView
            if (dr == DialogResult.OK)
            {
                string rowNumber = addRow.RowNumber;
                ListViewItem newRow = this.ReadSingleRowUsingDataAccess2(rowNumber);
                if (newRow != null)
                {
                    newRow.Name = rowNumber;
                    TableListView.Items.Add(newRow);
                    
                    // Select the new item in the list view.
                    int index = TableListView.Items.IndexOf(newRow);
                    TableListView.Items[index].Selected = true;
                    TableListView.EnsureVisible(index);
                    TableListView.Select();
                }
            }
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Use this method to add a line to the statistic list view.
        /// If you want to do this from a separate thread, you have to use the delegate.
        /// </summary>
        /// <param name="message">The message to show in the statistics list view.</param>
        /// <param name="duration">The duration to show in the statistics list view.</param>
        private void AddStatisticLine(string message, TimeSpan duration)
        {
            StatisticsListView.BeginUpdate();
            StatisticsListView.Items.Add(message).SubItems.Add(duration.TotalMilliseconds.ToString());
            StatisticsListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            StatisticsListView.EndUpdate();
        }

        /// <summary>
        /// Initializes the max value of the progress bar. Set this to the amount of tools in tool table.
        /// If you want to do this from a separate thread, you have to use the delegate.
        /// </summary>
        /// <param name="maxProgress">The max value of the progress bar.</param>
        private void InitProgressBarMax(int maxProgress)
        {
            LoadingProgressBar.Maximum = maxProgress;
            LoadingProgressBar.Value = 0;
        }

        /// <summary>
        /// Initializes the row count in the combo box header.
        /// If you want to do this from a separate thread, you have to use the delegate.
        /// </summary>
        /// <param name="toolCount">Amount of tool in tool table.</param>
        private void InitRowCount(int toolCount)
        {
            StatisticsGroupBox.Text = "Statistics (" + toolCount + " rows found in table)";
        }

        /// <summary>
        /// Stops background worker and resets all jobs.
        /// </summary>
        private void StopWorker()
        {
            if (this.worker.WorkerSupportsCancellation == true)
            {
                this.worker.CancelAsync();
            }

            // reset all jobs
            this.readTableColumnNamesWithDataAccess2 = false;
            this.readTableColumnNamesWithDataAccess3 = false;
            this.readCellByCellUsingDataAccess2 = false;
            this.readCellByCellUsingDataAccess3 = false;
            this.readLineByLineUsingDataAccess3 = false;
            this.readOnTheWholeUsingDataAccess3 = false;
        }

        // --- Read Column Header Names -----------------------------------------------------------

        /// <summary>
        /// Read the table column names using the DataAccess2 interface.
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadTableColumnNamesWithDataAccess2(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (worker == null)
            {
                return;
            }

            IJHDataEntry table = null;
            IJHDataEntryList tableColumnNames = null;
            IJHDataEntry tableColumnName = null;
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                table = this.dataAccess.GetDataEntry(TableAccessors.ColumnNameList);
                tableColumnNames = table.childList;
                int tableColumnsCount = tableColumnNames.Count;
                this.Invoke(new InitProgressBarMaxHandler(this.InitProgressBarMax), tableColumnsCount - 1);

                ColumnHeader[] columnHeaderList;
                if (this.isNck)
                {
                    columnHeaderList = new ColumnHeader[tableColumnsCount];
                }
                else
                {
                    // Add one element for the primary key of the table
                    // --> The iTNC530 for example don't transmit the primary key of the table by fetching the column names
                    columnHeaderList = new ColumnHeader[tableColumnsCount + 1];
                    ColumnHeader columnHeader = new ColumnHeader(AppSettings.Default.PrimaryKey);
                    columnHeader.Text = AppSettings.Default.PrimaryKey;
                    columnHeaderList[0] = columnHeader;
                }

                for (int i = 0; i < tableColumnsCount; i++)
                {
                    tableColumnName = tableColumnNames[i];

                    ColumnHeader columnHeader = new ColumnHeader(tableColumnName.bstrName);
                    columnHeader.Text = tableColumnName.bstrName;
                    columnHeaderList[this.isNck ? i : i + 1] = columnHeader;

                    if (tableColumnName != null)
                    {
                        Marshal.ReleaseComObject(tableColumnName);
                    }

                    if (!this.dontShowStatistic)
                    {
                        worker.ReportProgress(i);
                    }
                }

                e.Result = columnHeaderList;
                timer.Stop();

                if (!this.dontShowStatistic)
                {
                    this.Invoke(new AddStatisticLineHandler(this.AddStatisticLine), "Read Column Names Using DataAccess2", timer.Elapsed);
                }
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
                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }

                if (tableColumnNames != null)
                {
                    Marshal.ReleaseComObject(tableColumnNames);
                }

                if (tableColumnName != null)
                {
                    Marshal.ReleaseComObject(tableColumnName);
                }
            }
        }

        /// <summary>
        /// Read the tool table column names using the DataAccess3 interface.
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadTableColumnNamesWithDataAccess3(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (worker == null)
            {
                return;
            }

            IJHDataEntry2 table = null;
            IJHDataEntry2List tableColumnNames = null;
            IJHDataEntry2 tableColumnName = null;
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                table = this.dataAccess.GetDataEntry2(TableAccessors.ColumnNameList, DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                tableColumnNames = table.GetChildList();
                int tableColumnsCount = tableColumnNames.Count;
                this.Invoke(new InitProgressBarMaxHandler(this.InitProgressBarMax), tableColumnsCount - 1);

                ColumnHeader[] columnHeaderList = new ColumnHeader[tableColumnsCount];
                for (int i = 0; i < tableColumnsCount; i++)
                {
                    tableColumnName = tableColumnNames[i];

                    ColumnHeader columnHeader = new ColumnHeader(tableColumnName.bstrName);
                    columnHeader.Text = tableColumnName.bstrName;
                    columnHeaderList[i] = columnHeader;

                    if (tableColumnName != null)
                    {
                        Marshal.ReleaseComObject(tableColumnName);
                    }

                    if (!this.dontShowStatistic)
                    {
                        worker.ReportProgress(i);
                    }
                }

                e.Result = columnHeaderList;
                timer.Stop();

                if (!this.dontShowStatistic)
                {
                    this.Invoke(new AddStatisticLineHandler(this.AddStatisticLine), "Read Column Names Using DataAccess3", timer.Elapsed);
                }
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
                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }

                if (tableColumnNames != null)
                {
                    Marshal.ReleaseComObject(tableColumnNames);
                }

                if (tableColumnName != null)
                {
                    Marshal.ReleaseComObject(tableColumnName);
                }
            }
        }

        // --- Read Tool Table --------------------------------------------------------------------

        /// <summary>
        /// Read the tool table cell by cell using the DataAccess2 interface.
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadCellByCellUsingDataAccess2(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (worker == null)
            {
                return;
            }

            IJHDataEntry table = null;
            IJHDataEntryList tableLines = null;
            IJHDataEntry tableLine = null;
            IJHDataEntryList tableCells = null;
            IJHDataEntry tableCell = null;
            IJHDataEntryPropertyList cellPropertyList = null;
            IJHDataEntryProperty cellProperty = null;
            object cellValue = null;
            string cellValueString = null;

            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                table = this.dataAccess.GetDataEntry(TableAccessors.PrimaryKeyList);
                tableLines = table.childList;
                int tableLinesCount = tableLines.Count;
                this.Invoke(new InitProgressBarMaxHandler(this.InitProgressBarMax), tableLinesCount - 1);
                this.BeginInvoke(new InitToolCountHandler(this.InitRowCount), tableLinesCount);

                ListViewItem[] listViewTableLines = new ListViewItem[tableLinesCount];
                for (int i = 0; i < tableLinesCount; i++)
                {
                    // check if worker has to be stoped
                    if (worker.CancellationPending == true)
                    {
                        e.Result = listViewTableLines;
                        e.Cancel = true;
                        break;
                    }

                    tableLine = tableLines[i];

                    // get child list from server
                    tableCells = tableLine.childList;

                    ListViewItem listViewTableLine = null;
                    for (int j = 0; j < tableCells.Count; j++)
                    {
                        tableCell = tableCells[j];

                        // iTNC530 throws DNC_E_INVALID_PARAM (0x80040264) if line is empty
                        try
                        {
                            cellPropertyList = tableCell.propertyList;
                            cellProperty = cellPropertyList.get_property(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                            cellValue = cellProperty.varValue;
                        }
                        catch (COMException)
                        {
                            cellValue = string.Empty;
                        }
                        finally
                        {
                            if (cellValue != null)
                            {
                                // You only need this for TOOL_P access.
                                if (AppSettings.Default.TableAccessor == "TOOL_P" && tableCell.bstrName == "P")
                                {
                                    int[] pocketTableKey = (int[])cellValue;
                                    cellValueString = pocketTableKey[0].ToString();
                                    cellValueString += "." + pocketTableKey[1].ToString();
                                }
                                else
                                {
                                    cellValueString = cellValue.ToString();
                                }

                                if (tableCell.bstrName == AppSettings.Default.PrimaryKey)
                                {
                                    cellValueString = cellValueString.Replace(",", ".");
                                }
                            }
                            else
                            {
                                cellValueString = "<undefined>";
                            }

                            if (listViewTableLine == null)
                            {
                                if (!this.isNck)
                                {
                                    listViewTableLine = new ListViewItem(Convert.ToString(i));
                                    listViewTableLine.Name = Convert.ToString(i);
                                    /* MSDN INFO:
                                     * The Name property corresponds to the key for a ListViewItem in the ListView.ListViewItemCollection.
                                     * The key comparison is not case-sensitive. If the key parameter is null or an empty string, IndexOfKey returns -1.
                                     * --> We need the key to remove the item from list in a handy way
                                     */

                                    listViewTableLine.SubItems.Add(cellValueString);
                                }
                                else
                                {
                                    listViewTableLine = new ListViewItem(cellValueString);
                                    listViewTableLine.Name = cellValueString;
                                }
                            }
                            else
                            {
                                listViewTableLine.SubItems.Add(cellValueString);
                            }
                        }

                        if (tableCell != null)
                        {
                            Marshal.ReleaseComObject(tableCell);
                        }

                        if (cellPropertyList != null)
                        {
                            Marshal.ReleaseComObject(cellPropertyList);
                        }

                        if (cellProperty != null)
                        {
                            Marshal.ReleaseComObject(cellProperty);
                        }
                    }

                    listViewTableLines[i] = listViewTableLine;

                    if (tableLine != null)
                    {
                        Marshal.ReleaseComObject(tableLine);
                    }

                    if (tableCells != null)
                    {
                        Marshal.ReleaseComObject(tableCells);
                    }

                    if (!this.dontShowStatistic)
                    {
                        worker.ReportProgress(i);
                    }
                }

                e.Result = listViewTableLines;
                timer.Stop();

                if (!this.dontShowStatistic)
                {
                    this.Invoke(new AddStatisticLineHandler(this.AddStatisticLine), "Read Cell By Cell Using DataAccess2", timer.Elapsed);
                }
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
                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }

                if (tableLines != null)
                {
                    Marshal.ReleaseComObject(tableLines);
                }

                if (tableLine != null)
                {
                    Marshal.ReleaseComObject(tableLine);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (cellPropertyList != null)
                {
                    Marshal.ReleaseComObject(cellPropertyList);
                }

                if (cellProperty != null)
                {
                    Marshal.ReleaseComObject(cellProperty);
                }
            }
        }

        /// <summary>
        /// Read the tool table cell by cell using the DataAccess3 interface.
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadCellByCellUsingDataAccess3(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (worker == null)
            {
                return;
            }

            IJHDataEntry2 table = null;
            IJHDataEntry2List tableLines = null;
            IJHDataEntry2 tableLine = null;
            IJHDataEntry2List tableCells = null;
            IJHDataEntry2 tableCell = null;
            IJHDataEntryProperty2List cellPropertyList = null;
            IJHDataEntryProperty2 cellProperty = null;
            object cellValue = null;
            string cellValueString = null;
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                table = this.dataAccess.GetDataEntry2(TableAccessors.PrimaryKeyList, DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);

                // get child list from server
                tableLines = table.GetChildList();
                int tableLinesCount = tableLines.Count;
                this.Invoke(new InitProgressBarMaxHandler(this.InitProgressBarMax), tableLinesCount - 1);
                this.BeginInvoke(new InitToolCountHandler(this.InitRowCount), tableLinesCount);

                ListViewItem[] listViewTableLines = new ListViewItem[tableLinesCount];
                for (int i = 0; i < tableLinesCount; i++)
                {
                    // check if worker has to be stoped
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    tableLine = tableLines[i];

                    // get child list from server
                    tableCells = tableLine.GetChildList();

                    ListViewItem listViewTableLine = null;
                    for (int j = 0; j < tableCells.Count; j++)
                    {
                        tableCell = tableCells[j];

                        // get property from server
                        cellValue = tableCell.GetPropertyValue(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                        if (cellValue != null)
                        {
                            // You only need this for TOOL_P access.
                            if (AppSettings.Default.TableAccessor == "TOOL_P" && tableCell.bstrName == "P")
                            {
                                int[] pocketTableKey = (int[])cellValue;
                                cellValueString = pocketTableKey[0].ToString();
                                cellValueString += "." + pocketTableKey[1].ToString();
                            }
                            else
                            {
                                cellValueString = cellValue.ToString();
                            }

                            if (tableCell.bstrName == AppSettings.Default.PrimaryKey)
                            {
                                cellValueString = cellValueString.Replace(",", ".");
                            }
                        }
                        else
                        {
                            cellValueString = "<undefined>";
                        }

                        if (listViewTableLine == null)
                        {
                            listViewTableLine = new ListViewItem(cellValueString);

                            /* MSDN INFO:
                             * The Name property corresponds to the key for a ListViewItem in the ListView.ListViewItemCollection.
                             * The key comparison is not case-sensitive. If the key parameter is null or an empty string, IndexOfKey returns -1.
                             * --> We need the key to remove the item from list in a handy way
                             */
                            listViewTableLine.Name = cellValueString;
                        }
                        else
                        {
                            listViewTableLine.SubItems.Add(cellValueString);
                        }

                        if (tableCell != null)
                        {
                            Marshal.ReleaseComObject(tableCell);
                        }

                        if (cellPropertyList != null)
                        {
                            Marshal.ReleaseComObject(cellPropertyList);
                        }

                        if (cellProperty != null)
                        {
                            Marshal.ReleaseComObject(cellProperty);
                        }
                    }

                    listViewTableLines[i] = listViewTableLine;

                    if (tableLine != null)
                    {
                        Marshal.ReleaseComObject(tableLine);
                    }

                    if (tableCells != null)
                    {
                        Marshal.ReleaseComObject(tableCells);
                    }

                    if (!this.dontShowStatistic)
                    {
                        worker.ReportProgress(i);
                    }
                }

                e.Result = listViewTableLines;
                timer.Stop();

                if (!this.dontShowStatistic)
                {
                    this.Invoke(new AddStatisticLineHandler(this.AddStatisticLine), "Read Cell By Cell Using DataAccess3", timer.Elapsed);
                }
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
                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }

                if (tableLines != null)
                {
                    Marshal.ReleaseComObject(tableLines);
                }

                if (tableLine != null)
                {
                    Marshal.ReleaseComObject(tableLine);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (cellPropertyList != null)
                {
                    Marshal.ReleaseComObject(cellPropertyList);
                }

                if (cellProperty != null)
                {
                    Marshal.ReleaseComObject(cellProperty);
                }
            }
        }

        /// <summary>
        /// Read the table line by line using the DataAccess3 interface.
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReadLineByLineUsingDataAccess3(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (worker == null)
            {
                return;
            }

            IJHDataEntry2 table = null;
            IJHDataEntry2List tableLines = null;
            IJHDataEntry2 tableLine = null;
            IJHDataEntry2List tableCells = null;
            IJHDataEntry2 tableCell = null;
            IJHDataEntryProperty2List cellPropertyList = null;
            IJHDataEntryProperty2 cellProperty = null;
            object cellValue = null;
            string cellValueString = null;
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                table = this.dataAccess.GetDataEntry2(TableAccessors.PrimaryKeyList, DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                tableLines = table.GetChildList();
                int tableLinesCount = tableLines.Count;
                this.Invoke(new InitProgressBarMaxHandler(this.InitProgressBarMax), tableLinesCount - 1);
                this.BeginInvoke(new InitToolCountHandler(this.InitRowCount), tableLinesCount);

                ListViewItem[] listViewToolLines = new ListViewItem[tableLinesCount];
                for (int i = 0; i < tableLinesCount; i++)
                {
                    // check if worker has to be stoped
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    tableLine = tableLines[i];

                    // Fetch Properties every tool (tool line)
                    tableLine.ReadTreeValues(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                    // get the local child list
                    tableCells = tableLine.childList2;

                    ListViewItem listViewTableLine = null;

                    for (int j = 0; j < tableCells.Count; j++)
                    {
                        tableCell = tableCells[j];

                        // We do not want to fetch from server
                        // CellValue = ToolCell.GetPropertyValue(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                        // get local property value
                        cellValue = tableCell.propertyValue;

                        if (cellValue != null)
                        {
                            // You only need this for TOOL_P access.
                            if (AppSettings.Default.TableAccessor == "TOOL_P" && tableCell.bstrName == "P")
                            {
                                int[] pocketTableKey = (int[])cellValue;
                                cellValueString = pocketTableKey[0].ToString();
                                cellValueString += "." + pocketTableKey[1].ToString();
                            }
                            else
                            {
                                cellValueString = cellValue.ToString();
                            }
 
                            if (tableCell.bstrName == AppSettings.Default.PrimaryKey)
                            {
                                cellValueString = cellValueString.Replace(",", ".");
                            }
                        }
                        else
                        {
                            cellValueString = "<undefined>";
                        }

                        if (listViewTableLine == null)
                        {
                            listViewTableLine = new ListViewItem(cellValueString);

                            /* MSDN INFO:
                             * The Name property corresponds to the key for a ListViewItem in the ListView.ListViewItemCollection.
                             * The key comparison is not case-sensitive. If the key parameter is null or an empty string, IndexOfKey returns -1.
                             * --> We need the key to remove the item from list in a handy way
                             */
                            listViewTableLine.Name = cellValueString;
                        }
                        else
                        {
                            listViewTableLine.SubItems.Add(cellValueString);
                        }

                        if (tableCell != null)
                        {
                            Marshal.ReleaseComObject(tableCell);
                        }

                        if (cellPropertyList != null)
                        {
                            Marshal.ReleaseComObject(cellPropertyList);
                        }

                        if (cellProperty != null)
                        {
                            Marshal.ReleaseComObject(cellProperty);
                        }
                    }

                    listViewToolLines[i] = listViewTableLine;

                    if (tableLine != null)
                    {
                        Marshal.ReleaseComObject(tableLine);
                    }

                    if (tableCells != null)
                    {
                        Marshal.ReleaseComObject(tableCells);
                    }

                    if (!this.dontShowStatistic)
                    {
                        worker.ReportProgress(i);
                    }
                }

                e.Result = listViewToolLines;
                timer.Stop();

                if (!this.dontShowStatistic)
                {
                    this.Invoke(new AddStatisticLineHandler(this.AddStatisticLine), "Read Line By Line Using DataAccess3", timer.Elapsed);
                }
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
                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }

                if (tableLines != null)
                {
                    Marshal.ReleaseComObject(tableLines);
                }

                if (tableLine != null)
                {
                    Marshal.ReleaseComObject(tableLine);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (cellPropertyList != null)
                {
                    Marshal.ReleaseComObject(cellPropertyList);
                }

                if (cellProperty != null)
                {
                    Marshal.ReleaseComObject(cellProperty);
                }
            }
        }

        /// <summary>
        /// Read the whole table at once using the DataAccess3 interface.
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

            IJHDataEntry2 table = null;
            IJHDataEntry2List tableWithIndexList = null;
            IJHDataEntry2 tableWithIndex = null;
            IJHDataEntry2List tableLines = null;
            IJHDataEntry2 toolLine = null;
            IJHDataEntry2List tableCells = null;
            IJHDataEntry2 tableCell = null;
            object cellValue = null;
            string cellValueString = null;
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                table = this.dataAccess.GetDataEntry2(TableAccessors.WholeTable, DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                table.ReadTreeValues(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                tableWithIndexList = table.childList2;
                tableWithIndex = tableWithIndexList[0];
                tableLines = tableWithIndex.childList2;
                int toolLinesCount = tableLines.Count;
                this.Invoke(new InitProgressBarMaxHandler(this.InitProgressBarMax), toolLinesCount - 1);
                this.BeginInvoke(new InitToolCountHandler(this.InitRowCount), toolLinesCount);

                ListViewItem[] listViewToolLines = new ListViewItem[toolLinesCount];
                for (int i = 0; i < toolLinesCount; i++)
                {
                    // check if worker has to be stoped
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    toolLine = tableLines[i];
                    tableCells = toolLine.childList2;

                    ListViewItem listViewToolLine = null;

                    for (int j = 0; j < tableCells.Count; j++)
                    {
                        tableCell = tableCells[j];

                        // We do not want to fetch from server
                        // CellValue = ToolCell.GetPropertyValue(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                        // get local property value
                        cellValue = tableCell.propertyValue;

                        if (cellValue != null)
                        {
                            // You only need this for TOOL_P access.
                            if (AppSettings.Default.TableAccessor == "TOOL_P" && tableCell.bstrName == "P")
                            {
                                int[] pocketTableKey = (int[])cellValue;
                                cellValueString = pocketTableKey[0].ToString();
                                cellValueString += "." + pocketTableKey[1].ToString();
                            }
                            else
                            {
                                cellValueString = cellValue.ToString();
                            }

                            if (tableCell.bstrName == AppSettings.Default.PrimaryKey)
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

                        if (tableCell != null)
                        {
                            Marshal.ReleaseComObject(tableCell);
                        }
                    }

                    listViewToolLines[i] = listViewToolLine;

                    if (toolLine != null)
                    {
                        Marshal.ReleaseComObject(toolLine);
                    }

                    if (tableCells != null)
                    {
                        Marshal.ReleaseComObject(tableCells);
                    }

                    if (!this.dontShowStatistic)
                    {
                        worker.ReportProgress(i);
                    }
                }

                e.Result = listViewToolLines;
                timer.Stop();

                if (!this.dontShowStatistic)
                {
                    this.Invoke(new AddStatisticLineHandler(this.AddStatisticLine), "Read On The Whole Using DataAccess3", timer.Elapsed);
                }
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
                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }

                if (tableLines != null)
                {
                    Marshal.ReleaseComObject(tableLines);
                }

                if (toolLine != null)
                {
                    Marshal.ReleaseComObject(toolLine);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (tableWithIndexList != null)
                {
                    Marshal.ReleaseComObject(tableWithIndexList);
                }

                if (tableWithIndex != null)
                {
                    Marshal.ReleaseComObject(tableWithIndex);
                }
            }
        }

        // --- Table handling ----------------------------------------------------------------------+

        /// <summary>
        /// Read data of a single table row.
        /// </summary>
        /// <param name="primaryKey">Read data from this row.</param>
        /// <returns>A List view element with the data of the selected row.</returns>
        private ListViewItem ReadSingleRowUsingDataAccess2(string primaryKey)
        {
            ListViewItem retVal = new ListViewItem();
            IJHDataEntry tableLine = null;
            IJHDataEntryList tableCells = null;
            IJHDataEntry tableCell = null;
            IJHDataEntryPropertyList cellPropertyList = null;
            IJHDataEntryProperty cellProperty = null;
            object cellValue = null;
            string cellValueString = null;

            try
            {
                tableLine = this.dataAccess.GetDataEntry(TableAccessors.PrimaryKeyList + @"\" + primaryKey);
                tableCells = tableLine.childList;
                int toolCellsCount = tableCells.Count;

                for (int i = 0; i < toolCellsCount; i++)
                {
                    tableCell = tableCells[i];
                    cellPropertyList = tableCell.propertyList;
                    cellProperty = cellPropertyList.get_property(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                    cellValue = cellProperty.varValue;

                    if (cellValue == null)
                    {
                        cellValueString = "<undefined>";
                    }
                    else if (cellValue.GetType() == typeof(System.DBNull))
                    {
                        cellValueString = "<undefined>";
                    }
                    else
                    {
                        // You only need this for TOOL_P access.
                        if (AppSettings.Default.TableAccessor == "TOOL_P" && tableCell.bstrName == "P")
                        {
                            int[] pocketTableKey = (int[])cellValue;
                            cellValueString = pocketTableKey[0].ToString();
                            cellValueString += "." + pocketTableKey[1].ToString();
                        }
                        else
                        {
                            cellValueString = cellValue.ToString();
                        }

                        if (tableCell.bstrName == AppSettings.Default.PrimaryKey)
                        {
                            cellValueString = cellValueString.Replace(",", ".");
                            retVal.Text = cellValueString;
                            retVal.Name = cellValueString;
                        }
                    }

                    if (!(tableCell.bstrName == AppSettings.Default.PrimaryKey))
                    {
                        retVal.SubItems.Add(cellValueString);
                    }

                    Debug.WriteLine(i.ToString() + ": " + tableCell.bstrName + ": " + cellValueString);

                    if (tableCell != null)
                    {
                        Marshal.ReleaseComObject(tableCell);
                        tableCell = null;
                    }

                    if (cellPropertyList != null)
                    {
                        Marshal.ReleaseComObject(cellPropertyList);
                        cellPropertyList = null;
                    }

                    if (cellProperty != null)
                    {
                        Marshal.ReleaseComObject(cellProperty);
                        cellProperty = null;
                    }
                }
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
                if (tableLine != null)
                {
                    Marshal.ReleaseComObject(tableLine);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (cellPropertyList != null)
                {
                    Marshal.ReleaseComObject(cellPropertyList);
                }

                if (cellProperty != null)
                {
                    Marshal.ReleaseComObject(cellProperty);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Delete row using specified primary key.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value of the row to delete.</param>
        /// <returns>A list of deleted rows.</returns>
        private List<string> DeleteRows(string primaryKeyValue)
        {
            List<string> deletedRows = new List<string>();

            // This is a dummy "null object" used to delete a data record in the table
            object keyEmptyValue = null;

            // List of all tools found by request
            HeidenhainDNCLib.IJHDataEntry2 requestResult = null;
            HeidenhainDNCLib.IJHDataEntry2List foundRows = null;

            // Tools and tool columns
            HeidenhainDNCLib.IJHDataEntry2 row = null;
            HeidenhainDNCLib.IJHDataEntry2 field = null;

            // Select an entry including all indices (only one digit in this example)
            string dataSelection = TableAccessors.PrimaryKeyList + @"\";
            if (primaryKeyValue.Contains(@"."))
            {
                dataSelection += primaryKeyValue;
            }
            else
            {
                dataSelection += "'" + primaryKeyValue + @".0-" + primaryKeyValue + @".9'";
            }

            try
            {
                // Read the necessary information from the control
                requestResult = this.dataAccess.GetDataEntry2(dataSelection, DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                requestResult.ReadTreeValues(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                #region "Workaround"
                // This is only a workaround for NCK based controls <= MLST 13 (for TNC 640 <= 340595 09)
                // --> If only one row is found using the given data selection (for example "\TABLE\TOOL\T\'5.0-5.9'")
                //     the controls replys the fields of this row instead of a list containing only a single entry (this row).
                bool indicesFound = false;
                try
                {
                    field = requestResult.get_child(AppSettings.Default.PrimaryKey);
                }
                catch (COMException cex)
                {
                    if (cex.ErrorCode == (int)HeidenhainDNCLib.DNC_HRESULT.DNC_E_ITEM_NOT_FOUND)
                    {
                        indicesFound = true;
                    }
                    else
                    {
                        throw cex;
                    }
                }
                finally
                {
                    if (field != null)
                    {
                        Marshal.ReleaseComObject(field);
                    }
                }

                if (!indicesFound)
                {
                    field = requestResult.get_child(AppSettings.Default.PrimaryKey);

                    // This method deletes the row on the control
                    field.SetPropertyValue(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA, keyEmptyValue, false);

                    deletedRows.Add(primaryKeyValue);
                    return deletedRows;
                }
                #endregion

                // delete found tools
                foundRows = requestResult.childList2;
                for (int i = foundRows.Count - 1; i >= 0; i--)
                {
                    row = foundRows[i];

                    field = row.get_child(AppSettings.Default.PrimaryKey);

                    // This method deletes the row on the control
                    field.SetPropertyValue(DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA, keyEmptyValue, false);

                    string deletedRow = row.bstrName.TrimEnd(".0".ToCharArray());
                    deletedRows.Add(deletedRow);

                    Marshal.ReleaseComObject(field);
                    Marshal.ReleaseComObject(row);
                }
            }
            finally
            {
                if (requestResult != null)
                {
                    Marshal.ReleaseComObject(requestResult);
                }

                if (foundRows != null)
                {
                    Marshal.ReleaseComObject(foundRows);
                }

                if (row != null)
                {
                    Marshal.ReleaseComObject(row);
                }

                if (field != null)
                {
                    Marshal.ReleaseComObject(field);
                }
            }

            return deletedRows;
        }
        #endregion
    }
}
