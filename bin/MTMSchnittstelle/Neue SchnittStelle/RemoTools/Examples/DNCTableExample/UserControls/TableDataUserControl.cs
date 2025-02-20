// ------------------------------------------------------------------------------------------------
// <copyright file="TableDatauserControl.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// This Class represents a small user control witch can be used to show 
    /// a tool data with its column name
    /// </summary>
    public partial class TableDataUserControl : UserControl
    {
        #region "fields"
        /// <summary>
        /// Tool-Tip to show the data type of the given value
        /// </summary>
        private ToolTip dataTypeInfo = new ToolTip();
        #endregion

        #region "constructor & destructor"

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDataUserControl"/> class.
        /// </summary>
        /// <param name="columnName">The column name describes the value. e.g. TNAME, L or ANGLE</param>
        /// <param name="varValueType">The data type of the value. e.g. System.string</param>
        public TableDataUserControl(string columnName, string varValueType)
        {
            this.InitializeComponent();
            this.ColumnName = columnName;
            this.VarValueType = varValueType;
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets or sets a value indicating whether the text in the label above the text box.
        /// Set this property to the tool table column name of the specific tool data.
        /// </summary>
        public string ColumnName
        {
            get
            {
                return ColumnNameLabel.Text;
            }

            set
            {
                ColumnNameLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in the tool tip of the text box
        /// Set this property to value type of the specific tool data.
        /// </summary>
        public string VarValueType
        {
            get
            {
                return this.dataTypeInfo.ToolTipTitle;
            }

            set
            {
                this.dataTypeInfo.SetToolTip(this.VarValueTextBox, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in the tool box.
        /// Set this property to the value of the specific tool data.
        /// </summary>
        public string VarValue
        {
            get
            {
                return VarValueTextBox.Text;
            }

            set
            {
                VarValueTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text box is enabled.
        /// Use this property to lock the text box if the specific tool data is read only.
        /// For example: the T-number of the tool (unique primary key)
        /// </summary>
        public bool Lock
        {
            get
            {
                return !VarValueTextBox.Enabled;
            }

            set
            {
                VarValueTextBox.Enabled = !value;
            }
        }
        #endregion
    }
}
