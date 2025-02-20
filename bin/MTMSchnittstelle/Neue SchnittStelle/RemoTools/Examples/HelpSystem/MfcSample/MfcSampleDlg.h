// MfcSampleDlg.h : header file
//

#pragma once
#include "afxwin.h"

class CMachineSink;
class CAutomaticSink;


// CMfcSampleDlg dialog
class CMfcSampleDlg : public CDialog
{
// Construction
public:
    CMfcSampleDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
    enum { IDD = IDD_MFCSAMPLE_DIALOG };

    protected:
    virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
    HICON m_hIcon;

    // Generated message map functions
    virtual BOOL OnInitDialog();
    afx_msg void OnPaint();
    afx_msg HCURSOR OnQueryDragIcon();
    afx_msg void OnBnClickedConnect();
    afx_msg void OnBnClickedDisconnect();
    afx_msg void OnBnClickedConfigureConnections();
    afx_msg void OnClose();
    // Application specific message handlers
    afx_msg LRESULT OnConnectionStateChanged( WPARAM wParam, LPARAM lParam );
    afx_msg LRESULT OnProgStatChanged( WPARAM wParam, LPARAM lParam );
    DECLARE_MESSAGE_MAP()

private:
    // Button makes DNC connection to CNC
    CButton m_connectButton;
    // Button breaks the DNC connection
    CButton m_disconnectButton;
    // Button activates the Heidenhain Connection dialog
    CButton m_configureConnectionsButton;
    // Connection list
    CComboBox m_connectionList;
    // Connection state text box
    CEdit m_connectionStateTextBox;
    // Program state text box
    CEdit m_programStateTextBox;
    // Connecting state
    HeidenhainDNCLib::DNC_STATE m_connectionState;
    // Program state(Automatic interface)
    HeidenhainDNCLib::DNC_STS_PROGRAM m_programState;
    // Pointer to Machine object
    CComPtr< HeidenhainDNCLib::IJHMachine2 > m_pMachine;
    // Pointer to Automatic interface object
    CComPtr< HeidenhainDNCLib::IJHAutomatic2 > m_pAutomatic;
    // Pointer to MachineEvent sink
    CComObject<CMachineSink> * m_pMachineSink;
    // Pointer to AutomaticEvent sink
    CComObject<CAutomaticSink> * m_pAutomaticSink;
    // Cookie for Advise/Release of the machine event sink
    unsigned long m_machineEventCookie;
    // Cookie for Advise/Release of the automatic event sink
    unsigned long m_automaticEventCookie;

    // Pointer to DataAccess interface object
    CComPtr< HeidenhainDNCLib::IJHDataAccess3 > m_pDataAccess;


private:
    // Fill the connection list. Select the first item in the list when the specified name is not in the list
    void FillConnectionList( CString SelectedMachine );
    // Make the connection
    void Connect( void );
    // Break the connection
    void Disconnect( void );
    // Set the connection state text box
    void SetConnectionState();
    // Set the program state text box
    void SetProgramState();

    // Dummy method to allow calling example methods. This allows the compiler to check the arguments.
    void ExampleMethods();

    // Add a record to a table (IJHDataAccess "\\TABLE")
    void AddRecordToTable(CComBSTR a_bstrTableIdent, CComBSTR a_bstrPrimaryKeyName, CComBSTR a_bstrPrimaryKeyValue);
    void DeleteRecordFromTable(CComBSTR a_bstrTableIdent, CComBSTR a_bstrPrimaryKeyName, CComBSTR a_bstrPrimaryKeyValue);
    void Subscribe( CComBSTR a_bstrIdent );

};
