// ------------------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHError interface.
// </summary>
// ------------------------------------------------------------------------------------------------

/* 
 * Define the type of event mechanism you like to use:
 * ---------------------------------------------------
 * 
 * #define ERROR_V1 or #define ERROR_V2:
 * -------------------------------------
 * Hook up the event handlers to each required COM event, using the C# event abstraction approach.
 * (As opposed to the raw approach (as in ErrorListener))
 * IMPORTANT:
 *    A COM event connection is created for each event handler that is added.
 *    As a result, each COM event will be fired <n>-times, but only one will actually call the handler method.
 * 
 * #define RAW_COM_EVENTS:
 * -----------------------
 *  Hook up the event handlers to the COM interface, using the more efficient raw approach.
 *  Pro:
 *    - each COM event is fired only once
 *  Con:
 *    - a handler must be defined for each COM event of the given COM interface
 */
#define ERROR_V1

#if ((ERROR_V1 && ERROR_V2) || (ERROR_V1 && RAW_COM_EVENTS) || (ERROR_V2 && RAW_COM_EVENTS) || (!ERROR_V1 && !ERROR_V2 && !RAW_COM_EVENTS))
#error The type of event processing must be selected uniquely
#endif

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHVersion interface.
    /// </summary>
    public partial class Error : UserControl
    {
        #region "const"
        /// <summary>
        /// The column index of the channel in the ErrorListView.
        /// </summary>
        private const int CHANNELCOLUMNINDEX = 0;

        /// <summary>
        /// The column index of the text in the ErrorListView.
        /// </summary>
        private const int TEXTCOLUMNINDEX = 1;

        /// <summary>
        /// The column index of the gone time in the ErrorListView.
        /// </summary>
        private const int GONECOLUMNINDEX = 6;
        #endregion

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
        /// Object of HeidenhainDNC IJHError interface.
        /// </summary>
        private HeidenhainDNCLib.JHError error = null;

        /// <summary>
        /// Type of the connected CNC control.
        /// </summary>
        private HeidenhainDNCLib.DNC_CNC_TYPE cncType;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// The GUI list of all shown errors.
        /// </summary>
        private ListView.ListViewItemCollection errorList = null;

#if RAW_COM_EVENTS
        /// <summary>
        /// A helper class to listen to the VTable event implementation.
        /// </summary>
        private ErrorListener errorListener = null;
#endif
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Error(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Error_Disposed);
            this.errorList = new ListView.ListViewItemCollection(ErrorListView);

            this.machine = parentForm.Machine;
            this.cncType = parentForm.CncType;
        }
        #endregion

        #region "delegate"
        /// <summary>
        /// Delegate OnError to pass event handler to GUI thread.
        /// </summary>
        /// <param name="errorGroup">The group of the error.</param>
        /// <param name="lErrorNumber">The number of the error.</param>
        /// <param name="errorClass">The class of the error.</param>
        /// <param name="bstrError">The error short text.</param>
        /// <param name="lChannel">The error channel.</param>
        internal delegate void OnErrorHandler(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int lErrorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string bstrError, int lChannel);

        /// <summary>
        /// Delegate OnErrorCleared to pass event handler to GUI thread.
        /// </summary>
        /// <param name="lErrorNumber">The number of the error.</param>
        /// <param name="lChannel">The channel of the error.</param>
        internal delegate void OnErrorClearedHandler(int lErrorNumber, int lChannel);

        /// <summary>
        /// Delegate OnError2 to pass event handler to GUI thread.
        /// </summary>
        /// <param name="pErrorEntry">Helper Object of IJHError2 interface.</param>
        internal delegate void OnErrorHandler2(HeidenhainDNCLib.IJHErrorEntry2 pErrorEntry);

        /// <summary>
        /// Delegate OnErrorCleared2 to pass event handler to GUI thread.
        /// </summary>
        /// <param name="pErrorEntry">Helper Object of IJHError2 interface.</param>
        internal delegate void OnErrorClearedHandler2(HeidenhainDNCLib.IJHErrorEntry2 pErrorEntry);
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
                this.error = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHERROR);

                // --- advise events here -----------------------------------------------------------------
#if RAW_COM_EVENTS
                this.errorListener = new ErrorListener(this, this.error);
