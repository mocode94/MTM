// ------------------------------------------------------------------------------------------------
// <copyright file="Automatic4.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHAutomatic4 interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHVersion interface.
    /// </summary>
    public partial class Automatic4 : UserControl
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
        /// Object of the HeidenhainDNC IJHVersion interface.
        /// </summary>
        private HeidenhainDNCLib.JHAutomatic automatic = null;

        /// <summary>
        /// Object of the HeidenhainDNC IJHFileSystem interface.
        /// </summary>
        private HeidenhainDNCLib.JHFileSystem fileSystem = null;

        /// <summary>
        /// Object of the HeidenhainDNC IJHDataAccess interface.
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
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// The name of the NC-Program used for importing the tool usage list by a csv file.
        /// This is just a demo file needed to explain the functionality.
        /// </summary>
        private string fileName = @"Plattformpedal";

        /// <summary>
        /// The working folder (for this example) on the tnc.
        /// </summary>
        private string folderOnTnc = @"TNC:\DNCDemo";

        /// <summary>
        /// The working folder on the PC where the files for this example are stored.
        /// </summary>
        private string folderOnPC = string.Empty;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Automatic4"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Automatic4(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Automatic4_Disposed);

            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
            this.cncType = parentForm.CncType;
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
                // --- get interfaces here ----------------------------------------------------------------
                this.automatic = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAUTOMATIC);
                this.fileSystem = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHFILESYSTEM);
                this.dataAccess = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHDATAACCESS);

                //// --- advise events here -----------------------------------------------------------------

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
        private void Automatic4_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            // ----- ImportToolUsage --------------------------------------------------------------
            FileNameLabel.Text = this.fileName;
            FolderOnTncLabel.Text = this.folderOnTnc;
            this.folderOnPC = Path.Combine(Application.StartupPath, @"DemoFiles");
            FolderOnPcLabel.Text = this.folderOnPC;
        }

        /// <summary>
        /// Prepare the tnc control for the import of the tool usage file. Then import the tool usage file.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ImportToolUsageButton_Click(object sender, EventArgs e)
        {
            // Warn user "changing the tool table on the control"
            DialogResult result = Utils.WorkingOnTheControlWarning();

            // Check if notification " Working in the tool table on control" is acknowledged
            if (result == DialogResult.No)
            {
                return;
            }

            // Change Label text according to the selection
            this.CommonProgressDiagram.ResetDiagram();
            this.CommonProgressDiagram.ChangeLabel(ProgressDiagram.FunctionType.ImportToolUsageCSV);
            Application.DoEvents();
            
            try
            {
                // ----- 1. Prepare working folder on the control ---------------------------------
                this.CommonProgressDiagram.StatusToolUsageDiagram(ProgressDiagram.Status.PreparingWorkingFolder);

                // Check if fould "DNCDemo" exists on the control
                if (this.FolderExist(this.folderOnTnc))
                {
                    // Delete file in the working folder if the control.
                    this.DeleteFile(Path.Combine(this.folderOnTnc, this.fileName + @".csv"));
                    this.DeleteFile(Path.Combine(this.folderOnTnc, this.fileName + @".h"));
                    this.DeleteFile(Path.Combine(this.folderOnTnc, this.fileName + @".h.t.dep"));
                }
                else
                {
                     // Create working folder on the control.
                     this.fileSystem.MakeDirectory(this.folderOnTnc);
                }

                // Copy files to the control
                this.fileSystem.TransmitFile(Path.Combine(this.folderOnPC, this.fileName + @".csv"), Path.Combine(this.folderOnTnc, this.fileName + @".csv"));
                this.fileSystem.TransmitFile(Path.Combine(this.folderOnPC, this.fileName + @".h"), Path.Combine(this.folderOnTnc, this.fileName + @".h"));

                this.CommonProgressDiagram.StatusToolUsageDiagram(ProgressDiagram.Status.WorkingFolder_OK);

                // ----- 2. Prepare tool data -----------------------------------------------------
                this.CommonProgressDiagram.StatusToolUsageDiagram(ProgressDiagram.Status.PreparingToolData);

                ToolTable toolTable = new ToolTable();
                toolTable.ReadToolTableFile(Path.Combine(this.folderOnPC, @"tool.t"));
                toolTable.TransmitToolsToControl(this.dataAccess);

                this.CommonProgressDiagram.StatusToolUsageDiagram(ProgressDiagram.Status.ToolData_OK);

                // ----- 3. Import tool data csv --------------------------------------------------
                this.CommonProgressDiagram.StatusToolUsageDiagram(ProgressDiagram.Status.ImportingToolUsage);

                string csvFileName = Path.Combine(this.folderOnTnc, this.fileName + @".csv");
                this.automatic.ImportToolUsageCSV(0, csvFileName);

                this.CommonProgressDiagram.StatusToolUsageDiagram(ProgressDiagram.Status.ImportToolUsage_OK);
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
        /// Clear the progress flow diagram.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ClearDiagramButton_Click(object sender, EventArgs e)
        {
            this.CommonProgressDiagram.ResetDiagram();
        }

        /// <summary>
        /// Fetch the *.t.dep file from the control, and show it in an external text editor.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ShowDepFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceFile = Path.Combine(this.folderOnTnc, this.fileName + @".h.t.dep");
                string targetFile = Path.Combine(Path.GetTempPath(), this.fileName + @".h.t.dep");

                this.fileSystem.ReceiveFile(sourceFile, targetFile);

                ProcessStartInfo psi = new ProcessStartInfo("notepad.exe", targetFile);
                Process.Start(psi);
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
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Automatic4_Disposed(object sender, EventArgs e)
        {
            if (this.automatic != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.automatic);
            }

            if (this.fileSystem != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.fileSystem);
            }

            if (this.dataAccess != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.dataAccess);
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
        #endregion

        #region "private methods"
        /// <summary>
        /// Checks if a file exists on the control.
        /// </summary>
        /// <param name="fullFileName">The full path of the file to check.</param>
        /// <returns>True if file exists.</returns>
        private bool FileExist(string fullFileName)
        {
            HeidenhainDNCLib.IJHDirectoryEntryList directoryEntryList = null;
            HeidenhainDNCLib.IJHDirectoryEntry direcotryEntry = null;
            HeidenhainDNCLib.JHFileAttributes attributeSelection = null;
            HeidenhainDNCLib.JHFileAttributes attributeState = null;

            try
            {
                string foulderName = System.IO.Path.GetDirectoryName(fullFileName);
                string fileName = System.IO.Path.GetFileName(fullFileName);

                attributeSelection = new HeidenhainDNCLib.JHFileAttributes();
                attributeState = new HeidenhainDNCLib.JHFileAttributes();

                directoryEntryList = this.fileSystem.ReadDirectory(foulderName, attributeSelection, attributeState);

                for (int i = 0; i < directoryEntryList.Count; i++)
                {
                    direcotryEntry = directoryEntryList[i];

                    if (!direcotryEntry.attributes.IsAttributeSet(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_DIR))
                    {
                        if (direcotryEntry.name == fileName)
                        {
                            return true;
                        }
                    }

                    Marshal.ReleaseComObject(direcotryEntry);
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
                if (directoryEntryList != null)
                {
                    Marshal.ReleaseComObject(directoryEntryList);
                }

                if (direcotryEntry != null)
                {
                    Marshal.ReleaseComObject(direcotryEntry);
                }

                if (attributeSelection != null)
                {
                    Marshal.ReleaseComObject(attributeSelection);
                }

                if (attributeState != null)
                {
                    Marshal.ReleaseComObject(attributeState);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a folder exists on the control.
        /// </summary>
        /// <param name="fullFolderName">The full path of the folder to check.</param>
        /// <returns>True if file exists.</returns>
        private bool FolderExist(string fullFolderName)
        {
            HeidenhainDNCLib.IJHDirectoryEntryList directoryEntryList = null;
            HeidenhainDNCLib.IJHDirectoryEntry direcotryEntry = null;
            HeidenhainDNCLib.JHFileAttributes attributeSelection = null;
            HeidenhainDNCLib.JHFileAttributes attributeState = null;

            try
            {
                string folderName = System.IO.Path.GetFileName(fullFolderName);
                string pathName = System.IO.Path.GetDirectoryName(fullFolderName);

                attributeSelection = new HeidenhainDNCLib.JHFileAttributes();
                attributeSelection.SetAttribute(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_DIR);
                attributeState = new HeidenhainDNCLib.JHFileAttributes();
                attributeState.SetAttribute(HeidenhainDNCLib.DNC_ATTRIBUTE_TYPE.DNC_ATTRIBUTE_DIR);

                directoryEntryList = this.fileSystem.ReadDirectory(pathName, attributeSelection, attributeState);

                for (int i = 0; i < directoryEntryList.Count; i++)
                {
                    direcotryEntry = directoryEntryList[i];

                    if (direcotryEntry.name == folderName)
                    {
                        return true;
                    }

                    Marshal.ReleaseComObject(direcotryEntry);
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
                if (directoryEntryList != null)
                {
                    Marshal.ReleaseComObject(directoryEntryList);
                }

                if (direcotryEntry != null)
                {
                    Marshal.ReleaseComObject(direcotryEntry);
                }

                if (attributeSelection != null)
                {
                    Marshal.ReleaseComObject(attributeSelection);
                }

                if (attributeState != null)
                {
                    Marshal.ReleaseComObject(attributeState);
                }
            }

            return false;
        }

        /// <summary>
        /// Delete the specified file on the control. Do nothing if the file does not exist.
        /// </summary>
        /// <param name="fullFileName">The full path of the file to delete.</param>
        private void DeleteFile(string fullFileName)
        {
            try
            {
                this.fileSystem.DeleteFile(fullFileName);
            }
            catch (COMException cex)
            {
                // Just to nothing if file does not exist.
                if (cex.ErrorCode != (int)HeidenhainDNCLib.DNC_HRESULT.DNC_E_FILE_NOT_FOUND)
                {
                    throw cex;
                }
            }
        }
        #endregion
    }
}
