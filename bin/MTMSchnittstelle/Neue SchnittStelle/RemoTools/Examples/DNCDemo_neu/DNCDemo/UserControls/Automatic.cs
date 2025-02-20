// ------------------------------------------------------------------------------------------------
// <copyright file="Automatic.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHAutomatic interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHVersion interface.
    /// </summary>
    public partial class Automatic : UserControl
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
        private HeidenhainDNCLib.JHAutomatic automatic = null;

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
        /// Initializes a new instance of the <see cref="Automatic"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Automatic(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Automatic_Disposed);

            this.cncType = parentForm.CncType;
            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
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
                this.automatic = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAUTOMATIC);

                //// --- Subscribe for the event(s) -------------------------------------------------

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
        private void Automatic_Load(object sender, EventArgs e)
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
        private void Automatic_Disposed(object sender, EventArgs e)
        {
            if (this.automatic != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.automatic);
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
        /// Get the override values for feed, speed and rapid from control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void GetOverrideInfoButton_Click(object sender, EventArgs e)
        {
            this.GetOverride();
        }

        /// <summary>
        /// Set the feed override value on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetFeedButton_Click(object sender, EventArgs e)
        {
            try
            {
                int feed = Convert.ToInt32(FeedTextBox.Text);
                this.automatic.SetOverrideFeed(feed);
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
        /// Set the speed override value on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetSpeedButton_Click(object sender, EventArgs e)
        {
            try
            {
                int speed = Convert.ToInt32(SpeedTextBox.Text);
                this.automatic.SetOverrideSpeed(speed);
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
        /// Set the rapid override value on control.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetRapidButton_Click(object sender, EventArgs e)
        {
            try
            {
                int rapid = Convert.ToInt32(RapidTextBox.Text);
                this.automatic.SetOverrideRapid(rapid);
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
        /// Select program on controls.
        /// Only possible if control is in automatic mode.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SelectProgramButton_Click(object sender, EventArgs e)
        {
            try
            {
                int channel = Convert.ToInt32(ChannelNumericUpDown.Value);
                string programName = SelectProgramTextBox.Text;
                int startBlockNumber = Convert.ToInt32(StartBlockNumberNumericUpDown.Value);

                this.automatic.SelectProgram(channel, programName, startBlockNumber);
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
        /// Start or restart the specified program with optionally a list of break points.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StartProgramButton_Click(object sender, EventArgs e)
        {
            try
            {
                string programName = SelectProgramTextBox.Text;
                this.automatic.StartProgram(programName);
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
        /// Start or restart the program selected on contorl with optionally a list of break points.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void NCStartButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.automatic.StartProgram(string.Empty);
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
        /// Stop executing the active NC part program. 
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StopProgramButton_Click(object sender, EventArgs e)
        {
            try
            {
                int channel = Convert.ToInt32(ChannelNumericUpDown.Value);

                this.automatic.StopProgram(channel, false);
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
        /// Cancels the execution of a stopped NC program. 
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CancelProgramButton_Click(object sender, EventArgs e)
        {
            try
            {
                int channel = Convert.ToInt32(ChannelNumericUpDown.Value);

                this.automatic.CancelProgram(channel);
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
        /// Get the current position of the tool tip in the WCS.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void GetCutterLocationButton_Click(object sender, EventArgs e)
        {
            this.GetCutterLocation();
        }

        /// <summary>
        /// Get the current exceutionPoint
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void GetExecutionPointButton_Click(object sender, EventArgs e)
        {
            this.GetExecutionPoint();
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            this.GetOverride();
            this.GetCutterLocation();
            this.GetExecutionPoint();
        }

        /// <summary>
        /// Get the current position of the tool tip in the WCS.
        /// </summary>
        private void GetCutterLocation()
        {
            HeidenhainDNCLib.IJHCutterLocationList cutterLocationList = null;
            HeidenhainDNCLib.IJHCutterLocation cutterLocation = null;

            try
            {
                int channel = Convert.ToInt32(ChannelNumericUpDown.Value);

                cutterLocationList = this.automatic.GetCutterLocation(channel);

                CutterLocationListView.BeginUpdate();
                CutterLocationListView.Items.Clear();
                for (int i = 0; i < cutterLocationList.Count; i++)
                {
                    cutterLocation = cutterLocationList[i];

                    ListViewItem item = new ListViewItem(cutterLocation.bstrCoordinateName);
                    item.SubItems.Add(cutterLocation.dPosition.ToString("0.000"));
                    string unit = cutterLocation.bIsInch ? "inch" : "mm";
                    item.SubItems.Add(unit);

                    CutterLocationListView.Items.Add(item);

                    if (cutterLocation != null)
                    {
                        Marshal.ReleaseComObject(cutterLocation);
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
                CutterLocationListView.EndUpdate();

                if (cutterLocationList != null)
                {
                    Marshal.ReleaseComObject(cutterLocationList);
                }

                if (cutterLocation != null)
                {
                    Marshal.ReleaseComObject(cutterLocation);
                }
            }
        }

        /// <summary>
        /// Get the current exceutionPoint
        /// </summary>
        private void GetExecutionPoint()
        {
            HeidenhainDNCLib.IJHProgramPositionList programPositionList = null;
            HeidenhainDNCLib.IJHProgramPosition programPosition = null;
            object selectedProgram = null;            

            string executionPoint = string.Empty;

            try
            {
                programPositionList = this.automatic.GetExecutionPoint(ref selectedProgram);
                
                if (selectedProgram == null || selectedProgram.ToString() == string.Empty)
                {
                    executionPoint = "<no program selected>";
                }
                else
                {
                    executionPoint = selectedProgram.ToString();
                }
                
                for (int i = 0; i < programPositionList.Count; i++)
                {
                    programPosition = programPositionList[i];

                    executionPoint += System.Environment.NewLine 
                        + "[" + programPosition.blockNumber.ToString() + "] "
                        + programPosition.programName + ": "
                        + programPosition.blockContent;

                    if (programPosition != null)
                    {
                        Marshal.ReleaseComObject(programPosition);
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
                ExecutionPointTextBox.Text = executionPoint;

                if (programPositionList != null)
                {
                    Marshal.ReleaseComObject(programPositionList);
                }

                if (programPosition != null)
                {
                    Marshal.ReleaseComObject(programPosition);
                }
            }

        }

        /// <summary>
        /// Gets the override values for fee, speed and rapid from control and updates GUI.
        /// </summary>
        private void GetOverride()
        {
            try
            {
                object feed = null;
                object speed = null;
                object rapid = null;

                this.automatic.GetOverrideInfo(ref feed, ref speed, ref rapid);

                FeedTextBox.Text = Convert.ToString(feed);
                SpeedTextBox.Text = Convert.ToString(speed);
                RapidTextBox.Text = Convert.ToString(rapid);
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
    }
}
