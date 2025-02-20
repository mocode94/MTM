// MfcSampleDlg.cpp : implementation file
//

#include "stdafx.h"
#include "MfcSample.h"
#include "MfcSampleDlg.h"
#include "MachineSink.h"
#include "AutomaticSink.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMfcSampleDlg dialog



// Constructor
CMfcSampleDlg::CMfcSampleDlg( CWnd* pParent /*=NULL*/ )
    : CDialog(CMfcSampleDlg::IDD, pParent)
    , m_connectionState( HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED )
    , m_programState( HeidenhainDNCLib::DNC_PRG_STS_IDLE )
    , m_pMachine( 0 )
    , m_pAutomatic( 0 )
    , m_pMachineSink ( 0 )
    , m_pAutomaticSink( 0 )
    , m_machineEventCookie( 0 )
    , m_automaticEventCookie( 0 )
{
    m_hIcon = AfxGetApp()->LoadIcon( IDR_MAINFRAME );
}

void CMfcSampleDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    DDX_Control(pDX, IDC_CONNECT, m_connectButton);
    DDX_Control(pDX, IDC_DISCONNECT, m_disconnectButton);
    DDX_Control(pDX, IDC_CONFIGURE_CONNECTIONS, m_configureConnectionsButton);
    DDX_Control(pDX, IDC_MACHINE_LIST, m_connectionList);
    DDX_Control(pDX, IDC_MACHINE_STATE, m_connectionStateTextBox);
    DDX_Control(pDX, IDC_PROGRAM_STATE, m_programStateTextBox);

    if( !pDX->m_bSaveAndValidate )
    {	// Set enable state of the controls
        m_connectButton.EnableWindow( ( m_connectionState == HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED )
                                        && ( m_connectionList.GetCount() > 0 ) );
        m_disconnectButton.EnableWindow( m_connectionState != HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED );
        m_connectionList.EnableWindow( m_connectionState == HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED );
        m_configureConnectionsButton.EnableWindow( m_connectionState == HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED );

        // Update the connection state and program state text boxes
        SetConnectionState();
        SetProgramState();
    }
}

BEGIN_MESSAGE_MAP(CMfcSampleDlg, CDialog)
    ON_WM_PAINT()
    ON_WM_QUERYDRAGICON()
    //}}AFX_MSG_MAP
    ON_BN_CLICKED(IDC_CONNECT, &CMfcSampleDlg::OnBnClickedConnect)
    ON_BN_CLICKED(IDC_DISCONNECT, &CMfcSampleDlg::OnBnClickedDisconnect)
    ON_BN_CLICKED(IDC_CONFIGURE_CONNECTIONS, &CMfcSampleDlg::OnBnClickedConfigureConnections)
    ON_WM_CLOSE()

    // User messages
    ON_MESSAGE(WM_CONNECTIONSTATE_CHANGED, &CMfcSampleDlg::OnConnectionStateChanged)
    ON_MESSAGE( WM_PROGSTAT_CHANGED, &CMfcSampleDlg::OnProgStatChanged)
END_MESSAGE_MAP()


// CMfcSampleDlg message handlers

