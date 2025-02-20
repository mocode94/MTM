// ------------------------------------------------------------------------------------------------
// <copyright file="ITNCTable.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>07.06.2017</date>
// <summary>
// This file contains a class which shows how to use the ITNCTable interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the ITNCTable interface.
    /// </summary>
    public partial class ITNCTable : UserControl
    {
        #region "constants"
        /// <summary>
        /// The location of the tool table file on the control.
        /// </summary>
        private const string TOOLTABLE = @"TNC:\TOOL.T";
        #endregion

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
        /// Object of HeidenhainDNC ITNCTable interface.
        /// </summary>
        private HeidenhainDNCLib.ITNCTable itncTable = null;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// A list of the header titles.
        /// </summary>
        private List<string> columnTitles = null;

        /// <summary>
        /// The positions of the header titles in the first line of the tool table.
        /// </summary>
        private List<int> columnPos = null;

        /// <summary>
        /// The location to store a temporary tool table file.
        /// </summary>
        private string temporaryFile;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="ITNCTable"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public ITNCTable(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.ITNCTable_Disposed);

            this.machine = parentForm.Machine;
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// Get all interfaces and subscribe for all events here.
        /// </summary>
        /// <returns>Initialization successful.</returns>
        public bool Init()
        {
            try
            {
                // --- Get the interface Object(s) ------------------------------------------------
                this.itncTable = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_TNCTABLE);

                //// --- Subscribe for the event(s) -------------------------------------------------

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
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ITNCTable_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            // Generate file name for the temporary tool table in the system temp path. 
            this.temporaryFile = Path.Combine(Path.GetTempPath(), "TOOLEXT.T");

            // ----- Put predefines querries to combo box ----------------------------------------
            string[] predefinedQueries = new string[]
            {
                "WHERE T>=0",               // All tools
                "WHERE T>=10 AND T<=20",    // Tools between T10 and T20
                "WHERE L>=100",             // Tools with a length greater than 100mm
                "WHERE ACC>0",              // All tool with activated function ACC
                "WHERE AFC NOT LIKE ''"      // All tool with selected AFC stategy
            };

            QuerryComboBox.DataSource = predefinedQueries;

            // ----- Init column header of the list view ------------------------------------------
            // Get column headers from control
            Array columnTitles = null;
            Array columnPos = null;
            try
            {
                this.itncTable.GetTableInfo(TOOLTABLE, ref columnTitles, ref columnPos);
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

            // Store the column header in .NET typical data types
            this.columnTitles = new List<string>((string[])columnTitles);
            this.columnPos = new List<int>((int[])columnPos);

            // Initialize the column header in the list view
            foreach (string title in this.columnTitles)
            {
                ToolTableListView.Columns.Add("ch" + title, title);
            }
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ITNCTable_Disposed(object sender, EventArgs e)
        {
            if (this.itncTable != null)
            {
                // --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.itncTable);
            }
#if !DEBUG
     /* In this application we release all interfaces, event handlers and JH helper objects
      * explicitly using the Marshal.ReleaseComObject(<COM Object>) mechanism.
      * If we try to disconnect before all objects are released, we get an error in the windows
      * event log. With the information from the windows event log, we can find the orphan object.
      * Therefore in DEBUG built we do not use the garbage collector for releasing all COM objects.
      * 
      * But for the release software built we use the garbage collector to avoid memory leaks,
      * even when a object was not explicitly released my Marshall.ReleaseComObject
      */
   
      /*  MSDN: How to: Handle Events Raised by a COM Source
      *   Note that COM objects that raise events within a .NET client require two Garbage Collector (GC) collections before they are released.
      *   This is caused by the reference cycle that occurs between COM objects and managed clients.
      *   If you need to explicitly release a COM object you should call the Collect method twice.
      */
      GC.Collect();
      GC.Collect();
      GC.WaitForPendingFinalizers();
#endif
        }

        /// <summary>
        /// Opens a file dialog to choose a tool table file to transmit to the control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TransmitFromFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Tool tables (*.T)|*.T|All files (*.*)|*.*";
            fd.Multiselect = false;

            if (!(fd.ShowDialog() == DialogResult.OK))
            {
                return;
            }

            string fileName = fd.FileName;

            if (!this.CheckiTncTable(fileName))
            {
                MessageBox.Show(
                           "The selected file does not match the required format." + Environment.NewLine +
                           "Are you sure you have selected an iTNC530 table in a valid format?",
                           "Format error!",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                return;
            }

            try
            {
                this.itncTable.TransmitTablePart(fileName, TOOLTABLE);
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
        }

        /// <summary>
        /// Transmit the content of the list view to the connected control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TransmitFromListViewButton_Click(object sender, EventArgs e)
        {
            // ----- Create data structure (this implementation only supports the unit MM) --------
            int lineCount = ToolTableListView.Items.Count;
            string header = @"BEGIN TOOLEXT.T MM";
            string columnHeader = this.CreateTableLine(this.columnPos, this.columnTitles);
            List<string> tableContent = new List<string>(lineCount);

            for (int i = 0; i < lineCount; i++)
            {
                // Store the content of the sub items to a string list.
                // --> A sub item is a tool data like the Name oder the Length
                List<string> tableLine = ToolTableListView.Items[i].SubItems.Cast<ListViewItem.ListViewSubItem>().Select(x => x.Text).ToList();

                // Store all the items in string list
                // --> An item is a tool line like T5 or T5.3 
                tableContent.Add(this.CreateTableLine(this.columnPos, tableLine));
            }

            // ----- Write data to temporary file -------------------------------------------------
            // It is important to set the correct encoding. Otherwise errors can occur with the German umlauts.
            using (StreamWriter sw = new StreamWriter(this.temporaryFile, false, System.Text.Encoding.Default))
            {
                // Create the correct header (important)
                sw.WriteLine(header);
                sw.WriteLine(columnHeader);

                // Write the tool data
                foreach (string line in tableContent)
                {
                    sw.WriteLine(line);
                }

                // Create the correct footer (important)
                sw.WriteLine(@"[END]");
            }

            // ----- Transmit temporary tool table file to the control ----------------------------
            this.itncTable.TransmitTablePart(this.temporaryFile, TOOLTABLE);
        }

        /// <summary>
        /// Receive table lines according to the selected query and show the result in a list view.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReceiveTableLinesButton_Click(object sender, EventArgs e)
        {
            Array localTable = null;
            try
            {
                // Receive table lines for the selected querry
                string querryString = QuerryComboBox.Text;
                this.itncTable.ReceiveTableLines(TOOLTABLE, querryString, ref localTable);

                this.ShowTableArray(ref localTable);
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
        }

        /// <summary>
        /// Delete the by the given queries selected record(s).
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DeleteRecordButton_Click(object sender, EventArgs e)
        {
            try
            {
                string querryString = QuerryComboBox.Text;
                
                // This method deletes only the first found record.
                // Please refer also the RemoTools SDK manual.
                this.itncTable.DeleteRecord(TOOLTABLE, querryString);
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
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Check the tool table file if it meets the required format.
        /// </summary>
        /// <param name="file">The full file name of the tool table file.</param>
        /// <returns>The file is valid.</returns>
        private bool CheckiTncTable(string file)
        {
            string header;
            string columnTitles;

            // Read the first 2 lines of the file
            using (StreamReader sr = new StreamReader(file, System.Text.Encoding.Default))
            {
                header = sr.ReadLine();
                columnTitles = sr.ReadLine();
            }

            // Consistency check "header" using a regular expression:
            /* ************************************************************************************
             * \b^BEGIN\s\b                   --> The first string has to be the single word BEGIN
             *                                    an it has to be tailes with a single white space.
             * (?=(\w|\s){7}\.T)\w+\s*\.T(\s) --> The file name 9 char including .T ail
             * (\bMM\b|\bINCH\b)              --> MM or INCH
             *************************************************************************************/
            string headerExpression = @"\b^BEGIN\s\b" + @"(?=(\w|\s){7}\.T)\w+\s*\.T(\s)" + @"(\bMM\b|\bINCH\b)";
            Regex headerRegex = new Regex(headerExpression);
            Match headerMatch = headerRegex.Match(header);

            if (!headerMatch.Success)
            {
                return false;
            }

            // Consistency check "column titles" using a regular expression:
            /* ************************************************************************************
             * ^[T]   --> The single chat "T" must be the first character in this line
             * \s{7}  --> It has to be tailed with at least 7 white spaces
             * In the example below the numeric literal 7 is replaces by the real size of the
             * T column fetched from the connected control.
             * Subtract 2 because:
             * --> lColumnPos is not 0 based (subtract 1)
             * --> The size of the name has to be excludet (T.length = 1 --> subtract another one 1)
             * 
             * lColumnPos is fetched in this.ITNCTable_Load()
             * see --> GetTableInfo (..., ...,[in, out] SAFEARRAY(long)*lColumnPos) 
             *************************************************************************************/
            string columnTitlesExpression = @"^T\s{" + string.Format("{0}", this.columnPos[1] - 2)  + "}";
            Regex columnTitlesRegex = new Regex(columnTitlesExpression);
            Match columnTitlesMatch = columnTitlesRegex.Match(columnTitles);
            if (!columnTitlesMatch.Success)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show the array with the tool data in a list view control.
        /// </summary>
        /// <param name="localTable">The array with the tool data.</param>
        private void ShowTableArray(ref Array localTable)
        {
            ToolTableListView.Items.Clear();

            string line;
            string cell;

            for (int i = 0; i < localTable.Length; i++)
            {
                line = Convert.ToString(localTable.GetValue(i));
                ListViewItem item = null;
                for (int j = 0; j < this.columnPos.Count - 1; j++)
                {
                    cell = line.Substring(this.columnPos[j] - 1, this.columnPos[j + 1] - this.columnPos[j] - 1).TrimEnd();

                    if (j == 0)
                    {
                        item = new ListViewItem(cell);
                    }
                    else
                    {
                        item.SubItems.Add(cell);
                    }
                }

                ToolTableListView.Items.Add(item);
            }
        }

        /// <summary>
        /// Create a table line in the required format.
        /// </summary>
        /// <param name="columnPos">The positions of the columns.</param>
        /// <param name="columnTitles">The column titles.</param>
        /// <returns>Returns a table line in the required format.</returns>
        private string CreateTableLine(List<int> columnPos, List<string> columnTitles)
        {
            string retval = string.Empty;

            int length;
            int whiteSpace;

            for (int i = 0; i < this.columnPos.Count - 1; i++)
            {
                length = columnPos[i + 1] - columnPos[i];
                whiteSpace = length - columnTitles[i].Length;
                retval += columnTitles[i] + string.Empty.PadLeft(whiteSpace);
            }

            return retval;
        }
        #endregion
    }
}
