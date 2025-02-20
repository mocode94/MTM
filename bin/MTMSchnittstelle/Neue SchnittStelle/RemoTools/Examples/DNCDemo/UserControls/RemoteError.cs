// ------------------------------------------------------------------------------------------------
// <copyright file="RemoteError.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHRemoteError interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHRemoteError interface.
    /// </summary>
    public partial class RemoteError : UserControl
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
        /// Object of HeidenhainDNC IJHVersion interface.
        /// </summary>
        private HeidenhainDNCLib.JHRemoteError remoteError = null;

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
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteError"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public RemoteError(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.RemoteError_Disposed);

            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
            this.cncType = parentForm.CncType;
        }
        #endregion

        #region "delegate"
        /// <summary>
        /// The server request the clearance of a remote error entry, previously raised by this client.
        /// </summary>
        /// <param name="remoteErrorEntry">The DNC remote error entry to remove.</param>
        internal delegate void OnRemoveRequestHandler(HeidenhainDNCLib.IJHRemoteErrorEntry remoteErrorEntry);
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
                this.remoteError = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHREMOTEERROR);

                //// --- advise events here -----------------------------------------------------------------
                this.remoteError.OnRemoveRequest += new HeidenhainDNCLib._DJHRemoteErrorEvents_OnRemoveRequestEventHandler(this.RemoteError_OnRemoveRequest);

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
        private void RemoteError_Load(object sender, EventArgs e)
        {
            List<string> validErrorClass = new List<string>(Enum.GetNames(typeof(HeidenhainDNCLib.DNC_ERROR_CLASS)));
            validErrorClass.Remove(HeidenhainDNCLib.DNC_ERROR_CLASS.DNC_EC_NONE.ToString());
            ErrorClassComboBox.DataSource = validErrorClass;

            if (!this.initOkay)
            {
                return;
            }
        }

        /// <summary>
        /// The server request the clearance of a remote error entry, previously raised by this client.
        /// </summary>
        /// <param name="remoteErrorEntry">The DNC remote error entry to remove.</param>
        private void RemoteError_OnRemoveRequest(HeidenhainDNCLib.JHRemoteErrorEntry remoteErrorEntry)
        {
            Debug.WriteLine(string.Format(@"RemoteError::OnRemoveRequest - ErrorNumber: {0}",  Convert.ToString(remoteErrorEntry.Number)));
            this.Invoke(new OnRemoveRequestHandler(this.OnRemoveRequestImpl), remoteErrorEntry);
        }

        /// <summary>
        /// Resizes all the remote error entries in the flow layout panel list if size has changed.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoteErrorEntryFlowLayoutPanel_Layout(object sender, LayoutEventArgs e)
        {
            int width = RemoteErrorEntryFlowLayoutPanel.Width;
            foreach (Control item in RemoteErrorEntryFlowLayoutPanel.Controls)
            {
                item.Width = width - 6;
            }
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoteError_Disposed(object sender, EventArgs e)
        {
            if (this.remoteError != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------
                this.remoteError.OnRemoveRequest -= new HeidenhainDNCLib._DJHRemoteErrorEvents_OnRemoveRequestEventHandler(this.RemoteError_OnRemoveRequest);

                // --- 3. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.remoteError);
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

        private void ErrorNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            CreateErrorEntryButton.Enabled = !string.IsNullOrWhiteSpace(ErrorNumberTextBox.Text) && !string.IsNullOrWhiteSpace(ErrorMessageTextBox.Text);
        }

        private void ErrorMessageTextBox_TextChanged(object sender, EventArgs e)
        {
            CreateErrorEntryButton.Enabled = !string.IsNullOrWhiteSpace(ErrorNumberTextBox.Text) && !string.IsNullOrWhiteSpace(ErrorMessageTextBox.Text);
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// The server request to clearance of a remote error entry, previously raised by this client.
        /// </summary>
        /// <param name="remoteErrorEntry">The JHRemoteErrorEntry object describes a new CNC error. This helper object is used by JHRemoteError.</param>
        private void OnRemoveRequestImpl(HeidenhainDNCLib.IJHRemoteErrorEntry remoteErrorEntry)
        {
            // On remove request -> Move the error entry to the top of the error entry list.
            RemoteErrorEntry item = this.GetErrorByNumber(remoteErrorEntry.Number);
            if (item != null)
            {
                item.RequestToRemoveError();
                RemoteErrorEntryFlowLayoutPanel.Controls.SetChildIndex(item, 0);
            }
            else
            {
                MessageBox.Show("Error number not found in the list!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Marshal.ReleaseComObject(remoteErrorEntry);
        }

        /// <summary>
        /// Create error entry and add it to the list in the gui. Does not raise the error.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CreateErrorEntryButton_Click(object sender, EventArgs e)
        {
            try
            {
                int errorNumber = Convert.ToInt32(ErrorNumberTextBox.Text);

                // Only create error entry if error number doesn't exist.
                if (this.GetErrorByNumber(errorNumber) == null)
                {
                    HeidenhainDNCLib.DNC_ERROR_CLASS errorClass = (HeidenhainDNCLib.DNC_ERROR_CLASS)Enum.Parse(typeof(HeidenhainDNCLib.DNC_ERROR_CLASS), ErrorClassComboBox.Text);
                    string errorString = ErrorMessageTextBox.Text;
                    bool withRemoteAck = RemoveAckCheckBox.Checked;

                    HeidenhainDNCLib.IJHRemoteErrorEntry tmpEntry = null;
                    tmpEntry = this.remoteError.CreateEntry(errorNumber, errorClass, errorString);
                    tmpEntry.Cause = CauseTextBox.Text;
                    tmpEntry.Action = ActionTextBox.Text;

                    RemoteErrorEntry tmpRemoteErrorEntry = new RemoteErrorEntry(tmpEntry, withRemoteAck);
                    tmpRemoteErrorEntry.Name = Convert.ToString(errorNumber);
                    RemoteErrorEntryFlowLayoutPanel.Controls.Add(tmpRemoteErrorEntry);
                    RemoteErrorEntryFlowLayoutPanel.Refresh();
                }
                else
                {
                    MessageBox.Show("A remote error entry with this number already exists!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Find fist occurrence of the error entry with the specified error number.
        /// </summary>
        /// <param name="errorNumber">The error number of the error entry to find.</param>
        /// <returns>The fist match of error entry with the specified number.</returns>
        private RemoteErrorEntry GetErrorByNumber(int errorNumber)
        {
            RemoteErrorEntry firstFoundControl = null;
            string errorNumberString = Convert.ToString(errorNumber);

            Control[] items = RemoteErrorEntryFlowLayoutPanel.Controls.Find(errorNumberString, false);
            if (items.Length > 0)
            {
                firstFoundControl = (RemoteErrorEntry)items[0];
            }

            return firstFoundControl;
        }

        /// <summary>
        /// Disable ACK: CheckBox if error class DNC_EC_NOTE is selected
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ErrorClassComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HeidenhainDNCLib.DNC_ERROR_CLASS errorClass = (HeidenhainDNCLib.DNC_ERROR_CLASS)Enum.Parse(typeof(HeidenhainDNCLib.DNC_ERROR_CLASS), ErrorClassComboBox.Text);
            RemoveAckCheckBox.Checked = (HeidenhainDNCLib.DNC_ERROR_CLASS.DNC_EC_NOTE == errorClass) ? false : RemoveAckCheckBox.Checked;
            RemoveAckCheckBox.Enabled = (HeidenhainDNCLib.DNC_ERROR_CLASS.DNC_EC_NOTE == errorClass) ? false : true;
        }
        #endregion
    }
}