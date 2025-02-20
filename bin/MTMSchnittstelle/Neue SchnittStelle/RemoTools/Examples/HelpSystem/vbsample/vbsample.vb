'Delegate definitions
Public Delegate Sub OnConnectionStateChangedHandler(ByVal stateEvent As HeidenhainDNCLib.DNC_EVT_STATE)
Public Delegate Sub OnProgramStateChangedHandler(ByVal progStatEvent As HeidenhainDNCLib.DNC_EVT_PROGRAM)

Public Class VbSampleDialog

    ' Machine object
    Private m_machine As HeidenhainDNCLib.JHMachine = Nothing
    ' Connection state
    Private m_connectionState As HeidenhainDNCLib.DNC_STATE = _
        HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED

    ' Automatic object
    Private m_automatic As HeidenhainDNCLib.JHAutomatic = Nothing
    ' Program state
    Private m_programState As HeidenhainDNCLib.DNC_STS_PROGRAM = _
        HeidenhainDNCLib.DNC_STS_PROGRAM.DNC_PRG_STS_IDLE

    '----------------------------------------------------------------------------------------
    ' Event handlers
    '----------------------------------------------------------------------------------------

    ' Load event handler
    Private Sub VbSampleDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Create the Machine object
        m_machine = New HeidenhainDNCLib.JHMachine

        ' Initialize the controls
        FillConnectionList("")
        UpdateControls()
    End Sub

    ' Connect button click handler
    Private Sub connectButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles connectButton.Click
        Text = "VB Example - Connecting..."
        m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE
        UpdateControls()

        ' Request the DNC connection and connect the OnStateChanged event handler
        Try
            m_machine.ConnectRequest(connectionList.Text)
            AddHandler m_machine.OnStateChanged, AddressOf OnCOMConnectionStateChanged

            'Get the initial connection state. This also starts the state events coming
            m_connectionState = m_machine.GetState()
            If m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE Then
                Connect()
            Else
                Text = "VB Example - Waiting for " + connectionList.Text + " to start..."
            End If
            UpdateControls()
        Catch cex As System.Runtime.InteropServices.COMException
            Dim errMsg
            If connectionList.Text.Length = 0 Then
                errMsg = "No connection selected"
            Else
                errMsg = "Could not connect to " + connectionList.Text + vbNewLine + vbNewLine + "Error: " + cex.Message
            End If
            MessageBox.Show(errMsg, "VB Example", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Disconnect()
            m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED
            UpdateControls()
            Text = "VB Example - Not connected"
        End Try
    End Sub

    ' Disconnect button click handler
    Private Sub disconnectButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles disconnectButton.Click
        Disconnect()
        m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED
        UpdateControls()
        Text = "VB Example - Not connected"
    End Sub

    ' Configure connections button click handler
    Private Sub configureConnectionsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles configureConnectionsButton.Click
        Dim connectionName = connectionList.Text
        Enabled = False    ' Disable the dialog while the Connections dialog is active
        m_machine.ConfigureConnection(HeidenhainDNCLib.DNC_CONFIGURE_MODE.DNC_CONFIGURE_MODE_ALL, connectionName)
        Enabled = True     ' Enable the dialog again
        FillConnectionList(connectionName)
    End Sub

    ' Exit button click handler
    Private Sub exitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Close()
    End Sub

    ' FormClosing event handler 
    Private Sub VbSampleDialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Disconnect()
        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_machine)
    End Sub

    ' Connection state changed handler
    Private Sub OnConnectionStateChanged(ByVal stateEvent As HeidenhainDNCLib.DNC_EVT_STATE)
        If stateEvent = HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_AVAILABLE Then
            Connect()
        ElseIf stateEvent = HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_PERMISSION_DENIED Then
            ' DNC access permission has been denied by CNC operator
            Text = "VB Example - Access denied by " + connectionList.Text
        ElseIf (stateEvent = HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_MACHINE_SHUTTING_DOWN) _
            OrElse (stateEvent = HeidenhainDNCLib.DNC_EVT_STATE.DNC_EVT_STATE_CONNECTION_LOST) Then
            ' CNC is shutting down or connection lost
            Text = "VB Example - Connection closed by " + connectionList.Text
        End If

        m_connectionState = m_machine.GetState()
        UpdateControls()
    End Sub

    ' Program state changed handler
    Private Sub OnProgramStateChanged(ByVal progStatEvent As HeidenhainDNCLib.DNC_EVT_PROGRAM)
        m_programState = m_automatic.GetProgramStatus()
        UpdateControls()
    End Sub

    ' State changed COM event handler
    Private Sub OnCOMConnectionStateChanged(ByVal stateEvent As HeidenhainDNCLib.DNC_EVT_STATE)
        System.Diagnostics.Trace.WriteLine("OnCOMConnectionStateChanged: " + stateEvent.ToString())
        ' Thread-safe invocation of the OnConnectionStateChanged handler
        Invoke(New OnConnectionStateChangedHandler(AddressOf OnConnectionStateChanged), stateEvent)
    End Sub

    ' Program state changed COM event handler
    Private Sub OnCOMProgStatChanged(ByVal progStatEvent As HeidenhainDNCLib.DNC_EVT_PROGRAM)
        System.Diagnostics.Trace.WriteLine("onCOMProgStatChanged: " + progStatEvent.ToString())
        ' Thread-safe invocation of the OnProgramStateChanged handler
        Invoke(New OnProgramStateChangedHandler(AddressOf OnProgramStateChanged), progStatEvent)
    End Sub

    '----------------------------------------------------------------------------------------
    ' Helper functions
    '----------------------------------------------------------------------------------------

    ' Fill the connection list combo box
    Private Sub FillConnectionList(ByVal selectedConnection As String)
        connectionList.Items.Clear()

        ' The For Each compiles correctly but gives a runtime exception (connection = null)
        ' For Each connection As HeidenhainDNCLib.IJHConnection In m_machine.ListConnections()
        '     connectionList.Items.Add(connection.name)
        ' Next
        Dim connections As HeidenhainDNCLib.IJHConnectionList = m_machine.ListConnections()
        Dim count As Integer = connections.Count
        For index As Integer = 0 To count - 1
            connectionList.Items.Add(connections(index).name)
        Next

        If String.IsNullOrEmpty(selectedConnection) Then
            connectionList.SelectedIndex = 0
        Else
            connectionList.SelectedIndex = connectionList.FindStringExact(selectedConnection, 0)
        End If
    End Sub

    ' Make the DNC connection
    Private Sub Connect()
        Text = "VB Example - Connected to " + connectionList.Text

        ' Create the Automatic object and connect the OnProgramStateChanged event handler
        m_automatic = m_machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAUTOMATIC)
        AddHandler m_automatic.OnProgramStatusChanged, AddressOf OnCOMProgStatChanged

        ' Get the initial program status.
        m_programState = m_automatic.GetProgramStatus()
    End Sub

    ' Break the DNC connection
    Private Sub Disconnect()
        If Not m_automatic Is Nothing Then
            RemoveHandler m_automatic.OnProgramStatusChanged, AddressOf OnCOMProgStatChanged
            System.Runtime.InteropServices.Marshal.ReleaseComObject(m_automatic)
            m_automatic = Nothing
        End If

        RemoveHandler m_machine.OnStateChanged, AddressOf OnCOMConnectionStateChanged

        Try
            m_machine.Disconnect()
        Catch ex As Exception
            ' Ignore errors
        End Try

    End Sub

    'Update the controls on the Form
    Private Sub UpdateControls()
        ' Set enable state of the controls
        connectButton.Enabled = (m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED) And (connectionList.Items.Count > 0)
        disconnectButton.Enabled = m_connectionState <> HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED
        connectionList.Enabled = m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED
        configureConnectionsButton.Enabled = m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_NOT_INITIALIZED

        connectionStateTextBox.Text = m_connectionState.ToString()
        connectionStateTextBox.Update()

        If m_connectionState = HeidenhainDNCLib.DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE Then
            programStateTextBox.Text = m_programState.ToString()
        Else
            programStateTextBox.Text = "???"
        End If
        programStateTextBox.Update()
    End Sub

End Class