#endif
#if (ERROR_V1)
                this.error.OnError += new HeidenhainDNCLib._DJHErrorEvents_OnErrorEventHandler(this.OnError);
                this.error.OnErrorCleared += new HeidenhainDNCLib._DJHErrorEvents_OnErrorClearedEventHandler(this.OnErrorCleared);
#endif
#if (ERROR_V2)
                this.error.OnError2 += new HeidenhainDNCLib._DJHErrorEvents_OnError2EventHandler(this.OnError2);
                this.error.OnErrorCleared2 += new HeidenhainDNCLib._DJHErrorEvents_OnErrorCleared2EventHandler(this.OnErrorCleared2);
#endif

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
        /* COM callback methods:
       *   These callbacks will be executed in the context of the event firing thread of the COM object.
       *   Using the Invoke() or BeginInvoke() method the execution is passed to the application thread.
       *   See On<event>Impl() methods
       */
#if ERROR_V1
        /// <summary>
        /// Event from _DJHErrorEvents::OnError.
        /// </summary>
        /// <param name="errorGroup">The group of the error.</param>
        /// <param name="errorNumber">The number of the error.</param>
        /// <param name="errorClass">The class of the error.</param>
        /// <param name="text">The error short text.</param>
        /// <param name="channel">The error channel.</param>
        internal void OnError(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int errorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string text, int channel)
        {
            Debug.WriteLine("Error::OnError");
            this.BeginInvoke(new OnErrorHandler(this.OnErrorImpl), errorGroup, errorNumber, errorClass, text, channel);
        }

        /// <summary>
        /// Event from _DJHErrorEvents::OnErrorCleared.
        /// </summary>
        /// <param name="errorNumber">The number of the error.</param>
        /// <param name="channel">The channel of the error.</param>
        internal void OnErrorCleared(int errorNumber, int channel)
        {
            Debug.WriteLine("Error::OnErrorCleared");
            this.BeginInvoke(new OnErrorClearedHandler(this.OnErrorClearedImpl), errorNumber, channel);
        }
#endif

#if ERROR_V2
        /// <summary>
        /// Event from _DJHErrorEvents_OnError2EventHandler.
        /// </summary>
        /// <param name="errorEntry">Helper Object of IJHError2 interface.</param>
        internal void OnError2(HeidenhainDNCLib.IJHErrorEntry2 errorEntry)
        {
            Debug.WriteLine("Error::OnError2");
            this.BeginInvoke(new OnErrorHandler2(this.OnError2Impl), errorEntry);
        }

        /// <summary>
        /// Event from _DJHErrorEvents_OnError2ClearedEventHandler.
        /// </summary>
        /// <param name="errorEntry">Helper Object of IJHError2 interface.</param>
        internal void OnErrorCleared2(HeidenhainDNCLib.IJHErrorEntry2 errorEntry)
        {
            Debug.WriteLine("Error::OnErrorCleared2");
            this.BeginInvoke(new OnErrorClearedHandler2(this.OnErrorCleared2Impl), errorEntry);
        }
