// ------------------------------------------------------------------------------------------------
// <copyright file="ToolTable.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>19.04.2017</date>
// <summary>
// This class is used to read tool data from a file "tool.t" ans transmit it to a control
// via IJHDataAccess.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class is used to read tool data from a file "tool.t" and transmit it to a control
    /// via IJHDataAccess.
    /// </summary>
    public class ToolTable
    {
        #region "fiels"
        /// <summary>
        /// A list of the column names of the tool table "tool.t".
        /// </summary>
        private List<string> columnHeader = new List<string>();

        /// <summary>
        /// The data of the tool table. The first row contains the column names of the tool table "tool.t".
        /// </summary>
        private List<List<string>> tools = new List<List<string>>();
        #endregion

        #region "properties"
        /// <summary>
        /// Gets the list of the tool read from tool.t file.
        /// </summary>
        public List<List<string>> GetTools
        {
            get
            {
                return this.tools;
            }
        }

        /// <summary>
        /// Gets the list of tool numbers read from tool.t file.
        /// </summary>
        public List<string> GetToolNumbers
        {
            get
            {
                List<string> toolNumbers = new List<string>();
                for (int i = 1; i < this.tools.Count; i++)
                {
                    toolNumbers.Add(this.tools[i][0]);
                }

                return toolNumbers;
            }
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// Read the tool table "tool.t" by filename.
        /// </summary>
        /// <param name="toolTableFile">The full filename of a valid tool table file.</param>
        public void ReadToolTableFile(string toolTableFile)
        {
            // ----- Read the tool table file -----------------------------------------------------
            string[] toolList = File.ReadAllLines(toolTableFile);
            int startIndex = 0;
            int stopIndex = toolList.Length - 1;

            // ----- Prepare for further processing -----------------------------------------------
            if (toolList[0].StartsWith("BEGIN"))
            {
                startIndex++;
            }

            if (toolList[stopIndex].StartsWith("[END]"))
            {
                stopIndex--;
            }

            // ----- Determine column size --------------------------------------------------------
            List<int> columnSize = new List<int>();

            string headerString = toolList[startIndex];
            bool lastCharWasEmpty = true;
            bool charIsEmpty;

            for (int i = 0; i < headerString.Length; i++)
            {
                charIsEmpty = headerString[i] == ' ';
                if (lastCharWasEmpty && !charIsEmpty)
                {
                    columnSize.Add(i);
                }

                lastCharWasEmpty = charIsEmpty;
            }

            columnSize.Add(headerString.Length);

            // ----- Read column names (column header) --------------------------------------------
            for (int i = 0; i < columnSize.Count - 1; i++)
            {
                this.columnHeader.Add(headerString.Substring(columnSize[i], columnSize[i + 1] - columnSize[i]).Trim());
            }

            startIndex++;
            this.tools.Add(this.columnHeader);

            // ----- Store tool data in a handy .NET data type ------------------------------------
            string toolValueString = string.Empty;
            for (int i = startIndex; i <= stopIndex; i++)
            {
                List<string> toolValue = new List<string>();
                for (int j = 0; j < columnSize.Count - 1; j++)
                {
                    toolValueString = toolList[i].Substring(columnSize[j], columnSize[j + 1] - columnSize[j]).Trim();
                    toolValue.Add(toolValueString);
                }

                this.tools.Add(toolValue);
            }
        }

        /// <summary>
        /// Transmit the tool data to a control. Be shure to read the tool data first.
        /// </summary>
        /// <param name="dataAccess">Object of the HeidenhainDNC IJHDataAccess interface.</param>
        public void TransmitToolsToControl(HeidenhainDNCLib.IJHDataAccess3 dataAccess)
        {
            HeidenhainDNCLib.IJHDataEntry2 tool = null;
            HeidenhainDNCLib.IJHDataEntry2List toolCells = null;
            HeidenhainDNCLib.IJHDataEntry2 toolCell = null;
            try
            {
                string toolName;
                string dataSelection;

                // Check if all given tool numbers exists on the control
                List<string> toolsToCheck = this.GetToolNumbers;
                List<string> toolsNotPresentOnControl = this.CheckIfToolsExist(toolsToCheck, dataAccess);

                for (int i = 1; i < this.tools.Count; i++)
                {
                    toolName = this.tools[i][0];

                    // Create new table line for each tool number that does not exists on the control
                    foreach (string toolToAdd in toolsNotPresentOnControl)
                    {
                        this.AddRecordToTable(@"\TABLE\TOOL", @"T", toolToAdd, dataAccess);
                    }

                    dataSelection = @"\TABLE\TOOL\T\" + toolName;
                    int lockHandle = dataAccess.LockTable(dataSelection);

                    tool = dataAccess.GetDataEntry2(dataSelection, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                    tool.ReadTreeValues(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                    toolCells = tool.childList2;

                    for (int j = 1; j < this.tools[0].Count; j++)
                    {
                        for (int k = 1; k < toolCells.Count; k++)
                        {
                            toolCell = toolCells[k];

                            if (toolCell.bstrName == this.tools[0][j])
                            {
                                toolCell.set_propertyValue(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA, this.tools[i][j]);
                                Marshal.ReleaseComObject(toolCell);
                                break;
                            }

                            Marshal.ReleaseComObject(toolCell);
                        }
                    }
                    
                    tool.WriteTreeValues(false);
                    dataAccess.UnlockTable(lockHandle);

                    Marshal.ReleaseComObject(toolCells);
                    Marshal.ReleaseComObject(tool);
                }
            }
            finally
            {
                if (tool != null)
                {
                    Marshal.ReleaseComObject(tool);
                }

                if (toolCells != null)
                {
                    Marshal.ReleaseComObject(toolCells);
                }

                if (toolCell != null)
                {
                    Marshal.ReleaseComObject(toolCell);
                }
            }
        }

        /// <summary>
        /// Add a new record to a table on control.
        /// </summary>
        /// <param name="tableIdent">The data selection of the table to add the new record.</param>
        /// <param name="primaryKeyName">The name of the primary key of the selected table.</param>
        /// <param name="primaryKeyValue">The value for the new primary key.</param>
        /// <param name="dataAccess">Object of the HeidenhainDNC IJHDataAccess interface.</param>
        public void AddRecordToTable(string tableIdent, string primaryKeyName, string primaryKeyValue, HeidenhainDNCLib.IJHDataAccess3 dataAccess)
        {
            // to add a record, a new (non-existing) primary key has to be added to the list
            HeidenhainDNCLib.IJHDataEntry2 primaryKeyColumn = null;
            HeidenhainDNCLib.IJHDataEntry2 newRecord = null;
            HeidenhainDNCLib.IJHDataEntry2 primaryKeyField = null;

            try
            {
                // create ident for the primary key column
                string primaryKeyIdent = tableIdent + @"\" + primaryKeyName;

                // get primary key column: e.g. "\TABLE\TOOL\T"
                primaryKeyColumn = dataAccess.GetDataEntry2(primaryKeyIdent, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);

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
        /// Checks if a given list tools exists in tool table on the control.
        /// </summary>
        /// <param name="toolNumbers">The number of the tools.</param>
        /// <param name="dataAccess">An instance of the >= IJHDataAccess3 interface.</param>
        /// <returns>Returns a list of all tool numbers that does not exist on the control.</returns>
        public List<string> CheckIfToolsExist(List<string> toolNumbers, HeidenhainDNCLib.IJHDataAccess3 dataAccess)
        {
            // deep copy the list of the tool numbers to check
            List<string> notPresentOnControl = new List<string>(toolNumbers);

            HeidenhainDNCLib.IJHDataEntry2 toolTable = null;
            HeidenhainDNCLib.IJHDataEntry2List toolNumberList = null;
            HeidenhainDNCLib.IJHDataEntry2 tool = null;

            try
            {
                toolTable = dataAccess.GetDataEntry2(@"\TABLE\TOOL\T", HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                toolNumberList = toolTable.GetChildList();

                for (int i = 0; i < toolNumberList.Count; i++)
                {
                    tool = toolNumberList[i];
                    string toolnumber = tool.bstrName.Replace("'", string.Empty);

                    for (int j = 0; j < toolNumbers.Count; j++)
                    {
                        if (toolnumber == toolNumbers[j])
                        {
                            notPresentOnControl.Remove(toolNumbers[j]);
                        }
                    }

                    Marshal.ReleaseComObject(tool);
                }
            }
            finally
            {
                if (toolTable != null)
                {
                    Marshal.ReleaseComObject(toolTable);
                }

                if (toolNumberList != null)
                {
                    Marshal.ReleaseComObject(toolNumberList);
                }

                if (tool != null)
                {
                    Marshal.ReleaseComObject(tool);
                }
            }

            return notPresentOnControl;
        }
        #endregion
    }
}
