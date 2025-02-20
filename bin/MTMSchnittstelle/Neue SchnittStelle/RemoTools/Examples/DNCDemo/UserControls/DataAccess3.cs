// ------------------------------------------------------------------------------------------------
// <copyright file="DataAccess3.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Tobias Habermann</author>
// <date>26.07.2016</date>
// <summary>
// This file contains a class which shows how to handle the IJHDataAccess3 interface.
// </summary>
// -----------------------------------------------------------------------

////#define RAW_COM_EVENTS

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This user control is used to show the IJHDataAccess3 functionalities.
    /// </summary>
    public partial class DataAccess3 : UserControl
    {
        #region "type defs"
        #endregion

        #region "fields"
        /// <summary>
        /// The parent data access control. Is needet to access to the dala selection.
        /// </summary>
        private DataAccess parentDataAccess = null;

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
        /// Object of HeidenhainDNC IJHDataAccess interface.
        /// </summary>
        private HeidenhainDNCLib.JHDataAccess dataAccess = null;

        /// <summary>
        /// The JHDataEntry object is the central helper object of the IJHDataAccess Interface.
        /// All data entries are located in a tree structure. Therefore a data entry is either a node or an endpoint.
        /// </summary>
        private HeidenhainDNCLib.IJHDataEntry mainDataEntry = null;

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
        /// List of all subscribed data.
        /// The handle property is needed to unsubscribe.
        /// </summary>
        private List<SubscriptionData> subscriptionList = new List<SubscriptionData>();

        /// <summary>
        /// Logging Class.
        /// </summary>
        private Logging log = new Logging();

        /// <summary>
        /// Logging state.
        /// </summary>
        private bool loggingActive = false;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccess3"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="mainForm">Reference to the main application Form.</param>
        public DataAccess3(MainForm mainForm, DataAccess parentDataAccess)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.machine = mainForm.Machine;
            this.isNck = mainForm.IsNck;
            this.cncType = mainForm.CncType;

            this.parentDataAccess = parentDataAccess;

            this.Disposed += new EventHandler(this.DataAccess3_Disposed);
        }
        #endregion

        #region "delegates"
        /// <summary>
        /// This method handles the event which is fired, if the subscribed data has changed on the control or the timer has expired.
        /// </summary>
        /// <param name="subscriptionHandle">Number that was returned by the method IJHDataEntry::Subscribe() to identify the data without the need for a string compare.</param>
        /// <param name="timeStamp">Timestamp for the moment that the data was stored. </param>
        /// <param name="changeList">IJHDataEntry2List of changed data.</param>
        internal delegate void OnData2Handler(int subscriptionHandle, DateTime timeStamp, HeidenhainDNCLib.IJHDataEntry2List changeList);

        /// <summary>
        /// This method handles the event which is fired, if a child in the child list becomes double clicked.
        /// </summary>
        /// <param name="sender">The sender of the delegate.</param>
        /// <param name="e">The event arguments containing the data selection.</param>
        internal delegate void DataSelectionDoubleClickedDelegate(object sender, DataSelectionEventArgs e);

        /// <summary>
        /// This method handles the event which is fired, if the selected child in the child list changes.
        /// </summary>
        /// <param name="sender">The sender of the delegate.</param>
        /// <param name="e">The event arguments containing the data selection.</param>
        internal delegate void DataSelectionChangedDelegate(object sender, DataSelectionEventArgs e);
        #endregion

        #region "Events"
        /// <summary>
        /// The event if the selected child in the child list changes.
        /// </summary>
        internal event DataSelectionChangedDelegate DataSelectionChanged;

        /// <summary>
        /// The event if a child in the child list becomes double clicked.
        /// </summary>
        internal event DataSelectionDoubleClickedDelegate DataSelectionDoubleClicked;
        #endregion

        #region "Enums"
        #endregion

        #region "properties"
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
                this.dataAccess = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHDATAACCESS);

                //// --- Subscribe for the event(s) -------------------------------------------------
                if (this.isNck)
                {
                    this.dataAccess.SetAccessMode(HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_TABLEDATAACCESS, string.Empty);
                    this.dataAccess.SetAccessMode(HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_PLCDATAACCESS, string.Empty);
                    this.dataAccess.SetAccessMode(HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_CFGDATAACCESS, string.Empty);
                }