#endif

        /// <summary>
        /// Implementation for OnErrorHandler.
        /// </summary>
        /// <param name="errorGroup">The group of the error.</param>
        /// <param name="errorNumber">The number of the error.</param>
        /// <param name="errorClass">The class of the error.</param>
        /// <param name="text">The error short text.</param>
        /// <param name="channel">The error channel.</param>
        private void OnErrorImpl(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int errorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string text, int channel)
        {
            this.ShowError(errorGroup, errorNumber, errorClass, text, channel);
        }

        /// <summary>
        /// Implementation for OnErrorClearedHandler.
        /// </summary>
        /// <param name="errorNumber">The number of the error.</param>
        /// <param name="channel">The channel of the error.</param>
        private void OnErrorClearedImpl(int errorNumber, int channel)
        {
            this.ShowErrorCleared(errorNumber, channel);
        }

        /// <summary>
        /// Implementation for OnError2Handler.
        /// </summary>
        /// <param name="errorEntry">Helper Object of IJHError2 interface.</param>
        private void OnError2Impl(HeidenhainDNCLib.IJHErrorEntry2 errorEntry)
        {
            HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup = errorEntry.Group;
            int errorNumber = errorEntry.Number;
            HeidenhainDNCLib.DNC_ERROR_CLASS errorClass = errorEntry.Class;
            string errorText = errorEntry.Text;
            int errorChannel = errorEntry.Channel;
            int errorHandle = errorEntry.Handle;

            this.ShowError(errorGroup, errorNumber, errorClass, errorText, errorChannel, errorHandle);

            Marshal.ReleaseComObject(errorEntry);
        }

        /// <summary>
        /// Implementation for OnErrorCleared2Handler.
        /// </summary>
        /// <param name="errorEntry">Helper Object of IJHError2 interface.</param>
        private void OnErrorCleared2Impl(HeidenhainDNCLib.IJHErrorEntry2 errorEntry)
        {
            int errorNumber = errorEntry.Number;
            int errorChannel = errorEntry.Channel;
            int errorHandle = errorEntry.Handle;

            this.ShowErrorCleared(errorNumber, errorChannel, errorHandle);

            Marshal.ReleaseComObject(errorEntry);
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Error_Disposed(object sender, EventArgs e)
        {
            if (this.error != null)
            {
#if (ERROR_V1)
                // --- 1. unadvice all event handlers here ------------------------------------------------
                this.error.OnError -= new HeidenhainDNCLib._DJHErrorEvents_OnErrorEventHandler(this.OnError);
                this.error.OnErrorCleared -= new HeidenhainDNCLib._DJHErrorEvents_OnErrorClearedEventHandler(this.OnErrorCleared);
#endif

#if (ERROR_V2)
                this.error.OnError2 -= new HeidenhainDNCLib._DJHErrorEvents_OnError2EventHandler(this.OnError2);
                this.error.OnErrorCleared2 -= new HeidenhainDNCLib._DJHErrorEvents_OnErrorCleared2EventHandler(this.OnErrorCleared2);
#endif

#if RAW_COM_EVENTS
                this.errorListener.Stop();
                this.errorListener = null;
#endif
                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.error);
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
        /// Clear all errors on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ClearAllErrorsOnTncButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.error.ClearAllErrors();
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
        /// Clear content in ErrorListView.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ClearListButton_Click(object sender, EventArgs e)
        {
            this.errorList.Clear();
        }

        /// <summary>
        /// Save the content of ErrorListView to file.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SaveListButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Ask user for desired destination of csv file
                SaveFileDialog fd = new SaveFileDialog();
                fd.Filter = "log files (*.log)|*.log|text files (*.txt)|*.txt|all files (*.*)|*.*";
                fd.FileName = "DNC_CSharp_Example_JHError.log";
                DialogResult result = fd.ShowDialog();
                if (result == DialogResult.Abort)
                {
                    return;
                }

                string fileDestination = fd.FileName;
                if (string.IsNullOrEmpty(fileDestination))
                {
                    return;
                }

                // Get errorlist and build string for saving to file
                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in this.errorList)
                {
                    ListViewItem.ListViewSubItemCollection subItemCollection = item.SubItems;
                    int subItemCount = subItemCollection.Count;
                    for (int i = 0; i < subItemCount; i++)
                    {
                        sb.Append(subItemCollection[i].Text);
                        if (i < subItemCount - 1)
                        {
                            // no comma seperation for last column
                            sb.Append(", ");
                        }
                    }

                    sb.Append(Environment.NewLine);
                }

                // write csv file
                File.WriteAllText(fileDestination, sb.ToString());
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
        #endregion

        #region "private methods"
        /// <summary>
        /// Use this to show new errors in a new line on ErrorListView.
        /// </summary>
        /// <param name="errorGroup">The group of the error.</param>
        /// <param name="errorNumber">The number of the error.</param>
        /// <param name="errorClass">The class of the error.</param>
        /// <param name="text">The error short text.</param>
        /// <param name="channel">The error channel.</param>
        /// <param name="handle">Optional, the error handle (only IJHErrorEntry2).</param>
        private void ShowError(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int errorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string text, int channel, int handle = -1)
        {
            try
            {
                ErrorListView.BeginUpdate();
                string errorChannel = Convert.ToString(channel);

                // --- On iTNC530 controls PLC error numbers start with offset 0x81000000 -----------------
                // --> show iTNC530 PLC errors in hex
                string errorNumberString = string.Empty;
                if (this.cncType == HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_ITNC && errorGroup == HeidenhainDNCLib.DNC_ERROR_GROUP.DNC_EG_PLC)
                {
                    errorNumberString = "0x" + errorNumber.ToString("X8");
                }
                else
                {
                    errorNumberString = Convert.ToString(errorNumber);
                }

                string errorText = text;
                string errorGroupString = Enum.GetName(typeof(HeidenhainDNCLib.DNC_ERROR_GROUP), errorGroup);
                string errorClassString = Enum.GetName(typeof(HeidenhainDNCLib.DNC_ERROR_CLASS), errorClass);
                string errorArrivedTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                string errorGoneTime = string.Empty;

                ListViewItem item = new ListViewItem(errorChannel);
                item.SubItems.Add(errorNumberString);
                item.SubItems.Add(errorText);
                item.SubItems.Add(errorGroupString);
                item.SubItems.Add(errorClassString);
                item.SubItems.Add(errorArrivedTime);
                item.SubItems.Add(errorGoneTime);
                item.Name = handle.ToString();
                this.errorList.Add(item);
                ErrorListView.EndUpdate();
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
        /// Use this to show cleared errors on ErrorListView.
        /// If there is a active error with the same handle, then the gone time of its line becomes updated.
        /// If there is no active error with the same handle, or the handle is invalid,
        /// then a new line with the error and channel number becomes added.
        /// </summary>
        /// <param name="errorNumber">The number of the error.</param>
        /// <param name="channel">The channel of the error.</param>
        /// <param name="handle">The handle of the error.</param>
        private void ShowErrorCleared(int errorNumber, int channel, int handle = -1)
        {
            try
            {
                ErrorListView.BeginUpdate();
                string errorGone = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();

                if (handle > -1)
                {
                    // error with valid handle (JHError2::handle)
                    int index = ErrorListView.Items.IndexOfKey(handle.ToString());
                    ListViewItem item = ErrorListView.Items[index];
                    item.SubItems[GONECOLUMNINDEX].Text = errorGone;
                }
                else
                {
                    // error without valid handle (JHError2::handle)
                    string errorChannelText = Convert.ToString(channel);
                    string errorNumberText = Convert.ToString(errorNumber);

                    ListViewItem item = new ListViewItem(errorChannelText);
                    item.SubItems.Add(errorNumberText);
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add(errorGone);
                    item.Name = errorNumberText;
                    this.errorList.Add(item);
                }

                ErrorListView.EndUpdate();
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
        #endregion

#if RAW_COM_EVENTS
        /// <summary>
        /// This is a helper class to use the VTable event implementation of the _IJHErrorEvents.
        /// The VTable event implementation "_IJHErrorEvents" is more effective than the
        /// IDispatch event implementation "_DJHErrorEvents".
        /// </summary>
        private class ErrorListener : HeidenhainDNCLib._IJHErrorEvents2
        {
        #region "Properties
            /// <summary>
            /// The parent dialog is stored, so the events can be passed.
            /// </summary>
            private Error dialog;

            /// <summary>
            /// The IConnectionPoint interface is stored, so it can be used by Stop()/destructor.
            /// </summary>
            private System.Runtime.InteropServices.ComTypes.IConnectionPoint icp;

            /// <summary>
            /// The cookie identifies the advise operation.
            /// </summary>
            private int cookie = -1;
        #endregion

        #region "constructor & destructor"
            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorListener"/> class.
            /// </summary>
            /// <param name="dialog">The parent dialog.</param>
            /// <param name="error">The error interface.</param>
            public ErrorListener(Error dialog, HeidenhainDNCLib.JHError error)
            {
                this.dialog = dialog;

                // get IConnectionPointContainer
                System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)error;

                Guid g = typeof(HeidenhainDNCLib._IJHErrorEvents2).GUID;

                // get IConnectionPoint for the required interface
                icpc.FindConnectionPoint(ref g, out icp);

                // Advise the interface:
                // This means that the COM events are attached to the client implementation (a.k.a. as the event sink)
                // Here the event sink is this class.
                this.icp.Advise(this, out this.cookie);
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="ErrorListener"/> class.
            /// Implicitly stops the event listener.
            /// </summary>
            ~ErrorListener()
            {
                this.Stop();
            }
        #endregion

        #region "event handler"
            /// <summary>
            /// Event from _IJHErrorEvents::OnError.
            /// </summary>
            /// <param name="errorGroup">The group of the error.</param>
            /// <param name="errorNumber">The number of the error.</param>
            /// <param name="errorClass">The class of the error.</param>
            /// <param name="text">The error short text.</param>
            /// <param name="channel">The error channel.</param>
            public void OnError(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int errorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string text, int channel)
            {
                Debug.WriteLine("ErrorListener::OnError");

                // Since this code is executed in the context of the COM event server (callback),
                // the call must be passed to the client thread.
                this.dialog.BeginInvoke(new OnErrorHandler(this.dialog.OnErrorImpl), errorGroup, errorNumber, errorClass, text, channel);
            }

            /// <summary>
            /// Event from _IJHErrorEvents::OnErrorCleared.
            /// </summary>
            /// <param name="errorNumber">The number of the error.</param>
            /// <param name="channel">The error channel.</param>
            public void OnErrorCleared(int errorNumber, int channel)
            {
                Debug.WriteLine("ErrorListener::OnErrorCleared");

                // Since this code is executed in the context of the COM event server (callback),
                // the call must be passed to the client thread.
                this.dialog.BeginInvoke(new OnErrorClearedHandler(this.dialog.OnErrorClearedImpl), errorNumber, channel);
            }

            /// <summary>
            /// Event from _IJHErrorEvents::OnError2.
            /// </summary>
            /// <param name="errorEntry">Helper Object of IJHError2 interface.</param>
            public void OnError2(HeidenhainDNCLib.JHErrorEntry errorEntry)
            {
                Debug.WriteLine("ErrorListener::OnError2");

                // Since this code is executed in the context of the COM event server (callback),
                // the call must be passed to the client thread.
                this.dialog.Invoke(new OnErrorHandler2(this.dialog.OnError2Impl), errorEntry);

                // The JHErrorEntry holds a reference to the parent JHError.
                // This must be released prior to disconnecting.
                // The .NET garbage collector will sometime release the JHErrorEntry.
                // To ensure it is released before disconnecting, it is released here explicitly.
                // This also ensures that memory is not claimed unnecessarily.
                // As a fallback the Disconnect() activates the garbage collector.
                Marshal.ReleaseComObject(errorEntry);
            }

            /// <summary>
            /// Event from _IJHErrorEvents::OnErrorCleared2.
            /// </summary>
            /// <param name="errorEntry">Helper Object of IJHError2 interface.</param>
            public void OnErrorCleared2(HeidenhainDNCLib.JHErrorEntry errorEntry)
            {
                Debug.WriteLine("ErrorListener::OnErrorCleared2");

                // Since this code is executed in the context of the COM event server (callback),
                // the call must be passed to the client thread.
                this.dialog.Invoke(new OnErrorClearedHandler2(this.dialog.OnErrorCleared2Impl), errorEntry);

                // The JHErrorEntry holds a reference to the parent JHError.
                // This must be released prior to disconnecting.
                // The .NET garbage collector will sometime release the JHErrorEntry.
                // To ensure it is released before disconnecting, it is released here explicitly.
                // This also ensures that memory is not claimed unnecessarily.
                // As a fallback the Disconnect() activates the garbage collector.
                Marshal.ReleaseComObject(errorEntry);
            }

            // end: VTable IJHErrorEvents event sink class
        #endregion

        #region "internal methods"
            /// <summary>
            /// Stops the event listener.
            /// </summary>
            internal void Stop()
            {
                if (this.cookie != -1)
                {
                    // Unadvise the interface:
                    // This means that the COM events are detached from the client implementation (a.k.a. as the event sink)
                    this.icp.Unadvise(this.cookie);
                    this.cookie = -1;
                }

                // The IConnectionPoint is no longer required: release it
                Marshal.ReleaseComObject(this.icp);
            }
        #endregion
        }
#endif
    }
}