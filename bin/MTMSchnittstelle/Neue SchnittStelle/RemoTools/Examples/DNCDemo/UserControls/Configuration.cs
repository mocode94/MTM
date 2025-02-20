// ------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHConfiguration interface.
// </summary>
// ------------------------------------------------------------------------------------------------
namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHConfiguration interface.
    /// </summary>
    public partial class Configuration : UserControl
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
        private HeidenhainDNCLib.JHConfiguration configuration = null;

        /// <summary>
        /// Object of HeidenhainDNC JHVirtualMachine interface.
        /// </summary>
        private HeidenhainDNCLib.JHVirtualMachine virtualMachine = null;
        
        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Configuration(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Configuration_Disposed);

            this.machine = parentForm.Machine;
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
                // --- Get the virtual machine interface Object (will fail on real control) ---------
                this.virtualMachine = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHVIRTUALMACHINE);

                // --- Subscribe for the event(s) -------------------------------------------------
            }
            catch
            {
                // hide AxesPositionCloumn and Button 
                AxisInfoDataGridView.Columns[3].Visible = false;
                AxisInfoDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                SetPositionButton.Visible = false;
                this.MainTableLayoutPanel.SetRowSpan(this.ChannelInfoGroupBox, 1);
            }

            try
            {
                // --- Get the interface Object(s) ------------------------------------------------
                this.configuration = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHCONFIGURATION);

                // --- Subscribe for the event(s) -------------------------------------------------

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
        private void Configuration_Load(object sender, EventArgs e)
        {
            if (this.initOkay)
            {
                this.UpdateAxisInfo();
                this.UpdateChannelInfo();
            }
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Configuration_Disposed(object sender, EventArgs e)
        {
            if (this.configuration != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.configuration);
            }
            if (this.virtualMachine != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.virtualMachine);
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
        /// Update axis info on GUI.
        /// </summary>
        private void UpdateAxisInfo()
        {
            // --- fetch the infomations and put it to display (AxisInfo) -------------------------------
            HeidenhainDNCLib.IJHAxisInfoList axisInfoList = null;
            HeidenhainDNCLib.IJHAxisInfo axisInfo = null;
            try
            {
                AxisInfoDataGridView.SuspendLayout();
                AxisInfoDataGridView.Rows.Clear();

                axisInfoList = this.configuration.GetAxesInfo();
                for (int i = 0; i < axisInfoList.Count; i++)
                {
                    axisInfo = axisInfoList[i];

                    AxisInfoDataGridView.Rows.Add();
                    AxisInfoDataGridView.Rows[i].Cells[0].Value = axisInfo.bstrAxisName;
                    AxisInfoDataGridView.Rows[i].Cells[1].Value = axisInfo.lAxisId.ToString();
                    AxisInfoDataGridView.Rows[i].Cells[2].Value = axisInfo.axisType.ToString();

                    if (axisInfo != null)
                    {
                        Marshal.ReleaseComObject(axisInfo);
                        axisInfo = null;
                    }
                }

                AxisInfoDataGridView.ResumeLayout();
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
                if (axisInfoList != null)
                {
                    Marshal.ReleaseComObject(axisInfoList);
                }

                if (axisInfo != null)
                {
                    Marshal.ReleaseComObject(axisInfo);
                }
            }
        }

        /// <summary>
        /// Update channel info on GUI.
        /// </summary>
        private void UpdateChannelInfo()
        {
            // --- fetch the infomations and put it to display (ChannelInfo) ----------------------------
            HeidenhainDNCLib.IJHChannelInfoList channelInfoList = null;
            HeidenhainDNCLib.IJHChannelInfo channelInfo = null;
            HeidenhainDNCLib.IJHAxisInfoList axisInfoList = null;
            HeidenhainDNCLib.IJHAxisInfo axisInfo = null;
            try
            {
                ChannelInfoListView.BeginUpdate();
                ChannelInfoListView.Items.Clear();  // clear old items

                channelInfoList = this.configuration.GetChannelInfo();
                for (int i = 0; i < channelInfoList.Count; i++)
                {
                    channelInfo = channelInfoList[i];
                    axisInfoList = channelInfo.pAxisInfoList;

                    ListViewItem channelInfoItem = new ListViewItem(channelInfo.lChannelId.ToString());

                    // collect all axes of the channel in StringBuilder an show them as one single string (sb.ToString)
                    StringBuilder sb = new StringBuilder();
                    int axisCount = axisInfoList.Count;
                    for (int j = 0; j < axisCount; j++)
                    {
                        axisInfo = axisInfoList[j];
                        string axisName = axisInfo.bstrAxisName;

                        if (j < axisCount - 1)
                        {
                            axisName += ", ";
                        }

                        sb.Append(axisName);

                        if (axisInfo != null)
                        {
                            Marshal.ReleaseComObject(axisInfo);
                            axisInfo = null;
                        }
                    }

                    channelInfoItem.SubItems.Add(sb.ToString());

                    // Add items to ListView
                    ChannelInfoListView.Items.Add(channelInfoItem);

                    if (axisInfoList != null)
                    {
                        Marshal.ReleaseComObject(axisInfoList);
                        axisInfoList = null;
                    }

                    if (channelInfo != null)
                    {
                        Marshal.ReleaseComObject(channelInfo);
                        channelInfo = null;
                    }
                }

                ChannelInfoListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
                ChannelInfoListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                ChannelInfoListView.EndUpdate();
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
                if (channelInfoList != null)
                {
                    Marshal.ReleaseComObject(channelInfoList);
                }

                if (channelInfo != null)
                {
                    Marshal.ReleaseComObject(channelInfo);
                }

                if (axisInfoList != null)
                {
                    Marshal.ReleaseComObject(axisInfoList);
                }

                if (axisInfo != null)
                {
                    Marshal.ReleaseComObject(axisInfo);
                }
            }
        }
        #endregion

        private void SetPositionButton_Click(object sender, EventArgs e)
        {
            HeidenhainDNCLib.JHAxisPositionList positionList = new HeidenhainDNCLib.JHAxisPositionList();
            HeidenhainDNCLib.JHPositioningResult positionResult = null;
            HeidenhainDNCLib.JHAxisPosition position = null;
            
            try
            {
                foreach (DataGridViewRow row in AxisInfoDataGridView.Rows)
                {
                    if (row.Cells[3].Value != null)
                    {
                        position = new HeidenhainDNCLib.JHAxisPosition();
                        position.lAxisId = Convert.ToInt32(row.Cells[1].Value);
                        position.dPosition = Convert.ToDouble(row.Cells[3].Value);
                        positionList.AddItem(position);
                        Marshal.ReleaseComObject(position);
                    }
                }
                if (positionList.Count > 0)
                {
                    positionResult = virtualMachine.SetPosition(0, positionList);
                    Debug.WriteLine("Collision: " + positionResult.bCollision + ", EndSwitchTripped: " + positionResult.bEndSwitchTripped);
                }
                else
                {
                    MessageBox.Show(
                              "Please enter at least one axis position!",
                              "Attention!",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
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
                if (position != null)
                {
                    Marshal.ReleaseComObject(position);
                }
                if (positionList != null)
                {
                    Marshal.ReleaseComObject(positionList); 
                }
                if (positionResult != null)
                {
                    Marshal.ReleaseComObject(positionResult); 
                }
            }
        }
    }
}