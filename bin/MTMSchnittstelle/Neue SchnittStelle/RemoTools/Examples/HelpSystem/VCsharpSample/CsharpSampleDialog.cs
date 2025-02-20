#define INPROC              // use HeidenhainDNC in-process with this application
#define VTABLE_EVENTS       // use VTABLE event interface, i.s.o. IDispatch
#define RAW_COM_EVENTS      // use RAW event handlers, i.s.o. the .NET event abstraction
//#define ERROR_V1            // use the base IJHErrorEvents interface, i.s.o. IJHErrorEvents2
//#define CONNECTREQUEST3     // connect using ad-hoc connection object

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CsharpSample
{
    ////////////////////////////////////////////////////////////////////////////////
    // Delegate definitions

    // IJHMachine
    public delegate void OnStateChangedHandler(HeidenhainDNCLib.DNC_EVT_STATE stateEvent);
    // IJHAutomatic
    public delegate void OnProgramStateChangedHandler(HeidenhainDNCLib.DNC_EVT_PROGRAM progStatEvent);
    public delegate void OnDncModeChangedHandler(HeidenhainDNCLib.DNC_MODE dncMode);
    // IJHAutomatic2
    public delegate void OnExecutionMessageHandler(int a_lChannel, object a_varNumericValue, string a_strValue);
    public delegate void OnProgramChangedHandler(int a_lChannel, System.DateTime a_dTimeStamp, string a_strNewProgram);
    public delegate void OnToolChangedHandler(int a_lChannel, HeidenhainDNCLib.IJHToolId a_pidToolOut, HeidenhainDNCLib.IJHToolId a_pidToolIn, System.DateTime a_dTimeStamp);
    // IJHAutomatic3
    public delegate void OnExecutionModeChangedHandler(int a_lChannel, HeidenhainDNCLib.DNC_EXEC_MODE executionMode);

    // IJHError
    public delegate void OnErrorHandler(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int lErrorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string bstrError, int lChannel);
    public delegate void OnErrorClearedHandler( int lErrorNumber, int lChannel );
    // IJHError2
    public delegate void OnError2Handler(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry);
    public delegate void OnErrorCleared2Handler(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry);


    ////////////////////////////////////////////////////////////////////////////////
    // The main dialog class
    public partial class CsharpSampleDialog : Form
    {

        // Machine object
#if INPROC
        private HeidenhainDNCLib.JHMachineInProcess m_machine = null;
#else
        private HeidenhainDNCLib.JHMachine m_machine = null;
#endif
#if RAW_COM_EVENTS
        MachineListener m_machineListener = null;
#endif
        // Server state
        private HeidenhainDNCLib.DNC_STATE m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;

        // Automatic object
        private HeidenhainDNCLib.JHAutomatic m_automatic = null;
#if RAW_COM_EVENTS
        AutomaticListener m_automaticListener = null;
#endif
        // Program state
        private HeidenhainDNCLib.DNC_STS_PROGRAM m_programState = HeidenhainDNCLib.DNC_STS_PROGRAM.DNC_PRG_STS_IDLE;

        // Error object
        private HeidenhainDNCLib.JHError m_error = null;
#if RAW_COM_EVENTS
        ErrorListener m_errorListener = null;
#endif

        ////////////////////////////////////////////////////////////////////////////
        // Constructor
        public CsharpSampleDialog()
        {
            InitializeComponent();
        }

        ////////////////////////////////////////////////////////////////////////////
        // Event handlers
        ////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////
        // Load form event handler
        private void CsharpSampleDialog_Load(object sender, EventArgs e)
        {
            // Initialize the controls
            FillConnectionList("");
            UpdateControls();
        }

        private void immediateConnectButton_Click(object sender, EventArgs e)
        {
            Text = "VC# Example - Connecting...";
            m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE;
            UpdateControls();

            // Request the DNC connection and connect the OnStateChanged event handler
            try
            {
                // Create the machine object
#if INPROC
                m_machine = new HeidenhainDNCLib.JHMachineInProcess();
#else
                m_machine = new HeidenhainDNCLib.JHMachine();
#endif

                m_machine.Connect(connectionList.Text);

                // assume connected (no exception)
                m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE;

                // Machine is available, so complete the connection
                Connect();

                UpdateControls();
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                string errMsg = connectionList.Text.Length == 0
                                    ? "No connection selected"
                                    : "Could not connect to " + connectionList.Text + "\n\nError: " + cex.Message;
                MessageBox.Show(errMsg, "VC# Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect();
                m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
                UpdateControls();
                Text = "VC# Example - Not connected";
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // Connect button click event handler
        private void connectButton_Click(object sender, EventArgs e)
        {
            Text = "VC# Example - Connecting...";
            m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE;
            UpdateControls();

            // Request the DNC connection and connect the OnStateChanged event handler
            try
            {
                // Create the machine object
#if INPROC
                m_machine = new HeidenhainDNCLib.JHMachineInProcess();
#else
                m_machine = new HeidenhainDNCLib.JHMachine();
#endif

#if CONNECTREQUEST3
                HeidenhainDNCLib.IJHConnection2 newConnection = (HeidenhainDNCLib.IJHConnection2)m_machine;
                newConnection.Configure(HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_TNC6xx_NCK, HeidenhainDNCLib.DNC_PROTOCOL.DNC_PROT_RPC);

                newConnection.set_propertyValue(HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_HOST, (object)"NL02PC537VM_TNC640_M9");
                m_machine.ConnectRequest3(newConnection);
#else
                // (Try to) connect to the given CNC.
                // The CNC may be offline.
                m_machine.ConnectRequest(connectionList.Text);
#endif

                // Attach the JHMachine event OnServerStateChanged to our handler
#if VTABLE_EVENTS

    #if RAW_COM_EVENTS
                /*  Hook up the event handlers to the COM interface, using the more efficient raw approach.
                    *  Pro:
                    *      - each COM event is fired only once
                    *  Con:
                    *      - a handler must be defined for each COM event of the given COM interface
                    */
                m_machineListener = new MachineListener(this, m_machine);
    #else
                /*  Hook up the event handlers to each required COM event, using the C# event abstraction approach.
                    *  (As opposed to the raw approach (as in ErrorListener))
                    *  IMPORTANT:
                    *      A COM event connection is created for each event handler that is added.
                    *      As a result, each COM event will be fired <n>-times, but only one will actually call the handler method.
                    *
                    *      Hooking up all 4 events, will result in an interface leak as signalled by the JHMachine::Disconnect method.
                    *      This is caused by the use of the thread-safe BeginInvoke method inside the event handler.
                    */
               ((HeidenhainDNCLib._IJHMachineEvents2_Event)m_machine).OnStateChanged +=
                    new HeidenhainDNCLib._IJHMachineEvents2_OnStateChangedEventHandler(OnCOMStateChanged);
    #endif
#else
               m_machine.OnStateChanged += new HeidenhainDNCLib._DJHMachineEvents2_OnStateChangedEventHandler(OnCOMStateChanged);
                // .NET4: Event invocation for COM objects requires event to be attributed with DispIdAttribute""-> set "Embed Interop Types" property on HeidenhainDNCLib Reference to false.
#endif

                // Get the initial (actual) server state. This also starts the state events coming.
                m_serverState = m_machine.GetState();
                if (m_serverState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE)
                {
                    // Machine is available, so complete the connection now
                    Connect();
                }
                else
                {
                    // Machine is still offline or not completely booted yet.
                    // The Connect() is executed by the OnStateChanged event handler: OnStateChangedImpl(),
                    // as soon as the required state is reached.
                }

                UpdateControls();
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                string errMsg = connectionList.Text.Length == 0
                                    ? "No connection selected"
                                    : "Could not connect to " + connectionList.Text + "\n\nError: " + cex.Message;
                MessageBox.Show(errMsg, "VC# Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect();
                m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
                UpdateControls();
                Text = "VC# Example - Not connected";
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // DiscConnect button click event handler
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
            m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
            UpdateControls();
            Text = "VC# Example - Not connected";
        }

        ////////////////////////////////////////////////////////////////////////////
        // Configure Connections button click event handler
        private void configureConnectionsbutton_Click(object sender, EventArgs e)
        {
            string connectionName = connectionList.Text;
            // Copy the connectionName string to an object.
            object var = (object) connectionName;
            Enabled = false;    // Disable the dialog while the Connections dialog is active

            HeidenhainDNCLib.JHMachineInProcess machine = new HeidenhainDNCLib.JHMachineInProcess();
            machine.ConfigureConnection(HeidenhainDNCLib.DNC_CONFIGURE_MODE.DNC_CONFIGURE_MODE_ALL, ref var);

            Enabled = true;     // Enable the dialog again
            connectionName = (string) var;
            FillConnectionList(connectionName);

            // release COM object
            System.Runtime.InteropServices.Marshal.ReleaseComObject(machine);
        }

        ////////////////////////////////////////////////////////////////////////////
        // Exit button click event handler
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Form closing event handler
        private void CsharpSampleDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // event handler methods
        //      These methods are executed in the application thread, when called through the BeginInvoke() method.
        //      See OnCOM<event>() methods.
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////
        // Server state change handler
        public void OnStateChangedImpl(HeidenhainDNCLib.DNC_EVT_STATE stateEvent)
        {
            switch (stateEvent)
            {
                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_HOST_NOT_AVAILABLE:
                    {
                        Text = "VC# Example - " + connectionList.Text + " cannot be reached";
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_HOST_AVAILABLE:
                    {
                        Text = "VC# Example - " + connectionList.Text + " is powered up";
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_WAIT_PERMISSION:
                    {
                        Text = "VC# Example - Waiting permission from " + connectionList.Text;
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_AVAILABLE:
                    {
                        Text = "VC# Example - Server running on " + connectionList.Text;
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_BOOTED:
                    {
                        Text = "VC# Example - CNC has booted on " + connectionList.Text;
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_INITIALIZING:
                    {
                        Text = "VC# Example - CNC is initializing on " + connectionList.Text;
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_AVAILABLE:
                    {
                        // CNC is available, so make the actual connect
                        Connect();
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_SHUTTING_DOWN:
                    {
                        Text = "VC# Example - " + connectionList.Text + " is shutting down";
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_DNC_STOPPED:
                    {
                        Text = "VC# Example - Server stopped on " + connectionList.Text;
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_CONNECTION_LOST:
                    {
                        Text = "VC# Example - Connection closed by " + connectionList.Text;
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_HOST_STOPPED:
                    {
                        // CNC is shutting down or connection lost
                        Text = "VC# Example - " + connectionList.Text + " was powered down";
                    }
                    break;

                case HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_PERMISSION_DENIED:
                    {
                        // DNC access permission has been denied by CNC operator
                        Text = "VC# Example - Access denied by " + connectionList.Text;
                    }
                    break;
            }

            m_serverState = m_machine.GetState();
            UpdateControls();
        }

        ////////////////////////////////////////////////////////////////////////////
        // Program state change handler
        public void OnProgramStateChangedImpl(HeidenhainDNCLib.DNC_EVT_PROGRAM progStatEvent)
        {
            m_programState = m_automatic.GetProgramStatus();
            UpdateControls();
        }

        public void OnToolChangedImpl(int a_lChannel, HeidenhainDNCLib.IJHToolId a_pidToolOut, HeidenhainDNCLib.IJHToolId a_pidToolIn, System.DateTime a_dTimeStamp)
        {
            System.Diagnostics.Trace.WriteLine("OnToolChangedImpl: out: "
                + a_pidToolOut.lToolId + "." + a_pidToolOut.lSpareToolId + "." + a_pidToolOut.lIndex
                + ", in: "
                + a_pidToolIn.lToolId + "." + a_pidToolIn.lSpareToolId + "." + a_pidToolIn.lIndex
                );

            // release argument COM object
            System.Runtime.InteropServices.Marshal.ReleaseComObject(a_pidToolIn);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(a_pidToolOut);
        }

        public void OnDncModeChangedImpl(HeidenhainDNCLib.DNC_MODE newDncMode)
        {}

        public void OnProgramChangedImpl(int a_lChannel, System.DateTime a_dTimeStamp, string a_strNewProgram)
        {}

        public void OnExecutionMessageImpl(int a_lChannel, object a_varNumericValue, string a_strValue)
        {
            System.Diagnostics.Trace.WriteLine("OnExecutionMessageImpl:"
                + "\n numeric: "
                + a_varNumericValue.ToString()
                + "\n text:    "
                + a_strValue
                );
        }

        public void OnExecutionModeChangedImpl(int lChannel, HeidenhainDNCLib.DNC_EXEC_MODE executionMode)
        {}

        public void OnErrorImpl(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int lErrorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string bstrError, int lChannel)
        {
            System.Diagnostics.Trace.WriteLine("OnErrorImpl");

            // show new error
            errorsTextBox.AppendText("++1 : " + lErrorNumber.ToString("X") + ":" + bstrError + "\n");
        }

        public void OnErrorClearedImpl(int lErrorNumber, int lChannel)
        {
            System.Diagnostics.Trace.WriteLine("OnErrorClearedImpl");

            // show cleared error
            errorsTextBox.AppendText("--1 : " + lErrorNumber.ToString("X") + "\n");
        }

        ////////////////////////////////////////////////////////////////////////////
        // Error raised handler
        public void OnError2Impl(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry)
        {
            System.Diagnostics.Trace.WriteLine("OnError2Impl");

            // show new error
            errorsTextBox.AppendText("++2 : " + a_pErrorEntry.Number.ToString("X") + ":" + a_pErrorEntry.Text + "\n");
        }

        ////////////////////////////////////////////////////////////////////////////
        // Error cleared handler
        public void OnErrorCleared2Impl(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry)
        {
            System.Diagnostics.Trace.WriteLine("OnErrorCleared2Impl");

            // show cleared error
            errorsTextBox.AppendText("--2 : " + a_pErrorEntry.Number.ToString("X") + ":" + a_pErrorEntry.Text + "\n");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // COM callback methods
        //      These callbacks will be executed in the context of the event firing thread of the COM object.
        //      Using the BeginInvoke() method the execution is passed to the application thread.
        //      See On<event>Impl() methods.
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////
        // Server state changed COM event handler
        private void OnCOMStateChanged(HeidenhainDNCLib.DNC_EVT_STATE eventValue)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMStateChanged: " + eventValue);

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnStateChangedHandler(OnStateChangedImpl), eventValue);
        }

        ////////////////////////////////////////////////////////////////////////////
        // Program state changed COM event handler
        private void OnCOMProgramStateChanged(HeidenhainDNCLib.DNC_EVT_PROGRAM eventValue)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMProgramStateChanged: " + eventValue);

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnProgramStateChangedHandler(OnProgramStateChangedImpl), eventValue);
        }

        private void OnCOMToolChanged(int a_lChannel, HeidenhainDNCLib.IJHToolId a_pidToolOut, HeidenhainDNCLib.IJHToolId a_pidToolIn, System.DateTime a_dTimeStamp)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMToolChanged");

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnToolChangedHandler(OnToolChangedImpl), a_lChannel, a_pidToolOut, a_pidToolIn, a_dTimeStamp);
        }

        private void OnCOMDncModeChanged(HeidenhainDNCLib.DNC_MODE eventValue)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMDncModeChanged: " + eventValue);

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnDncModeChangedHandler(OnDncModeChangedImpl), eventValue);
        }

        private void OnCOMProgramChanged(int a_lChannel, System.DateTime a_dTimeStamp, string a_strNewProgram)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMProgramChanged");

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnProgramChangedHandler(OnProgramChangedImpl), a_lChannel, a_dTimeStamp, a_strNewProgram);
        }

        private void OnCOMExecutionMessage(int a_lChannel, object a_varNumericValue, string a_strValue)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMExecutionMesage");

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnExecutionMessageHandler(OnExecutionMessageImpl), a_lChannel, a_varNumericValue, a_strValue);
        }

        private void OnCOMExecutionModeChanged(int lChannel, HeidenhainDNCLib.DNC_EXEC_MODE executionMode)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMExecutionModeChanged: " + executionMode);

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnExecutionModeChangedHandler(OnExecutionModeChangedImpl), lChannel, executionMode);
        }

        private void OnCOMError(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int lErrorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string bstrError, int lChannel)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMError: " + errorClass + ", " + lErrorNumber);

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnErrorHandler(OnErrorImpl), errorGroup, lErrorNumber, errorClass, bstrError, lChannel);
        }

        private void OnCOMErrorCleared(int lErrorNumber, int lChannel)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMErrorCleared: " + lErrorNumber);

            // Thread-safe invocation of the event handler
            BeginInvoke(new OnErrorClearedHandler(OnErrorClearedImpl), lErrorNumber, lChannel);
        }

        ////////////////////////////////////////////////////////////////////////////
        // Error COM event handler
        private void OnCOMError2(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMError2"); System.Diagnostics.Trace.Flush();

            {
                // 1- copy COM object argument
                HeidenhainDNCLib.JHErrorEntry myError;
                myError = a_pErrorEntry;

                // 2- Thread-safe invocation of the event handler
                // Note that passing a COM object as argument may result in an interface leak as signalled by JHMachine::Disconnect
                // Alternatively the contents of the COM object can be read and passed to the GUI thread in another way.
                BeginInvoke(new OnError2Handler(OnError2Impl), myError);

                // 3- release COM object
                // IMPORTANT: steps 1 and 3 are required to prevent interface leaks (error DNC_E_NOT_POS_NOW on IJHMachine::Disconnect)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myError);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // ErrorCleared COM event handler
        private void OnCOMErrorCleared2(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry)
        {
            System.Diagnostics.Trace.WriteLine("OnCOMErrorCleared2");

            {
                // 1- copy COM object argument
                HeidenhainDNCLib.JHErrorEntry myError;
                myError = a_pErrorEntry;

                // 2- Thread-safe invocation of the event handler
                // Note that passing a COM object as argument may result in an interface leak as signalled by JHMachine::Disconnect
                // Alternatively the contents of the COM object can be read and passed to the GUI thread in another way.
                BeginInvoke(new OnErrorCleared2Handler(OnErrorCleared2Impl), myError);

                // 3- release COM object
                // IMPORTANT: steps 1 and 3 are required to prevent interface leaks (error DNC_E_NOT_POS_NOW on IJHMachine::Disconnect)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myError);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // Helper functions
        ////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////
        // Fill the connection list combobox
        private void FillConnectionList(string selectedConnection)
        {
            connectionList.Items.Clear();

            HeidenhainDNCLib.JHMachineInProcess machine = new HeidenhainDNCLib.JHMachineInProcess();

            foreach (HeidenhainDNCLib.IJHConnection connection in machine.ListConnections())
                connectionList.Items.Add(connection.name);

            if( String.IsNullOrEmpty(selectedConnection) )
                connectionList.SelectedIndex = 0;
            else
                connectionList.SelectedIndex = connectionList.FindStringExact(selectedConnection, 0);

            // release COM object
            System.Runtime.InteropServices.Marshal.ReleaseComObject(machine);
        }

        ////////////////////////////////////////////////////////////////////////////
        // Make the DNC connection
        private void Connect()
        {
            Text = "VC# Example - Connected to " + connectionList.Text;

            // JHAutomatic
            {
                // Create the Automatic object
                m_automatic = (HeidenhainDNCLib.JHAutomatic)m_machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAUTOMATIC);

                /*  Hook up the event handlers to each required COM event, using the C# event abstraction approach.
                 *  (As opposed to the raw approach (as in ErrorListener))
                 *  IMPORTANT:
                 *      A COM event connection is created for each event handler that is added.
                 *      As a result, each COM event will be fired <n>-times, but only one will actually call the handler method.
                 */
                {
#if VTABLE_EVENTS

    #if RAW_COM_EVENTS
                    /*  Hook up the event handlers to the COM interface, using the more efficient raw approach.
                     *  Pro:
                     *      - each COM event is fired only once
                     *  Con:
                     *      - a handler must be defined for each COM event of the given COM interface
                     */
                    m_automaticListener = new AutomaticListener(this, m_automatic);
    #else

                    /*  Typecasting to <vtable interface>_Event advises the COM VTable interface.
                     *  VTable (as opposed to IDispatch) is more efficient.
                     */
                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnProgramStatusChanged +=
                        new HeidenhainDNCLib._IJHAutomaticEvents_OnProgramStatusChangedEventHandler(OnCOMProgramStateChanged);

        #if NOTUSED
                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnToolChanged +=
                        new HeidenhainDNCLib._IJHAutomaticEvents2_OnToolChangedEventHandler(OnCOMToolChanged);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnDncModeChanged +=
                        new HeidenhainDNCLib._IJHAutomaticEvents_OnDncModeChangedEventHandler(OnCOMDncModeChanged);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnExecutionMessage +=
                        new HeidenhainDNCLib._IJHAutomaticEvents2_OnExecutionMessageEventHandler(OnCOMExecutionMessage);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnExecutionModeChanged +=
                        new HeidenhainDNCLib._IJHAutomaticEvents3_OnExecutionModeChangedEventHandler(OnCOMExecutionModeChanged);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnProgramChanged +=
                        new HeidenhainDNCLib._IJHAutomaticEvents2_OnProgramChangedEventHandler(OnCOMProgramChanged);
        #endif
    #endif
#else
                    /*  The default event abstraction will advise the COM IDispatch interface.
                     *  IDispatch uses an event ID to find the correct handler, which is less efficient.
                     */

                    m_automatic.OnProgramStatusChanged +=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramStatusChangedEventHandler(OnCOMProgramStateChanged);
                    m_automatic.OnExecutionMessage +=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionMessageEventHandler(OnCOMExecutionMessage);
                    m_automatic.OnToolChanged +=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnToolChangedEventHandler(OnCOMToolChanged);

    #if NOTUSED
                    m_automatic.OnDncModeChanged +=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnDncModeChangedEventHandler(OnCOMDncModeChanged);

                    m_automatic.OnProgramChanged +=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramChangedEventHandler(OnCOMProgramChanged);

                    m_automatic.OnExecutionModeChanged +=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionModeChangedEventHandler(OnCOMExecutionModeChanged);
    #endif
#endif
                }

                // Get the initial program status.
                m_programState = m_automatic.GetProgramStatus();
            }

            // JHError
            {
                // Create the Error object
                m_error = (HeidenhainDNCLib.JHError)m_machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHERROR);

                {
#if VTABLE_EVENTS

    #if RAW_COM_EVENTS
                    /*  Hook up the event handlers to the COM interface, using the more efficient raw approach.
                     *  Pro:
                     *      - each COM event is fired only once
                     *  Con:
                     *      - a handler must be defined for each COM event of the given COM interface
                     */
                    m_errorListener = new ErrorListener(this, m_error);
    #else
                    /*  Hook up the event handlers to each required COM event, using the C# event abstraction approach.
                     *  (As opposed to the raw approach (as in ErrorListener))
                     *  IMPORTANT:
                     *      A COM event connection is created for each event handler that is added.
                     *      As a result, each COM event will be fired <n>-times, but only one will actually call the handler method.
                     *
                     *      Hooking up all 4 events, will result in an interface leak as signalled by the JHMachine::Disconnect method.
                     *      This is caused by the use of the thread-safe BeginInvoke method inside the event handler.
                     */
        #if ERROR_V1
                    // Note the use of _IJHErrorEvents_Event (version 1) and _IJHErrorEvents_<event>EventHandler_2 (extra '_2')
                    ((HeidenhainDNCLib._IJHErrorEvents_Event)m_error).OnError +=
                        new HeidenhainDNCLib._IJHErrorEvents_OnErrorEventHandler_2(OnCOMError);
                    ((HeidenhainDNCLib._IJHErrorEvents_Event)m_error).OnErrorCleared +=
                        new HeidenhainDNCLib._IJHErrorEvents_OnErrorClearedEventHandler_2(OnCOMErrorCleared);
        #else
                    // Note the use of _IJHErrorEvents2_Event (version 2) and _IJHErrorEvents2_<event>EventHandler
                    ((HeidenhainDNCLib._IJHErrorEvents2_Event)m_error).OnError2 +=
                        new HeidenhainDNCLib._IJHErrorEvents2_OnError2EventHandler(OnCOMError2);
                    ((HeidenhainDNCLib._IJHErrorEvents2_Event)m_error).OnErrorCleared2 +=
                        new HeidenhainDNCLib._IJHErrorEvents2_OnErrorCleared2EventHandler(OnCOMErrorCleared2);
        #endif
    #endif
#else
    #if ERROR_V1
                    m_error.OnError +=
                        new HeidenhainDNCLib._DJHErrorEvents_OnErrorEventHandler(OnCOMError);
                    m_error.OnErrorCleared +=
                        new HeidenhainDNCLib._DJHErrorEvents_OnErrorClearedEventHandler(OnCOMErrorCleared);
    #else
                    m_error.OnError2 +=
                        new HeidenhainDNCLib._DJHErrorEvents_OnError2EventHandler(OnCOMError2);
                    m_error.OnErrorCleared2 +=
                        new HeidenhainDNCLib._DJHErrorEvents_OnErrorCleared2EventHandler(OnCOMErrorCleared2);
    #endif
#endif
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // Stop the DNC connection
        private void Disconnect()
        {
            /*  MSDN: How to: Handle Events Raised by a COM Source
             *  Note that COM objects that raise events within a .NET client require two Garbage Collector (GC) collections before they are released.
             *  This is caused by the reference cycle that occurs between COM objects and managed clients.
             *  If you need to explicitly release a COM object you should call the Collect method twice.
             *
             *  This is important for COM events that pass COM objects as arguments, s.a. JHError::OnError2.
             *  If the argument was not explicitly released, it will hold a reference to the parent JHError object, thus preventing a successful Disconnect().
             *
             *  IMPORTANT:
             *      If the C# event abstraction is used instead of the raw approach (as in ErrorListener), a COM event connection is created for each event handler that is added.
             *      As a result, each COM event will be fired <n>-times, but only one will actually call the handler method.
             *      Since no handler code is executed for the other connections, the arguments cannot be explicitly released.
             *      That can only be done by the general garbage collector as shown here.
             */
            System.GC.Collect();
            System.GC.Collect();

            // prevent re-entry
            if (m_serverState != HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED)
            {
                if (m_automatic != null)
                {
                    // Unhook the JHAutomatic event handler(s)
#if VTABLE_EVENTS
    #if RAW_COM_EVENTS
                    // Stop the event listener
                    m_automaticListener.Stop();
                    m_automaticListener = null;
    #else

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnProgramStatusChanged -=
                        new HeidenhainDNCLib._IJHAutomaticEvents_OnProgramStatusChangedEventHandler(OnCOMProgramStateChanged);

        #if NOTUSED
                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnToolChanged -=
                        new HeidenhainDNCLib._IJHAutomaticEvents2_OnToolChangedEventHandler(OnCOMToolChanged);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnDncModeChanged -=
                        new HeidenhainDNCLib._IJHAutomaticEvents_OnDncModeChangedEventHandler(OnCOMDncModeChanged);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnProgramChanged -=
                        new HeidenhainDNCLib._IJHAutomaticEvents2_OnProgramChangedEventHandler(OnCOMProgramChanged);

                    ((HeidenhainDNCLib._IJHAutomaticEvents3_Event)m_automatic).OnExecutionMessage -=
                        new HeidenhainDNCLib._IJHAutomaticEvents2_OnExecutionMessageEventHandler(OnCOMExecutionMessage);
        #endif
    #endif
#else
                    m_automatic.OnProgramStatusChanged -=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramStatusChangedEventHandler(OnCOMProgramStateChanged);
                    m_automatic.OnExecutionMessage -=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionMessageEventHandler(OnCOMExecutionMessage);
                    m_automatic.OnToolChanged -=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnToolChangedEventHandler(OnCOMToolChanged);

    #if NOTUSED
                    m_automatic.OnDncModeChanged -=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnDncModeChangedEventHandler(OnCOMDncModeChanged);

                    m_automatic.OnProgramChanged -=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnProgramChangedEventHandler(OnCOMProgramChanged);

                    m_automatic.OnExecutionModeChanged -=
                        new HeidenhainDNCLib._DJHAutomaticEvents_OnExecutionModeChangedEventHandler(OnCOMExecutionModeChanged);
    #endif
#endif

                    // Explicitly release the JHAutomatic COM object, to allow a successful disconnect.
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_automatic);
                    m_automatic = null;
                }

                if (m_error != null)
                {
                    // Unhook the JHError event handler(s)
#if VTABLE_EVENTS
    #if RAW_COM_EVENTS
                    // Stop the event listener
                    m_errorListener.Stop();
                    m_errorListener = null;
    #else
        #if ERROR_V1
                    ((HeidenhainDNCLib._IJHErrorEvents_Event)m_error).OnError +=
                        new HeidenhainDNCLib._IJHErrorEvents_OnErrorEventHandler_2(OnCOMError);
                    ((HeidenhainDNCLib._IJHErrorEvents_Event)m_error).OnErrorCleared +=
                        new HeidenhainDNCLib._IJHErrorEvents_OnErrorClearedEventHandler_2(OnCOMErrorCleared);
        #else
                    ((HeidenhainDNCLib._IJHErrorEvents2_Event)m_error).OnError2 -=
                        new HeidenhainDNCLib._IJHErrorEvents2_OnError2EventHandler(OnCOMError2);
                    ((HeidenhainDNCLib._IJHErrorEvents2_Event)m_error).OnErrorCleared2 -=
                        new HeidenhainDNCLib._IJHErrorEvents2_OnErrorCleared2EventHandler(OnCOMErrorCleared2);
        #endif
    #endif

#else
        #if ERROR_V1
                    m_error.OnError -=
                        new HeidenhainDNCLib._DJHErrorEvents_OnErrorEventHandler(OnCOMError);
                    m_error.OnErrorCleared -=
                        new HeidenhainDNCLib._DJHErrorEvents_OnErrorClearedEventHandler(OnCOMErrorCleared);
        #else
                    m_error.OnError2 -=
                        new HeidenhainDNCLib._DJHErrorEvents_OnError2EventHandler(OnCOMError2);
                    m_error.OnErrorCleared2 -=
                        new HeidenhainDNCLib._DJHErrorEvents_OnErrorCleared2EventHandler(OnCOMErrorCleared2);
        #endif
#endif

                    // Explicitly release the JHError COM object, to allow a successful disconnect.
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_error);
                    m_error = null;
                }

                // Unhook the JHMachine event handler(s)
                {
#if VTABLE_EVENTS
    #if RAW_COM_EVENTS
                    // Stop the event listener
                    m_machineListener.Stop();
                    m_machineListener = null;
    #else
                    ((HeidenhainDNCLib._IJHMachineEvents2_Event)m_machine).OnStateChanged -=
                        new HeidenhainDNCLib._IJHMachineEvents2_OnStateChangedEventHandler(OnCOMStateChanged);
    #endif
#else
                    m_machine.OnStateChanged -= new HeidenhainDNCLib._DJHMachineEvents2_OnStateChangedEventHandler(OnCOMStateChanged);
#endif
                }

                // disconnect from the server
                try
                {
                    m_machine.Disconnect();

                    // Explicitly release the JHMachine COM object, to allow a successful new connection.
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_machine);
                    m_machine = null;

                    m_serverState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
                }
                catch (System.Runtime.InteropServices.COMException cex)
                {
                    string errMsg = "Error: " + cex.Message;
                    MessageBox.Show(errMsg, "VC# Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
            }
        }
        // end: Stop the DNC connection

        ////////////////////////////////////////////////////////////////////////////
        // Update the controls on the Dialog
        private void UpdateControls()
        {
            // Set enable state of the controls

            // active upon not connected
            connectButton.Enabled = (m_serverState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED)
                                    && (connectionList.Items.Count > 0);
            immediateConnectButton.Enabled = (m_serverState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED)
                                    && (connectionList.Items.Count > 0);

            connectionList.Enabled = m_serverState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;
            configureConnectionsButton.Enabled = m_serverState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;

            // active upon connected
            disconnectButton.Enabled = m_serverState != HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED;


            serverStateTextBox.Text = m_serverState.ToString();
            serverStateTextBox.Update();
            programStateTextBox.Text = m_serverState == HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE
                                                             ? m_programState.ToString()
                                                             : "???";
            programStateTextBox.Update();
        }
   }

#if RAW_COM_EVENTS

    // VTable IJHMachineEvents event sink class
    public class MachineListener : HeidenhainDNCLib._IJHMachineEvents2
    {
        // the parent dialog is stored, so the events can be passed
        private CsharpSampleDialog m_dialog;

        // The IConnectionPoint interface is stored, so it can be used by Stop()/destructor
        private System.Runtime.InteropServices.ComTypes.IConnectionPoint icp;
        // The cookie identifies the advise operation. It is passed to the Unadvise method.
        private int cookie = -1;

        // constructor, passing the parent dialog and the parent COM interface as arguments
#if INPROC
        public MachineListener(CsharpSampleDialog a_rDialog, HeidenhainDNCLib.JHMachineInProcess a_machine)
#else
        public MachineListener(CsharpSampleDialog a_rDialog, HeidenhainDNCLib.JHMachine a_machine)
#endif
        {
            m_dialog = a_rDialog;

            // get IConnectionPointContainer
            System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)a_machine;

            Guid g = typeof(HeidenhainDNCLib._IJHMachineEvents2).GUID;
            // get IConnectionPoint for the required interface
            icpc.FindConnectionPoint(ref g, out icp);

            // Advise the interface:
            // This means that the COM events are attached to the client implementation (a.k.a. as the event sink)
            // Here the event sink is this class.
            icp.Advise(this, out cookie);
        }

        ~MachineListener()
        {
            Stop();
        }

        public void Stop()
        {
            if (cookie != -1)
            {
                // Unadvise the interface:
                // This means that the COM events are detached from the client implementation (a.k.a. as the event sink)
                icp.Unadvise(cookie);
                cookie = -1;
            }

            // The IConnectionPoint is no longer required: release it
            System.Runtime.InteropServices.Marshal.ReleaseComObject(icp);
        }

        // Event Handlers

        // IJHMachineEvents2
        public void OnStateChanged(HeidenhainDNCLib.DNC_EVT_STATE a_event)
        {
            System.Diagnostics.Trace.WriteLine("MachineListener::OnStateChanged");
            m_dialog.BeginInvoke(new OnStateChangedHandler(m_dialog.OnStateChangedImpl), a_event);
        }
    }
    // end: VTable IJHMachineEvents event sink class

    // VTable IJHErrorEvents event sink class
    public class ErrorListener : HeidenhainDNCLib._IJHErrorEvents2
    {
        // the parent dialog is stored, so the events can be passed
        private CsharpSampleDialog m_dialog;

        // The IConnectionPoint interface is stored, so it can be used by Stop()/destructor
        private System.Runtime.InteropServices.ComTypes.IConnectionPoint icp;
        // The cookie identifies the advise operation. It is passed to the Unadvise method.
        private int cookie = -1;

        // constructor, passing the parent dialog and the parent COM interface as arguments
        public ErrorListener(CsharpSampleDialog a_rDialog, HeidenhainDNCLib.JHError a_error)
        {
            m_dialog = a_rDialog;

            // get IConnectionPointContainer
            System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)a_error;

            Guid g = typeof(HeidenhainDNCLib._IJHErrorEvents2).GUID;
            // get IConnectionPoint for the required interface
            icpc.FindConnectionPoint(ref g, out icp);

            // Advise the interface:
            // This means that the COM events are attached to the client implementation (a.k.a. as the event sink)
            // Here the event sink is this class.
            icp.Advise(this, out cookie);
        }

        ~ErrorListener()
        {
            Stop();
        }

        public void Stop()
        {
            if (cookie != -1)
            {
                // Unadvise the interface:
                // This means that the COM events are detached from the client implementation (a.k.a. as the event sink)
                icp.Unadvise(cookie);
                cookie = -1;
            }

            // The IConnectionPoint is no longer required: release it
            System.Runtime.InteropServices.Marshal.ReleaseComObject(icp);
        }

        // Event Handlers

        // IJHErrorEvents
        public void OnError(HeidenhainDNCLib.DNC_ERROR_GROUP errorGroup, int lErrorNumber, HeidenhainDNCLib.DNC_ERROR_CLASS errorClass, string bstrError, int lChannel)
        {
            System.Diagnostics.Trace.WriteLine("ErrorListener::OnError");
            m_dialog.BeginInvoke(new OnErrorHandler(m_dialog.OnErrorImpl), errorGroup, lErrorNumber, errorClass, bstrError, lChannel );
        }

        public void OnErrorCleared(int lErrorNumber, int lChannel)
        {
            System.Diagnostics.Trace.WriteLine("ErrorListener::OnErrorCleared");
            m_dialog.BeginInvoke(new OnErrorClearedHandler(m_dialog.OnErrorClearedImpl), lErrorNumber, lChannel);
        }

        // IJHErrorEvents2
        public void OnError2(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry)
        {
            System.Diagnostics.Trace.WriteLine("ErrorListener::OnError2");

            // Since this code is executed in the context of the COM event server (callback),
            // the call must be passed to the client thread.
            m_dialog.Invoke(new OnError2Handler(m_dialog.OnError2Impl), a_pErrorEntry);

            // The JHErrorEntry holds a reference to the parent JHError.
            // This must be released prior to disconnecting.
            // The .NET garbage collector will sometime release the JHErrorEntry.
            // To ensure it is released before disconnecting, it is released here explicitly.
            // This also ensures that memory is not claimed unnecessarily.
            // As a fallback the Disconnect() activates the garbage collector.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(a_pErrorEntry);
        }

        public void OnErrorCleared2(HeidenhainDNCLib.JHErrorEntry a_pErrorEntry)
        {
            System.Diagnostics.Trace.WriteLine("ErrorListener::OnErrorCleared2");

            // Since this code is executed in the context of the COM event server (callback),
            // the call must be passed to the client thread.
            m_dialog.Invoke(new OnErrorCleared2Handler(m_dialog.OnErrorCleared2Impl), a_pErrorEntry);

            // The JHErrorEntry holds a reference to the parent JHError.
            // This must be released prior to disconnecting.
            // The .NET garbage collector will sometime release the JHErrorEntry.
            // To ensure it is released before disconnecting, it is released here explicitly.
            // This also ensures that memory is not claimed unnecessarily.
            // As a fallback the Disconnect() activates the garbage collector.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(a_pErrorEntry);
        }

    }
    // end: VTable IJHErrorEvents event sink class

   // VTable IJHAutomaticEvents event sink class
    public class AutomaticListener : HeidenhainDNCLib._IJHAutomaticEvents3
    {
        private CsharpSampleDialog m_dialog;

        private System.Runtime.InteropServices.ComTypes.IConnectionPoint icp;
        private int cookie = -1;

        public AutomaticListener(CsharpSampleDialog a_rDialog, HeidenhainDNCLib.JHAutomatic a_automatic)
        {
            m_dialog = a_rDialog;

            System.Runtime.InteropServices.ComTypes.IConnectionPointContainer icpc = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer)a_automatic;

            Guid g = typeof(HeidenhainDNCLib._IJHAutomaticEvents3).GUID;
            icpc.FindConnectionPoint(ref g, out icp);

            icp.Advise(this, out cookie);

            // releasing the CPC will destruct the COM object !!!!
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(icpc);
        }

        ~AutomaticListener()
        {
            Stop();
        }

        public void Stop()
        {
            if (cookie != -1)
            {
                icp.Unadvise(cookie);
                cookie = -1;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(icp);
        }

        // Event Handlers

        // _IJHAutomaticEvents
        public void OnProgramStatusChanged (/*[in]*/ HeidenhainDNCLib.DNC_EVT_PROGRAM programEvent )
        {
            System.Diagnostics.Trace.WriteLine("raw: OnProgramStatusChanged: " + programEvent);

            // Thread-safe invocation of the event handler
            m_dialog.BeginInvoke(new OnProgramStateChangedHandler(m_dialog.OnProgramStateChangedImpl), programEvent);
        }

        public void OnDncModeChanged (      /*[in]*/ HeidenhainDNCLib.DNC_MODE newDncMode )
        {}

        // _IJHAutomaticEvents2
        public void OnExecutionMessage (    /*[in]*/ int lChannel, /*[in]*/ object varNumericValue,                /*[in]*/ string bstrValue )
        {}
        public void OnProgramChanged (      /*[in]*/ int lChannel, /*[in]*/ System.DateTime dTimeStamp,            /*[in]*/ string bstrNewProgram )
        {}
        public void OnToolChanged (         /*[in]*/ int lChannel, /*[in]*/ HeidenhainDNCLib.IJHToolId pidToolOut,  /*[in]*/ HeidenhainDNCLib.IJHToolId pidToolIn, /*[in]*/ System.DateTime dTimeStamp )
        {
            System.Diagnostics.Trace.WriteLine("raw: OnToolChanged");

            // Thread-safe invocation of the event handler
            m_dialog.BeginInvoke(new OnToolChangedHandler(m_dialog.OnToolChangedImpl),  lChannel, pidToolOut, pidToolIn, dTimeStamp );
        }

        //_IJHAutomaticEvents3
        public void OnExecutionModeChanged (/*[in]*/ int lChannel, /*[in]*/ HeidenhainDNCLib.DNC_EXEC_MODE executionMode )
        {}
    }
    // end: VTable IJHAutomaticEvents event sink class

#endif

}