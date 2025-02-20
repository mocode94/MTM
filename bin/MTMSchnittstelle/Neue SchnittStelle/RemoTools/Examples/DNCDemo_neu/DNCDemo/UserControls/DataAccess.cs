// ------------------------------------------------------------------------------------------------
// <copyright file="DataAccess.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>26.07.2016</date>
// <summary>
// This file contains a class which handels the user controls for the
// different JHDataAccess interfaces.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHDataAccess interface.
    /// </summary>
    public partial class DataAccess : UserControl
    {
        #region "fields"
        /// <summary>
        /// The nck base software is organized in milestones and service packs.
        /// </summary>
        private Utils.NCK_Version nckVersion;

        /// <summary>
        /// Information if the connected CNC control NCK based.
        /// </summary>
        private bool isNck;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// The user control to handle the IJHDataAccess2 interface.
        /// </summary>
        private DataAccess2 dataAccess2 = null;

        /// <summary>
        /// The user control to handle the IJHDataAccess3 interface.
        /// </summary>
        private DataAccess3 dataAccess3 = null;

        /// <summary>
        /// The user control to handle the IJHDataAccess4 interface.
        /// </summary>
        private DataAccess4 dataAccess4 = null;

        /// <summary>
        /// Info panel. Used if one of the interfaces can't be initialized or is not available for the given NCK software version.
        /// </summary>
        private InfoPanel infoPanel = null;

#if RAW_COM_EVENTS
        /// <summary>
        /// A helper class to listen to the VTable event implementation.
        /// </summary>
        private DataListener dataListener = null;
#endif
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccess"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public DataAccess(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.DataAccess_Disposed);

            // connect external properties
            this.AppMainForm = parentForm;
            this.nckVersion = parentForm.NckVersion;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "Enums"
        /// <summary>
        /// All available data access interfaces.
        /// </summary>
        internal enum DataAccessInterface
        {
            /// <summary>
            /// The IJHDataAccess2 interface.
            /// </summary>
            IJHDataAccess2,

            /// <summary>
            /// The IJHDataAccess3 interface.
            /// </summary>
            IJHDataAccess3,

            /// <summary>
            /// The IJHDataAccess4 interface.
            /// </summary>
            IJHDataAccess4
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets the reference to the main application Form.
        /// </summary>
        internal MainForm AppMainForm { get; private set; }

        /// <summary>
        /// Sets a value indicating whether the data selection is enabled on the gui.
        /// </summary>
        internal bool EnableDataSelection
        {
            set
            {
                DataSelectionBackButton.Enabled = value;
                DataSelectionTextBox.Enabled = value;
                GetDataEntryButton.Enabled = value;
                SubscribeButton.Enabled = value;
            }
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// No initialization needed.
        /// </summary>
        /// <returns>Initialization successful.</returns>
        public bool Init()
        {
            try
            {
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

        #region "event handler methods"
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DataAccess_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            string oemPassword = Properties.Settings.Default.oemPassword;
            PasswordTextBox.Text = oemPassword;

            InterfaceComboBox.Items.Add(DataAccessInterface.IJHDataAccess2.ToString());

            if (this.isNck)
            {
                InterfaceComboBox.Items.Add(DataAccessInterface.IJHDataAccess4.ToString());

                // --- init access modes combo box --------------------------------------
                List<string> dataAccessModes = new List<string>();
                dataAccessModes.Add("DNC_ACCESS_MODE_TABLEDATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_PLCDATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_CFGDATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_GEODATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_GEOSIMDATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_SPLCDATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_HWSDATAACCESS");
                dataAccessModes.Add("DNC_ACCESS_MODE_OEM");

                AccessModeComboBox.DataSource = dataAccessModes;
            }
            else
            {
                // --- init access mode controls --------------------------------------
                AccessModeComboBox.Enabled = false;
                PasswordTextBox.Enabled = false;
                SetAccessModeButton.Enabled = false;
            }

            // Preselect IJHDataAccess, because it is available on nearly every JH control.
            InterfaceComboBox.SelectedItem = DataAccessInterface.IJHDataAccess2.ToString();

            this.UpdateGui();
        }

        /// <summary>
        /// Release all data access user controls.
        /// </summary>
        private void ReleaseDataAccessInterface()
        {
            // Remove other user controls if necessary
            if (this.dataAccess2 != null)
            {
                this.dataAccess2.Dispose();
                this.dataAccess2 = null;
            }

            if (this.dataAccess3 != null)
            {
                this.dataAccess3.Dispose();
                this.dataAccess3 = null;
            }

            if (this.dataAccess4 != null)
            {
                this.dataAccess4.Dispose();
                this.dataAccess4 = null;
            }
        }

        /// <summary>
        /// Release all data access user controls here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DataAccess_Disposed(object sender, EventArgs e)
        {
            this.ReleaseDataAccessInterface();
        }

        /// <summary>
        /// Sets the access mode using the values from DataSelectionTextBox and PasswordTextBox.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetAccessModeButton_Click(object sender, EventArgs e)
        {
            this.SetAccessMode(AccessModeComboBox.Text, PasswordTextBox.Text);
        }

        /// <summary>
        /// Navigates the current path one folder back and shows the content.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DataSelectionBackButton_Click(object sender, EventArgs e)
        {
            string dataSelectionString = DataSelectionTextBox.Text;
            int index = dataSelectionString.LastIndexOf(@"\");
            if (index > 1)
            {
                DataSelectionTextBox.Text = dataSelectionString.Substring(0, index);
            }
            else
            {
                DataSelectionTextBox.Text = @"\";
            }

            this.GetDataEntry(DataSelectionTextBox.Text);
        }

        /// <summary>
        /// Shows the content of the current data selection.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void GetDataEntryButton_Click(object sender, EventArgs e)
        {
            this.GetDataEntry(DataSelectionTextBox.Text);
        }

        /// <summary>
        /// Subscribe for current data selection.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SubscribeButton_Click(object sender, EventArgs e)
        {
            DataAccessInterface dataAccessInterface = (DataAccessInterface)Enum.Parse(typeof(DataAccessInterface), InterfaceComboBox.SelectedItem.ToString());

            switch (dataAccessInterface)
            {
                case DataAccessInterface.IJHDataAccess2:
                    this.dataAccess2.Subscribe(DataSelectionTextBox.Text);
                    break;
                case DataAccessInterface.IJHDataAccess3:
                    this.dataAccess3.Subscribe(DataSelectionTextBox.Text);
                    break;
                case DataAccessInterface.IJHDataAccess4:
                    break;
            }
        }

        /// <summary>
        /// Changes the data access user control if the  selection of the combo box becomes changed.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void InterfaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReleaseDataAccessInterface();

            DataAccessInterface dataAccessInterface = (DataAccessInterface)Enum.Parse(typeof(DataAccessInterface), InterfaceComboBox.SelectedItem.ToString());
            switch (dataAccessInterface)
            {
                case DataAccessInterface.IJHDataAccess2:
                    this.dataAccess2 = new DataAccess2(this.AppMainForm, this);
                    if (this.dataAccess2.Init())
                    {
                        this.EnableDataSelection = true;
                        this.dataAccess2.DataSelectionChanged += new DataAccess2.DataSelectionChangedDelegate(this.DataAccess2_DataSelectionChanged);
                        this.dataAccess2.DataSelectionDoubleClicked += new DataAccess2.DataSelectionDoubleClickedDelegate(this.DataAccess2_DataSelectionDoubleClicked);
                    }
                    else
                    {
                        this.dataAccess2.Dispose();
                        this.dataAccess2 = null;
                    }

                    break;
                case DataAccessInterface.IJHDataAccess3:
                    this.dataAccess3 = new DataAccess3(this.AppMainForm, this);
                    if (this.dataAccess3.Init())
                    {
                        this.EnableDataSelection = true;
                        this.dataAccess3.DataSelectionChanged += new DataAccess3.DataSelectionChangedDelegate(this.DataAccess3_DataSelectionChanged);
                        this.dataAccess3.DataSelectionDoubleClicked += new DataAccess3.DataSelectionDoubleClickedDelegate(this.DataAccess3_DataSelectionDoubleClicked);
                    }
                    else
                    {
                        this.dataAccess3.Dispose();
                        this.dataAccess3 = null;
                    }

                    break;
                case DataAccessInterface.IJHDataAccess4:
                    //if (this.nckVersion.MileStone >= 11)
                    //{
                        this.dataAccess4 = new DataAccess4(this.AppMainForm);
                        if (this.dataAccess4.Init())
                        {
                            this.EnableDataSelection = false;
                        }
                        else
                        {
                            this.dataAccess4.Dispose();
                            this.dataAccess4 = null;
                        }
                    //}
                    //else
                   // {
                   //     this.infoPanel = new InfoPanel("IJHDataAccess4 is only available on TNC controls containing DNC_SW_TYPE_NCKERN 597110 >= 11");
                   // }

                    break;
            }

            this.UpdateGui();
        }

        /// <summary>
        /// Sets data selection if selected index of child list has changed.
        /// </summary>
        /// <param name="sender">The child list list box.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ChildListListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                ListBox listBox = (ListBox)sender;
                if (listBox.SelectedItem != null)
                {
                    string selectedItem = listBox.SelectedItem.ToString();
                    DataSelectionTextBox.Text = selectedItem;
                }
            }
        }

        /// <summary>
        /// Gets the content of the double clicked item in child list list box.
        /// </summary>
        /// <param name="sender">The child list list box.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ChildListListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender != null)
            {
                ListBox listBox = (ListBox)sender;
                if (listBox.SelectedItem != null)
                {
                    string strSelectedItem = listBox.SelectedItem.ToString();
                    DataSelectionTextBox.Text = strSelectedItem;
                    this.GetDataEntry(strSelectedItem);
                }
            }
        }

        /// <summary>
        /// Open save file dialog to select log file.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SelectLogfileButton_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can choose or create a logfile.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "DNC_Demo OnData Logfile|*.log";
            saveFileDialog.Title = "Choose a filename to write logging to.";
            saveFileDialog.FileName = "IJHDataAccess";

            if (!Directory.Exists(LogFilePathTextBox.Text))
            {
                saveFileDialog.InitialDirectory = System.IO.Path.GetTempPath();
            }
            else
            {
                saveFileDialog.InitialDirectory = LogFilePathTextBox.Text;
            }

            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                LogFilePathTextBox.Text = Path.GetDirectoryName(saveFileDialog.FileName);
                LoggingCheckBox.Enabled = true;
            }
        }

        /// <summary>
        /// Open log file in standard text editor.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void EditLogFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(LogFilePathTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error opening logfile with external editor.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Start or stop logging.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void LoggingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dataAccess2 != null)
            {
                try
                {
                    this.dataAccess2.SwitchLogging(LoggingCheckBox.Checked, LogFilePathTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Please choose valid logfile folder!", "Logging error...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoggingCheckBox.Checked = false;
                }
            }

            if (this.dataAccess3 != null)
            {
                try
                {
                    this.dataAccess3.SwitchLogging(LoggingCheckBox.Checked, LogFilePathTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Please choose valid logfile folder!", "Logging error...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoggingCheckBox.Checked = false;
                }
            }
        }

        /// <summary>
        /// Get the data entry, if a child in the child list becomes double clicked.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The standard event arguments containing the data selection.</param>
        private void DataAccess3_DataSelectionDoubleClicked(object sender, DataSelectionEventArgs e)
        {
            this.GetDataEntry(e.DataSelection);
        }

        /// <summary>
        /// Updates the data Selection in the user control is a child in the child list becomes single clicked.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The standard event arguments containing the data selection.</param>
        private void DataAccess3_DataSelectionChanged(object sender, DataSelectionEventArgs e)
        {
            DataSelectionTextBox.Text = e.DataSelection;
        }

        /// <summary>
        /// Get the data entry, if a child in the child list becomes double clicked.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The standard event arguments containing the data selection.</param>
        private void DataAccess2_DataSelectionDoubleClicked(object sender, DataSelectionEventArgs e)
        {
            this.GetDataEntry(e.DataSelection);
        }

        /// <summary>
        /// Updates the data Selection in the user control is a child in the child list becomes single clicked.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The standard event arguments containing the data selection.</param>
        private void DataAccess2_DataSelectionChanged(object sender, DataSelectionEventArgs e)
        {
            DataSelectionTextBox.Text = e.DataSelection;
        }

        private void DataSelectionTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            if (this.dataAccess2 != null)
            {
                DataAccessTableLayoutPanel.Controls.Add(this.dataAccess2);
            }
            else
            {
                DataAccessTableLayoutPanel.Controls.Remove(this.dataAccess2);
            }

            if (this.dataAccess3 != null)
            {
                DataAccessTableLayoutPanel.Controls.Add(this.dataAccess3);
            }
            else
            {
                DataAccessTableLayoutPanel.Controls.Remove(this.dataAccess3);
            }

            if (this.dataAccess4 != null)
            {
                DataAccessTableLayoutPanel.Controls.Add(this.dataAccess4);
            }
            else
            {
                DataAccessTableLayoutPanel.Controls.Remove(this.dataAccess4);
            }

            if (this.infoPanel != null)
            {
                DataAccessTableLayoutPanel.Controls.Add(this.infoPanel);
            }
            else
            {
                DataAccessTableLayoutPanel.Controls.Remove(this.infoPanel);
            }
        }

        /// <summary>
        /// This method sets the JHDataAccess interface to an extended access mode or back to default user access. Use this method to gain access to protected data sub-trees.
        /// </summary>
        /// <param name="accessMode">The Access mode value.</param>
        /// <param name="password">String specifying the password needed for extended access.</param>
        private void SetAccessMode(string accessMode, string password)
        {
            try
            {
                HeidenhainDNCLib.DNC_ACCESS_MODE accessModeEnum = (HeidenhainDNCLib.DNC_ACCESS_MODE)Enum.Parse(typeof(HeidenhainDNCLib.DNC_ACCESS_MODE), accessMode);

                if (this.dataAccess2 != null)
                {
                    this.dataAccess2.SetAccessMode(accessModeEnum, password);
                }

                if (this.dataAccess3 != null)
                {
                    this.dataAccess3.SetAccessMode(accessModeEnum, password);
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
        /// Get data entry for all data access user controls, that are alive.
        /// </summary>
        /// <param name="dataSelection">Full path name of the required JHDataEntry object.</param>
        private void GetDataEntry(string dataSelection)
        {
            if (this.dataAccess2 != null)
            {
                this.dataAccess2.GetDataEntry(dataSelection);
            }

            if (this.dataAccess3 != null)
            {
                this.dataAccess3.GetDataEntry(dataSelection);
            }
        }
        #endregion
    }

    /// <summary>
    /// Delivers the standard event arguments and the data selection.
    /// </summary>
    public class DataSelectionEventArgs : EventArgs
    {
        #region "fields"
        /// <summary>
        /// Full path name of the required JHDataEntry object.
        /// </summary>
        private string dataSelection;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSelectionEventArgs"/> class.
        /// </summary>
        /// <param name="dataSelection">Full path name of the required JHDataEntry object.</param>
        public DataSelectionEventArgs(string dataSelection)
        {
            this.dataSelection = dataSelection;
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets the path name of the required JHDataEntry object.
        /// </summary>
        /// <value>Full path name of the required JHDataEntry object.</value>
        public string DataSelection
        {
            get
            {
                return this.dataSelection;
            }
        }
        #endregion
    }
}
