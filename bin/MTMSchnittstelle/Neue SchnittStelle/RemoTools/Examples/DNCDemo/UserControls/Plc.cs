// ------------------------------------------------------------------------------------------------
// <copyright file="Plc.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJPlc interface.
// </summary>
// ------------------------------------------------------------------------------------------------

/* 
 * Define the type of event mechanism you like to use:
 * ---------------------------------------------------
 * 
 * #define IDISPATCH:
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

#define RAW_COM_EVENTS

#if ((IDISPATCH && RAW_COM_EVENTS) || (!IDISPATCH && !RAW_COM_EVENTS))
#error The type of event processing must be selected uniquely
#endif

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHPLC interface.
    /// </summary>
    public partial class Plc : UserControl
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
        /// Object of HeidenhainDNC IJHPLC interface.
        /// </summary>
        private HeidenhainDNCLib.JHPlc plc = null;

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
        /// The max marker count depends on the control.
        /// </summary>
        private int maxMarker = 0;

        /// <summary>
        /// The max double word count depends on the control.
        /// </summary>
        private int maxDWord = 0;

        /// <summary>
        /// Counts the OnData events.
        /// </summary>
        private uint onPlcDataEventCounter = 0;

        /// <summary>
        /// Counts the received strings.
        /// </summary>
        private uint stringReceivedCounter = 0;

        /// <summary>
        /// Counts the received marker sets.
        /// </summary>
        private uint markersReceivedCounter = 0;

        /// <summary>
        /// Counts the received double word sets.
        /// </summary>
        private uint dwordsReceivedCounter = 0;

        /// <summary>
        /// Counts the SetPLCData calls.
        /// </summary>
        private uint sendCounter = 0;

        /// <summary>
        /// Counts the sent strings.
        /// </summary>
        private uint stringSendCounter = 0;

        /// <summary>
        /// Counts the sent marker sets.
        /// </summary>
        private uint markersSendCounter = 0;

        /// <summary>
        /// Counts the sent double word sets.
        /// </summary>
        private uint dwordsSendCounter = 0;

#if RAW_COM_EVENTS
        /// <summary>
        /// A helper class to listen to the VTable event implementation.
        /// </summary>
        private PlcListener plcListener = null;
#endif
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Plc"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Plc(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.Disposed += new EventHandler(this.Plc_Disposed);

            this.cncType = parentForm.CncType;
            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "delegates"
        /// <summary>
        /// Delegate for _DJHPLC::OnData event handler.
        /// </summary>
        /// <param name="bstrPlcString">String from PLC.</param>
        /// <param name="ppsalPlcMarkers">Marker set from PLC.</param>
        /// <param name="ppsalPlcDWords">Double word set from PLC.</param>
        internal delegate void OnPlcDataHandler(string bstrPlcString, ref Array ppsalPlcMarkers, ref Array ppsalPlcDWords);
        #endregion

        #region "properties"
        /// <summary>
        /// Gets or sets the counter for OnData events and updates GUI with new counter value.
        /// </summary>
        private uint OnPlcDataEventCounter
        {
            get
            {
                return this.onPlcDataEventCounter;
            }

            set
            {
                this.onPlcDataEventCounter = value;
                ReceiveGroupBox.Text = "Receive (counter = " + this.onPlcDataEventCounter + ")";
            }
        }

        /// <summary>
        /// Gets or sets the counter for received strings and updates GUI with new counter value.
        /// </summary>
        private uint StringReceivedCounter
        {
            get
            {
                return this.stringReceivedCounter;
            }

            set
            {
                this.stringReceivedCounter = value;
                ReceiveStringGroupBox.Text = "STRING (counter = " + this.stringReceivedCounter + ")";
            }
        }

        /// <summary>
        /// Gets or sets the counter for received marker sets and updates GUI with new counter value.
        /// </summary>
        private uint MarkersReceivedCounter
        {
            get
            {
                return this.markersReceivedCounter;
            }

            set
            {
                this.markersReceivedCounter = value;
                ReceiveMarkerGroupBox.Text = "(counter = " + this.markersReceivedCounter + ")";
            }
        }

        /// <summary>
        /// Gets or sets the counter for received double word sets and updates GUI with new counter value.
        /// </summary>
        private uint DWordsReceivedCounter
        {
            get
            {
                return this.dwordsReceivedCounter;
            }

            set
            {
                this.dwordsReceivedCounter = value;
                ReceiveDWordGroupBox.Text = "(counter = " + this.dwordsReceivedCounter + ")";
            }
        }

        /// <summary>
        /// Gets or sets the counter for SetPLCData calls and updates GUI with new counter value.
        /// </summary>
        private uint SendCounter
        {
            get
            {
                return this.sendCounter;
            }

            set
            {
                this.sendCounter = value;
                SendGroupBox.Text = "Send (counter = " + this.sendCounter + ")";
            }
        }

        /// <summary>
        /// Gets or sets the counter for sent strings and updates GUI with new counter value.
        /// </summary>
        private uint StringSendCounter
        {
            get
            {
                return this.stringSendCounter;
            }

            set
            {
                this.stringSendCounter = value;
                SendStringGroupBox.Text = "STRING (counter = " + this.stringSendCounter + ")";
                this.SendCounter++;
            }
        }

        /// <summary>
        /// Gets or sets the counter for sent marker sets and updates GUI with new counter value.
        /// </summary>
        private uint MarkersSendCounter
        {
            get
            {
                return this.markersSendCounter;
            }

            set
            {
                this.markersSendCounter = value;
                SendMarkerGroupBox.Text = "(counter = " + this.markersSendCounter + ")";
                this.SendCounter++;
            }
        }

        /// <summary>
        /// Gets or sets the counter for sent double word sets and updates GUI with new counter value.
        /// </summary>
        private uint DWordsSendCounter
        {
            get
            {
                return this.dwordsSendCounter;
            }

            set
            {
                this.dwordsSendCounter = value;
                SendDWordGroupBox.Text = "(counter = " + this.dwordsSendCounter + ")";
                this.SendCounter++;
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
                this.plc = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHPLC);

                //// --- Subscribe for the event(s) -------------------------------------------------
#if IDISPATCH
                this.plc.OnPlcData += new HeidenhainDNCLib._DJHPlcEvents_OnPlcDataEventHandler(this.Plc_OnPlcData);
#endif
#if RAW_COM_EVENTS
                this.plcListener = new PlcListener(this, this.plc);
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

        #region "event handler"
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Plc_Load(object sender, EventArgs e)
        {
            // --- set limits for the Marker and DWord array size -------------------
            switch (this.cncType)
            {
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_ITNC:
                    this.maxMarker = 0;
                    this.maxDWord = 1;
                    SendMarkerGroupBox.Enabled = false;
                    ReceiveMarkerGroupBox.Enabled = false;
                    break;

                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_AR6000_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_ATEKM_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_CNCPILOT6xx_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_GRINDPLUS_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_GRINDPLUS640_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_MILLPLUSIT_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_TNC128_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_TNC320_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_TNC6xx_NCK:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_MANUALPLUS_NCK:
                    this.maxMarker = 64;
                    this.maxDWord = 32;
                    break;
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_MILLPLUS:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_MILLPLUSIT:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_TURNPLUS:
                case HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_ATEKM:
                default:
                    this.maxMarker = 0;
                    this.maxDWord = 0;
                    break;
            }

            // --- lititation of the "numeric up down" controls ---------------------
            MarkerNumericUpDown.Maximum = this.maxMarker;
            DWordNumericUpDown.Maximum = this.maxDWord;

            // --- show CNC type in the header of this user control -----------------
            string cncTypeString = Enum.GetName(typeof(HeidenhainDNCLib.DNC_CNC_TYPE), this.cncType);
            CncTypeLabel.Text = cncTypeString + " " + (this.isNck ? "(NCK based)" : "(non NCK based)");
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Plc_Disposed(object sender, EventArgs e)
        {
            if (this.plc != null)
            {
                // --- 1. unadvice all event handlers here ------------------------------------------------
#if IDISPATCH
                this.plc.OnPlcData -= new HeidenhainDNCLib._DJHPlcEvents_OnPlcDataEventHandler(this.Plc_OnPlcData);
#endif
#if RAW_COM_EVENTS
                this.plcListener.Stop();
                this.plcListener = null;
#endif
                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.plc);
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
        /// Invoke OnPLCDataHandler if this events was fired.
        /// </summary>
        /// <param name="bstrPlcString">Received string from PLC.</param>
        /// <param name="ppsalPlcMarkers">Received marker set from PLC.</param>
        /// <param name="ppsalPlcDWords">Received double word set from PLC.</param>
        private void Plc_OnPlcData(string bstrPlcString, ref Array ppsalPlcMarkers, ref Array ppsalPlcDWords)
        {
            // Do not use BeginInvoke here. (danger of race condition)
            this.Invoke(new OnPlcDataHandler(this.OnPlcDataImpl), bstrPlcString, ppsalPlcMarkers, ppsalPlcDWords);
        }

        /// <summary>
        /// Shows received values in GUI and updates the counter values.
        /// </summary>
        /// <param name="bstrPlcString">Received string from PLC.</param>
        /// <param name="ppsalPlcMarkers">Received marker set from PLC.</param>
        /// <param name="ppsalPlcDWords">Received double word set from PLC.</param>
        private void OnPlcDataImpl(string bstrPlcString, ref Array ppsalPlcMarkers, ref Array ppsalPlcDWords)
        {
            // --- OnPlcData event counter ------------------------------------------
            this.OnPlcDataEventCounter++;

            // --- show string ------------------------------------------------------
            if (bstrPlcString != string.Empty)
            {
                ReceiveStringTextBox.Text = bstrPlcString;
                ReceiveStringTextBox.BackColor = Color.Green;
                this.StringReceivedCounter++;
            }

            // --- show marker(s) ---------------------------------------------------
            if (ppsalPlcMarkers.Length > 0)
            {
                this.ShowArrayOnListView(ref this.ReceiveMarkerListView, ppsalPlcMarkers);
                this.MarkersReceivedCounter++;
            }

            // --- show DWord(s) ----------------------------------------------------
            if (ppsalPlcDWords.Length > 0)
            {
                this.ShowArrayOnListView(ref this.ReceiveDWordListView, ppsalPlcDWords);
                this.DWordsReceivedCounter++;
            }
        }

        /// <summary>
        /// Add marker(s) to send list.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AddMarkerButton_Click(object sender, EventArgs e)
        {
            this.AddListViewLines(ref this.SendMarkerListView, MarkerNumericUpDown.Value, this.maxMarker);
        }

        /// <summary>
        /// Remove markers from send list.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoveMarkerButton_Click(object sender, EventArgs e)
        {
            this.RemoveListViewLines(ref this.SendMarkerListView, MarkerNumericUpDown.Value);
        }

        /// <summary>
        /// Add double word to send list.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AddDWordButton_Click(object sender, EventArgs e)
        {
            this.AddListViewLines(ref this.SendDWordListView, DWordNumericUpDown.Value, this.maxDWord);
        }

        /// <summary>
        /// Remove double word from send list.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoveDWordButton_Click(object sender, EventArgs e)
        {
            this.RemoveListViewLines(ref this.SendDWordListView, DWordNumericUpDown.Value);
        }

        /// <summary>
        /// Universal handler to edit the list view elements for the marker and the double word list views.
        /// </summary>
        /// <param name="sender">The clicked list view.</param>
        /// <param name="e">The mouse position.</param>
        private void UniversalListView_MouseClick(object sender, MouseEventArgs e)
        {
            // edit item if clicked once (in listview)
            ListView listView = (ListView)sender;
            ListViewItem selectedItem = listView.GetItemAt(e.X, e.Y);
            selectedItem.BeginEdit();
        }

        /// <summary>
        /// After label edit, check if new value is valid.
        /// </summary>
        /// <param name="sender">The edited list view.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SendMarkerListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ListView listView = (ListView)sender;
            uint newVal;

            // check if value is convertable to a unsigned integer type
            if (!uint.TryParse(e.Label, out newVal))
            {
                e.CancelEdit = true;
                return;
            }

            // check if value isn't bigger than 1 (bool)
            if (newVal > 1)
            {
                e.CancelEdit = true;
                return;
            }

            listView.BackColor = Color.White;
        }

        /// <summary>
        /// After label edit, check if new value is valid.
        /// </summary>
        /// <param name="sender">The edited list view.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SendDWordListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ListView listView = (ListView)sender;
            uint newVal;

            // check if value is convertable to a unsigned integer type
            if (!uint.TryParse(e.Label, out newVal))
            {
                e.CancelEdit = true;
            }
        }

        /// <summary>
        /// Set the background color of the text box to white after text has changed.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SendStringTextBox_TextChanged(object sender, EventArgs e)
        {
            SendStringTextBox.BackColor = Color.White;
        }

        /// <summary>
        /// Send string, marker and double word data using SetPLCData() depending on the check boxes.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SendButton_Click(object sender, EventArgs e)
        {
            // --- reset controls "BackColor" -----------------------------------------------------------
            SendStringTextBox.BackColor = Color.White;
            SendMarkerListView.BackColor = Color.White;
            SendDWordListView.BackColor = Color.White;

            ReceiveStringTextBox.BackColor = Color.White;
            ReceiveMarkerListView.BackColor = Color.White;
            ReceiveDWordListView.BackColor = Color.White;

            string sendString = string.Empty;
            Array markerArray = new int[0];
            Array dwordArray = new int[0];

            bool doSendString = false;
            bool doSendMarkers = false;
            bool doSendDWords = false;

            // --- send string ------------------------------------------------------
            if (StringCheckBox.Checked)
            {
                sendString = SendStringTextBox.Text;
                doSendString = true;
            }

            // --- send Marker(s) ---------------------------------------------------
            if (MarkerCheckBox.Checked)
            {
                doSendMarkers = this.GetArrayFromListView(this.SendMarkerListView, out markerArray, this.maxMarker);
            }

            // --- send DWord(s) ----------------------------------------------------
            if (DWordCheckBox.Checked)
            {
                doSendDWords = this.GetArrayFromListView(this.SendDWordListView, out dwordArray, this.maxDWord);
            }

            try
            {
                this.plc.SetPlcData(sendString, ref markerArray, ref dwordArray);
                if (doSendString)
                {
                    this.StringSendCounter++;
                    SendStringTextBox.BackColor = Color.Green;
                }

                if (doSendMarkers)
                {
                    this.MarkersSendCounter++;
                    SendMarkerListView.BackColor = Color.Green;
                }

                if (doSendDWords)
                {
                    this.DWordsSendCounter++;
                    SendDWordListView.BackColor = Color.Green;
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
        #endregion

        #region "private methods"
        /// <summary>
        /// Shows the data from parameter list in the list views from parameter list.
        /// If values has changed, the background becomes set to green.
        /// </summary>
        /// <param name="listView">Target list view to show data.</param>
        /// <param name="data">Data to show.</param>
        private void ShowArrayOnListView(ref ListView listView, Array data)
        {
            int arraySize = data.Length;
            if (arraySize > 0)
            {
                listView.Items.Clear();
                foreach (int value in data)
                {
                    // get all received marker values and set the background color to green to show the user,
                    // that new values has arrived
                    string strLine = Convert.ToString(value);
                    listView.Items.Add(new ListViewItem(strLine));
                }

                listView.BackColor = Color.Green;
            }
            else
            {
                // do not refresh the values in the listview if an empty array was received
                // but change the color to white to show the user, that no new values has arrived
                listView.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Extract data as type "array" from the list view control. 
        /// </summary>
        /// <param name="listView">The list view to extract the data from.</param>
        /// <param name="data">The array to store the data.</param>
        /// <param name="arrayMaxSize">The max allowed array size.</param>
        /// <returns>Returns false if array is smaller than 1.</returns>
        private bool GetArrayFromListView(ListView listView, out Array data, int arrayMaxSize)
        {
            int arraySize = listView.Items.Count;
            data = new int[arraySize];
            if (arraySize > arrayMaxSize)
            {
                MessageBox.Show(
                           "Can't get the array data from the list view elements",
                           "Uuuups...",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                return false;
            }

            if (arraySize < 1)
            {
                return false;
            }

            // fetch marker data from listView
            for (int i = 0; i < arraySize; i++)
            {
                data.SetValue(Convert.ToInt32(listView.Items[i].Text), i);
            }

            return true;
        }

        /// <summary>
        /// Removes lines from the list view controls.
        /// </summary>
        /// <param name="listView">List view to remove line from.</param>
        /// <param name="linesToRemove">Amount of lines to remove.</param>
        private void RemoveListViewLines(ref ListView listView, decimal linesToRemove)
        {
            int removeLinesCount = Convert.ToInt32(linesToRemove);
            int actLinesCount = listView.Items.Count;

            // do nothing, if there is no marker in the listview
            if (actLinesCount < 1)
            {
                return;
            }

            // remove all markers, if the remove count is higher than the markers in the listview 
            if (removeLinesCount > actLinesCount)
            {
                removeLinesCount = actLinesCount;
            }

            int remainingLinesCount = actLinesCount - removeLinesCount;

            // remove line in list view
            for (int i = actLinesCount; i > remainingLinesCount; i--)
            {
                listView.Items.RemoveAt(i - 1);
            }

            // change collor of all items to white if at least one was removed
            if (removeLinesCount > 0)
            {
                listView.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Add lines to the list view controls.
        /// </summary>
        /// <param name="listView">The list view to add lines.</param>
        /// <param name="linesToAdd">The amount of lines to add.</param>
        /// <param name="maxLinesAllowed">The max allowed array size.</param>
        private void AddListViewLines(ref ListView listView, decimal linesToAdd, int maxLinesAllowed)
        {
            int addLinesCount = Convert.ToInt32(linesToAdd);
            int actLinesCount = listView.Items.Count;

            int addableLines = maxLinesAllowed - actLinesCount;
            if (addLinesCount > addableLines)
            {
                addLinesCount = addableLines;
            }

            // Add lines in list view
            for (int i = 0; i < addLinesCount; i++)
            {
                ListViewItem newItem = new ListViewItem("1");
                listView.Items.Add(newItem);
            }

            // change collor of all items to white if at least one was added
            if (addLinesCount > 0)
            {
                listView.BackColor = Color.White;
            }
        }
        #endregion

#if RAW_COM_EVENTS
        /// <summary>
        /// This is a helper class to use the VTable event implementation of the _IJHPlcEvents.
        /// The VTable event implementation "_IJHPlcEvents" is more effective than the
        /// IDispatch event implementation "_DJHPlcEvents".
        /// </summary>
        private class PlcListener : HeidenhainDNCLib._IJHPlcEvents
        {
            #region "Properties
            /// <summary>
            /// The parent dialog is stored, so the events can be passed.
            /// </summary>
            private Plc dialog;

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
            /// Initializes a new instance of the <see cref="PlcListener"/> class.
            /// </summary>
            /// <param name="dialog">The parent dialog.</param>
            /// <param name="plc">The error interface.</param>
            public PlcListener(Plc dialog, HeidenhainDNCLib.JHPlc plc)
            {
                this.dialog = dialog;

                // get IConnectionPointContainer
                System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)plc;

                Guid g = typeof(HeidenhainDNCLib._IJHPlcEvents).GUID;

                // get IConnectionPoint for the required interface
                icpc.FindConnectionPoint(ref g, out this.icp);

                // Advise the interface:
                // This means that the COM events are attached to the client implementation (a.k.a. as the event sink)
                // Here the event sink is this class.
                this.icp.Advise(this, out this.cookie);
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="PlcListener"/> class.
            /// Implicitly stops the event listener.
            /// </summary>
            ~PlcListener()
            {
                this.Stop();
            }
            #endregion

            #region "event handler"
            /// <summary>
            /// This method handles the event initiated by the PLC program in the CNC when it sends data.
            /// </summary>
            /// <param name="bstrPlcString">PLC string that was transmitted from the Control.</param>
            /// <param name="ppsalPlcMarkers">List of long words was transmitted as markers from the Control (0: FALSE, != 0: TRUE).</param>
            /// <param name="ppsalPlcDWords">List of long words that was transmitted as DWORDs from the Control.</param>
            public void OnPlcData(string bstrPlcString, ref Array ppsalPlcMarkers, ref Array ppsalPlcDWords)
            {
                Debug.WriteLine("PlcListener::OnPlcData");
                this.dialog.Invoke(new OnPlcDataHandler(this.dialog.OnPlcDataImpl), bstrPlcString, ppsalPlcMarkers, ppsalPlcDWords);
            }

            // end: VTable IJHPlcEvents event sink class
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