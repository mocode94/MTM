// ------------------------------------------------------------------------------------------------
// <copyright file="AxesPositionStreaming.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHAxesPositionStreaming interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHAxesPositionStreaming and the IJHVirtualMachine interface.
    /// </summary>
    public partial class AxesPositionStreaming : UserControl
    {
        #region "fields"
        /// <summary>
        /// Count for the log columns (without axes position columns).
        /// </summary>
        private const int SampleInfoColumnCount = 7;

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
        /// Object of HeidenhainDNC IJHAxesPositionStreaming interface.
        /// </summary>
        private HeidenhainDNCLib.JHAxesPositionStreaming axesPositionStreaming = null;

        /// <summary>
        /// Object of HeidenhainDNC IJHVirtualMachine interface.
        /// </summary>
        private HeidenhainDNCLib.JHVirtualMachine virtualMachine = null;

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
        /// List of a variable count of axis position lists.
        /// </summary>
        private HeidenhainDNCLib.IJHAxisPositionListList axisPositionListList = null;

        /// <summary>
        /// Parameter list for JHAxesPositionStreaming::GetAxesPositionSamples().
        /// </summary>
        private AxesPositionSample axesPositionSample = null;

        /// <summary>
        /// List with assignment between "axis number" and "axis name".
        /// The source for this information is IJHConfiguration::AxesInfo().
        /// </summary>
        private Dictionary<int, string> axisNameAssignment = new Dictionary<int, string>();

        /// <summary>
        /// List of axes IDs and axes positions.
        /// </summary>
        private Dictionary<int, double> axisPositions = new Dictionary<int, double>();

        /// <summary>
        /// Logging Class (AxesPositionSamples).
        /// </summary>
        private Logging log = new Logging();

        /// <summary>
        /// Indicating whether the streaming is active or not.
        /// </summary>
        private bool streamingIsActivated = false;

        /// <summary>
        /// If only the by GetAxesPositionSamples transmitted positions becomes written (CloseGapsCheckBox not set)
        /// then an initial value has to be written to the log file.
        /// </summary>
        private bool initialPositionsLogged = false;

        /// <summary>
        /// The last stored time stamp value from GetAxesPositionSamples() method. Stored at TurboFactorMeasuringTimer.Tick().
        /// </summary>
        private DateTime lastTimeStamp = DateTime.MinValue;

        /// <summary>
        /// The simple mean average filter.
        /// </summary>
        private SimpleAverageFilter turboFactor = new SimpleAverageFilter(10);

        /// <summary>
        /// Do not use System.Windows.Form.Timer.
        /// To simplify synchronization, these timers run in the gui thread. This can cause a lot
        /// of jitter in the tick event. Prefer System.Timers.Timer or System.Threading.Timer.
        /// These timers are executed in a separate thread. If you want to write to the gui,
        /// you need the same mechanisms (Invoke, BeginInvoke) as for example with the background worker or other thread types.
        /// </summary>
        private System.Timers.Timer turboFactorMeasuringTimer = null;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="AxesPositionStreaming"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public AxesPositionStreaming(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.AxesPositionStreaming_Disposed);

            this.machine = parentForm.Machine;
            this.cncType = parentForm.CncType;
            this.isNck = parentForm.IsNck;

            this.turboFactorMeasuringTimer = new System.Timers.Timer(500);
            this.turboFactorMeasuringTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.TurboFactorMeasuringTimer_Elapsed);
        }
        #endregion

        #region "delegate"
        /// <summary>
        /// Delegate for _IJHAxesPositionStreamingEvents::OnAxesPositionsAvailable() event handler.
        /// </summary>
        public delegate void OnAxesPositionsAvailableHandler();
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
                // Must be before GetInterface DNC_INTERFACE_JHAXESPOSITIONSTREAMING for iTNC 530 (See OnAxesPositionsAvailable annotation)
                this.virtualMachine = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHVIRTUALMACHINE);

                this.axesPositionStreaming = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAXESPOSITIONSTREAMING);
                this.axesPositionStreaming.OnAxesPositionsAvailable += new HeidenhainDNCLib._DJHAxesPositionStreamingEvents_OnAxesPositionsAvailableEventHandler(this.AxesPositionStreaming_OnAxesPositionsAvailable);

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

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            // --- display axis positions in list view --------------------------------------------
            PositionsListView.BeginUpdate();
            PositionsListView.Items.Clear();
            var sortedDict = from entry in this.axisPositions orderby entry.Value ascending select entry;
            foreach (KeyValuePair<int, double> item in this.axisPositions)
            {
                string axisName = null;
                this.axisNameAssignment.TryGetValue(item.Key, out axisName);
                string position = item.Value.ToString("0.0000").Replace(".", MainForm.DecimalSeparator);
                PositionsListView.Items.Add(axisName).SubItems.Add(position);
            }

            PositionsListView.EndUpdate();

            // --- display additional informations ------------------------------------------------
            if (axisPositionListList != null)
            {
                int sampleSize = this.axisPositionListList.Count;
                LineNrLabel.Text = this.axesPositionSample.GetLineNrAt(sampleSize - 1);
                BlockEndLabel.Text = this.axesPositionSample.GetIsBlockEndpointAt(sampleSize - 1);
                TimeStampLabel.Text = this.axesPositionSample.GetTimeStampAt(sampleSize - 1);
                FeedModeLabel.Text = this.axesPositionSample.GetFeedModeAt(sampleSize - 1);
                SpindleStateLabel.Text = this.axesPositionSample.GetSpindleStateAt(sampleSize - 1);
                MotionTypeLabel.Text = this.axesPositionSample.GetMotionTypeAt(sampleSize - 1);
            }

            MeasuredTurboFactorTextBox.Text = this.turboFactor.ToString();
        }

        /// <summary>
        /// Initializes axisNameAssignment dictionary with the axisID and the axis name.
        /// </summary>
        private void InitAxisNames()
        {
            HeidenhainDNCLib.JHConfiguration configuration = null;
            HeidenhainDNCLib.IJHAxisInfoList axisInfoList = null;
            HeidenhainDNCLib.IJHAxisInfo axisInfo = null;
            try
            {
                configuration = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHCONFIGURATION);
                axisInfoList = configuration.GetAxesInfo();

                for (int i = 0; i < axisInfoList.Count; i++)
                {
                    axisInfo = axisInfoList[i];
                    this.axisNameAssignment.Add(axisInfo.lAxisId, axisInfo.bstrAxisName);
                }

                if (axisInfo != null)
                {
                    Marshal.ReleaseComObject(axisInfo);
                    axisInfo = null;
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
                if (configuration != null)
                {
                    Marshal.ReleaseComObject(configuration);
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

        /// <summary>
        /// Initializes MinimumSampleRateTextBox text box with current sample rate.
        /// </summary>
        private void InitMinSampleRate()
        {
            try
            {
                double sampleRate = this.axesPositionStreaming.GetSampleRate();
                sampleRate *= 1000;  // seconds to miliseconds
                MinimumSampleRateTextBox.Text = sampleRate.ToString() + " ms";
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
        /// Initializes LoggingTypeComboBox with all available logging types.
        /// </summary>
        private void InitLoggingType()
        {
            string[] arrLoggingType = Enum.GetNames(typeof(HeidenhainDNCLib.DNC_POSSAMPLES_LOGGING_TYPE));
            LoggingTypeComboBox.DataSource = arrLoggingType;

            this.SetLoggingType();
        }

        /// <summary>
        /// Initializes NotificationSampleLimitComboBox with some useful values.
        /// </summary>
        private void InitNotificationSampleLimit()
        {
            const int MIN = 1;
            const int MAX = 64;
            const int STEP = 1;
            List<string> sampleLimit = new List<string>();
            for (int i = MIN; i <= MAX; i += STEP)
            {
                sampleLimit.Add(i.ToString());
            }

            NotificationSampleLimitComboBox.DataSource = sampleLimit;

            this.SetNotificationSampleLimit();
        }

        /// <summary>
        /// Initializes NotificationTimeLimitTextBox with the standard values of the specific controls.
        /// </summary>
        private void InitNotificationTimeLimit()
        {
            // standard value for NCK based controls
            double notificationTimeLimit = 0.0;
            if (this.cncType == HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_ITNC)
            {
                // standard value for iTNC530
                notificationTimeLimit = 0.5;
            }

            NotificationTimeLimitTextBox.Text = notificationTimeLimit.ToString("0.000").Replace(".", MainForm.DecimalSeparator);

            this.SetNotificationTimeLimit();
        }

        /// <summary>
        /// Initializes FilterTCPDataTextBox with 0.0000.
        /// </summary>
        private void InitFilterTcpData()
        {
            FilterTcpDataTextBox.Text = "0" + MainForm.DecimalSeparator + "000";

            this.SetFilterTcp();
        }

        /// <summary>
        /// Initializes FilterAxesDeltaFlowLayout with all axes in the system and marks the axes in channel 0.
        /// </summary>
        private void InitFilterAxesDelta()
        {
            HeidenhainDNCLib.IJHConfiguration configuration = null;
            HeidenhainDNCLib.IJHAxisInfoList axisInfoList = null;
            HeidenhainDNCLib.IJHAxisInfo axisInfo = null;
            HeidenhainDNCLib.IJHChannelInfoList channelInfoList = null;
            HeidenhainDNCLib.IJHChannelInfo channelInfo = null;
            HeidenhainDNCLib.JHAxisInfoList axisInChannelList = null;
            HeidenhainDNCLib.JHAxisInfo axisInChannel = null;
            try
            {
                Dictionary<int, string> axisInfoDict = new Dictionary<int, string>();
                List<int> channelInfoIntList = new List<int>();

                // --- Fetch informations from TNC
                configuration = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHCONFIGURATION);

                // Get axis info
                axisInfoList = configuration.GetAxesInfo();
                for (int i = 0; i < axisInfoList.Count; i++)
                {
                    axisInfo = axisInfoList[i];

                    axisInfoDict.Add(axisInfo.lAxisId, axisInfo.bstrAxisName);

                    Marshal.ReleaseComObject(axisInfo);
                }

                // Get channel info for channel 0
                channelInfoList = configuration.GetChannelInfo();
                channelInfo = channelInfoList[0];
                axisInChannelList = channelInfo.pAxisInfoList;
                for (int i = 0; i < axisInChannelList.Count; i++)
                {
                    axisInChannel = axisInChannelList[i];

                    channelInfoIntList.Add(axisInChannel.lAxisId);

                    Marshal.ReleaseComObject(axisInChannel);
                }

                // Create user controls
                foreach (KeyValuePair<int, string> axis in axisInfoDict)
                {
                    bool isAxisInChannel = false;
                    if (channelInfoIntList.Contains(axis.Key))
                    {
                        isAxisInChannel = true;
                    }

                    FilterAxesDeltaFlowLayoutPanel.Controls.Add(new UserControls.AxisPosition(axis.Key, axis.Value, 1.0, isAxisInChannel));
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
                if (configuration != null)
                {
                    Marshal.ReleaseComObject(configuration);
                }

                if (axisInfoList != null)
                {
                    Marshal.ReleaseComObject(axisInfoList);
                }

                if (axisInfo != null)
                {
                    Marshal.ReleaseComObject(axisInfo);
                }

                if (channelInfoList != null)
                {
                    Marshal.ReleaseComObject(channelInfoList);
                }

                if (channelInfo != null)
                {
                    Marshal.ReleaseComObject(channelInfo);
                }

                if (axisInChannelList != null)
                {
                    Marshal.ReleaseComObject(axisInChannelList);
                }

                if (axisInChannel != null)
                {
                    Marshal.ReleaseComObject(axisInChannel);
                }
            }
        }

        /// <summary>
        /// Initializes axis positions.
        /// This is only necessary on NCK based controls. This controls usually only transmits positions if they have changes.
        /// See IJHAxesPositionStreaming documentation for more information about logging filters.
        /// </summary>
        private void InitPositions()
        {
            this.axisPositions.Clear();

            HeidenhainDNCLib.IJHAxisPositionList axisPositionList = null;
            HeidenhainDNCLib.IJHAxisPosition axisPosition = null;

            try
            {
                // Get position for all axis (including auxiliary axis)
                axisPositionList = this.virtualMachine.GetPosition(-1);
                for (int i = 0; i < axisPositionList.Count; i++)
                {
                    axisPosition = axisPositionList[i];

                    int axisId = axisPosition.lAxisId;
                    double position = axisPosition.dPosition;

                    this.axisPositions.Add(axisId, position);

                    if (axisPosition != null)
                    {
                        Marshal.ReleaseComObject(axisPosition);
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
                if (axisPositionList != null)
                {
                    Marshal.ReleaseComObject(axisPositionList);
                }

                if (axisPosition != null)
                {
                    Marshal.ReleaseComObject(axisPosition);
                }
            }
        }

        /// <summary>
        /// Initializes the turbo factor.
        /// </summary>
        private void InitTurboFactor()
        {
            TurboFactorTextBox.Text = "2" + MainForm.DecimalSeparator + "0";
        }

        /// <summary>
        /// Specify the desired maximum time before the notification event OnAxesPositionsAvailable()
        /// is fired, even if the buffer is not filled yet.
        /// </summary>
        private void SetNotificationTimeLimit()
        {
            try
            {
                // get notification time limit from text box
                double notificationTimeLimit = Convert.ToDouble(NotificationTimeLimitTextBox.Text);

                // set notification time limit
                this.axesPositionStreaming.SetNotificationTimeLimit(notificationTimeLimit);
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
        /// Specify a vector length (3D delta way) for the TCP movement.
        /// A sample is stored into the buffer if the TCP has moved further than the defined vector length.
        /// </summary>
        private void SetFilterTcp()
        {
            try
            {
                // get filter Tcp data from text box
                double setFilterTcpData = Convert.ToDouble(FilterTcpDataTextBox.Text);

                // set filter Tcp data
                this.axesPositionStreaming.SetFilterTcpDelta(setFilterTcpData);
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
        /// Specify the number of samples in the buffer after which the notification event OnAxesPositionsAvailable() is fired.
        /// </summary>
        private void SetNotificationSampleLimit()
        {
            try
            {
                // Get notification sample limit from combo box
                string notificationSampleLimitString = NotificationSampleLimitComboBox.Text;
                int notificationSampleLimit = Convert.ToInt32(notificationSampleLimitString);

                // Init axes position container with the correct sample size
                this.axesPositionSample = null;
                this.axesPositionSample = new AxesPositionSample(Math.Abs(notificationSampleLimit));

                // Set notification sample limit on control
                this.axesPositionStreaming.SetNotificationSampleLimit(notificationSampleLimit);
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
        /// This method configures the type of logging that is done.
        /// To minimize the amount of data stored and thus transferred, look at the documentation
        /// to find the correct logging type for your application.
        /// </summary>
        private void SetLoggingType()
        {
            try
            {
                // get logging type from combo box
                string loggingTypeString = LoggingTypeComboBox.SelectedItem.ToString();
                HeidenhainDNCLib.DNC_POSSAMPLES_LOGGING_TYPE loggingType;
                loggingType = (HeidenhainDNCLib.DNC_POSSAMPLES_LOGGING_TYPE)Enum.Parse(typeof(HeidenhainDNCLib.DNC_POSSAMPLES_LOGGING_TYPE), loggingTypeString);

                // set logging type
                this.axesPositionStreaming.SetLoggingType(loggingType);
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
        /// Initializes Synchronous Mode.
        /// </summary>
        private void SetSynchronousMode()
        {
            try
            {
                // Activates the synchronous mode on control. The effect is that the sample buffer on the control can only be filled with 1 sample. 
                // Then the execution stops until the sample is fetched.
                this.axesPositionStreaming.SetNotificationSampleLimit(-1);  // Set notification sample limit on control by using the "magic" parameter value -1
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
        /// Starts streaming.
        /// </summary>
        private void StartStreaming()
        {
            try
            {
                if (!this.streamingIsActivated)
                {
                    // only needed for NCK based controls, because only changed positions are transmitted at every sample
                    // iTNC transmits all positions at every sample
                    if (this.isNck)
                    {
                        this.InitPositions();
                    }

                    if (LoggingCheckBox.Checked)
                    {
                        if (string.IsNullOrEmpty(LogFilePathTextBox.Text))
                        {
                            MessageBox.Show("Please select a log file first!", "Operating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        this.initialPositionsLogged = false;
                        this.log.Logfile = LogFilePathTextBox.Text;
                        this.log.HeadLine = this.CreateHeadLine();
                        this.log.Start(false);
                    }

                    ConfigurationGroupBox.Enabled = false;
                    StartButton.Enabled = false;
                    StopButton.Enabled = true;

                    LoggingCheckBox.Enabled = false;
                    LogFilePathTextBox.Enabled = false;
                    SelectLogfileButton.Enabled = false;
                    if (this.isNck)
                    {
                        FillGapsCheckBox.Enabled = false;
                    }

                    UpdateGuiTimer.Start();

                    this.axesPositionStreaming.StartStreaming(0);
                    this.streamingIsActivated = true;
                }
            }
            catch (COMException cex)
            {
                // usually DNC_E_NO_STREAMING_ACTIVE or DNC32_E_NOT_CONN
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
        /// Stop streaming.
        /// </summary>
        private void StopStreaming()
        {
            try
            {
                if (this.streamingIsActivated)
                {
                    UpdateGuiTimer.Stop();
                    this.axesPositionStreaming.StopStreaming(0);

                    ConfigurationGroupBox.Enabled = true;
                    StartButton.Enabled = true;
                    StopButton.Enabled = false;

                    LoggingCheckBox.Enabled = true;
                    LogFilePathTextBox.Enabled = true;
                    SelectLogfileButton.Enabled = true;
                    if (this.isNck)
                    {
                        FillGapsCheckBox.Enabled = true;
                    }

                    this.axisPositions.Clear();
                    PositionsListView.Items.Clear();

                    this.streamingIsActivated = false;

                    this.log.Stop();
                }
            }
            catch (COMException cex)
            {
                // usually DNC_E_NO_STREAMING_ACTIVE or DNC32_E_NOT_CONN
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
        /// Create head line for the output CSV file.
        /// </summary>
        /// <returns>Column headers.</returns>
        private string[] CreateHeadLine()
        {
            int arrayIndex = 0;
            string[] headLine = new string[SampleInfoColumnCount + this.axisNameAssignment.Count];

            string timeStamp = "TimeStamp";
            string sampleIndex = "Index";
            string lineNr = "LineNr";
            string feedMode = "FeedMode";
            string spindleState = "SpindleState";
            string motionType = "MotionType";
            string blockEndpoint = "BlockEndpoint";

            // insert lieding white spaces
            headLine[arrayIndex++] = timeStamp.PadLeft(23);
            headLine[arrayIndex++] = sampleIndex.PadLeft(6);
            headLine[arrayIndex++] = lineNr.PadLeft(8);
            headLine[arrayIndex++] = feedMode.PadLeft(16);
            headLine[arrayIndex++] = spindleState.PadLeft(31);
            headLine[arrayIndex++] = motionType.PadLeft(30);
            headLine[arrayIndex++] = blockEndpoint.PadLeft(5);

            // create header text for the axis <axisName-axisId>.
            foreach (KeyValuePair<int, string> axis in this.axisNameAssignment)
            {
                string axisName = string.Empty;
                this.axisNameAssignment.TryGetValue(axis.Key, out axisName);
                string headerText = axisName + "-" + axis.Key;
                headLine[arrayIndex++] = headerText.PadLeft(9);
            }

            return headLine;
        }

        /// <summary>
        /// Write all axes position streaming information to the log file.
        /// </summary>
        private void TracePositions()
        {
            if (this.axisPositionListList != null)
            {
                Dictionary<int, double> axisPositionSample = new Dictionary<int, double>();

                HeidenhainDNCLib.IJHAxisPositionList axisPositionList = null;
                HeidenhainDNCLib.IJHAxisPosition axisPosition = null;

                try
                {
                    //Debug.WriteLine(axisPositionListList.Count);
                    for (int i = 0; i < this.axisPositionListList.Count; i++)
                    {
                        int arrayIndex = 0;
                        string[] traceLine = new string[SampleInfoColumnCount + this.axisNameAssignment.Count];

                        // Append infos to logging
                        traceLine[arrayIndex++] = this.axesPositionSample.GetTimeStampAt(i);
                        traceLine[arrayIndex++] = Convert.ToString(i);
                        traceLine[arrayIndex++] = this.axesPositionSample.GetLineNrAt(i);
                        traceLine[arrayIndex++] = this.axesPositionSample.GetFeedModeAt(i);
                        traceLine[arrayIndex++] = this.axesPositionSample.GetSpindleStateAt(i);
                        traceLine[arrayIndex++] = this.axesPositionSample.GetMotionTypeAt(i);
                        traceLine[arrayIndex++] = this.axesPositionSample.GetIsBlockEndpointAt(i);

                        axisPositionList = this.axisPositionListList[i];

                        // get all the changed axis positions
                        for (int j = 0; j < axisPositionList.Count; j++)
                        {
                            axisPosition = axisPositionList[j];

                            int axisId = axisPosition.lAxisId;
                            double position = axisPosition.dPosition;

                            // Add axis if not exist
                            if (axisPositionSample.ContainsKey(axisId))
                            {
                                axisPositionSample[axisId] = position;
                            }
                            else
                            {
                                axisPositionSample.Add(axisId, position);
                            }

                            Marshal.ReleaseComObject(axisPosition);
                        }

                        // Concatenate the received positions with the stored positions. If the key exists in both dictionarys, the value from the received positions has priority.
                        this.axisPositions = axisPositionSample.Concat(this.axisPositions).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
                        this.axisPositions = this.axisPositions.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);

                        // Choose dictionary for logging
                        Dictionary<int, double> sampleToTrace = null;
                        if (!FillGapsCheckBox.Checked)
                        {
                            if (this.initialPositionsLogged)
                            {
                                sampleToTrace = axisPositionSample;
                            }
                            else
                            {
                                sampleToTrace = this.axisPositions;
                                this.initialPositionsLogged = true;
                            }
                        }
                        else
                        {
                            sampleToTrace = this.axisPositions;
                        }

                        foreach (KeyValuePair<int, string> axis in this.axisNameAssignment)
                        {
                            double position;
                            if (sampleToTrace.TryGetValue(axis.Key, out position))
                            {
                                traceLine[arrayIndex++] = position.ToString("0.0000").Replace(".", MainForm.DecimalSeparator);
                            }
                            else
                            {
                                traceLine[arrayIndex++] = string.Empty;
                            }
                        }

                        this.log.LogMessage(traceLine);

                        if (axisPositionList != null)
                        {
                            Marshal.ReleaseComObject(axisPositionList);
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
                    if (axisPositionList != null)
                    {
                        Marshal.ReleaseComObject(axisPositionList);
                    }

                    if (axisPosition != null)
                    {
                        Marshal.ReleaseComObject(axisPosition);
                    }
                }
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
        private void AxesPositionStreaming_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            if (!this.isNck)
            {
                FillGapsCheckBox.Enabled = false;
            }

            this.InitAxisNames();
            this.InitMinSampleRate();
            this.InitLoggingType();
            this.InitNotificationSampleLimit();
            this.InitNotificationTimeLimit();
            this.InitFilterTcpData();
            this.InitFilterAxesDelta();
            this.InitTurboFactor();

            // Set standart logfile path to environment %temp% path
            LogFilePathTextBox.Text = System.IO.Path.GetTempPath() + "axesPosStreaming.log";
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AxesPositionStreaming_Disposed(object sender, EventArgs e)
        {
            if (this.axesPositionStreaming != null)
            {
                this.StopStreaming();

                // --- 1. unadvice all event handlers here ------------------------------------------------
                this.axesPositionStreaming.OnAxesPositionsAvailable -= new HeidenhainDNCLib._DJHAxesPositionStreamingEvents_OnAxesPositionsAvailableEventHandler(this.AxesPositionStreaming_OnAxesPositionsAvailable);

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.axesPositionStreaming);
            }

            if (this.virtualMachine != null)
            {
                Marshal.ReleaseComObject(this.virtualMachine);
            }

            if (this.axisPositionListList != null)
            {
                Marshal.ReleaseComObject(this.axisPositionListList);
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
        /// Starts the streaming for channel 0.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            this.StartStreaming();
        }

        /// <summary>
        /// Stops the streaming for channel 0.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void StopButton_Click(object sender, EventArgs e)
        {
            this.StopStreaming();
        }

        /// <summary>
        /// Gets triggered if the control notifies the application, that a new package of axes positions is available.
        /// Do not call GetAxesPositionSamples here! This has to be done in a separate thread. (best asynchronously).
        /// If event OnAxesPositionsAvailable() was fired the events are locked until
        /// the axes position samples get fetched using GetAxesPositionSamples().
        /// So you can use BeginInvoke and do this asynchronously.
        /// </summary>
        private void AxesPositionStreaming_OnAxesPositionsAvailable()
        {
            this.BeginInvoke(new OnAxesPositionsAvailableHandler(this.OnAxesPositionsAvailable));
        }

        /// <summary>
        /// Fetch all the axes data from control. Do not call this directly in a event handler.
        /// </summary>
        private void OnAxesPositionsAvailable()
        {
            try
            {
                if (this.streamingIsActivated)
                {
                    // get axes position samples
                    this.axesPositionSample.Init();
                    this.axisPositionListList = this.axesPositionStreaming.GetAxesPositionSamples(
                                                                      ref this.axesPositionSample.TimeStamp,
                                                                      ref this.axesPositionSample.LineNr,
                                                                      ref this.axesPositionSample.FeedMode,
                                                                      ref this.axesPositionSample.SpindleState,
                                                                      ref this.axesPositionSample.MotionType,
                                                                      ref this.axesPositionSample.IsBlockEndpoint);

                    this.TracePositions();
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

        /// <summary>
        /// Specify the desired maximum time before the notification event OnAxesPositionsAvailable() is fired, even if the buffer is not filled yet.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetNotificationTimeLimitButton_Click(object sender, EventArgs e)
        {
            this.SetNotificationTimeLimit();
        }

        /// <summary>
        /// Specify a vector length (3D delta way) for the TCP movement.
        /// A sample is stored into the buffer if the TCP has moved further than the defined vector length.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetFilterTcpButton_Click(object sender, EventArgs e)
        {
            this.SetFilterTcp();
        }

        /// <summary>
        /// Specify a list of delta ways per machine axis.
        /// A sample is stored if at least one of the machine axes has moved further than the defined delta way for that axis.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetFilterAxesDeltaButton_Click(object sender, EventArgs e)
        {
            HeidenhainDNCLib.JHAxisPositionList axisPositionList = new HeidenhainDNCLib.JHAxisPositionList();
            try
            {
                foreach (UserControls.AxisPosition axisPos in FilterAxesDeltaFlowLayoutPanel.Controls)
                {
                    axisPositionList.AddItem(axisPos.JhAxisPosition);
                }

                this.axesPositionStreaming.SetFilterAxesDelta(axisPositionList);
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
                if (axisPositionList != null)
                {
                    Marshal.ReleaseComObject(axisPositionList);
                }
            }
        }

        /// <summary>
        /// Specify the number of samples in the buffer after which the notification event OnAxesPositionsAvailable() is fired.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetNotificationSampleTimeButton_Click(object sender, EventArgs e)
        {
            this.SetNotificationSampleLimit();
        }

        /// <summary>
        /// This method configures the type of logging that is done.
        /// To minimize the amount of data stored and thus transferred, look at the documentation
        /// to find the correct logging type for your application.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SetLoggingTypeButton_Click(object sender, EventArgs e)
        {
            this.SetLoggingType();
        }

        /// <summary>
        /// Switches turbo mode on.
        /// You can also do this while streaming is active.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TurboModeOnButton_Click(object sender, EventArgs e)
        {
            try
            {
                TurboModeOffButton.BackColor = SystemColors.Control;
                TurboModeOnButton.BackColor = MainForm.JHGREEN;
                TurboFactorTextBox.Enabled = false;

                // IJHVirtualMachine Turbo Mode Start
                double turboFactor = Convert.ToDouble(TurboFactorTextBox.Text);
                this.virtualMachine.SetTurboFactor(turboFactor);
                this.virtualMachine.ActivateTurboMode(true);
                this.turboFactorMeasuringTimer.Start();
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
        /// Switches turbo mode off.
        /// You can also do this while streaming is active.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TurboModeOffButton_Click(object sender, EventArgs e)
        {
            try
            {
                TurboModeOffButton.BackColor = Color.Red;
                TurboModeOnButton.BackColor = SystemColors.Control;
                TurboFactorTextBox.Enabled = true;

                // IJHVirtualMachine Turbo Mode Stop
                this.virtualMachine.ActivateTurboMode(false);
                this.turboFactorMeasuringTimer.Stop();
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
        /// Open save file dialog to select log file.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SelectLogfileButton_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can choose or create a logfile.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "DNC_Demo AxesPositionStreaming Logfile|*.log";
            saveFileDialog.Title = "Choose a filename to write logging to.";
            saveFileDialog.InitialDirectory = System.IO.Path.GetTempPath();
            saveFileDialog.FileName = LogFilePathTextBox.Text;

            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                LogFilePathTextBox.Text = saveFileDialog.FileName;
            }
        }

        /// <summary>
        /// Open log file in standard text editor.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void EditLogFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(LogFilePathTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error opening logfile with external editor.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Timer to measure the effective turbo factor.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TurboFactorMeasuringTimer_Elapsed(object sender, EventArgs e)
        {
            if (this.lastTimeStamp == DateTime.MinValue)
            {
                this.lastTimeStamp = this.axesPositionSample.GetLastTimeStamp();
                return;
            }

            DateTime lastTimeStampFromSample = this.axesPositionSample.GetLastTimeStamp();

            // Do nothing if there isn't any time stamp in the axesPositionSample.
            if (lastTimeStampFromSample == DateTime.MinValue)
            {
                return;
            }

            TimeSpan diffTime = lastTimeStampFromSample - this.lastTimeStamp;
            this.lastTimeStamp = lastTimeStampFromSample;

            double timerIntervall = this.turboFactorMeasuringTimer.Interval;
            double factor = diffTime.TotalMilliseconds / timerIntervall;

            this.turboFactor.AddValue(factor);
        }

        /// <summary>
        /// Disable check box after checking it.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void SynchronousModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                this.SetSynchronousMode();
                checkBox.Enabled = false;
            }
        }

        /// <summary>
        /// Updates the Gui in an equidistant rate. 
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void UpdateGuiTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateGui();
        }
        #endregion

        #region "Helper Classes"
        /// <summary>
        /// Helper class to initialize and store axes position samples.
        /// </summary>
        private class AxesPositionSample
        {
            #region "fields"
            /// <summary>
            /// List of timestamps of every sample.
            /// </summary>
            internal Array TimeStamp = null;

            /// <summary>
            /// List of line or block numbers of every sample.
            /// </summary>
            internal Array LineNr = null;

            /// <summary>
            /// List of feed modes of every sample.
            /// </summary>
            internal Array FeedMode = null;

            /// <summary>
            /// List of spindle states of every sample.
            /// </summary>
            internal Array SpindleState = null;

            /// <summary>
            /// List of motion types of every sample.
            /// </summary>
            internal Array MotionType = null;

            /// <summary>
            /// List of block endpoint indicators of every sample.
            /// </summary>
            internal Array IsBlockEndpoint = null;

            /// <summary>
            /// Size of the sample arrays.
            /// </summary>
            internal int SampleSize = 0;
            #endregion

            #region "constructor & destructor"
            /// <summary>
            /// Initializes a new instance of the <see cref="AxesPositionSample"/> class.
            /// </summary>
            /// <param name="maxSampleSize">Size of sample packages.</param>
            internal AxesPositionSample(int maxSampleSize)
            {
                this.SampleSize = maxSampleSize;
                this.TimeStamp = new DateTime[maxSampleSize];
                this.LineNr = new int[maxSampleSize];
                this.FeedMode = new HeidenhainDNCLib.DNC_FEED_MODE[maxSampleSize];
                this.SpindleState = new HeidenhainDNCLib.DNC_SPINDLE_STATE[maxSampleSize];
                this.MotionType = new HeidenhainDNCLib.DNC_POSSAMPLES_MOTION_TYPE[maxSampleSize];
                this.IsBlockEndpoint = new bool[maxSampleSize];
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="AxesPositionSample"/> class.
            /// </summary>
            ~AxesPositionSample()
            {
                this.TimeStamp = null;
                this.LineNr = null;
                this.FeedMode = null;
                this.SpindleState = null;
                this.MotionType = null;
                this.IsBlockEndpoint = null;
            }
            #endregion

            #region "public methods"
            /// <summary>
            /// Initialize all arrays with standard values.
            /// </summary>
            internal void Init()
            {
                this.TimeStamp.Initialize();
                this.LineNr.Initialize();
                this.FeedMode.Initialize();
                this.SpindleState.Initialize();
                this.MotionType.Initialize();
                this.IsBlockEndpoint.Initialize();
            }

            /// <summary>
            /// Get the time stamp at specified position and convert it to the string format.
            /// </summary>
            /// <param name="arrayIndex">The position to get the value from.</param>
            /// <returns>The time stamp from requested array position.</returns>
            internal string GetTimeStampAt(int arrayIndex)
            {
                DateTime timeStamp = (DateTime)this.TimeStamp.GetValue(arrayIndex);
                return timeStamp.ToString("dd/MM/yyyy HH:mm:ss.fff");
            }

            /// <summary>
            /// Get last time stamp. If there isn't any, you'll get DateTime.MinValue.
            /// </summary>
            /// <returns>Last time stamp from GetAxesPositionSamples.</returns>
            internal DateTime GetLastTimeStamp()
            {
                if (this.SampleSize <= 0)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return (DateTime)this.TimeStamp.GetValue(this.TimeStamp.Length - 1);
                }
            }

            /// <summary>
            /// Get the line number at specified position and convert it to the string format.
            /// </summary>
            /// <param name="arrayIndex">The position to get the value from.</param>
            /// <returns>The line number from requested array position.</returns>
            internal string GetLineNrAt(int arrayIndex)
            {
                int lineNr = (int)this.LineNr.GetValue(arrayIndex);
                return lineNr.ToString();
            }

            /// <summary>
            /// Get the feed mode at specified position and convert it to the string format.
            /// </summary>
            /// <param name="arrayIndex">The position to get the value from.</param>
            /// <returns>The feed mode from requested array position.</returns>
            internal string GetFeedModeAt(int arrayIndex)
            {
                HeidenhainDNCLib.DNC_FEED_MODE feedMode = (HeidenhainDNCLib.DNC_FEED_MODE)this.FeedMode.GetValue(arrayIndex);
                return Enum.GetName(typeof(HeidenhainDNCLib.DNC_FEED_MODE), feedMode);
            }

            /// <summary>
            /// Get the spindle state at specified position and convert it to the string format.
            /// </summary>
            /// <param name="arrayIndex">The position to get the value from.</param>
            /// <returns>The spindle state from requested array position.</returns>
            internal string GetSpindleStateAt(int arrayIndex)
            {
                HeidenhainDNCLib.DNC_SPINDLE_STATE spindleState = (HeidenhainDNCLib.DNC_SPINDLE_STATE)this.SpindleState.GetValue(arrayIndex);
                return Enum.GetName(typeof(HeidenhainDNCLib.DNC_SPINDLE_STATE), spindleState);
            }

            /// <summary>
            /// Get the motion type at specified position and convert it to the string format.
            /// </summary>
            /// <param name="arrayIndex">The position to get the value from.</param>
            /// <returns>The motion type from requested array position.</returns>
            internal string GetMotionTypeAt(int arrayIndex)
            {
                HeidenhainDNCLib.DNC_POSSAMPLES_MOTION_TYPE motionType = (HeidenhainDNCLib.DNC_POSSAMPLES_MOTION_TYPE)this.MotionType.GetValue(arrayIndex);
                return Enum.GetName(typeof(HeidenhainDNCLib.DNC_POSSAMPLES_MOTION_TYPE), motionType);
            }

            /// <summary>
            /// Get the information if the block is a endpoint at specified position and convert it to the string format.
            /// </summary>
            /// <param name="arrayIndex">The position to get the value from.</param>
            /// <returns>The block endpoint information from requested array position.</returns>
            internal string GetIsBlockEndpointAt(int arrayIndex)
            {
                bool isBlockEndpoint = (bool)this.IsBlockEndpoint.GetValue(arrayIndex);
                return isBlockEndpoint.ToString();
            }
            #endregion
        }

        /// <summary>
        /// Helper class to filter the measured turbo factor.
        /// </summary>
        private class SimpleAverageFilter
        {
            #region "fields"
            /// <summary>
            /// Filter buffer.
            /// </summary>
            private double[] buffer = null;

            /// <summary>
            /// Buffer size. (mean average order).
            /// </summary>
            private int size;

            /// <summary>
            /// Array write pointer.
            /// </summary>
            private int writePointer;
            #endregion

            #region "constructor & destructor"
            /// <summary>
            /// Initializes a new instance of the <see cref="SimpleAverageFilter"/> class.
            /// </summary>
            /// <param name="bufferSize">Buffer size of the mean average filter.</param>
            public SimpleAverageFilter(int bufferSize)
            {
                if (bufferSize <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.size = bufferSize;
                this.Clear();
            }
            #endregion

            #region "public methods"
            /// <summary>
            /// Add value to filter buffer.
            /// </summary>
            /// <param name="newValue">The new value for the filter.</param>
            public void AddValue(double newValue)
            {
                this.buffer[this.writePointer] = newValue;
                if (this.writePointer == (this.buffer.Length - 1))
                {
                    this.writePointer = 0;
                }
                else
                {
                    this.writePointer++;
                }
            }

            /// <summary>
            /// Clear the filter.
            /// </summary>
            public void Clear()
            {
                this.buffer = new double[this.size];
                this.writePointer = 0;
            }

            /// <summary>
            /// Get the mean average of the filter.
            /// </summary>
            /// <returns>The mean average.</returns>
            public double AverageMean()
            {
                double meanValue = 0;

                for (int i = 0; i < this.size; i++)
                {
                    meanValue += this.buffer[i];
                }

                return meanValue / this.size;
            }

            /// <summary>
            /// Get the mean average as string value.
            /// </summary>
            /// <returns>The mean average value.</returns>
            public override string ToString()
            {
                double averageMean = this.AverageMean();
                return averageMean.ToString("0.00").Replace(".", MainForm.DecimalSeparator);
            }
            #endregion
        }
        #endregion
    }
}