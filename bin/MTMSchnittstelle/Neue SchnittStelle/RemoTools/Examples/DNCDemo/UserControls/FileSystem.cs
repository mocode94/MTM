// ------------------------------------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHFileSystem interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHFileSystem interface.
    /// </summary>
    public partial class FileSystem : UserControl
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
        private HeidenhainDNCLib.JHFileSystem fileSystem = null;

        /// <summary>
        /// Attributes to consider.
        /// </summary>
        private HeidenhainDNCLib.JHFileAttributes attributesSelection = null;

        /// <summary>
        /// Attributes that are set.
        /// </summary>
        private HeidenhainDNCLib.JHFileAttributes attributesState = null;

        /// <summary>
        /// Type of the connected CNC control.
        /// </summary>
        private HeidenhainDNCLib.DNC_CNC_TYPE cncType;

        /// <summary>
        /// Information if the connected CNC control NCK based.
        /// </summary>
        private bool isNck;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// Current path on control.
        /// </summary>
        private string currentPath = @"TNC:\";

        /// <summary>
        /// Directory entry list. Used to display the directory info in the list view control.
        /// </summary>
        private List<MyDirectoryEntry> directoryEntryList = new List<MyDirectoryEntry>();

        /// <summary>
        /// Selected item (in list view).
        /// </summary>
        private ListViewItem selectedItem = new ListViewItem();

        /// <summary>
        /// Stores last target folder. This is used to preset folder for next user of the FolderBrowserDialog.
        /// </summary>
        private string lastTargetFolder = string.Empty;

        /// <summary>
        /// Stores last source folder. This is used to preset folder for next user of the FolderBrowserDialog.
        /// </summary>
        private string lastSourceFolder = string.Empty;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public FileSystem(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.FileSystem_Disposed);

            this.machine = parentForm.Machine;
            this.cncType = parentForm.CncType;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets or sets a local field to the current path on control.
        /// In case of set, the text box PathTextBox becomes updated.
        /// </summary>
        private string CurrentPath
        {
            get
            {
                return this.currentPath;
            }

            set
            {
                PathTextBox.Text = value;
                this.currentPath = value;
            }
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
                this.fileSystem = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHFILESYSTEM);
                //// --- Subscribe for the event(s) -------------------------------------------------

                // --- Init File Attributes -------------------------------------------
                this.attributesSelection = new HeidenhainDNCLib.JHFileAttributes();
                this.attributesState = new HeidenhainDNCLib.JHFileAttributes();

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
        private void FileSystem_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            // init text boxes
            PathTextBox.Text = this.CurrentPath;
            string oemPassword = Properties.Settings.Default.oemPassword;
            PasswordTextBox.Text = oemPassword;

            // init combo boxes
            string[] accessModes = new string[2];
            accessModes[0] = HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_DEFAULT.ToString();
            accessModes[1] = HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_OEM.ToString();
            AccessModeComboBox.DataSource = accessModes;

            AttributeSelectionComboBox.DataSource = Enum.GetNames(typeof(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE));
            AttributeStateComboBox.DataSource = Enum.GetNames(typeof(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE));

            this.GetPath();
            this.UpdateGUI();
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void FileSystem_Disposed(object sender, EventArgs e)
        {
            // --- 1. unadvice all event handlers here ------------------------------------------------

            // --- 2. release interfaces here ---------------------------------------------------------
            if (this.fileSystem != null)
            {
                Marshal.ReleaseComObject(this.fileSystem);
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
      *
      *   This is important for COM events that pass COM objects as arguments, s.a. JHError::OnError2.
      *   If the argument was not explicitly released, it will hold a reference to the parent JHError object, thus preventing a successful Disconnect().
      *
      *   IMPORTANT:
      *       If the C# event abstraction is used instead of the raw approach (as in ErrorListener), a COM event connection is created for each event handler that is added.
      *       As a result, each COM event will be fired <n>-times, but only one will actually call the handler method.
      *       Since no handler code is executed for the other connections, the arguments cannot be explicitly released.
      *       That can only be done by the general garbage collector as shown here.
      */

      GC.Collect();
      GC.Collect();
      GC.WaitForPendingFinalizers();
#endif
        }

        /// <summary>
        /// Navigate one folder back and updates GUI.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.CurrentPath = this.NavigatePath(this.CurrentPath, "..");
            this.GetPath();
            this.UpdateGUI();
        }

        /// <summary>
        /// Set file system access mode from AccessModeComboBox with password from PasswordTextBox.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetAccessModeButton_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedItem = AccessModeComboBox.Text;
                HeidenhainDNCLib.DNC_ACCESS_MODE accessMode = (HeidenhainDNCLib.DNC_ACCESS_MODE)Enum.Parse(typeof(HeidenhainDNCLib.DNC_ACCESS_MODE), selectedItem);
                string password = PasswordTextBox.Text;

                this.fileSystem.SetAccessMode(accessMode, password);
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
            }
        }

        /// <summary>
        /// Set the in AttributeSelectionComboBox selected attribute selection.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetAttributeSelectionButton_Click(object sender, EventArgs e)
        {
            string typeString = Convert.ToString(AttributeSelectionComboBox.SelectedItem);
            HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE type = (HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE)Enum.Parse(typeof(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE), typeString);
            this.attributesSelection.SetAttribute(type);
        }

        /// <summary>
        /// Set the in AttributeStateComboBox selected attribute state.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetAttributeStateButton_Click(object sender, EventArgs e)
        {
            string typeString = Convert.ToString(AttributeStateComboBox.SelectedItem);
            HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE type = (HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE)Enum.Parse(typeof(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE), typeString);
            this.attributesState.SetAttribute(type);
        }

        /// <summary>
        /// Reset all selected attribute selections.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ResetAttribSelectionButton_Click(object sender, EventArgs e)
        {
            if (this.attributesSelection != null)
            {
                Marshal.ReleaseComObject(this.attributesSelection);
                this.attributesSelection = new HeidenhainDNCLib.JHFileAttributes();
            }
        }

        /// <summary>
        /// Reset all selected attribute states.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ResetAttribStateButton_Click(object sender, EventArgs e)
        {
            if (this.attributesState != null)
            {
                Marshal.ReleaseComObject(this.attributesState);
                this.attributesState = new HeidenhainDNCLib.JHFileAttributes();
            }
        }

        /// <summary>
        /// Get content of actual path and update GUI.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void GetPathButton_Click(object sender, EventArgs e)
        {
            this.GetPath();
            this.UpdateGUI();
        }

        /// <summary>
        /// Update local field of current path with the information on the text box "PathTextBox".
        /// Does not update the current patch on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The key arguments. Used to decode the enter key.</param>
        private void PathTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CurrentPath = PathTextBox.Text;
            }
        }

        /// <summary>
        /// Navigate to double clicked list view item, if it is a folder.
        /// </summary>
        /// <param name="sender">The list view control.</param>
        /// <param name="e">The mouse event arguments. Used to decode the left mouse button.</param>
        private void FolderListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListView listView = (ListView)sender;
                ListViewItem item = listView.GetItemAt(e.X, e.Y);
                ListViewItem.ListViewSubItem subItem = item.SubItems["Dir"];
                bool isDir = Convert.ToBoolean(subItem.Text);
                if (isDir)
                {
                    string navigationOffset = item.Text;
                    this.CurrentPath = this.NavigatePath(this.CurrentPath, navigationOffset);
                    this.GetPath();
                    this.UpdateGUI();
                }
            }
        }

        /// <summary>
        /// Open context menu on right clicking a item in list view.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The mouse event arguments. Used to decode the right mouse button.</param>
        private void FolderListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (FolderListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    // Show context menu if right mouse button was pressed
                    FolderViewContextMenuStrip.Show(Cursor.Position);
                    this.selectedItem = FolderListView.FocusedItem;
                }
            }
        }

        /// <summary>
        /// Deletes selected item.
        /// If the selected item is a folder, it can only get deleted if it is empty.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string itemString = this.selectedItem.Text;
                ListViewItem.ListViewSubItem subItem = this.selectedItem.SubItems["Dir"];
                bool isDir = Convert.ToBoolean(subItem.Text);
                string fullPath = this.NavigatePath(this.CurrentPath, itemString);

                if (isDir)
                {
                    this.fileSystem.DeleteDirectory(fullPath);
                }
                else
                {
                    this.fileSystem.DeleteFile(fullPath);
                }

                this.GetPath();
                this.UpdateGUI();
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
            }
        }

        /// <summary>
        /// Receive file from control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string itemString = this.selectedItem.Text;
                ListViewItem.ListViewSubItem subItem = this.selectedItem.SubItems["Dir"];
                bool isDir = Convert.ToBoolean(subItem.Text);
                string fullPath = this.NavigatePath(this.CurrentPath, itemString);

                if (!isDir)
                {
                    FolderBrowserDialog targetFolderDialog = new FolderBrowserDialog();
                    targetFolderDialog.SelectedPath = this.lastTargetFolder;
                    DialogResult dialogResult = targetFolderDialog.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        this.lastTargetFolder = targetFolderDialog.SelectedPath;
                        this.fileSystem.ReceiveFile(fullPath, this.NavigatePath(this.lastTargetFolder, itemString));
                    }
                }
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
            }
        }

        /// <summary>
        /// Transmit file to control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TransmitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string pathString = PathTextBox.Text;

                OpenFileDialog sourceFileDialog = new OpenFileDialog();
                sourceFileDialog.Multiselect = false;
                sourceFileDialog.CheckFileExists = true;
                sourceFileDialog.InitialDirectory = this.lastSourceFolder;
                DialogResult dialogResult = sourceFileDialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string selectedFileWithPath = sourceFileDialog.FileName;
                    string selectedFile = Path.GetFileName(selectedFileWithPath);
                    this.lastSourceFolder = Path.GetDirectoryName(selectedFileWithPath);
                    this.fileSystem.TransmitFile(selectedFileWithPath, this.NavigatePath(pathString, selectedFile));

                    this.GetPath();
                    this.UpdateGUI();
                }
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
            }
        }

        /// <summary>
        /// Copy selected path (file or folder) to clipboard.
        /// You can use this to pick files and select it in the automatic section IJHAutomatic3::SelectProgram().
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CopyPathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string itemString = this.selectedItem.Text;
            ListViewItem.ListViewSubItem subItem = this.selectedItem.SubItems["Dir"];
            bool isDir = Convert.ToBoolean(subItem.Text);
            string fullPath = this.NavigatePath(this.CurrentPath, itemString);

            // After this call, the data (string) is placed on the clipboard and tagged
            // with a data format of "Text".
            Clipboard.SetData(DataFormats.Text, (object)fullPath);
        }

        #endregion

        #region "private methods"
        /// <summary>
        /// Updates the GUI with the local data representations.
        /// </summary>
        private void UpdateGUI()
        {
            this.GetDiskSpace();

            try
            {
                // folder view
                FolderListView.BeginUpdate();
                FolderListView.Items.Clear();
                foreach (MyDirectoryEntry entry in this.directoryEntryList)
                {
                    ListViewItem item = new ListViewItem(entry.Name);

                    // FileSize
                    ListViewItem.ListViewSubItem subItemFileSize = new ListViewItem.ListViewSubItem();
                    subItemFileSize.Text = Convert.ToString(entry.FileSize);
                    subItemFileSize.Name = "FileSize";
                    item.SubItems.Add(subItemFileSize);

                    // TimeStamp
                    ListViewItem.ListViewSubItem subItemTimeStamp = new ListViewItem.ListViewSubItem();
                    subItemTimeStamp.Text = Convert.ToString(entry.TimeStamp);
                    subItemTimeStamp.Name = "TimeStamp";
                    item.SubItems.Add(subItemTimeStamp);

                    // ReadOnly
                    ListViewItem.ListViewSubItem subItemReadOnly = new ListViewItem.ListViewSubItem();
                    subItemReadOnly.Text = Convert.ToString(entry.ReadOnly);
                    subItemReadOnly.Name = "ReadOnly";
                    item.SubItems.Add(subItemReadOnly);

                    // Hidden
                    ListViewItem.ListViewSubItem subItemHidden = new ListViewItem.ListViewSubItem();
                    subItemHidden.Text = Convert.ToString(entry.Hidden);
                    subItemHidden.Name = "Hidden";
                    item.SubItems.Add(subItemHidden);

                    // Dir
                    ListViewItem.ListViewSubItem subItemDir = new ListViewItem.ListViewSubItem();
                    subItemDir.Text = Convert.ToString(entry.Dir);
                    subItemDir.Name = "Dir";
                    item.SubItems.Add(subItemDir);

                    // System
                    ListViewItem.ListViewSubItem subItemSystem = new ListViewItem.ListViewSubItem();
                    subItemSystem.Text = Convert.ToString(entry.System);
                    subItemSystem.Name = "System";
                    item.SubItems.Add(subItemSystem);

                    // Modified
                    ListViewItem.ListViewSubItem subItemModified = new ListViewItem.ListViewSubItem();
                    subItemModified.Text = Convert.ToString(entry.Modified);
                    subItemModified.Name = "Modified";
                    item.SubItems.Add(subItemModified);

                    // Locked
                    ListViewItem.ListViewSubItem subItemLocked = new ListViewItem.ListViewSubItem();
                    subItemLocked.Text = Convert.ToString(entry.Locked);
                    subItemLocked.Name = "Locked";
                    item.SubItems.Add(subItemLocked);

                    FolderListView.Items.Add(item);
                }

                FolderListView.EndUpdate();
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().ToString();
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                MessageBox.Show(ex.Message, this.Name + " --> " + strMethodName);
            }
        }

        /// <summary>
        /// Gets size of disk space.
        /// On programming stations only the size which is visible there (VBox image size).
        /// </summary>
        private void GetDiskSpace()
        {
            try
            {
                object freeSpace = null;
                object totalSize = null;

                // Get disk space
                this.fileSystem.GetDiskSpace(".", ref freeSpace, ref totalSize);
                double freeSpaceValue = Convert.ToDouble(freeSpace);
                double totalSizeValue = Convert.ToDouble(totalSize);

                // prepare string
                FreeSpaceLabel.Text = string.Format("{0:n}", freeSpaceValue / 1048576) + " MB";
                TotalSizeLabel.Text = string.Format("{0:n}", totalSizeValue / 1048576) + " MB";
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
            }
        }

        /// <summary>
        /// Get directory content filtered by attribute selection and attribute state.
        /// </summary>
        private void GetPath()
        {
            HeidenhainDNCLib.IJHDirectoryEntryList directoryEntryList = null;
            HeidenhainDNCLib.IJHDirectoryEntry directoryEntry = null;
            HeidenhainDNCLib.JHFileAttributes fileAttributes = null;
            try
            {
                this.CurrentPath = PathTextBox.Text;
                this.directoryEntryList.Clear();

                directoryEntryList = this.fileSystem.ReadDirectory(this.CurrentPath, this.attributesSelection, this.attributesState);
                int dirCount = directoryEntryList.Count;

                for (int i = 0; i < dirCount; i++)
                {
                    directoryEntry = directoryEntryList[i];

                    // get entry informations
                    MyDirectoryEntry entry = new MyDirectoryEntry();
                    entry.Name = directoryEntry.name;
                    entry.TimeStamp = directoryEntry.dateTime;
                    entry.FileSize = directoryEntry.size;

                    // get file attributes
                    fileAttributes = directoryEntry.attributes;
                    entry.Dir = fileAttributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_DIR);
                    entry.Hidden = fileAttributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_HIDDEN);
                    entry.Locked = fileAttributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_LOCKED);
                    entry.Modified = fileAttributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_MODIFIED);
                    entry.ReadOnly = fileAttributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_READONLY);
                    entry.System = fileAttributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_SYSTEM);

                    // add to list
                    this.directoryEntryList.Add(entry);

                    if (directoryEntry != null)
                    {
                        Marshal.ReleaseComObject(directoryEntry);
                    }

                    if (fileAttributes != null)
                    {
                        Marshal.ReleaseComObject(fileAttributes);
                    }
                }
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
            }
            finally
            {
                if (fileAttributes != null)
                {
                    Marshal.ReleaseComObject(fileAttributes);
                }

                if (directoryEntry != null)
                {
                    Marshal.ReleaseComObject(directoryEntry);
                }

                if (directoryEntryList != null)
                {
                    Marshal.ReleaseComObject(directoryEntryList);
                }
            }
        }

        /// <summary>
        /// Navigate path string to a specific position.
        /// </summary>
        /// <param name="part1">Actual path string.</param>
        /// <param name="part2">Navigation offset.</param>
        /// <returns>New path string.</returns>
        private string NavigatePath(string part1, string part2)
        {
            string fullPath = part1;
            switch (part2)
            {
                case ".":
                    break;
                case "..":
                    if (part1.EndsWith(@"\") && part1.Length > 5)
                    {
                        part1 = part1.Substring(0, part1.Length - 3);
                    }

                    int lastFolderPos = part1.LastIndexOf(@"\");
                    if (lastFolderPos >= 0)
                    {
                        fullPath = part1.Substring(0, lastFolderPos + 1);
                    }

                    break;
                default:
                    if (part1.EndsWith(@"\"))
                    {
                        fullPath = part1 + part2;
                    }
                    else
                    {
                        fullPath = part1 + @"\" + part2;
                    }

                    break;
            }

            return fullPath;
        }
        #endregion

        /// <summary>
        /// Used to update list view control on GUI.
        /// </summary>
        private class MyDirectoryEntry
        {
            //// --- common properties-------------------------------------------------------------

            /// <summary>
            /// Gets or sets the file name.
            /// </summary>
            public string Name { get; set; }
            
            /// <summary>
            /// Gets or sets the last changed time stamp property.
            /// </summary>
            public DateTime TimeStamp { get; set; }
            
            /// <summary>
            /// Gets or sets the file size property.
            /// </summary>
            public double FileSize { get; set; }

            //// --- directory entry attributes----------------------------------------------------

            /// <summary>
            /// Gets or sets a value indicating whether the directory entry is read only or not.
            /// </summary>
            public bool ReadOnly { get; set; }
            
            /// <summary>
            /// Gets or sets a value indicating whether the directory entry is hidden or not.
            /// </summary>
            public bool Hidden { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the directory entry is a directory or a folder or not.
            /// </summary>
            public bool Dir { get; set; }
            
            /// <summary>
            /// Gets or sets a value indicating whether the directory entry is system type or not.
            /// </summary>
            public bool System { get; set; }
            
            /// <summary>
            /// Gets or sets a value indicating whether the directory entry is modified or not.
            /// </summary>
            public bool Modified { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the directory entry is locked or not.
            /// </summary>
            public bool Locked { get; set; }
        }
    }
}
