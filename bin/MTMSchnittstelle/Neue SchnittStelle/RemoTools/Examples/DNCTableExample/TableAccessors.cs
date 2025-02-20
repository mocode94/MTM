// ------------------------------------------------------------------------------------------------
// <copyright file="TableAccessors.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// -----------------------------------------------------------------------

namespace DNC_CSharp_Demo
{
    /// <summary>
    /// This Class stores the data selection accessors for the tool table.
    /// </summary>
    public static class TableAccessors
    {
        #region "fields"
        /// <summary>
        /// The data selection accessors to get the whole tool table.
        /// </summary>
        private static string wholeTable = @"\TABLE\" + Properties.Settings.Default.TableAccessor;

        /// <summary>
        /// The data selection accessors to get a list of all tool numbers.
        /// </summary>
        private static string primaryKeyList = wholeTable + @"\" + Properties.Settings.Default.PrimaryKey;

        /// <summary>
        /// The data selection accessors for the column names.
        /// This is different between iTNC530 and NCK based controls.
        /// </summary>
        private static string columnNameList = primaryKeyList + @"\" + Properties.Settings.Default.ColumnNames;
        #endregion

        #region "properties"
        /// <summary>
        /// Gets the data selection accessors to get the column names of the table.
        /// </summary>
        public static string ColumnNameList
        {
            get
            {
                return columnNameList;
            }
        }

        /// <summary>
        /// Gets the data selection accessors to get a list of the primary keys of all rows contained in the table.
        /// </summary>
        public static string PrimaryKeyList
        {
            get
            {
                return primaryKeyList;
            }
        }

        /// <summary>
        /// Gets the data selection accessor to get the whole table at once.
        /// </summary>
        public static string WholeTable
        {
            get
            {
                return wholeTable;
            }
        }
        #endregion
    }
}
