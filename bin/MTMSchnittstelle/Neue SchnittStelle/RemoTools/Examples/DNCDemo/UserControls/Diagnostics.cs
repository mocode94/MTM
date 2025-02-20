// ------------------------------------------------------------------------------------------------
// <copyright file="Diagnostics.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the user’s own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>24.11.2016</date>
// <summary>
// This file contains a class which shows how to use the IJHDiagnostics interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHDiagnostic interface.
    /// </summary>
    public partial class Diagnostics : UserControl
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
        /// Object of HeidenhainDNC IJHDiagnostic interface.
        /// </summary>
        private HeidenhainDNCLib.JHDiagnostics diagnostics = null;
        
        /// <summary>
        /// Object of HeidenhainDNC IJHFileSystem interface.
        /// </summary>
        private HeidenhainDNCLib.JHFileSystem fileSystem = null;

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
        /// List of full path of all temporary local stored screen shot bitmaps.
        /// </summary>
        private List<string> screenShotListLocal = new List<string>();

        /// <summary>
        /// List of full path of all temporary on tnc stored screen shot bitmaps.
        /// </summary>
        private List<string> screenShotListTnc = new List<string>();

        /// <summary>
        /// Stores the file name of the last created service file.
        /// </summary>
        private string serviceFileName = string.Empty;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Diagnostics"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Diagnostics(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Diagnostic_Disposed);

            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
            this.cncType = parentForm.CncType;
        }
        #endregion

        #region "delegate"
        /// <summary>
        /// Service file is created by the server and can be retrieved.
        /// </summary>
        /// <param name="result">The HRESULT value of the service file creation.</param>
        internal delegate void OnServiceFileCreatedHandler(int result);
        #endregion

        #region "enumerations"
        /// <summary>
        /// All possible logging types.
        /// </summary>
        private enum AvailableLogLevel
        {
            /// <summary>
            /// Only the events of the base configuration are logged.
            /// </summary>
            BASIC,

            /// <summary>
            /// All events are logged.
            /// </summary>
            ALL,

            /// <summary>
            /// The behavior must be configured individually for each event category.
            /// </summary>
            USERDEFINED
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
                this.diagnostics = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHDIAGNOSTICS);
                this.diagnostics.SetAccessMode(HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_USR, string.Empty);
                
                this.fileSystem = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHFILESYSTEM);

                //// --- advise events here -----------------------------------------------------------------
                this.diagnostics.OnServiceFileCreated += new HeidenhainDNCLib._DJHDiagnosticsEvents_OnServiceFileCreatedEventHandler(this.Diagnostics_OnServiceFileCreated);

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
        private void Diagnostics_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Create service file in folder TNC:\service\.
        /// The name is created automatically: DNC + date + time + .zip.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StartCreateServiceFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                ReceiveServieFileButton.Enabled = false;
                string serviceFile = @"\USR\service\ServiceDNC_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".zip";
                ServiceFileNameTextBox.Text = serviceFile;
                this.diagnostics.StartCreateServiceFile(serviceFile);
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
        /// Receive service file from the tnc control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ReceiveServieFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ServiceFileNameTextBox.Text))
                {
                    MessageBox.Show("No service file was created!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileName = Path.GetFileName(ServiceFileNameTextBox.Text);
                string fullPath = string.Empty;

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.InitialDirectory = Path.GetTempPath();
                fileDialog.Filter = "Service Files (*.zip)|*.zip|All files (*.*)|*.*";
                fileDialog.FileName = fileName;
                fileDialog.Title = "Select target of service file.";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    fullPath = Path.GetFullPath(fileDialog.FileName);
                    this.fileSystem.ReceiveFile(ServiceFileNameTextBox.Text, fullPath);
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
        }

        /// <summary>
        /// Make a screen shot from the control, receive it and show a preview on the gui.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void MakeScreenShotButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create screen shot file
                string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                string time = DateTime.Now.ToLongTimeString().Replace(":", string.Empty);

                string fileName = @"TNC:\service\DNCScreenShot" + "-" + date + "-" + time + ".BMP";
                this.diagnostics.MakeScreenShot(fileName);
                this.screenShotListTnc.Add(fileName);

                // Show file name in a text box
                ScreenShotFileNameTextBox.Text = fileName;

                // Receive screen shot from control an store it to the temp folder.
                string tmpFile = Path.GetTempPath() + Path.GetFileName(fileName);
                this.fileSystem.ReceiveFile(fileName, tmpFile);
                this.screenShotListLocal.Add(tmpFile);

                // Show preview of the screen shot.
                PreviewScreenShotPictureBox.Image = null;
                FileStream imageStream = new FileStream(tmpFile, FileMode.Open, FileAccess.Read);
                PreviewScreenShotPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                PreviewScreenShotPictureBox.Image = System.Drawing.Image.FromStream(imageStream);
                imageStream.Close();
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
        /// Gets thrown if a service file has been created on the tnc control.
        /// </summary>
        /// <param name="result">HRESULT value.</param>
        private void Diagnostics_OnServiceFileCreated(int result)
        {
            this.Invoke(new OnServiceFileCreatedHandler(this.OnServiceFileCreatedImpl), result);
        }

        /// <summary>
        /// Activate the receive service file button after the service file has been created.
        /// </summary>
        /// <param name="result">The HRESULT value of the create service file function.</param>
        private void OnServiceFileCreatedImpl(int result)
        {
            if (result == 0)
            {
                ReceiveServieFileButton.Enabled = true;
            }
            else
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(result, className, methodName);
            }
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Diagnostic_Disposed(object sender, EventArgs e)
        {
            // Delete temorary files from tnc.
            foreach (string file in this.screenShotListTnc)
            {
                try
                {
                    this.fileSystem.DeleteFile(file);
                }
                catch (Exception ex)
                {
                    // User has probably deleted the file himself.
                    Debug.WriteLine("Could not delete temp screen shot file: " + ex.Message);
                }
            }

            // Delete temporary files from local temp path.
            foreach (string file in this.screenShotListLocal)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    // User has probably deleted the file himself.
                    Debug.WriteLine("Could not delete temp screen shot file: " + ex.Message);
                }
            }

            if (this.diagnostics != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------
                this.diagnostics.OnServiceFileCreated -= new HeidenhainDNCLib._DJHDiagnosticsEvents_OnServiceFileCreatedEventHandler(this.Diagnostics_OnServiceFileCreated);

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.diagnostics);
            }

            if (this.fileSystem != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
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
      */
      GC.Collect();
      GC.Collect();
      GC.WaitForPendingFinalizers();
#endif
        }
        #endregion
    }
}