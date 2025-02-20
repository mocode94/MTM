// ------------------------------------------------------------------------------------------------
// <copyright file="ProgressDiagram.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Josef Kamml</author>
// <date>19.04.2017</date>
// <summary>
// This class is used to demonstrate the workflow of the ImportToolUsageCSV() and the
// PrepareToolLists() function of the Automatic4 Interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the ImportToolUsage() function 
    /// of the Automatic4 Interface
    /// </summary>
    public partial class ProgressDiagram : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDiagram"/> class.
        /// </summary>
        public ProgressDiagram()
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        #region "enumerations"
        /// <summary>
        /// ImportToolUsageCSV progress status
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// Preparing the working folder on the control.
            /// </summary>
            PreparingWorkingFolder,

            /// <summary>
            /// Working folder prepared successfully.
            /// </summary>
            WorkingFolder_OK,

            /// <summary>
            /// Preparing the tools in tool table on the control.
            /// </summary>
            PreparingToolData,

            /// <summary>
            /// Tool table prepared successfully.
            /// </summary>
            ToolData_OK,

            /// <summary>
            /// Call the Automatic4.ImportToolUsage() method on the control.
            /// </summary>
            ImportingToolUsage,

            /// <summary>
            /// ImportToolUsage() finished successfully
            /// </summary>
            ImportToolUsage_OK
        }

        /// <summary>
        /// The two possible functions types for Automatic4.
        /// </summary>
        public enum FunctionType
        {
            /// <summary>
            /// Function ImportToolUsageCSV selected.
            /// </summary>
            ImportToolUsageCSV,

            /// <summary>
            /// Function PrepareToolLists selected.
            /// </summary>
            PrepareToolLists,
        }
        #endregion

        /// <summary>
        /// Status model "ToolUsageDiagram".
        /// </summary>
        /// <param name="status">The parameter depicts the current status</param>
        public void StatusToolUsageDiagram(Status status)
        {
            switch (status)
            {
                case Status.PreparingWorkingFolder:
                    this.PrepareWorkingFolderStateBox.BackColor = Color.Yellow;
                    return;
                case Status.WorkingFolder_OK:
                    this.PrepareWorkingFolderStateBox.BackColor = Color.Green;
                    return;

                case Status.PreparingToolData:
                    this.PrepareToolDataStateBox.BackColor = Color.Yellow;
                    return;
                case Status.ToolData_OK:
                    this.PrepareToolDataStateBox.BackColor = Color.Green;
                    return;

                case Status.ImportingToolUsage:
                    this.UniversalFinishedStateBox.BackColor = Color.Yellow;
                    return;
                case Status.ImportToolUsage_OK:
                    this.UniversalFinishedStateBox.BackColor = Color.Green;
                    return;
            }
        }

        /// <summary>
        /// Clear the status model. 
        /// </summary>
        public void ResetDiagram()
        {
            this.PrepareWorkingFolderStateBox.BackColor = SystemColors.ControlLightLight;
            this.PrepareToolDataStateBox.BackColor = SystemColors.ControlLightLight;
            this.UniversalFinishedStateBox.BackColor = SystemColors.ControlLightLight;
        }

        /// <summary>
        /// Change the label of ImportToolUsageDiagram appropriate to the selection
        /// ImportToolUsageCSV or PrepareToolLists
        /// </summary>
        /// <param name="ftype">The new function type.</param>
        public void ChangeLabel(FunctionType ftype)
        {
            switch (ftype)
            {
                case FunctionType.ImportToolUsageCSV:
                    this.UniversalFunctionLabel.Text = "ImportToolUsageCSV()";
                    return;
                case FunctionType.PrepareToolLists:
                    this.UniversalFunctionLabel.Text = "PrepareToolLists()";
                    return;
            }
        }

        #region "private methods"
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ImportToolUsageDiagram_Load(object sender, EventArgs e)
        {
            // statusbox information
            ToolTip workingFolderToolTip = new ToolTip();
            ToolTip toolDataToolTip = new ToolTip();
            ToolTip importToolUsageToolTip = new ToolTip();

            workingFolderToolTip.SetToolTip(this.PrepareWorkingFolderStateBox, " -Prepare working folder on the control" + Environment.NewLine + " -Delete old files and create new working folder\n -Copy files to the control");
            toolDataToolTip.SetToolTip(this.PrepareToolDataStateBox, " -Prepare tool table" + Environment.NewLine + " -Check if all the required tools on the control ");
            importToolUsageToolTip.SetToolTip(this.UniversalFinishedStateBox, " -Create tool usage lists (.H.T.DEP)");
        }
        #endregion
    }
}
