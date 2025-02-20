// ------------------------------------------------------------------------------------------------
// <copyright file="AddOrEditForm.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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

namespace DNC_CSharp_Demo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using AppSettings = DNC_CSharp_Demo.Properties.Settings;

    /// <summary>
    /// Describes the job type. Add or edit tool (or tool index).
    /// </summary>
    public enum JobType
    {
        /// <summary>
        /// Add new row.
        /// </summary>
        addRow,

        /// <summary>
        /// Add index to existing row.
        /// </summary>
        addIndex,

        /// <summary>
        /// Edit existing row
        /// </summary>
        edit
    }

    /// <summary>
    /// Provides control elements for adding or editing table data.
    /// </summary>
    public partial class AddOrEditForm : Form
    {
        #region "fields"
        /// <summary>
        /// Object of HeidenhainDNC IJHVersion interface
        /// </summary>
        private HeidenhainDNCLib.JHDataAccess dataAccess = null;

        /// <summary>
        /// Used to specify the row to add or edit.
        /// <seealso cref="AddOrEditForm"/>
        /// </summary>
        private string primaryKeyValue = null;

        /// <summary>
        /// Used to specify the job while opening this user control.
        /// <seealso cref="AddOrEditForm"/>
        /// </summary>
        private JobType job;

        /// <summary>
        /// This is the internal lock handle.
        /// You get it from IJHDataAccess::LockTable and need it as reference for IJHDataAccess::UnLockTable
        /// </summary>
        private int tableLockHandle = -1;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrEditForm"/> class and initializes the components container.
        /// </summary>
        /// <param name="job">Type of the job.</param>
        /// <param name="primaryKeyValue">Primary key of the row.</param>
        /// <param name="dataAccess">The initialized IJHDataAccess interface.</param>
        public AddOrEditForm(JobType job, string primaryKeyValue, HeidenhainDNCLib.JHDataAccess dataAccess)
        {
            this.InitializeComponent();
            this.components = new Container();

            this.job = job;
            this.primaryKeyValue = primaryKeyValue;
            this.dataAccess = dataAccess;
        }
        #endregion

        #region "properies"
        /// <summary>
        /// Gets the chosen row number (which has been added or modified)
        /// </summary>
        public string RowNumber
        {
            get
            {
                return this.primaryKeyValue;
            }
        }

        /// <summary>
        /// Gets the full path of the chosen row number (which has been added or modified)
        /// </summary>
        private string DataSelectionRowNumber
        {
            get
            {
                return TableAccessors.PrimaryKeyList + @"\" + this.primaryKeyValue;
            }
        }
        #endregion

        #region "event handler methods"
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void FrmAddOrEdit_Load(object sender, EventArgs e)
        {
            switch (this.job)
            {
                case JobType.addRow:
                    this.Text = "Add row";
                    GeneralInfoLabel.Text = "Add new row";
                    Ok_Button.Enabled = false;

                    // create combo box (choose tool)
                    List<int> gaps = this.FindGapsInTable();
                    ChooseNewItemComboBox.DataSource = gaps;
                    break;
                case JobType.addIndex:
                    this.Text = "Add Index";
                    Ok_Button.Enabled = false;

                    // It is not allowed to stack points (index of index)
                    // Therefore add an index to the "main" tool number
                    if (this.primaryKeyValue.Contains("."))
                    {
                        this.primaryKeyValue = this.primaryKeyValue.Remove(this.primaryKeyValue.IndexOf('.'));
                    }

                    GeneralInfoLabel.Text = "Add index to row " + this.primaryKeyValue;

                    // Check if at least one unused index is available
                    List<int> unusedIndices = this.FindUnusedIndices(this.primaryKeyValue);
                    if (unusedIndices == null)
                    {
                        SelectItem_Button.Enabled = false;
                        Ok_Button.Enabled = false;

                        Label infoLabel = new Label();
                        infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                        infoLabel.AutoSize = true;
                        infoLabel.Text = "No unused index left!";
                        TableDataFlowLayoutPanel.Controls.Add(infoLabel);
                    }

                    // create combo box (choose index)
                    ChooseNewItemComboBox.DataSource = unusedIndices;
                    ChooseNewItemComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    break;
                case JobType.edit:
                    this.Text = "Edit existing row";
                    this.ChooseNewItemComboBox.Enabled = false;
                    this.SelectItem_Button.Enabled = false;
                    this.LockRowOnControl();
                    this.ReadTableData(true);
                    break;
            }
        }

        /// <summary>
        /// Select row or index to add, chosen from the ChooseNewItemComboBox.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SelectItemButton_Click(object sender, EventArgs e)
        {
            SelectItem_Button.Enabled = false;
            Ok_Button.Enabled = true;
            ChooseNewItemComboBox.Enabled = false;
            Ok_Button.Enabled = true;
            switch (this.job)
            {
                case JobType.addIndex:
                    this.primaryKeyValue += "." + ChooseNewItemComboBox.Text;
                    break;
                case JobType.addRow:
                    this.primaryKeyValue = ChooseNewItemComboBox.Text;
                    break;
            }

            this.AddRecordToTable(TableAccessors.WholeTable, AppSettings.Default.PrimaryKey, this.primaryKeyValue);
            this.ReadTableData();
        }

        /// <summary>
        /// Accepts the changes and writes it to control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (this.WriteTableData())
            {
                this.Close();
            }
        }

        /// <summary>
        /// Dismisses the changes an releases the lock for this tool on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.UnlockRowOnControl();
            this.Close();
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Locks the tool on control.
        /// TNC640 with a software version &lt;= 340590 05 locks the whole tool table.
        /// <seealso cref="UnlockRowOnControl"/>
        /// </summary>
        private void LockRowOnControl()
        {
            /* Till MLST9 (TNC640 software version 340 590 05) it is only possible to lock the whole table!
            * To lock single lines (for example tools) is supported from MLST10 (TNC640 software version 340 590 06) and above.
            */
            try
            {
                if (this.tableLockHandle < 0)
                {
                    this.tableLockHandle = this.dataAccess.LockTable(this.DataSelectionRowNumber);
                    GeneralInfoLabel.Text = "Attention, row " + this.primaryKeyValue + " is locked on control now!";
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
        /// Unlocks the tool on control.
        /// <seealso cref="LockRowOnControl"/>
        /// </summary>
        private void UnlockRowOnControl()
        {
            try
            {
                if (this.tableLockHandle > 0)
                {
                    this.dataAccess.UnlockTable(this.tableLockHandle);
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
        /// Finds unused indices of a specific tool.
        /// </summary>
        /// <param name="toolNumber">The tool number to find the unused indices for.</param>
        /// <returns>A List of all unused indices for the requested tool.</returns>
        private List<int> FindUnusedIndices(string toolNumber)
        {
            HeidenhainDNCLib.IJHDataEntry2 tool = null;
            HeidenhainDNCLib.IJHDataEntry2List toolIndexList = null;
            HeidenhainDNCLib.IJHDataEntry2 toolIndex = null;
            HeidenhainDNCLib.IJHDataEntry2 tnumber = null;
            HeidenhainDNCLib.IJHDataEntry2List toolCells = null;

            try
            {
                // --- 1. Get selected tool with all indices
                string dataSelection = string.Empty;

                int index = toolNumber.IndexOf(".");
                if (index > 0)
                {
                    toolNumber = toolNumber.Substring(0, toolNumber.Length - index);
                }

                dataSelection = TableAccessors.PrimaryKeyList + @"\" + "'" + toolNumber + ".0-" + toolNumber + ".9'";
                tool = this.dataAccess.GetDataEntry2(dataSelection, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);

                if (!tool.bIsNode)
                {
                    return null;
                }

                toolIndexList = tool.GetChildList();
                int indexCount = toolIndexList.Count;

                // --- 2. find unused indices (gaps) in tool index list
                List<int> unusedIndices = new List<int>();
                double tnumberDouble = double.NaN;

                if (indexCount <= 0 || indexCount == 10)
                {
                    return null;
                }

                unusedIndices.AddRange(new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                if (indexCount <= 9)
                {
                    int actIndex = -1;
                    for (int i = 0; i < indexCount; i++)
                    {
                        toolIndex = toolIndexList[i];
                        if (!toolIndex.bIsNode)
                        {
                            return null;
                        }

                        toolIndex.ReadTreeValues(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                        tnumber = toolIndex.get_child(AppSettings.Default.PrimaryKey);
                        tnumberDouble = tnumber.get_propertyValue(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                        actIndex = (int)Math.Round((tnumberDouble * 10) % 10);

                        // remove existing indices from list
                        unusedIndices.Remove(actIndex);

                        if (tnumber != null)
                        {
                            Marshal.ReleaseComObject(tnumber);
                            tnumber = null;
                        }

                        if (toolIndex != null)
                        {
                            Marshal.ReleaseComObject(toolIndex);
                            toolIndex = null;
                        }

                        if (toolCells != null)
                        {
                            Marshal.ReleaseComObject(toolCells);
                            toolCells = null;
                        }
                    }
                }

                return unusedIndices;
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
                return null;
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
                return null;
            }
            finally
            {
                if (tool != null)
                {
                    Marshal.ReleaseComObject(tool);
                }

                if (toolIndexList != null)
                {
                    Marshal.ReleaseComObject(toolIndexList);
                }

                if (toolIndex != null)
                {
                    Marshal.ReleaseComObject(toolIndex);
                }

                if (tnumber != null)
                {
                    Marshal.ReleaseComObject(tnumber);
                }

                if (toolCells != null)
                {
                    Marshal.ReleaseComObject(toolCells);
                }
            }
        }

        /// <summary>
        /// Find gaps (empty lines) in tool table
        /// </summary>
        /// <returns>A list of empty lines (unused tool numbers) in tool table.</returns>
        private List<int> FindGapsInTable()
        {
            string toolNumberWithIndex = string.Empty;
            string[] split;
            List<int> toolNumbers = new List<int>();
            List<int> gapsInToolTable = new List<int>();

            // --- 1. get list of all tools
            HeidenhainDNCLib.IJHDataEntry2 toolTable = null;
            HeidenhainDNCLib.IJHDataEntry2List tnumbers = null;
            HeidenhainDNCLib.IJHDataEntry2 toolCell = null;
            try
            {
                toolTable = this.dataAccess.GetDataEntry2(TableAccessors.PrimaryKeyList, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                toolTable.ReadTreeValues(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                tnumbers = toolTable.childList2;

                // --- 1. Get list of all tools in tool table
                for (int i = 0; i < tnumbers.Count; i++)
                {
                    toolCell = tnumbers[i];

                    split = toolCell.bstrName.Split(new char[] { '.' });

                    toolNumbers.Add(Convert.ToInt32(split[0]));

                    if (toolCell != null)
                    {
                        Marshal.ReleaseComObject(toolCell);
                        toolCell = null;
                    }
                }

                // --- 2. Create list of all gaps in tool table list (TNumbers)
                toolNumbers.Sort();
                int firstToolNumber = toolNumbers[0];
                int lastToolNumber = toolNumbers[toolNumbers.Count - 1];
                for (int i = firstToolNumber; i <= lastToolNumber; i++)
                {
                    if (!toolNumbers.Contains(i))
                    {
                        gapsInToolTable.Add(i);
                    }
                }

                // If there are less than 10 gaps, then fill the list with tailing tool numbers
                if (gapsInToolTable.Count < 10)
                {
                    int newToolMin = (from l in toolNumbers select l).Max() + 1;
                    int newToolMax = newToolMin + (10 - gapsInToolTable.Count);
                    for (int i = newToolMin; i < newToolMax; i++)
                    {
                        gapsInToolTable.Add(i);
                    }
                }

                return gapsInToolTable;
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
                return null;
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
                return null;
            }
            finally
            {
                if (toolCell != null)
                {
                    Marshal.ReleaseComObject(toolCell);
                }

                if (tnumbers != null)
                {
                    Marshal.ReleaseComObject(tnumbers);
                }

                if (toolTable != null)
                {
                    Marshal.ReleaseComObject(toolTable);
                }
            }
        }

        /// <summary>
        /// Add a new record to a table on control.
        /// </summary>
        /// <param name="tableIdent">The data selection of the table to add the new record.</param>
        /// <param name="primaryKeyName">The name of the primary key of the selected table.</param>
        /// <param name="primaryKeyValue">The value for the new primary key.</param>
        private void AddRecordToTable(string tableIdent, string primaryKeyName, string primaryKeyValue)
        {
            // to add a record, a new (non-existing) primary key has to be added to the list
            HeidenhainDNCLib.IJHDataEntry2 primaryKeyColumn = null;
            HeidenhainDNCLib.IJHDataEntry2 newRecord = null;
            HeidenhainDNCLib.IJHDataEntry2 primaryKeyField = null;

            try
            {
                // create identifier for the primary key column
                string primaryKeyIdent = tableIdent + @"\" + primaryKeyName;

                // get primary key column: e.g. "\TABLE\TOOL\T"
                primaryKeyColumn = this.dataAccess.GetDataEntry2(primaryKeyIdent, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);

                // the child list of the primary key column contains all present primary keys
                // to add a record, a new (non-existing) primary key is added to the list
                // e.g. 999 for "\TABLE\TOOL\T\999"
                newRecord = primaryKeyColumn.add_child(primaryKeyValue);

                // post: "\TABLE\TOOL\T\999\T"

                // the new 'child' is still empty, so add a primary key field to it.
                // e.g. T for "\TABLE\TOOL\T\999\T"
                primaryKeyField = newRecord.add_child(primaryKeyName);

                // the new field has no value yet: write new primary key value
                // e.g. 999 for "\TABLE\TOOL\T\999\T" = 999

                // now modify the value of the DATA property of the primary key FIELD 
                // (e.g."\TABLE\TOOL\T\999\T" = 999)
                // AND update the server accordingly
                // Note: up to here all operations were done on the local data objects.
                // Note: although the primary key field may be a numeric value, 
                //       it is allowed to write it as a string here.
                object newPrimaryKeyValue = primaryKeyValue;
                primaryKeyField.SetPropertyValue(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA, newPrimaryKeyValue, false);

                // post: record "\TABLE\TOOL\T\999" is created on the server. 
                //       Since only the primary key field was given, 
                //       all other fields are initialized with their respective default values.

                // Note that Read-Only fields can only be given a non-default value, 
                // if they are written together with the primary key value.
                // This requires that first these fields are initialized in the local data tree, 
                // after which they can be written to the server all at once by WriteTreeValues().
                // An existing record should be used to get the correct property types.
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
            finally
            {
                if (primaryKeyColumn != null)
                {
                    Marshal.ReleaseComObject(primaryKeyColumn);
                }

                if (newRecord != null)
                {
                    Marshal.ReleaseComObject(newRecord);
                }

                if (primaryKeyField != null)
                {
                    Marshal.ReleaseComObject(primaryKeyField);
                }
            }
        }

        /// <summary>
        /// Read tool data from control.
        /// </summary>
        /// <param name="lockRow">Use this to lock the tool, if you want to edit data after reading.</param>
        private void ReadTableData(bool lockRow = false)
        {
            HeidenhainDNCLib.IJHDataEntry2 table = null;
            HeidenhainDNCLib.IJHDataEntry2List tableCells = null;
            HeidenhainDNCLib.IJHDataEntry2 tableCell = null;
            HeidenhainDNCLib.IJHDataEntryProperty2List tableCellPropertyList = null;
            HeidenhainDNCLib.IJHDataEntryProperty2 tableCellProperty = null;
            HeidenhainDNCLib.IJHDataEntryProperty2 tableCellPropertyType = null;
            object cellValue = null;
            string cellValueString = null;

            try
            {
                if (lockRow)
                {
                    this.LockRowOnControl();
                }

                table = this.dataAccess.GetDataEntry2(this.DataSelectionRowNumber, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                tableCells = table.GetChildList();

                for (int i = 0; i < tableCells.Count; i++)
                {
                    tableCell = tableCells[i];

                    tableCell.ReadPropertyValue(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                    tableCellPropertyList = tableCell.propertyList2;
                    tableCellProperty = tableCellPropertyList.get_property(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);

                    /* If varValue is empty, then the type given by C# Interop is System.DBNull!
                     * To write a specific value to control, we need to know the type of it to convert from
                     * textbox.Text (string) to this specific type.
                     * In this case we can use the DNC_DATAENTRY_PROPKIND_UPPER_BOUND or DNC_DATAENTRY_PROPKIND_LOWER_BOUND
                     * property kinds to detect the target type.
                     */
                    Type varValueType = tableCellProperty.varValue.GetType();

                    if (varValueType == typeof(System.DBNull))
                    {
                        tableCellPropertyType = tableCellPropertyList.get_property(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_UPPER_BOUND);
                        varValueType = tableCellPropertyType.varValue.GetType();
                        cellValueString = @"<undefined>";
                    }
                    else
                    {
                        cellValue = tableCellProperty.varValue;


                        // You only need this for TOOL_P access.
                        if (AppSettings.Default.TableAccessor == "TOOL_P" && tableCell.bstrName == "P")
                        {
                            int[] pockedTableKey = (int[])cellValue;
                            cellValueString = pockedTableKey[0].ToString();
                            cellValueString += "." + pockedTableKey[1].ToString();
                        }
                        else
                        {
                            cellValueString = cellValue.ToString();
                        }
                    }

                    UserControls.TableDataUserControl tableData = new UserControls.TableDataUserControl(tableCell.bstrName, varValueType.ToString());
                    tableData.VarValue = cellValueString;

                    // It is not allowed to write the columns T or LAST_USE
                    if (tableCell.bstrName == AppSettings.Default.PrimaryKey || tableCell.bstrName == "LAST_USE")
                    {
                        tableData.Lock = true;
                    }

                    this.components.Add(tableData);
                    TableDataFlowLayoutPanel.Controls.Add(tableData);

                    if (tableCellPropertyType != null)
                    {
                        Marshal.ReleaseComObject(tableCellPropertyType);
                        tableCellPropertyType = null;
                    }

                    if (tableCellPropertyList != null)
                    {
                        Marshal.ReleaseComObject(tableCellPropertyList);
                        tableCellPropertyList = null;
                    }

                    if (tableCellProperty != null)
                    {
                        Marshal.ReleaseComObject(tableCellProperty);
                        tableCellProperty = null;
                    }

                    if (tableCell != null)
                    {
                        Marshal.ReleaseComObject(tableCell);
                        tableCell = null;
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
            finally
            {
                if (tableCellPropertyType != null)
                {
                    Marshal.ReleaseComObject(tableCellPropertyType);
                }

                if (tableCellPropertyList != null)
                {
                    Marshal.ReleaseComObject(tableCellPropertyList);
                }

                if (tableCellProperty != null)
                {
                    Marshal.ReleaseComObject(tableCellProperty);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }
            }
        }

        /// <summary>
        /// Write tool data from ToolDataUserControl back to control.
        /// Unlocks the tool on control after writing.
        /// </summary>
        /// <returns>Writing successful</returns>
        private bool WriteTableData()
        {
            HeidenhainDNCLib.IJHDataEntry2 table = null;
            HeidenhainDNCLib.IJHDataEntry2List tableCells = null;
            HeidenhainDNCLib.IJHDataEntry2 tableCell = null;
            HeidenhainDNCLib.IJHDataEntryProperty2List tableCellPropertyList = null;
            HeidenhainDNCLib.IJHDataEntryProperty2 tableCellProperty = null;
            HeidenhainDNCLib.IJHDataEntryProperty2 tableCellPropertyType = null;

            try
            {
                table = this.dataAccess.GetDataEntry2(this.DataSelectionRowNumber, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                table.ReadTreeValues(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                tableCells = table.childList2;

                foreach (UserControls.TableDataUserControl tableData in TableDataFlowLayoutPanel.Controls)
                {
                    // It is not allowed to write the columns T or LAST_USE
                    if (tableData.ColumnName == AppSettings.Default.PrimaryKey || tableData.ColumnName == "LAST_USE")
                    {
                        continue;
                    }

                    if (tableData.VarValue == @"<undefined>")
                    {
                        continue;
                    }

                    tableCell = table.get_child(tableData.ColumnName);
                    tableCell.set_propertyValue(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA, (object)tableData.VarValue);

                    Marshal.ReleaseComObject(tableCell);
                }

                table.WriteTreeValues(true);
                this.UnlockRowOnControl();
                return true;
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
                return false;
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
                return false;
            }
            finally
            {
                if (tableCellPropertyType != null)
                {
                    Marshal.ReleaseComObject(tableCellPropertyType);
                }

                if (tableCellPropertyList != null)
                {
                    Marshal.ReleaseComObject(tableCellPropertyList);
                }

                if (tableCellProperty != null)
                {
                    Marshal.ReleaseComObject(tableCellProperty);
                }

                if (tableCell != null)
                {
                    Marshal.ReleaseComObject(tableCell);
                }

                if (tableCells != null)
                {
                    Marshal.ReleaseComObject(tableCells);
                }

                if (table != null)
                {
                    Marshal.ReleaseComObject(table);
                }
            }
        }
        #endregion
    }
}