#if RAW_COM_EVENTS
                this.dataListener = new DataListener(this, this.dataAccess);
#else
                this.dataAccess.OnData2 += new HeidenhainDNCLib._DJHDataAccessEvents_OnData2EventHandler(this.DataAccess_OnData2);
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

        /// <summary>
        /// Set the DNC_ACCESS_MODE for data access.
        /// </summary>
        /// <param name="accessMode">The access mode to set.</param>
        /// <param name="password">The associated password.</param>
        public void SetAccessMode(HeidenhainDNCLib.DNC_ACCESS_MODE accessMode, string password)
        {
            this.dataAccess.SetAccessMode(accessMode, password);
        }

        /// <summary>
        /// This method returns (a pointer to) the JHDataEntry2 object that is selected by the specified path.
        /// If a node is selected, the JHDataEntry2 object has a collection property that contains the JHDataEntry objects of the next level.
        /// </summary>
        /// <param name="dataSelection">Full path name of the required JHDataEntry object.</param>
        public void GetDataEntry(string dataSelection)
        {
            HeidenhainDNCLib.IJHDataEntryList dataEntryList = null;
            HeidenhainDNCLib.IJHDataEntry innerDataEntry = null;
            HeidenhainDNCLib.IJHDataEntryPropertyList dataEntryPropertyList = null;
            HeidenhainDNCLib.IJHDataEntryProperty dataEntryProperty = null;
            HeidenhainDNCLib.IJHDataEntryProperty dataEntryPropertyType = null;
            try
            {
                if (this.mainDataEntry != null)
                {
                    Marshal.ReleaseComObject(this.mainDataEntry);
                }

                // Reset all user controls
                ChildListTreeView.BeginUpdate();
                PropertyListListView.BeginUpdate();
                ChildListTreeView.Nodes.Clear();
                PropertyListListView.Items.Clear();
                VarValueTextBox.Enabled = true;
                VarValueTextBox.Clear();
                IsReadOnlyLabel.Text = @"R/W";
                IsReadOnlyLabel.BackColor = SystemColors.Control;

                TreeNode parentNode = null;
                TreeNode nextParentNode = null;
                string[] dataNodes = null; 
                string path = string.Empty;

                if (!dataSelection.Equals(@"\"))
                {
                    dataNodes = dataSelection.Split('\\');
                }
                else
                {
                    dataNodes = new string[] { string.Empty };
                }

                for (int node = 0; node < dataNodes.Length; node++)
                {
                    path += dataNodes[node] + @"\";
                    this.mainDataEntry = this.dataAccess.GetDataEntry(path);
                    if (this.mainDataEntry.bIsNode)
                    {
                        dataEntryList = this.mainDataEntry.childList;

                        for (int i = 0; i < dataEntryList.Count; i++)
                        {
                            innerDataEntry = dataEntryList[i];

                            TreeNode lastAddedNode = null;
                            if (parentNode != null)
                            {
                                lastAddedNode = parentNode.Nodes.Add(innerDataEntry.bstrName);
                                parentNode.Expand();
                            }
                            else
                            {
                                lastAddedNode = ChildListTreeView.Nodes.Add(innerDataEntry.bstrName);
                            }

                            if (node + 1 < dataNodes.Length && innerDataEntry.bstrName.Equals(dataNodes[node + 1]))
                            {
                                nextParentNode = lastAddedNode;
                            }
                            else
                            {
                                lastAddedNode.Nodes.Add(string.Empty); //// adding a fake child, to identify unexplored nodes
                            }

                            Marshal.ReleaseComObject(innerDataEntry);
                        }

                        parentNode = nextParentNode;
                        Marshal.ReleaseComObject(this.mainDataEntry);
                    }
                }

                this.mainDataEntry = this.dataAccess.GetDataEntry(dataSelection);

                if (this.mainDataEntry.bIsNode)
                {
                    //// load lower braches
                }
                else
                {
                    SetDataEntryButton.Enabled = true;
                    dataEntryPropertyList = this.mainDataEntry.propertyList;

                    // --- show all available DNC_DATAENTRY_PROPKIND for selected property ------------------
                    for (int i = 0; i < dataEntryPropertyList.Count; i++)
                    {
                        dataEntryProperty = dataEntryPropertyList[i];

                        string propertyKind = dataEntryProperty.kind.ToString();
                        string varValueString = "null";
                        string varValueTypeString = "null";

                        object varValue = dataEntryProperty.varValue;
                        if (varValue != null)
                        {
                            // Show also the enumeration text of DNC_TRIG_MODE for property kinds of type DNC_DATAENTRY_PROPKIND_SUBSCRIBE_CAPABILITY
                            if (dataEntryProperty.kind == HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_SUBSCRIBE_CAPABILITY)
                            {
                                int numericalEnumValue = Convert.ToInt32(varValue);
                                HeidenhainDNCLib.DNC_TRIG_MODE trigMode = (HeidenhainDNCLib.DNC_TRIG_MODE)numericalEnumValue;
                                varValueString = varValue.ToString() + " (" + trigMode.ToString() + ")";
                            }
                            else
                            {
                                varValueString = varValue.ToString();
                            }
                        }

                        object varValueType = dataEntryProperty.varValue;
                        if (varValueType != null)
                        {
                            varValueTypeString = varValueType.GetType().ToString();
                        }

                        bool isReadonly = dataEntryProperty.bIsReadOnly;
                        //// string sVarValueType = dataEntryProperty.varValueType.ToString(); --> does not work with C#

                        // -- show all property kind's in list
                        PropertyListListView.Items.Add(propertyKind).SubItems.AddRange(new[] { varValueString, varValueTypeString });

                        // --- show special (important) values also separately 
                        if (dataEntryProperty.kind == HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA)
                        {
                            VarValueTextBox.Text = varValueString;
                            VarValueTextBox.ReadOnly = isReadonly;
                            SetDataEntryButton.Enabled = !isReadonly;
                            IsReadOnlyLabel.Text = isReadonly ? "read-only" : "writable";
                            IsReadOnlyLabel.BackColor = isReadonly ? Color.Yellow : MainForm.JHGREEN;
                        }

                        Marshal.ReleaseComObject(dataEntryProperty);
                    }
                }
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
                Debug.WriteLine(cex.StackTrace);
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                ChildListTreeView.EndUpdate();
                PropertyListListView.EndUpdate();

                if (dataEntryList != null)
                {
                    Marshal.ReleaseComObject(dataEntryList);
                }

                if (innerDataEntry != null)
                {
                    Marshal.ReleaseComObject(innerDataEntry);
                }

                if (dataEntryPropertyList != null)
                {
                    Marshal.ReleaseComObject(dataEntryPropertyList);
                }

                if (dataEntryProperty != null)
                {
                    Marshal.ReleaseComObject(dataEntryProperty);
                }

                if (dataEntryPropertyType != null)
                {
                    Marshal.ReleaseComObject(dataEntryPropertyType);
                }
            }
        }

        /// <summary>
        /// Subscribe for data selection using a standard trigger condition.
        /// </summary>
        /// <param name="dataSelection">Data selection to subscribe.</param>
        public void Subscribe(string dataSelection)
        {
            HeidenhainDNCLib.IJHDataEntry2 dataEntry = null;
            HeidenhainDNCLib.IJHTrigger trigger = null;
            HeidenhainDNCLib.IJHTriggerCondition triggerCondition = null;
            HeidenhainDNCLib.IJHTriggerSampling triggerSampling = null;

            try
            {
                dataEntry = this.dataAccess.GetDataEntry2(dataSelection, HeidenhainDNCLib.DNC_DATA_UNIT_SELECT.DNC_DATA_UNIT_SELECT_METRIC, false);
                if (dataEntry.bIsNode)
                {
                    MessageBox.Show(
                               "IJHDataAccess2::Subscribe() can only be used to subscribe to properties." +
                               Environment.NewLine + Environment.NewLine +
                               "Please use IJHDataAccess3 to subscribe to nodes.",
                               "Subscription invalid!",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation);
                    return;
                }

                // --- configure trigger ------------------------------------------------------------------
                trigger = new HeidenhainDNCLib.JHTrigger();
                triggerCondition = trigger.GetInterface(HeidenhainDNCLib.DNC_TRIG_INTERFACE.DNC_TRIG_INTERFACE_CONDITION);
                triggerCondition.TriggerOnChange();
                triggerSampling = trigger.GetInterface(HeidenhainDNCLib.DNC_TRIG_INTERFACE.DNC_TRIG_INTERFACE_SAMPLING);
                triggerSampling.SampleOnTimer(0.5);

                // --- subscribe --------------------------------------------------------------------------
                int subscriptionHandle = dataEntry.Subscribe2WithTrigger(triggerCondition, triggerSampling);
                Debug.WriteLine(subscriptionHandle);
                this.subscriptionList.Add(new SubscriptionData(subscriptionHandle, dataEntry));

                this.UpdateGui();
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
                if (triggerCondition != null)
                {
                    Marshal.ReleaseComObject(triggerCondition);
                }

                if (triggerSampling != null)
                {
                    Marshal.ReleaseComObject(triggerSampling);
                }

                if (trigger != null)
                {
                    Marshal.ReleaseComObject(trigger);
                }
            }
        }

        /// <summary>
        /// Switch the logging on/off.
        /// </summary>
        /// <param name="state">The new logging state.</param>
        /// <param name="path">The directory where the log file has to be stored.</param>
        public void SwitchLogging(bool state, string path = null)
        {
            this.loggingActive = state;
            if (this.loggingActive)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    throw new ArgumentException();
                }

                this.log = new Logging();
                this.log.Logfile = path + @"\IJHDataAccess3.log";
                this.log.Start(true);
            }
            else
            {
                this.log.Stop();
                this.log = null;
            }
        }
        #endregion

        #region "protected methods"
        /// <summary>
        /// Fire the event DataSelectionChanged.
        /// </summary>
        /// <param name="e">The event arguments containing the data selection.</param>
        protected void OnDataSelectionChanged(DataSelectionEventArgs e)
        {
            this.DataSelectionChanged(this, e);
        }

        /// <summary>
        /// Fire the event DataSelectionDoubleClicked.
        /// </summary>
        /// <param name="e">The event arguments containing the data selection.</param>
        protected void OnDataSelectionDoubleClicked(DataSelectionEventArgs e)
        {
            this.DataSelectionDoubleClicked(this, e);
        }
        #endregion

        #region "event handler"
        /// <summary>
        /// This method handles the event which is fired, if the subscribed data has changed on the control or the timer has expired.
        /// </summary>
        /// <param name="subscriptionHandle">Number that was returned by the method IJHDataEntry::Subscribe() to identify the data without the need for a string compare.</param>
        /// <param name="timeStamp">Timestamp for the moment that the data was stored.</param>
        /// <param name="changeList">List of all changed parameters.</param>
        private void DataAccess_OnData2(int subscriptionHandle, DateTime timeStamp, HeidenhainDNCLib.JHDataEntry2List changeList)
        {
            Debug.WriteLine("IDispatch::OnData - Handle: " + Convert.ToString(subscriptionHandle) + " - " + changeList.ToString());
            this.BeginInvoke(new OnData2Handler(this.OnData2Impl), subscriptionHandle, timeStamp, changeList);
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DataAccess3_Disposed(object sender, EventArgs e)
        {
            if (this.mainDataEntry != null)
            {
                Marshal.ReleaseComObject(this.mainDataEntry);
            }

            if (this.dataAccess != null)
            {
                // --- 1. unadvise all event handlers here ------------------------------------------------
#if RAW_COM_EVENTS
                this.dataListener.Stop();
                this.dataListener = null;
#else
                this.dataAccess.OnData2 -= new HeidenhainDNCLib._DJHDataAccessEvents_OnData2EventHandler(this.DataAccess_OnData2);
#endif
                this.UnSubscribe();

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
        /// Set the value of the current data selection.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetDataEntryButton_Click(object sender, EventArgs e)
        {
            this.SetDataEntry(VarValueTextBox.Text);
        }

        /// <summary>
        /// Sets data selection if selected index of child list has changed.
        /// </summary>
        /// <param name="sender">The child list list box.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ChildListListBox_MouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e != null)
            {
                TreeNodeMouseClickEventArgs nodeClickEvent = (TreeNodeMouseClickEventArgs)e;
                if (nodeClickEvent.Node != null)
                {
                    string clickedNode = @"\" + nodeClickEvent.Node.FullPath.ToString();
                    DataSelectionEventArgs args = new DataSelectionEventArgs(clickedNode);
                    this.OnDataSelectionChanged(args);
                }
            }
        }

        /// <summary>
        /// Gets the content of the double clicked item in child list list box.
        /// </summary>
        /// <param name="sender">The child list list box.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ChildListListBox_MouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e != null)
            {
                TreeNodeMouseClickEventArgs nodeClickEvent = (TreeNodeMouseClickEventArgs)e;
                if (nodeClickEvent.Node != null)
                {
                    string clickedNode = @"\" + nodeClickEvent.Node.FullPath.ToString();
                    DataSelectionEventArgs args = new DataSelectionEventArgs(clickedNode);
                    this.OnDataSelectionDoubleClicked(args);
                }
            }
        }

        /// <summary>
        /// Unsubscribe item if it becomes right clicked on subscriptions list view.
        /// </summary>
        /// <param name="sender">The subscriptions list view.</param>
        /// <param name="e">The mouse event argument to decode the right button click.</param>
        private void SubscriptionsListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (!(e.Button == System.Windows.Forms.MouseButtons.Right))
            {
                return;
            }

            ListView listView = (ListView)sender;
            ListViewItem selectedItem = listView.GetItemAt(e.X, e.Y);
            int handle = Convert.ToInt32(selectedItem.Text);
            this.UnSubscribe(handle);
            this.UpdateGui();
        }

        /// <summary>
        /// Event gets triggered if the tree view has be be expanded.
        /// </summary>
        /// <param name="sender">The child list list box.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ChildListTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            HeidenhainDNCLib.IJHDataEntryList dataEntryList = null;
            HeidenhainDNCLib.IJHDataEntry innerDataEntry = null;
            HeidenhainDNCLib.IJHDataEntry dataEntry = null;

            // checking for fake child, to identify unexplored nodes
            if (e.Node.Nodes[0].Text.Equals(string.Empty)) 
            {
                e.Node.Nodes[0].Remove();
                try
                {
                    dataEntry = this.dataAccess.GetDataEntry(@"\" + e.Node.FullPath);
                    if (dataEntry.bIsNode)
                    {
                        dataEntryList = dataEntry.childList;
                        TreeNode parentNode = e.Node;
                        for (int i = 0; i < dataEntryList.Count; i++)
                        {
                            innerDataEntry = dataEntryList[i];

                            TreeNode lastAddedNode = null;

                            lastAddedNode = parentNode.Nodes.Add(innerDataEntry.bstrName);
                            lastAddedNode.Nodes.Add(string.Empty); //// adding a fake child, to identify unexplored nodes

                            Marshal.ReleaseComObject(innerDataEntry);
                        }

                        Marshal.ReleaseComObject(dataEntry);
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
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            SubscriptionsListView.BeginUpdate();
            SubscriptionsListView.Items.Clear();
            foreach (SubscriptionData item in this.subscriptionList)
            {
                ListViewItem newItem = new ListViewItem(item.Handle.ToString());
                newItem.SubItems.Add(item.DataSelection);
                newItem.SubItems.Add(item.Value);
                newItem.SubItems.Add(item.TimeStamp.ToLongTimeString());
                SubscriptionsListView.Items.Add(newItem);
            }

            SubscriptionsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            if (SubscriptionsListView.Items.Count > 0)
            {
                SubscriptionsListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            }

            SubscriptionsListView.EndUpdate();
        }

        /// <summary>
        /// This method handles the event which is fired, if the subscribed data has changed on the control or the timer has expired.
        /// </summary>
        /// <param name="subscriptionHandle">Number that was returned by the method IJHDataEntry::Subscribe() to identify the data without the need for a string compare.</param>
        /// <param name="timeStamp">Timestamp for the moment that the data was stored.  </param>
        /// <param name="changeList">List of all changed parameters.</param>
        private void OnData2Impl(int subscriptionHandle, DateTime timeStamp, HeidenhainDNCLib.IJHDataEntry2List changeList)
        {
            Debug.WriteLine("OnDataImpl - Handle: " + Convert.ToString(subscriptionHandle));

            HeidenhainDNCLib.IJHDataEntry2 changeListElement = null;
            for (int change = 0; change < changeList.Count; change++)
            {
                changeListElement = changeList[change];
                object varValue = changeListElement.propertyValue;
               
                for (int i = 0; i < this.subscriptionList.Count; i++)
                {
                    if (this.subscriptionList[i].Handle == subscriptionHandle)
                    {
                        if (this.loggingActive)
                        {
                            this.log.LogMessage("EventTime: " + timeStamp.ToLongTimeString() + "." + timeStamp.Millisecond.ToString("000") +
                                                " ; HandleID: " + subscriptionHandle.ToString() +
                                                " ; dataSelection: " + this.subscriptionList[i].DataSelection +
                                                " ; varValue: " + varValue.ToString());
                        }

                        this.subscriptionList[i].Value = Convert.ToString(varValue);
                        this.subscriptionList[i].TimeStamp = timeStamp;
                    }
                }

                Marshal.ReleaseComObject(changeListElement);
            }

            if (changeListElement != null)
            {
                Marshal.ReleaseComObject(changeListElement); 
            }

            this.UpdateGui();
        }

        /// <summary>
        /// Set current data selection value using the correct type.
        /// </summary>
        /// <param name="varValue">The value to set.</param>
        private void SetDataEntry(string varValue)
        {
            HeidenhainDNCLib.IJHDataEntryPropertyList dataEntryPropertyList = null;
            HeidenhainDNCLib.IJHDataEntryProperty dataEntryProperty = null;
            HeidenhainDNCLib.IJHDataEntryProperty dataEntryPropertyType = null;
            try
            {
                if (!this.mainDataEntry.bIsNode)
                {
                    dataEntryPropertyList = this.mainDataEntry.propertyList;
                    dataEntryProperty = dataEntryPropertyList.get_property(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_DATA);
                    if (!dataEntryProperty.bIsReadOnly)
                    {
                        Type varValueType = dataEntryProperty.varValue.GetType();          // not effective, but works

                        /* If value is empty, then the type given by C# Interopt is System.DBNull!
                         * To write a specific value to control, we need to know the type of it to convert from textbox.Text (string) to this specific type
                         * In this case we can use the DNC_DATAENTRY_PROPKIND_UPPER_BOUND or DNC_DATAENTRY_PROPKIND_LOWER_BOUND propkinds to detect the target type
                         */
                        if (varValueType == typeof(System.DBNull))
                        {
                            dataEntryPropertyType = dataEntryPropertyList.get_property(HeidenhainDNCLib.DNC_DATAENTRY_PROPKIND.DNC_DATAENTRY_PROPKIND_UPPER_BOUND);
                            varValueType = dataEntryPropertyType.varValue.GetType();
                        }

                        dataEntryProperty.varValue = TypeDescriptor.GetConverter(varValueType).ConvertFromString(varValue);
                    }
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
                if (dataEntryPropertyList != null)
                {
                    Marshal.ReleaseComObject(dataEntryPropertyList);
                }

                if (dataEntryProperty != null)
                {
                    Marshal.ReleaseComObject(dataEntryProperty);
                }

                if (dataEntryPropertyType != null)
                {
                    Marshal.ReleaseComObject(dataEntryPropertyType);
                }
            }
        }

        /// <summary>
        /// Unsubscribe data selection with specified handle.
        /// If the handle parameter is empty all subscribed data becomes unsubscribed.
        /// </summary>
        /// <param name="handle">Handle to unsubscribe. Leave empty if you want to unsubscribe all subscriptions.</param>
        private void UnSubscribe(int handle = -1)
        {
            // TODO: muss noch fuer DataAccess3 ueberarbeitet werden, da beim disconect die Subscriptions nicht abgemeldet werden.
            List<SubscriptionData> unsubscribeList = new List<SubscriptionData>();
            try
            {
                // --- find data to UnSubscribe -----------------------------------------------------------
                if (handle >= 0)
                {
                    foreach (SubscriptionData item in this.subscriptionList)
                    {
                        if (item.Handle == handle)
                        {
                            unsubscribeList.Add(item);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.subscriptionList.Count; i++)
                    {
                        unsubscribeList.Add(this.subscriptionList[i]);
                    }
                }

                // --- UnSubscribe an delete from list ----------------------------------------------------
                for (int i = 0; i < unsubscribeList.Count; i++)
                {
                    // 1. Remove from subscription list
                    this.subscriptionList.Remove(unsubscribeList[i]);

                    // 2. Dispose subscription data
                    unsubscribeList[i].Dispose();
                }

                unsubscribeList.Clear();
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

        #region "helper classes"
        /// <summary>
        /// Helper class which contains the most common data for subscribed data events.
        /// </summary>
        private class SubscriptionData
        {
            #region "constructor & destructor"
            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionData"/> class.
            /// </summary>
            /// <param name="handle">Stores the handle of the subscription.</param>
            /// <param name="dataEntry">Stored the JHDataEntry for which the subscription was made for.</param>
            internal SubscriptionData(int handle, HeidenhainDNCLib.IJHDataEntry2 dataEntry)
            {
                this.Handle = handle;
                this.DataEntryObject = dataEntry;
                this.DataSelection = this.DataEntryObject.bstrFullName;
            }
            #endregion

            #region "properties"
            /// <summary>
            /// Gets a reference to the JHDataEntry object for which the subscription was made for.
            /// </summary>
            internal HeidenhainDNCLib.IJHDataEntry2 DataEntryObject { get; private set; }

            /// <summary>
            /// Gets the handle of the subscribed data.
            /// </summary>
            internal int Handle { get; private set; }

            /// <summary>
            /// Gets the data selection of the subscribed data.
            /// </summary>
            internal string DataSelection { get; private set; }

            /// <summary>
            /// Gets or sets the timestamp for the moment that the data was stored.
            /// </summary>
            internal DateTime TimeStamp { get; set; }

            /// <summary>
            /// Gets or sets the value of the subscribed data.
            /// </summary>
            internal string Value { get; set; }
            #endregion

            #region "event handler methods"
            /// <summary>
            /// Unsubscribe and release helper object here.
            /// </summary>
            public void Dispose()
            {
                if (this.DataEntryObject != null)
                {
                    this.DataEntryObject.UnSubscribe(this.Handle);
                    Marshal.ReleaseComObject(this.DataEntryObject);
                    this.DataEntryObject = null;
                }
            }
            #endregion
        }

#if RAW_COM_EVENTS
        /// <summary>
        /// This is a helper class to use the VTable event implementation of the _IJHDataAccess::OnData event.
        /// The VTable event implementation "_IJHDataAccess" is more effective than the
        /// IDispatch event implementation "_DJHDataAccess".
        /// </summary>
        private class DataListener : HeidenhainDNCLib._IJHDataAccessEvents
        {
        #region "fields"
            /// <summary>
            /// The parent dialog is stored, so the events can be passed.
            /// </summary>
            private DataAccess dialog;

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
            /// Initializes a new instance of the <see cref="DataListener"/> class.
            /// </summary>
            /// <param name="dialog">The parent dialog.</param>
            /// <param name="dataAccess">The data Access interface.</param>
            internal DataListener(DataAccess dialog, HeidenhainDNCLib.JHDataAccess dataAccess)
            {
                this.dialog = dialog;

                // get IConnectionPointContainer
                System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)dataAccess;

                Guid g = typeof(HeidenhainDNCLib._IJHDataAccessEvents).GUID;

                // get IConnectionPoint for the required interface
                icpc.FindConnectionPoint(ref g, out icp);

                // Advise the interface:
                // This means that the COM events are attached to the client implementation (a.k.a. as the event sink)
                // Here the event sink is this class.
                this.icp.Advise(this, out this.cookie);
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="DataListener"/> class.
            /// Implicitly stops the event listener.
            /// </summary>
            ~DataListener()
            {
                this.Stop();
            }
        #endregion

        #region "event handler methods"
            /// <summary>
            /// This method handles the event which is fired, if the subscribed data has changed on the control or the timer has expired.
            /// </summary>
            /// <param name="subscribeHandle">Number that was returned by the method IJHDataEntry::Subscribe() to identify the data without the need for a string compare.</param>
            /// <param name="timeStamp">Timestamp for the moment that the data was stored.  </param>
            /// <param name="varData">Object that contains the data value(s).</param>
            public void OnData(int subscribeHandle, DateTime timeStamp, object varData)
            {
                // Since this code is executed in the context of the COM event server (callback),
                // the call must be passed to the client thread.
                Debug.WriteLine("VTABLE::OnData - Handle: " + Convert.ToString(subscribeHandle) + " - " + varData.ToString());
                dialog.BeginInvoke(new OnDataHandler(this.dialog.OnDataImpl), subscribeHandle, timeStamp, varData);
            }
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
        #endregion
    }
}
