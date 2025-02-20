// ------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This is the main form of the application. This form does also handle the connection.
// </summary>
// ------------------------------------------------------------------------------------------------

/* #define RAW_COM_EVENTS:
 * -----------------------
 *  Hook up the event handlers to the COM interface, using the more efficient raw approach.
 *  Pro:
 *    - each COM event is fired only once
 *  Con:
 *    - a handler must be defined for each COM event of the given COM interface
 */
#define RAW_COM_EVENTS

namespace DNC_CSharp_Demo
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Main application form
    /// </summary>
    public partial class MainForm : Form
    {
        #region "type defs"
        /// <summary>
        /// Heidenhain green
        /// </summary>
        public static readonly Color JHGREEN = Color.FromArgb(176, 203, 36);
        #endregion

        #region "fields"
        /// <summary>
        /// Culture dependant decimal separator.
        /// </summary>
        public static readonly string DecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        /// <summary>
        /// Represents the state of the application
        /// </summary>
        private ApplicationState appState = ApplicationState.disconnected;

        /// <summary>
        /// Represents the state of the CNC control
        /// </summary>
        private HeidenhainDNCLib.DNC_STATE controlState;

        /// <summary>
        /// Stores the last state of the CNC control
        /// </summary>
        private HeidenhainDNCLib.DNC_STATE previousControlState;

#if RAW_COM_EVENTS
        /// <summary>
        /// A helper class to listen to the VTable event implementation.
        /// </summary>
        private MachineStateListener machineStateListener = null;
#endif
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class and initializes the components container.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.components = new Container();
        }
        #endregion

        #region "delegates"
        /// <summary>
        /// Delegate for _IJHMachineEvents2::OnStateChanged event handler
        /// </summary>
        /// <param name="machineState">Control state transition</param>
        private delegate void OnStateChangedHandler(HeidenhainDNCLib.DNC_EVT_STATE machineState);
        #endregion

        #region "Enums"
        /// <summary>
        /// Application state. Application is ready only if state is "connected"
        /// </summary>
        public enum ApplicationState
        {
            /// <summary>
            /// The application is disconnected from control
            /// </summary>
            disconnected,

            /// <summary>
            /// The application tries to establish a connection to the control
            /// </summary>
            connecting,

            /// <summary>
            /// The application is connected to the control.
            /// It's now possible to communicate.
            /// </summary>
            connected,

            /// <summary>
            /// The application tried to disconnect from control
            /// </summary>
            disconnecting
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets the version string of the used HeidenhainDNC COM component
        /// </summary>
        internal string VersionComInterface { get; private set; }

        /// <summary>
        /// Gets the nck milestone version inclusive service pack.
        /// </summary>
        internal Utils.NCK_Version NckVersion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the connected CNC control is NCK based or not.
        /// </summary>
        internal bool IsNck { get; private set; }

        /// <summary>
        /// Gets the type of the connected CNC control.
        /// </summary>
        internal HeidenhainDNCLib.DNC_CNC_TYPE CncType { get; private set; }

        /// <summary>
        /// Gets the main communication object.
        /// Use this to get the interfaces.
        /// </summary>
#if MACHINE_IN_PROCESS
        internal HeidenhainDNCLib.JHMachineInProcess Machine { get; private set; }
#else
        internal HeidenhainDNCLib.JHMachine Machine { get; private set; }
#endif

        /// <summary>
        /// Gets the state of the application.
        /// By setting this state the GUI also becomes refreshed.
        /// </summary>
        internal ApplicationState AppState
        {
            get
            {
                return this.appState;
            }

            private set
            {
                this.SuspendLayout();
                switch (value)
                {
                    case ApplicationState.connecting:
                        ApplicationStatusLabel.BackColor = Color.Yellow;
                        ApplicationStatusLabel.Text = "connecting";
                        ConnectionListComboBox.Enabled = false;
                        ConfigureConnectionButton.Enabled = false;
                        ConnectButton.Enabled = false;
                        DisconnectButton.Enabled = true;
                        break;
                    case ApplicationState.connected:
                        ApplicationStatusLabel.BackColor = JHGREEN;
                        ApplicationStatusLabel.Text = "connected";
                        ConnectionListComboBox.Enabled = false;
                        ConfigureConnectionButton.Enabled = false;
                        ConnectButton.Enabled = false;
                        DisconnectButton.Enabled = true;

                        // --- Get nck version if connected control is nck a type ---------
                        if (this.IsNck)
                        {
                            this.GetNckVersion();
                        }

                        // --- automatically create user controls when connected ----------
                        this.CreateUserControls();

                        break;
                    case ApplicationState.disconnecting:
                        ApplicationStatusLabel.BackColor = Color.Yellow;
                        ApplicationStatusLabel.Text = "disconnecting";
                        ConnectionListComboBox.Enabled = false;
                        ConfigureConnectionButton.Enabled = false;
                        ConnectButton.Enabled = false;
                        DisconnectButton.Enabled = false;
                        break;
                    case ApplicationState.disconnected:
                        ApplicationStatusLabel.BackColor = Color.Red;
                        ApplicationStatusLabel.Text = "disconnected";
                        ConnectionListComboBox.Enabled = true;
                        ConfigureConnectionButton.Enabled = true;
                        ConnectButton.Enabled = true;
                        DisconnectButton.Enabled = false;
                        break;
                }

                this.ResumeLayout();

                this.appState = value;
            }
        }

        /// <summary>
        /// Gets the state of the CNC control.
        /// By setting this state the GUI also becomes refreshed.
        /// </summary>
        internal HeidenhainDNCLib.DNC_STATE ControlState
        {
            get
            {
                return this.controlState;
            }

            private set
            {
                this.controlState = value;

                // Update GUI
                ControlStatusLabel.Text = Enum.GetName(typeof(HeidenhainDNCLib.DNC_STATE), this.controlState);
                ControlStatusLabel.Refresh();

                if (this.controlState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE)
                {
                    this.AppState = ApplicationState.connected;
                }
            }
        }
        #endregion

        #region "event handler"
        // --- HeidenhainDNC events ---------------------------------------------------------------
#if !RAW_COM_EVENTS
        /// <summary>
        /// Gets fired if control state has changed. Don't forget to activate the event loop
        /// by calling IJHMachine::GetState() of IJHMachineInProcess::GetState() first.
        /// </summary>
        /// <param name="eventValue">Control state transition</param>
        private void OnStateChanged(HeidenhainDNCLib.DNC_EVT_STATE machineState)
        {
            Debug.WriteLine("MainForm::OnStateChanged");
            // Ensure the implementation is called asynchronously
            // Otherwise you may get a deadlock at unadvising the OnStateChanged event handler
            this.BeginInvoke(new OnStateChangedHandler(this.OnStateChangedImpl), machineState);
        }
#endif

        /// <summary>
        /// Implementation for changed control states
        /// </summary>
        /// <param name="eventValue">Control state transition</param>
        private void OnStateChangedImpl(HeidenhainDNCLib.DNC_EVT_STATE eventValue)
        {
            try
            {
                this.StateMachineNCK(eventValue);

                switch (eventValue)
                {
                    case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                        Debug.WriteLine("--- automatic Disconnect ---");
                        this.Disconnect();
                        if (AutoReconnectCheckBox.Checked)
                        {
                            Debug.WriteLine("--- automatic ConnectRequest ---");
                            
                            // Give the TCP/IP stack a moment to complete the disconnect correctly.
                            Thread.Sleep(1000);
                            this.Connect();
                        }

                        break;
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

        // --- Form events ------------------------------------------------------------------------

        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Check whether version is suitable for this application 
            string versionComInterface = Utils.CheckDncVersion(Properties.Settings.Default.requiredDncVersion);

            // Init caption of main window with actual dnc version
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            if (!string.IsNullOrEmpty(versionComInterface))
            {
                this.Text = assemblyName + " (using HEIDENHAIN DNC version " + versionComInterface + ")";
            }

            // Initialize the application state.
            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
            this.previousControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
            this.AppState = ApplicationState.disconnected;
            this.UpdateConnectionList(string.Empty);

            // Preselect the last selected connection in the combo box
            try
            {
                string lastConnected = Properties.Settings.Default.lastConnected;
                if (!string.IsNullOrEmpty(lastConnected))
                {
                    int Index = ConnectionListComboBox.FindStringExact(Properties.Settings.Default.lastConnected);
                    if (Index != -1)
                    {
                        ConnectionListComboBox.SelectedIndex = Index;
                    }
                }
            }
            catch
            {
                //// Setting "lastConnected" not available. This is probably the first execution of this application.
            }
        }

        /// <summary>
        /// This event is fired if the form becomes closed.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Forbid to close rhe form if application is in a transition state
            if (this.appState == ApplicationState.connecting || this.appState == ApplicationState.disconnecting)
            {
                e.Cancel = false;
                return;
            }

            this.Disconnect();
        }

        // --- Button events ----------------------------------------------------------------------

        /// <summary>
        /// Opens the configure connections dialog an refreshes connection combo box.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ConfigureConnectionButton_Click(object sender, EventArgs e)
        {
#if MACHINE_IN_PROCESS
            HeidenhainDNCLib.JHMachineInProcess machine = null;
#else
            HeidenhainDNCLib.JHMachine machine = null;
#endif
            try
            {
#if MACHINE_IN_PROCESS
                machine = new HeidenhainDNCLib.JHMachineInProcess();
#else
                machine = new HeidenhainDNCLib.JHMachine();
#endif

                // object claimed by C# COM wrapper (ConnectionName)
                object connectionNameObject = null;
                if (ConnectionListComboBox.SelectedItem != null)
                {
                    connectionNameObject = ConnectionListComboBox.SelectedItem.ToString();
                }
                this.Enabled = false;   // Disable the dialog while the Connections dialog is active

                // use "ref" in front of second parameter for allowing the method ConfigureConnection to read and write
                // value of objConnectionName
                machine.ConfigureConnection(HeidenhainDNCLib.DNC_CONFIGURE_MODE.DNC_CONFIGURE_MODE_ALL, ref connectionNameObject);
                this.Enabled = true;    // Enable the dialog again

                // check if a connection was chosen (only possible if second parameter of ConfigureConnection is "ref")
                string connectionName = connectionNameObject.ToString();
                this.UpdateConnectionList(connectionName);
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
            finally
            {
                if (machine != null)
                {
                    Marshal.ReleaseComObject(machine);
                }
            }
        }

        /// <summary>
        /// Connect button was clicked
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("--- manual ConnectRequest ---");
            Properties.Settings.Default.lastConnected = ConnectionListComboBox.Text;
            Properties.Settings.Default.Save();
            this.Connect();
        }

        /// <summary>
        /// Disconnect button was clicked
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("--- manual Disconnect ---");
            this.Disconnect();
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Updates the connection list to the combo box.
        /// Optional you can preselect a connection in the combo box.
        /// </summary>
        /// <param name="selectedConnection">Preselect this connection in the combo box</param>
        private void UpdateConnectionList(string selectedConnection = "")
        {
            ConnectionListComboBox.Items.Clear();

#if MACHINE_IN_PROCESS
            HeidenhainDNCLib.JHMachineInProcess machine = null;
#else
            HeidenhainDNCLib.JHMachine machine = null;
#endif

            HeidenhainDNCLib.IJHConnectionList connectionList = null;
            HeidenhainDNCLib.IJHConnection connection = null;

            ConnectionListComboBox.BeginUpdate();

            try
            {
                // --- list all configured connection in combo box --------------------
#if MACHINE_IN_PROCESS
                machine = new HeidenhainDNCLib.JHMachineInProcess();
#else
                machine = new HeidenhainDNCLib.JHMachine();
#endif
                connectionList = machine.ListConnections();


                for (int i = 0; i < connectionList.Count; i++)
                {
                    connection = connectionList[i];

                    ConnectionListComboBox.Items.Add(connection.name);

                    if (connection != null)
                    {
                        Marshal.ReleaseComObject(connection);
                    }
                }

                // --- preselect connection in combobox -------------------------------
                if (ConnectionListComboBox.Items.Count > 0)
                {
                    if (string.IsNullOrEmpty(selectedConnection))
                    {
                        ConnectionListComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        ConnectionListComboBox.SelectedIndex = ConnectionListComboBox.FindStringExact(selectedConnection);
                    }


                    ConnectButton.Enabled = true;
                }
                else
                {
                    ConnectionListComboBox.Items.Add("First add and select a connection entry with Configure Connection!");
                    ConnectionListComboBox.SelectedIndex = 0;

                    ConnectButton.Enabled = false;
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
                ConnectionListComboBox.EndUpdate();
                if (machine != null)
                {
                    Marshal.ReleaseComObject(machine);
                }

                if (connectionList != null)
                {
                    Marshal.ReleaseComObject(connectionList);
                }

                if (connection != null)
                {
                    Marshal.ReleaseComObject(connection);
                }
            }
        }

        /// <summary>
        /// Request a connection to control.
        /// ConnectRequest behaves different between iTNC530 and NCK based controls.
        /// Please take a look at the RemoTools SDK documentation for more details.
        /// </summary>
        private void Connect()
        {
            this.AppState = ApplicationState.connecting;

            // Lock disconnect button, because it is forbidden to disconnect if the ConnectRequest() function has not finished. 
            DisconnectButton.Enabled = false;

            HeidenhainDNCLib.IJHConnectionList connectionList = null;
            HeidenhainDNCLib.IJHConnection connection = null;
            try
            {
#if MACHINE_IN_PROCESS
                this.Machine = new HeidenhainDNCLib.JHMachineInProcess();
#else
                this.Machine = new HeidenhainDNCLib.JHMachine();
#endif

                // Use Connect() instead of ConnectRequestX() for older controls
                // like TNC 426/430 or CNC PILOT 4290 
                this.Machine.ConnectRequest(this.ConnectionListComboBox.Text);
                DisconnectButton.Enabled = true;

                string currentMachine = this.Machine.currentMachine;

                // Find out control type
                connectionList = this.Machine.ListConnections();
                connection = connectionList.get_Connection(currentMachine);
                this.CncType = connection.cncType;
                if (this.CncType.ToString().EndsWith("_NCK"))
                {
                    this.IsNck = true;
                }
                else
                {
                    this.IsNck = false;
                }

#if RAW_COM_EVENTS
                this.machineStateListener = new MachineStateListener(this);
#else
                this.Machine.OnStateChanged += new HeidenhainDNCLib._DJHMachineEvents2_OnStateChangedEventHandler(this.OnStateChanged);
#endif

                // This method also enables the OnStateChanged events.
                // Ensure to advise this event interface first, to prevent missing a state change.
                this.ControlState = this.Machine.GetState();
                Debug.WriteLine(string.Format(
                                       "{0}.{1:000}: GetState = {2}",
                                       DateTime.Now.ToLongTimeString(),
                                       DateTime.Now.Millisecond,
                                       this.ControlState.ToString()));
            }
            catch (COMException cex)
            {
                this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
                this.Disconnect();
            }
            catch (Exception ex)
            {
                this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
            finally
            {
                if (connectionList != null)
                {
                    Marshal.ReleaseComObject(connectionList);
                }

                if (connection != null)
                {
                    Marshal.ReleaseComObject(connection);
                }
            }
        }

        /// <summary>
        /// Disconnect from control if connected.
        /// This also disposes all to components container attached user controls.
        /// --> components.Dispose() is a Windows.Forms functionality
        /// </summary>
        private void Disconnect()
        {
            this.AppState = ApplicationState.disconnecting;

            try
            {
                // 1. Dispose all panels which contains the COM interfaces (JHVersion, JHAutomatic, etc.)
                //    Release all COM interfaces (Marshal.ReleaseComObject(JH...)) in the panel classes
                this.components.Dispose();

                if (this.Machine != null)
                {
                    // 2. Unsubscribe all events 
#if RAW_COM_EVENTS
                    if (this.machineStateListener != null)
                    {
                        this.machineStateListener.Stop();
                        this.machineStateListener = null;
                    }
#else
                    this.Machine.OnStateChanged -= new HeidenhainDNCLib._DJHMachineEvents2_OnStateChangedEventHandler(this.OnStateChanged);
#endif
                    this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;

                    // 3. Disconect from control
                    try
                    {
                        this.Machine.Disconnect();
                    }
                    catch (COMException cex)
                    {
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! VERY IMPORTANT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        // If Disconnect() returns HRESULT = 0x80040266 / DNC_HRESULT.DNC_E_NOT_POS_NOW,
                        // you probably forgot to release some HeidenhainDNC resources!
                        // In most cases of the DNC_E_NOT_POS_NOW HRESULT, the Disconnect() is not possible,
                        // because there are still HeidenhainDNC resources like the "interfaces", 
                        // "helper objects" or "events" in use.
                        // --> Please release them before calling the Disconnect() method to avoid memory leaks!
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! VERY IMPORTANT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        if (cex.ErrorCode == Convert.ToInt32(HeidenhainDNCLib.DNC_HRESULT.DNC_E_NOT_POS_NOW))
                        {
                            // This (2nd) Disconnect() forces to Disconnect() from control, even if there
                            // are some HeidenhainDNC resources in use. This should never happen.
                            // Please ensure to release all requested HeidenhainDNC resources!
                            this.Machine.Disconnect();

                            string className = this.GetType().Name;
                            string methodName = MethodInfo.GetCurrentMethod().Name;
                            Utils.ShowComException(cex.ErrorCode, className, methodName);
                        }
                        else
                        {
                            throw cex;
                        }
                    }

                    // 4. Release machine object
                    Marshal.ReleaseComObject(this.Machine);
                    this.Machine = null;
                }

                this.AppState = ApplicationState.disconnected;
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
        /// Get the milestone and the service pack version of the tnc control.
        /// Only for nck based controls like the TNC640!
        /// </summary>
        private void GetNckVersion()
        {
            this.NckVersion = Utils.GetNckVersion(this.Machine);
        }

        /// <summary>
        /// Create and add all additional user controls.
        /// This method also adds the user controls to the Windows.Forms components container.
        /// This ensures, that the user controls becomes disposed when a from System.Windows.Form
        /// derived class gets disposed.
        /// </summary>
        private void CreateUserControls()
        {
            // Create all user controls (containing DNC interfaces) if supported
            UserControls.DncInterface dncInterface = new UserControls.DncInterface(this);
            if (dncInterface.Init())
            {
                this.components.Add(dncInterface);
                MainTableLayoutPanel.Controls.Add(dncInterface, 0, 1);
            }
            else
            {
                // Important! Dispose the user control if the Init method failed.
                dncInterface.Dispose();

                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported");
                this.components.Add(notSupported);
                MainTableLayoutPanel.Controls.Add(notSupported, 0, 1);
            }
        }

        /// <summary>
        /// State machine (NCK based controls)
        /// Call this method every time the _IJHMachineEvents2::OnStateChanged event was fired.
        /// This method uses the by OnStateChanged given transition to decode the state of the
        /// connected control. It also writes the transition to the debug output.
        /// </summary>
        /// <param name="evtState">Transition given by _IJHMachineEvents2::OnStateChanged()</param>
        private void StateMachineNCK(HeidenhainDNCLib.DNC_EVT_STATE evtState)
        {
            // internal state machine of the NCK based controls
            // --> See HeidenhainDNC COM component dokumentation for more informations
            this.previousControlState = this.ControlState;

            switch (this.ControlState)
            {
                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_HOST_NOT_AVAILABLE:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_HOST_AVAILABLE:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_AVAILABLE;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_AVAILABLE:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_HOST_NOT_AVAILABLE:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_WAIT_PERMISSION:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_WAITING_PERMISSION;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_WAITING_PERMISSION:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_AVAILABLE:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_AVAILABLE;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_PERMISSION_DENIED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NO_PERMISSION;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_AVAILABLE:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_BOOTED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_BOOTED;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_BOOTED:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_INITIALIZING:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_INITIALIZING;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_INITIALIZING:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_AVAILABLE:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_SHUTTING_DOWN:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_SHUTTING_DOWN;
                            break;
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_SHUTTING_DOWN:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_CONNECTION_LOST:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_STOPPED:
                    switch (evtState)
                    {
                        case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                            this.ControlState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_DNC_IS_STOPPED;
                            break;
                    }

                    break;

                case HeidenhainDNCLib.DNC_STATE.DNC_STATE_NO_PERMISSION:
                    break;
            }

            // Write the transition (old state --> new state) to debug output.
            Debug.WriteLine(
                  string.Format("{0}.{1:000}: ", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond) +
                  this.previousControlState.ToString() +
                  " --> " + this.ControlState.ToString());
        }
        #endregion

#if RAW_COM_EVENTS
        /// <summary>
        /// This is a helper class to use the VTable event implementation of the _IJHErrorEvents.
        /// The VTable event implementation "_IJHErrorEvents" is more effective than the
        /// IDispatch event implementation "_DJHErrorEvents".
        /// </summary>
        private class MachineStateListener : HeidenhainDNCLib._IJHMachineEvents2
        {
            #region "Properties
            /// <summary>
            /// The parent dialog is stored, so the events can be passed.
            /// </summary>
            private MainForm dialog;

            /// <summary>
            /// The IConnectionPoint interface is stored, so it can be used by Stop() / destructor.
            /// </summary>
            private System.Runtime.InteropServices.ComTypes.IConnectionPoint icp;

            /// <summary>
            /// The cookie identifies the advise operation.
            /// </summary>
            private int cookie = -1;
            #endregion

            #region "constructor & destructor"
            /// <summary>
            /// Initializes a new instance of the <see cref="MachineStateListener"/> class.
            /// </summary>
            /// <param name="dialog">The parent dialog.</param>
            public MachineStateListener(MainForm dialog)
            {
                this.dialog = dialog;

                // get IConnectionPointContainer
                System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)dialog.Machine;

                Guid g = typeof(HeidenhainDNCLib._IJHMachineEvents2).GUID;

                // get IConnectionPoint for the required interface
                icpc.FindConnectionPoint(ref g, out this.icp);

                // Advise the interface:
                // This means that the COM events are attached to the client implementation (a.k.a. as the event sink)
                // Here the event sink is this class.
                this.icp.Advise(this, out this.cookie);
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="MachineStateListener"/> class.
            /// Implicitly stops the event listener.
            /// </summary>
            ~MachineStateListener()
            {
                this.Stop();
            }
            #endregion

            #region "event handler"
            /// <summary>
            /// Gets fired if control state has changed. Don't forget to activate the event loop
            /// by calling IJHMachine::GetState() of IJHMachineInProcess::GetState() first.
            /// </summary>
            /// <param name="machineState">Control state transition</param>
            public void OnStateChanged(HeidenhainDNCLib.DNC_EVT_STATE machineState)
            {
                Debug.WriteLine("MachineStateListener::OnStateChanged");

                // Since this code is executed in the context of the COM event server (callback),
                // the call must be passed to the client thread.
                this.dialog.BeginInvoke(new OnStateChangedHandler(this.dialog.OnStateChangedImpl), machineState);
            }

            // end: VTable IJHMachineEvents2 event sink class
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
