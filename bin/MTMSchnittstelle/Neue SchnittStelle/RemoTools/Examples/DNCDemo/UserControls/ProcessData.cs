// ------------------------------------------------------------------------------------------------
// <copyright file="ProcessData.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHProcessData interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHProcessData interface.
    /// </summary>
    public partial class ProcessData : UserControl
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
        /// Object of HeidenhainDNC IJHProcessData interface.
        /// </summary>
        private HeidenhainDNCLib.JHProcessData processData = null;

        /// <summary>
        /// Object of HeidenhainDNC IJHConfiguration interface.
        /// This interface is used here to get the ID and the NAME of the spindle.
        /// </summary>
        private HeidenhainDNCLib.JHConfiguration configuration = null;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        /// <summary>
        /// The name of the spindle on the connected control.
        /// </summary>
        private string spindleName = string.Empty;

        /// <summary>
        /// The Id of the spindle on the connected control. 
        /// </summary>
        private int spindleId = -1;

        /// <summary>
        /// This is the text prefix for the spindle group box.
        /// </summary>
        private string spindleTextforGroupBox = string.Empty;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessData"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public ProcessData(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.ProcessData_Disposed);

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
                // --- get interfaces here ----------------------------------------------------------------
                this.processData = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHPROCESSDATA);
                this.configuration = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHCONFIGURATION);

                //// --- Subscribe for the event(s) -------------------------------------------------

                this.spindleTextforGroupBox = SpindleRunningTimeGroupBox.Text;

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
        private void UCProcessData_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            this.UpdateGui();
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ProcessData_Disposed(object sender, EventArgs e)
        {
            if (this.processData != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.processData);
            }

            if (this.configuration != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.configuration);
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

        /// <summary>
        /// Explicitly refresh GUI.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            this.UpdateGui();
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            try
            {
                this.GetSpindelIdAndName();

                object hours = new object();
                object minutes = new object();

                // --- NC uptime --------------------------------------------------------------------------
                this.processData.GetNcUpTime(ref hours, ref minutes);
                NcUpTimeTextBox.Text = hours.ToString() + ":" + (Convert.ToInt32(minutes) > 9 ? minutes.ToString() : ("0" + minutes.ToString()));

                // --- Machine uptime ---------------------------------------------------------------------
                this.processData.GetMachineUpTime(ref hours, ref minutes);
                MachineUpTimeTextBox.Text = hours.ToString() + ":" + (Convert.ToInt32(minutes) > 9 ? minutes.ToString() : ("0" + minutes.ToString()));

                // --- Machine running time ---------------------------------------------------------------
                this.processData.GetMachineRunningTime(ref hours, ref minutes);
                MachineRunningTimeTextBox.Text = hours.ToString() + ":" + (Convert.ToInt32(minutes) > 9 ? minutes.ToString() : ("0" + minutes.ToString()));

                // --- Spindle running time ---------------------------------------------------------------
                if (this.spindleId >= 0)
                {
                    SpindleRunningTimeGroupBox.Enabled = true;
                    SpindleRunningTimeGroupBox.Text = this.spindleTextforGroupBox + " --> " + this.spindleName;

                    // Set the PLC timer number for the first parameter.
                    // in PLC basic program 2 = timer of spindle 1, 3 = timer of spindle 2.
                    // Please contact the machine builder if you don't get the correct spindel running times using the GetSpindleRunningTime() function.
                    this.processData.GetSpindleRunningTime(2, ref hours, ref minutes);
                    SpindleRunningTimeTextBox.Text = hours.ToString() + ":" + (Convert.ToInt32(minutes) > 9 ? minutes.ToString() : ("0" + minutes.ToString()));
                }
                else
                {
                    SpindleRunningTimeGroupBox.Enabled = false;
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
        /// Helper function to get the Id and the name of the spindle on control.
        /// </summary>
        private void GetSpindelIdAndName()
        {
            HeidenhainDNCLib.IJHAxisInfoList axisInfoList = null;
            HeidenhainDNCLib.IJHAxisInfo axisInfo = null;

            try
            {
                axisInfoList = this.configuration.GetAxesInfo();

                for (int i = 0; i < axisInfoList.Count; i++)
                {
                    axisInfo = axisInfoList[i];

                    // get the first spindle
                    if (axisInfo.axisType == HeidenhainDNCLib.DNC_AXISTYPE.DNC_AXISTYPE_SPINDLE)
                    {
                        this.spindleId = axisInfo.lAxisId;
                        this.spindleName = axisInfo.bstrAxisName;
                        break;
                    }

                    if (axisInfo != null)
                    {
                        Marshal.ReleaseComObject(axisInfo);
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
                if (axisInfoList != null)
                {
                    Marshal.ReleaseComObject(axisInfoList);
                }
            }
        }
        #endregion
    }
}
