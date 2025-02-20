// ------------------------------------------------------------------------------------------------
// <copyright file="AutomaticEvents.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the _IJHAutomaticEvents interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutomaticEvents"/> class.
    /// Copy some useful properties to local fields.
    /// </summary>
    /// <param name="parentForm">Reference to the parent Form.</param>
    public partial class AutomaticEvents : UserControl
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
        /// Object of HeidenhainDNC IJHAutomatic interface.
        /// </summary>
        private HeidenhainDNCLib.JHAutomatic automatic = null;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;

        // --- DNC Mode ---------------------------------------------------------------------------

        /// <summary>
        /// The current DNC mode.
        /// If mode is DNC_MODE_LOCAL, no remote program execution on the CNC is allowed.
        /// </summary>
        private string dncModeString = string.Empty;

        /// <summary>
        /// A counter how many times the DNC mode has changed.
        /// </summary>
        private int dncModeCounter = 0;

        // --- Program Status ---------------------------------------------------------------------

        /// <summary>
        /// The current program status.
        /// </summary>
        private string programStatusString = string.Empty;

        /// <summary>
        /// A counter how many times the program status has changed.
        /// </summary>
        private int programStatusCounter = 0;

        // --- Execution Mode ---------------------------------------------------------------------

        /// <summary>
        /// The channel of the shown execution mode.
        /// </summary>
        private string execModeChannelString = string.Empty;

        /// <summary>
        /// The current execution mode.
        /// </summary>
        private string execModeString = string.Empty;

        /// <summary>
        /// A counter how many times the execution mode has changed.
        /// </summary>
        private int execModeCounter = 0;

        // --- Tool Changed -----------------------------------------------------------------------

        /// <summary>
        /// The channel of the shown tool changed event.
        /// </summary>
        private string toolChangeChannelString = string.Empty;

        /// <summary>
        /// The time stamp of the current tool changed event.
        /// </summary>
        private string toolChangeTimeStampString = string.Empty;

        /// <summary>
        /// The ingoing tool of the shown tool changed event.
        /// </summary>
        private string toolChangeToolInString = string.Empty;

        /// <summary>
        /// The outgoing tool of the shown tool changed event.
        /// </summary>
        private string toolChangeToolOutString = string.Empty;

        /// <summary>
        /// A counter how many times the tool has been changed.
        /// </summary>
        private int toolChangeCounter = 0;

        // --- Execution Message ------------------------------------------------------------------

        /// <summary>
        /// The channel of the shown execution message.
        /// </summary>
        private string executionMessageChannelString = string.Empty;

        /// <summary>
        /// The execution message itself.
        /// </summary>
        private string executionMessageValueString = string.Empty;

        /// <summary>
        /// A additional execution message numeric value.
        /// Not supported by all controls.
        /// </summary>
        private string executionMessageNumericValueString = string.Empty;

        /// <summary>
        /// A counter how many times a execution message has arrived.
        /// </summary>
        private int executionMessageCounter = 0;

        // --- Program Changed --------------------------------------------------------------------

        /// <summary>
        /// The channel of the last program changed event.
        /// </summary>
        private string programChangedChannelString = string.Empty;

        /// <summary>
        /// The time stamp of the last program changed event.
        /// </summary>
        private string programChangedTimeStampString = string.Empty;

        /// <summary>
        /// The program name of the last program changed event.
        /// </summary>
        private string programChangedProgramName = string.Empty;

        /// <summary>
        /// A counter how many times the program has changed.
        /// </summary>
        private int programChangedCounter = 0;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticEvents"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public AutomaticEvents(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Automatic_Disposed);

            this.machine = parentForm.Machine;
        }
        #endregion

        #region "delegate"
        /// <summary>
        /// Program execution status change event.
        /// This event is fired when the CNC program execution status is changed.
        /// See the state diagrams below for the relation between the programEvent parameter and the program execution status.
        /// </summary>
        /// <param name="programEvent">Indication of the state change that occurred.</param>
        internal delegate void OnProgramStatusChangedHandler(HeidenhainDNCLib.DNC_EVT_PROGRAM programEvent);

        /// <summary>
        /// DNC mode change event.
        /// </summary>
        /// <param name="newDncMode">Notification of a change of the DNC mode.</param>
        internal delegate void OnDncModeChangedHandler(HeidenhainDNCLib.DNC_MODE newDncMode);

        /// <summary>
        /// This event is fired by the CNC on a specific part program command (CNC language dependant).
        /// The command may contain a string parameter as well as a numeric parameter.
        /// It can be used for communication between a part program and a remote application.
        /// </summary>
        /// <param name="channel">NC channel for which the notification is raised. (see GetChannelInfo).</param>
        /// <param name="varNumericValue">Numeric value (fixed or floating point) that was part of the NC message function call. (Not supported by all controls).</param>
        /// <param name="valueString">Text that was part of the NC message function call.</param>
        internal delegate void OnExecutionMessageHandler(int channel, object varNumericValue, string valueString);

        /// <summary>
        /// Execution switched to another program, subprogram or macro. 
        /// This event is fired by the CNC when a new program is selected or when program execution switches to a subprogram or a macro.
        /// </summary>
        /// <param name="channel">NC channel that executes the program. (see GetChannelInfo).</param>
        /// <param name="timeStamp">Point in time when the new program started executing.</param>
        /// <param name="newProgram">The full qualified path of the new part program that is being executed.</param>
        internal delegate void OnProgramChangedHandler(int channel, DateTime timeStamp, string newProgram);

        /// <summary>
        /// Tool in spindle changed event. 
        /// This event is fired by the CNC when a new tool is placed in the spindle or when the actual tool is removed from the spindle.
        /// </summary>
        /// <param name="channel">NC channel for which the notification is raised. (see GetChannelInfo).</param>
        /// <param name="pidToolOut">Tool ID that is removed from the spindle.</param>
        /// <param name="pidToolIn">Tool ID that is inserted into the spindle.</param>
        /// <param name="timeStamp">Time stamp for the moment that the tool change is completed.</param>
        internal delegate void OnToolChangedHandler(int channel, HeidenhainDNCLib.IJHToolId pidToolOut, HeidenhainDNCLib.IJHToolId pidToolIn, DateTime timeStamp);

        /// <summary>
        /// Execution mode changed event. 
        /// This event is fired by the CNC when the execution mode was changed (usually by the operator).
        /// </summary>
        /// <param name="channel">Specifies the NC channel in a multi-channel control.</param>
        /// <param name="executionMode">Newly entered execution mode.</param>
        internal delegate void OnExecutionModeChangedHandler(int channel, HeidenhainDNCLib.DNC_EXEC_MODE executionMode);
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
                this.automatic.OnDncModeChanged += new HeidenhainDNCLib._DJHAutomaticEvents_OnDncModeChangedEventHandler(this.Automatic_OnDncModeChanged);
                this.automatic.OnExecutionMessage += new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionMessageEventHandler(this.Automatic_OnExecutionMessage);
                this.automatic.OnExecutionModeChanged += new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionModeChangedEventHandler(this.Automatic_OnExecutionModeChanged);
                this.automatic.OnProgramChanged += new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramChangedEventHandler(this.Automatic_OnProgramChanged);
                this.automatic.OnProgramStatusChanged += new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramStatusChangedEventHandler(this.Automatic_OnProgramStatusChanged);
                this.automatic.OnToolChanged += new HeidenhainDNCLib._DJHAutomaticEvents_OnToolChangedEventHandler(this.Automatic_OnToolChanged);

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
        private void AutomaticEvents_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            // --- init DncMode -----------------------------------------------------------------------
            try
            {
                HeidenhainDNCLib.DNC_MODE dnc_mode = this.automatic.GetDncMode();
                this.dncModeString = dnc_mode.ToString();
            }
            catch
            {
                this.dncModeString = "<not supported>";
            }

            // --- init ProgramStatus -----------------------------------------------------------------
            try
            {
                HeidenhainDNCLib.DNC_STS_PROGRAM program_status = this.automatic.GetProgramStatus();
                this.programStatusString = program_status.ToString();
            }
            catch
            {
                this.programStatusString = "<not supported>";
            }

            // --- init ExecutionMode -----------------------------------------------------------------
            try
            {
                HeidenhainDNCLib.DNC_EXEC_MODE exec_mode = this.automatic.GetExecutionMode();
                ExecutionModeComboBox.DataSource = Enum.GetNames(typeof(HeidenhainDNCLib.DNC_EXEC_MODE));
                this.execModeString = exec_mode.ToString();
            }
            catch
            {
                this.execModeString = "<not supported>";
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
                // --- 1. unadvice all event handlers here ------------------------------------------------
                this.automatic.OnDncModeChanged -= new HeidenhainDNCLib._DJHAutomaticEvents_OnDncModeChangedEventHandler(this.Automatic_OnDncModeChanged);
                this.automatic.OnExecutionMessage -= new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionMessageEventHandler(this.Automatic_OnExecutionMessage);
                this.automatic.OnExecutionModeChanged -= new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionModeChangedEventHandler(this.Automatic_OnExecutionModeChanged);
                this.automatic.OnProgramChanged -= new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramChangedEventHandler(this.Automatic_OnProgramChanged);
                this.automatic.OnProgramStatusChanged -= new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramStatusChangedEventHandler(this.Automatic_OnProgramStatusChanged);
                this.automatic.OnToolChanged -= new HeidenhainDNCLib._DJHAutomaticEvents_OnToolChangedEventHandler(this.Automatic_OnToolChanged);

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
        /// Tool in spindle changed event. 
        /// This event is fired by the CNC when a new tool is placed in the spindle or when the actual tool is removed from the spindle.
        /// </summary>
        /// <param name="channel">NC channel for which the notification is raised. (see GetChannelInfo).</param>
        /// <param name="pidToolOut">Tool ID that is removed from the spindle.</param>
        /// <param name="pidToolIn">Tool ID that is inserted into the spindle.</param>
        /// <param name="timeStamp">Time stamp for the moment that the tool change is completed.</param>
        private void Automatic_OnToolChanged(int channel, HeidenhainDNCLib.IJHToolId pidToolOut, HeidenhainDNCLib.IJHToolId pidToolIn, DateTime timeStamp)
        {
            this.BeginInvoke(new OnToolChangedHandler(this.OnToolChanged), channel, pidToolOut, pidToolIn, timeStamp);
        }

        /// <summary>
        /// Tool in spindle changed event. 
        /// This event is fired by the CNC when a new tool is placed in the spindle or when the actual tool is removed from the spindle.
        /// </summary>
        /// <param name="channel">NC channel for which the notification is raised. (see GetChannelInfo).</param>
        /// <param name="pidToolOut">Tool ID that is removed from the spindle.</param>
        /// <param name="pidToolIn">Tool ID that is inserted into the spindle.</param>
        /// <param name="timeStamp">Time stamp for the moment that the tool change is completed.</param>
        private void OnToolChanged(int channel, HeidenhainDNCLib.IJHToolId pidToolOut, HeidenhainDNCLib.IJHToolId pidToolIn, DateTime timeStamp)
        {
            this.toolChangeChannelString = Convert.ToString(channel);
            this.toolChangeTimeStampString = timeStamp.ToShortDateString() + " - " + timeStamp.ToLongTimeString();
            this.toolChangeToolInString = Convert.ToString(pidToolIn.lToolId) + "." + Convert.ToString(pidToolIn.lIndex) + " (" + Convert.ToString(pidToolIn.lSpareToolId) + ")";
            this.toolChangeToolOutString = Convert.ToString(pidToolOut.lToolId) + "." + Convert.ToString(pidToolOut.lIndex) + " (" + Convert.ToString(pidToolOut.lSpareToolId) + ")";
            this.toolChangeCounter++;

            Debug.WriteLine("OnToolChanged --> channel: {0}, pidToolOut: {1}, pidToolIn: {2},  timeStamp: {3}", this.toolChangeChannelString, this.toolChangeTimeStampString, this.toolChangeToolInString, this.toolChangeToolOutString);
            this.UpdateGui();
        }

        /// <summary>
        /// Program execution status change event.
        /// This event is fired when the CNC program execution status is changed.
        /// See the state diagrams below for the relation between the programEvent parameter and the program execution status.
        /// </summary>
        /// <param name="programEvent">Indication of the state change that occurred.</param>
        private void Automatic_OnProgramStatusChanged(HeidenhainDNCLib.DNC_EVT_PROGRAM programEvent)
        {
            this.BeginInvoke(new OnProgramStatusChangedHandler(this.OnProgramStatusChanged), programEvent);
        }

        /// <summary>
        /// Program execution status change event.
        /// This event is fired when the CNC program execution status is changed.
        /// See the state diagrams below for the relation between the programEvent parameter and the program execution status.
        /// </summary>
        /// <param name="programEvent">Indication of the state change that occurred.</param>
        private void OnProgramStatusChanged(HeidenhainDNCLib.DNC_EVT_PROGRAM programEvent)
        {
            this.programStatusString = programEvent.ToString();
            this.programStatusCounter++;

            Debug.WriteLine("OnProgramStatusChanged --> programEvent: {0}", programEvent);
            this.UpdateGui();
        }

        /// <summary>
        /// Execution switched to another program, subprogram or macro. 
        /// This event is fired by the CNC when a new program is selected or when program execution switches to a subprogram or a macro.
        /// </summary>
        /// <param name="channel">NC channel that executes the program. (see GetChannelInfo).</param>
        /// <param name="timeStamp">Point in time when the new program started executing.</param>
        /// <param name="newProgram">The full qualified path of the new part program that is being executed.</param>
        private void Automatic_OnProgramChanged(int channel, DateTime timeStamp, string newProgram)
        {
            this.Invoke(new OnProgramChangedHandler(this.OnProgramChanged), channel, timeStamp, newProgram);
        }

        /// <summary>
        /// Execution switched to another program, subprogram or macro. 
        /// This event is fired by the CNC when a new program is selected or when program execution switches to a subprogram or a macro.
        /// </summary>
        /// <param name="channel">NC channel that executes the program. (see GetChannelInfo).</param>
        /// <param name="timeStamp">Point in time when the new program started executing.</param>
        /// <param name="newProgram">The full qualified path of the new part program that is being executed.</param>
        private void OnProgramChanged(int channel, DateTime timeStamp, string newProgram)
        {
            this.programChangedChannelString = channel.ToString();
            this.programChangedTimeStampString = timeStamp.ToShortDateString() + " - " + timeStamp.ToLongTimeString();
            this.programChangedProgramName = newProgram;
            this.programChangedCounter++;

            Debug.WriteLine("OnProgramChanged --> channel: {0}, timeStamp: {1}, newProgram: {2}", this.programChangedChannelString, this.programChangedTimeStampString, this.programChangedProgramName);
            this.UpdateGui();
        }

        /// <summary>
        /// Execution mode changed event. 
        /// This event is fired by the CNC when the execution mode was changed (usually by the operator).
        /// </summary>
        /// <param name="channel">Specifies the NC channel in a multi-channel control.</param>
        /// <param name="executionMode">Newly entered execution mode.</param>
        private void Automatic_OnExecutionModeChanged(int channel, HeidenhainDNCLib.DNC_EXEC_MODE executionMode)
        {
            this.BeginInvoke(new OnExecutionModeChangedHandler(this.OnExecutionModeChanged), channel, executionMode);
        }

        /// <summary>
        /// Execution mode changed event. 
        /// This event is fired by the CNC when the execution mode was changed (usually by the operator).
        /// </summary>
        /// <param name="channel">Specifies the NC channel in a multi-channel control.</param>
        /// <param name="executionMode">Newly entered execution mode.</param>
        private void OnExecutionModeChanged(int channel, HeidenhainDNCLib.DNC_EXEC_MODE executionMode)
        {
            if (channel == 0)
            {
                this.execModeChannelString = channel.ToString();
                this.execModeString = executionMode.ToString();
                this.execModeCounter++;

                Debug.WriteLine("OnExecutionModeChanged --> channel: {0}, executionMode {1}", this.execModeChannelString, this.execModeString);
                this.UpdateGui();
            }
        }

        /// <summary>
        /// This event is fired by the CNC on a specific part program command (CNC language dependant).
        /// The command may contain a string parameter as well as a numeric parameter.
        /// It can be used for communication between a part program and a remote application.
        /// </summary>
        /// <param name="channel">NC channel for which the notification is raised. (see GetChannelInfo).</param>
        /// <param name="varNumericValue">Numeric value (fixed or floating point) that was part of the NC message function call. (Not supported by all controls).</param>
        /// <param name="valueString">Text that was part of the NC message function call.</param>
        private void Automatic_OnExecutionMessage(int channel, object varNumericValue, string valueString)
        {
            this.BeginInvoke(new OnExecutionMessageHandler(this.OnExecutionMessage), channel, varNumericValue, valueString);
        }

        /// <summary>
        /// This event is fired by the CNC on a specific part program command (CNC language dependant).
        /// The command may contain a string parameter as well as a numeric parameter.
        /// It can be used for communication between a part program and a remote application.
        /// </summary>
        /// <param name="channel">NC channel for which the notification is raised. (see GetChannelInfo).</param>
        /// <param name="varNumericValue">Numeric value (fixed or floating point) that was part of the NC message function call. (Not supported by all controls).</param>
        /// <param name="valueString">Text that was part of the NC message function call.</param>
        private void OnExecutionMessage(int channel, object varNumericValue, string valueString)
        {
            this.executionMessageChannelString = channel.ToString();
            this.executionMessageValueString = valueString;
            this.executionMessageNumericValueString = varNumericValue.ToString();
            this.executionMessageCounter++;

            Debug.WriteLine("OnExecutionMessage --> channel: {0}, valueString {1}", this.executionMessageChannelString, this.executionMessageValueString);
            this.UpdateGui();
        }

        /// <summary>
        /// DNC mode change event.
        /// </summary>
        /// <param name="newDncMode">Notification of a change of the DNC mode.</param>
        private void Automatic_OnDncModeChanged(HeidenhainDNCLib.DNC_MODE newDncMode)
        {
            this.BeginInvoke(new OnDncModeChangedHandler(this.OnDncModeChanged), newDncMode);
        }

        /// <summary>
        /// DNC mode change event.
        /// </summary>
        /// <param name="newDncMode">Notification of a change of the DNC mode.</param>
        private void OnDncModeChanged(HeidenhainDNCLib.DNC_MODE newDncMode)
        {
            this.dncModeString = newDncMode.ToString();
            this.dncModeCounter++;

            Debug.WriteLine("OnDncModeChanged --> newDncMode: {0}", this.dncModeString);
            this.UpdateGui();
        }

        /// <summary>
        /// This method is used to force the NC into the requested execution mode.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetExecutionModeButton_Click(object sender, EventArgs e)
        {
            try
            {
                string execModeString = ExecutionModeComboBox.SelectedItem.ToString();
                HeidenhainDNCLib.DNC_EXEC_MODE execMode = (HeidenhainDNCLib.DNC_EXEC_MODE)Enum.Parse(typeof(HeidenhainDNCLib.DNC_EXEC_MODE), execModeString);

                this.automatic.SetExecutionMode(execMode);
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
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            // --- DncMode ------------------------------------------------------------------------------
            DncModeTextBox.Text = this.dncModeString;
            DncModeGroupBox.Text = "OnDncMode --> counter = " + this.dncModeCounter.ToString();

            // --- ProgramStatus ------------------------------------------------------------------------
            ProgramStatusTextBox.Text = this.programStatusString;
            ProgramStatusGroupBox.Text = "OnProgramStatus --> counter = " + this.programStatusCounter.ToString();

            // --- ExecutionMode ------------------------------------------------------------------------
            ExecutionModeTextBox.Text = this.execModeString;
            ExecutionModeGroupBox.Text = "OnExecutionMode (Channel 0) --> counter = " + this.execModeCounter.ToString();

            // --- ToolChanged --------------------------------------------------------------------------
            TCChannelTextBox.Text = this.toolChangeChannelString;
            TCTimeStampTextBox.Text = this.toolChangeTimeStampString;
            TCToolInTextBox.Text = this.toolChangeToolInString;
            TCToolOutTextBox.Text = this.toolChangeToolOutString;
            ToolChangedGroupBox.Text = "OnToolChanged --> counter = " + this.toolChangeCounter;

            // --- ExecutionMessage (FN 38) -------------------------------------------------------------
            EMChannelTextBox.Text = this.executionMessageChannelString;
            EMValueTextBox.Text = this.executionMessageValueString;
            EMNumerricValueTextBox.Text = this.executionMessageNumericValueString;
            OnExecutionMessageGroupBox.Text = "OnExecutionMessage --> counter = " + this.executionMessageCounter.ToString();

            // --- ProgramChanged -----------------------------------------------------------------------
            PCChannelTextBox.Text = this.programChangedChannelString;
            PCTimeStampTextBox.Text = this.programChangedTimeStampString;
            PCNCProgTextBox.Text = this.programChangedProgramName;
            OnProgramChangedGroupBox.Text = "OnProgramChanged --> counter = " + this.programChangedCounter.ToString();
        }
        #endregion
    }
}