BOOL CMfcSampleDlg::OnInitDialog()
{
    CDialog::OnInitDialog();

    // Set the icon for this dialog.  The framework does this automatically
    //  when the application's main window is not a dialog
    SetIcon(m_hIcon, TRUE);			// Set big icon
    SetIcon(m_hIcon, FALSE);		// Set small icon

    // TODO: Add extra initialization here
    // Initialize the machine list and select the first item from the list
    HRESULT hr = m_pMachine.CoCreateInstance(HeidenhainDNCLib::CLSID_JHMachine );
    if( SUCCEEDED( hr ) )
    {
        FillConnectionList( _T( "" ) );
    }
    UpdateData( FALSE );     // Force dialog to update

    return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMfcSampleDlg::OnPaint()
{
    if (IsIconic())
    {
        CPaintDC dc(this); // device context for painting

        SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

        // Center icon in client rectangle
        int cxIcon = GetSystemMetrics(SM_CXICON);
        int cyIcon = GetSystemMetrics(SM_CYICON);
        CRect rect;
        GetClientRect(&rect);
        int x = (rect.Width() - cxIcon + 1) / 2;
        int y = (rect.Height() - cyIcon + 1) / 2;

        // Draw the icon
        dc.DrawIcon(x, y, m_hIcon);
    }
    else
    {
        CDialog::OnPaint();
    }
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CMfcSampleDlg::OnQueryDragIcon()
{
    return static_cast<HCURSOR>(m_hIcon);
}

// Connect button click handler
void CMfcSampleDlg::OnBnClickedConnect()
{
    // TODO: Add your control notification handler code here
    SetWindowText( _T( "VC++ Example - Connecting..." ) );
    m_connectionState = HeidenhainDNCLib::DNC_STATE_HOST_IS_NOT_AVAILABLE;
    UpdateData( FALSE );

    CString reasonOfFailure;
    CString connectionName;

    // Request a connect to the selected machine
    m_connectionList.GetWindowText( connectionName );

    HRESULT hr = m_pMachine->ConnectRequest( CComBSTR( connectionName ) );
    if( SUCCEEDED( hr ) )
    {	// Connect the machine sink
        m_machineEventCookie = 0;
        m_pMachineSink->CreateInstance( &m_pMachineSink );
        m_pMachineSink->SetWindow( this );
        hr = m_pMachine.Advise( reinterpret_cast<IUnknown *>(m_pMachineSink)
            , HeidenhainDNCLib::IID__IJHMachineEvents2
            , &m_machineEventCookie );
        if( FAILED( hr ) )
        {	// Failed to advise the event sink
            reasonOfFailure = _T( "Could not advise MachineSink" );
        }
        else
        {	// Initialize the event sink and set the connection state text box
//            m_pMachineSink->SetWindow( this );
            hr = m_pMachine->GetState( &m_connectionState );
            TRACE( "OnBnClickedConnect: Connection state=%d\n", m_connectionState );
            if( m_connectionState == HeidenhainDNCLib::DNC_STATE_MACHINE_IS_AVAILABLE )
            {	// CNC is available, so complete the connection
                Connect();
            }   // Else wait for the required state change
            UpdateData( FALSE );
        }
    }
    else
    {	// The requested host does not respond
        reasonOfFailure = _T( "CNC probably not on line" );
    }

    if( FAILED( hr ) )
    { // Show the failure message box
        CString errMsg = connectionName.IsEmpty()
            ? _T( "No connection selected" )
            : _T( "Could not connect to " ) + connectionName + _T( "\n\nReason:\n" ) + reasonOfFailure;
        AfxMessageBox( errMsg, MB_OK | MB_ICONINFORMATION );
        SetWindowText( _T( "Not connected" ) );
        m_connectionState = HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED;
        UpdateData( FALSE );
    }
}

// Disconnect button click handler
void CMfcSampleDlg::OnBnClickedDisconnect()
{
    // TODO: Add your control notification handler code here
    SetWindowText( _T( "VC++ Example - Not connected" )) ;
    m_connectionState = HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED;
    Disconnect();

    m_programState = HeidenhainDNCLib::DNC_PRG_STS_IDLE;
    UpdateData( FALSE ); 
}

// Configure Connections button click handler
void CMfcSampleDlg::OnBnClickedConfigureConnections()
{
    // TODO: Add your control notification handler code here
    // Get the selected connection name
    CString connectionName;
    m_connectionList.GetWindowText( connectionName );

    // Activate the HeidenhainDNC connection dialog
    EnableWindow( FALSE );  // Disable the application while the Connection dialog is active
    _variant_t name( connectionName );
    HRESULT hr = m_pMachine->ConfigureConnection( HeidenhainDNCLib::DNC_CONFIGURE_MODE_ALL, &name );
    EnableWindow();
    FillConnectionList( CString( name ) );
    UpdateData( FALSE ); 
}

// Close dialog message handler
void CMfcSampleDlg::OnClose()
{
    // TODO: Add your message handler code here and/or call default
    Disconnect();
    m_pMachine.Release();

    CDialog::OnClose();
}

// Connection state message handler
LRESULT CMfcSampleDlg::OnConnectionStateChanged( WPARAM wParam, LPARAM lParam )
{
    HeidenhainDNCLib::DNC_EVT_STATE eventValue = (HeidenhainDNCLib::DNC_EVT_STATE) lParam;
    if( eventValue == HeidenhainDNCLib::DNC_EVT_STATE_MACHINE_AVAILABLE )
    {	// CNC is available, so make the actual connect
        Connect();
    }
    else if( eventValue == HeidenhainDNCLib::DNC_EVT_STATE_PERMISSION_DENIED )
    {   // CNC has denied access
        CString connectionName;
        m_connectionList.GetWindowText( connectionName );
        SetWindowText( _T( "VC++ Example - Access denied by " ) + connectionName );
    }

    HRESULT hr = m_pMachine->GetState( &m_connectionState );
    TRACE( "OnConnectionChanged: Connection state=%d\n", m_connectionState );
    UpdateData( FALSE );

    return 0;
}

// Program state message handler
LRESULT CMfcSampleDlg::OnProgStatChanged( WPARAM wParam, LPARAM lParam )
{
    HRESULT hr = m_pAutomatic->GetProgramStatus( &m_programState );
    UpdateData( FALSE );

    return 0;
}

// Helper functions

// Fill the connection list. Select the first item in the list when the specified name is not in the list
void CMfcSampleDlg::FillConnectionList( CString SelectedMachine )
{
    m_connectionList.ResetContent();

    CComPtr< HeidenhainDNCLib::IJHConnectionList > pConnections;
    HRESULT hr = m_pMachine->ListConnections( &pConnections );
    if( SUCCEEDED( hr ) )
    {
        long count = 0;
        hr = pConnections->get_Count( &count );
        for( int index = 0; index < count; index++ )
        {	// Load the know machine names in the connection list
            CComPtr< HeidenhainDNCLib::IJHConnection > pConnction;
            hr = pConnections->get_Item( index, &pConnction );
            if( SUCCEEDED( hr ) )
            {
                CComBSTR bstrName;
                hr = pConnction->get_name( &bstrName );
                if( SUCCEEDED( hr ) )
                {
                    m_connectionList.AddString( CString( bstrName ) );
                }
            }
        }

        // Select SelectedMachine
        if( m_connectionList.SelectString( 0, SelectedMachine) == CB_ERR )
        {   // If the selected string is empty or not valid, select the first machine name
            (void) m_connectionList.SetCurSel( 0 );
        }
    }
    UpdateData( FALSE );
}

// Make the connection
void CMfcSampleDlg::Connect(void)
{
    CString reasonOfFailure;

    // Get the Automatic Object
    CComPtr<IUnknown> pAutomaticObject;
    HRESULT hr = m_pMachine->GetInterface( HeidenhainDNCLib::DNC_INTERFACE_JHAUTOMATIC, &pAutomaticObject );
    if( FAILED( hr ) )
    {
        reasonOfFailure = _T( "Could not create IJHAutomatic object" );
    }
    else
    {
        m_pAutomatic = 0;
        hr = pAutomaticObject->QueryInterface( __uuidof( m_pAutomatic ), (void**) &m_pAutomatic );
        if( FAILED( hr ) )
        {
            reasonOfFailure = _T( "Could not QueryInterface IJHAutomatic interface" );
        }
    }
    // Explicitly release the smart pointer here, it is no longer required.
    pAutomaticObject.Release();

    if( SUCCEEDED( hr ) )
    {	// Connect the automatic sink
        m_automaticEventCookie = 0;
        m_pAutomaticSink->CreateInstance( &m_pAutomaticSink );
        hr = m_pAutomatic.Advise( reinterpret_cast<IUnknown *>(m_pAutomaticSink)
            , HeidenhainDNCLib::IID__IJHAutomaticEvents2
            , &m_automaticEventCookie );
        if( FAILED( hr ) )
        {	// Failed to advise the event sink
            reasonOfFailure = _T( "Could not advise AutomaticSink" );
        }
        else
        {	// Initialize the automatic sink and set the program state
            m_pAutomaticSink->SetWindow( this );
            hr = m_pAutomatic->GetProgramStatus( &m_programState );
        }
    }

    CString connectionName;
    m_connectionList.GetWindowText( connectionName );
    if( SUCCEEDED( hr ) )
    { // Connection made
        SetWindowText(  _T( "VC++ Example - Connected to " ) + connectionName );
    }
    else
    {	// Connection failed. Show message box
        CString errMsg = connectionName.IsEmpty()
            ? _T( "No connection selected" )
            : _T( "Could not connect to " ) + connectionName + _T( "\n\nReason:\n" ) + reasonOfFailure;
        AfxMessageBox( errMsg, MB_OK | MB_ICONINFORMATION );
        SetWindowText( _T( "VC++ Example - Not connected" ) );
        Disconnect();
    }
}

// Break the connection
void CMfcSampleDlg::Disconnect(void)
{
    if( m_automaticEventCookie != 0 )
    {
        AtlUnadvise( m_pMachine, HeidenhainDNCLib::IID__IJHAutomaticEvents2, m_automaticEventCookie );
        m_automaticEventCookie = 0;
    }

    if( m_pAutomatic != 0 )
    {
        m_pAutomatic.Release();
    }

    if( m_machineEventCookie != 0 )
    {
        AtlUnadvise( m_pMachine, HeidenhainDNCLib::IID__IJHMachineEvents2, m_machineEventCookie );
        m_machineEventCookie = 0;
    }

    HRESULT hr = m_pMachine->Disconnect();	// Ignore errors
}

// Set the connection state text box
void CMfcSampleDlg::SetConnectionState()
{
    CString connectionStateText("???");

    switch( m_connectionState )
    {
    case HeidenhainDNCLib::DNC_STATE_NOT_INITIALIZED:
        connectionStateText = _T( "DNC_STATE_NOT_INITIALIZED" );
        break;
    case HeidenhainDNCLib::DNC_STATE_HOST_IS_NOT_AVAILABLE:
        connectionStateText = _T( "DNC_STATE_HOST_IS_NOT_AVAILABLE" );
        break;
    case HeidenhainDNCLib::DNC_STATE_HOST_IS_AVAILABLE:
        connectionStateText = _T( "DNC_STATE_HOST_IS_AVAILABLE" );
        break;
    case HeidenhainDNCLib::DNC_STATE_WAITING_PERMISSION:
        connectionStateText = _T( "DNC_STATE_WAITING_PERMISSION" );
        break;
    case HeidenhainDNCLib::DNC_STATE_DNC_IS_AVAILABLE:
        connectionStateText = _T( "DNC_STATE_DNC_IS_AVAILABLE" );
        break;
    case HeidenhainDNCLib::DNC_STATE_MACHINE_IS_BOOTED:
        connectionStateText = _T( "DNC_STATE_MACHINE_IS_BOOTED" );
        break;
    case HeidenhainDNCLib::DNC_STATE_MACHINE_IS_INITIALIZING:
        connectionStateText = _T( "DNC_STATE_MACHINE_IS_INITIALIZING" );
        break;
    case HeidenhainDNCLib::DNC_STATE_MACHINE_IS_AVAILABLE:
        connectionStateText = _T( "DNC_STATE_MACHINE_IS_AVAILABLE" );
        break;
    case HeidenhainDNCLib::DNC_STATE_MACHINE_IS_SHUTTING_DOWN:
        connectionStateText = _T( "DNC_STATE_MACHINE_IS_SHUTTING_DOWN" );
        break;
    case HeidenhainDNCLib::DNC_STATE_DNC_IS_STOPPED:
        connectionStateText = _T( "DNC_STATE_DNC_IS_STOPPED" );
        break;
    case HeidenhainDNCLib::DNC_STATE_NO_PERMISSION:
        connectionStateText = _T( "DNC_STATE_NO_PERMISSION" );
        break;
    case HeidenhainDNCLib::DNC_STATE_HOST_IS_STOPPED:
        connectionStateText = _T( "DNC_STATE_HOST_IS_STOPPED" );
        break;
    default:
        // Do nothing
        break;
    }
    m_connectionStateTextBox.SetWindowText( connectionStateText );
}

// Set the program state text box
void CMfcSampleDlg::SetProgramState()
{
    CString programStateText("???");

    if( m_connectionState == HeidenhainDNCLib::DNC_STATE_MACHINE_IS_AVAILABLE )
    {
        switch( m_programState )
        {
        case HeidenhainDNCLib::DNC_PRG_STS_IDLE:
            programStateText = _T( "DNC_PRG_STS_IDLE" );
            break;
        case HeidenhainDNCLib::DNC_PRG_STS_RUNNING:
            programStateText = _T( "DNC_PRG_STS_RUNNING" );
            break;
        case HeidenhainDNCLib::DNC_PRG_STS_STOPPED:
            programStateText = _T( "DNC_PRG_STS_STOPPED" );
            break;
        case HeidenhainDNCLib::DNC_PRG_STS_INTERRUPTED:
            programStateText = _T( "DNC_PRG_STS_INTERRUPTED" );
            break;
        case HeidenhainDNCLib::DNC_PRG_STS_FINISHED:
            programStateText = _T( "DNC_PRG_STS_FINISHED" );
            break;
        case HeidenhainDNCLib::DNC_PRG_STS_ERROR:
            programStateText = _T( "DNC_PRG_STS_ERROR" );
            break;
        case HeidenhainDNCLib::DNC_PRG_STS_NOT_SELECTED:
            programStateText = _T( "DNC_PRG_STS_NOT_SELECTED" );
            break;
        default:
            // Do nothing
            break;
        }
    }

    m_programStateTextBox.SetWindowText( programStateText );
}

void CMfcSampleDlg::ExampleMethods()
{
    AddRecordToTable( L"\\TABLE\\TOOL", L"T", L"999" );

    DeleteRecordFromTable( L"\\TABLE\\TOOL", L"T", L"999" );
}

// AddRecordToTable( L"\\TABLE\\TOOL, L"T", L"999" )

void CMfcSampleDlg::AddRecordToTable(   CComBSTR a_bstrTableIdent, 
                                        CComBSTR a_bstrPrimaryKeyName, 
                                        CComBSTR a_bstrPrimaryKeyValue )
{
    // to add a record, a new (non-existing) primary key is added to the list

    HRESULT hr;
    CComPtr<HeidenhainDNCLib::IJHDataEntry2> pPrimaryKeyColumn;
    CComPtr<HeidenhainDNCLib::IJHDataEntry2> pNewRecord;
    CComPtr<HeidenhainDNCLib::IJHDataEntry2> pPrimaryKeyField;

    // create ident for the primary key column
    CComBSTR bstrPrimaryKeyIdent( a_bstrTableIdent );
    bstrPrimaryKeyIdent.Append(L"\\");
    bstrPrimaryKeyIdent.Append(a_bstrPrimaryKeyName);

    // get primary key column: e.g. "\TABLE\TOOL\T"
    hr = m_pDataAccess->GetDataEntry2(
        bstrPrimaryKeyIdent, 
        HeidenhainDNCLib::DNC_DATA_UNIT_SELECT_METRIC,         // unitSelect
        VARIANT_FALSE,                                         // asString
        &pPrimaryKeyColumn
        );

    // the child list of the primary key column contains all present primary keys
    // to add a record, a new (non-existing) primary key is added to the list
    // e.g. 999 for "\TABLE\TOOL\T\999"
    if(SUCCEEDED(hr))
    {
        hr = pPrimaryKeyColumn->add_child(a_bstrPrimaryKeyValue, &pNewRecord);
    }

    // post: "\TABLE\TOOL\T\999"

    // the new 'child' is still empty, so add a primary key field to it.
    // e.g. T for "\TABLE\TOOL\T\999\T"
    if(SUCCEEDED(hr))
    {
        hr = pNewRecord->add_child(a_bstrPrimaryKeyName, &pPrimaryKeyField);
    }

    // post: "\TABLE\TOOL\T\999\T"

    // the new field has no value yet: write new primary key value
    // e.g. 999 for "\TABLE\TOOL\T\999\T" = 999
    if(SUCCEEDED(hr))
    {
        // now modify the value of the DATA property of the primary key FIELD 
        // (e.g."\TABLE\TOOL\T\999\T" = 999)
        // AND update the server accordingly
        // Note: up to here all operations were done on the local data objects.
        // Note: although the primary key field may be a numeric value, 
        //       it is allowed to write it as a string here.
        CComVariant newPrimaryKeyValue(a_bstrPrimaryKeyValue);
        hr = pPrimaryKeyField->SetPropertyValue( 
            HeidenhainDNCLib::DNC_DATAENTRY_PROPKIND_DATA, 
            newPrimaryKeyValue, 
            VARIANT_FALSE       // no flush
            );

        // post: record "\TABLE\TOOL\T\999" is created on the server. 
        //       Since only the primary key field was given, 
        //       all other fields are initialized with their respective default values.

        // Note that Read-Only fields can only be given a non-default value, 
        // if they are written together with the primary key value.
        // This requires that first these fields are initialized in the local data tree, 
        // after which they can be written to the server all at once by WriteTreeValues().
        // An existing record should be used to get the correct property types.
    }

}//end-AddRecordToTable


void CMfcSampleDlg::DeleteRecordFromTable(  CComBSTR a_bstrTableIdent, 
                                            CComBSTR a_bstrPrimaryKeyName, 
                                            CComBSTR a_bstrPrimaryKeyValue )
{
    // to delete a record, the primary key field of a record is set to VT_EMPTY

    HRESULT hr;
    CComPtr<HeidenhainDNCLib::IJHDataEntry2> pPrimaryKeyField;

    // create ident for the primary key field, e.g. "\TABLE\TOOL\T\999\T"
    CComBSTR bstrPrimaryKeyFieldIdent( a_bstrTableIdent );
    bstrPrimaryKeyFieldIdent.Append(L"\\");
    bstrPrimaryKeyFieldIdent.Append(a_bstrPrimaryKeyName);
    bstrPrimaryKeyFieldIdent.Append(L"\\");
    bstrPrimaryKeyFieldIdent.Append(a_bstrPrimaryKeyValue);
    bstrPrimaryKeyFieldIdent.Append(L"\\");
    bstrPrimaryKeyFieldIdent.Append(a_bstrPrimaryKeyName);

    // get the primary key field, e.g. "\TABLE\TOOL\T\999\T"
    hr = m_pDataAccess->GetDataEntry2(   bstrPrimaryKeyFieldIdent, 
        HeidenhainDNCLib::DNC_DATA_UNIT_SELECT_METRIC,         // unitSelect
        VARIANT_FALSE,                                         // asString
        &pPrimaryKeyField
        );

    // the primary key field contains at least a DATA property
    // to delete the record, the DATA property is set to VT_EMPTY
    // e.g. "\TABLE\TOOL\T\999\T" = <empty>
    if(SUCCEEDED(hr))
    {
        CComVariant keyEmptyValue;
        hr = pPrimaryKeyField->SetPropertyValue( 
            HeidenhainDNCLib::DNC_DATAENTRY_PROPKIND_DATA, 
            keyEmptyValue, 
            VARIANT_FALSE       // no flush
            );
    }

}//end-DeleteRecordFromTable


void CMfcSampleDlg::Subscribe( CComBSTR a_bstrIdent )
{
    // to receive change notifications on a single data entry or a tree of data entries,
    // the data entry must be 'subscribed'

    HRESULT hr;
    CComPtr<HeidenhainDNCLib::IJHDataEntry2> pDataEntryToSubscribe;

    // get the requested data entry object, e.g. "\TABLE\TOOL"
    hr = m_pDataAccess->GetDataEntry2(
        a_bstrIdent, 
        HeidenhainDNCLib::DNC_DATA_UNIT_SELECT_METRIC,         // unitSelect
        VARIANT_FALSE,                                         // asString
        &pDataEntryToSubscribe
        );

    HeidenhainDNCLib::DNC_HANDLE subscriptionHandle;
    if(SUCCEEDED(hr))
    {
        hr = pDataEntryToSubscribe->Subscribe2( &subscriptionHandle );
    }

    if(SUCCEEDED(hr))
    {
        // post: the subscription handle uniquely identifies this subscription
        //       it is passed with the IJHDataAccessEvents2::OnData2 event
        //       it is also required to pass with the UnSubscribe method.
        //
        //       Any changes in the subscribed data (-tree) will now fire
        //       the IJHDataAccessEvents2::OnData2 event.
    }

}//end-Subscribe

STDMETHODIMP DataAccessSink::OnData2( HeidenhainDNCLib::DNC_HANDLE a_subscriptionHandle, DATE a_timeStamp, HeidenhainDNCLib::IJHDataEntry2List * a_pChangeList )
{
    HRESULT hr=S_OK;

    //--- NOTE: Make sure the receiver of this message deletes the allocated data !!!!
    CEventData* pData = new CEventData;

    if (pData)
    {
        pData->subscriptionHandle   = a_subscriptionHandle;
        pData->timeStamp            = a_varTimeStamp;
        a_pChangeList->QueryInterface( &pData->pChangeList );

        m_csQ.Lock();
        {
            m_eventQueue.push_back(pData);
        }
        m_csQ.Unlock();
    }

    return hr;

}//end-OnData2

