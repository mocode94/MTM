// IOSimDemoDlg.h : header file
//

#if !defined(AFX_IOSimDemoDLG_H__CA53C32E_4157_4B8A_9174_53BC1FF78EAD__INCLUDED_)
#define AFX_IOSimDemoDLG_H__CA53C32E_4157_4B8A_9174_53BC1FF78EAD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CIOSimDemoDlg dialog

class CIOSimDemoDlg : public CDialog
{
// Construction
public:
	CIOSimDemoDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CIOSimDemoDlg)
	enum { IDD = IDD_IOSimDemo_DIALOG };
	CButton	m_ButtonToggle;
	CButton	m_CheckSPLC;
	CComboBox	m_ComboSignalId;
	CButton	m_ButtonSetHSCI;
	CButton	m_ButtonSet;
	CButton	m_ButtonGet;
	CComboBox	m_ComboConnector;
	CComboBox	m_ComboHardware;
	CString	m_strName;
	CString	m_strType;
	UINT	m_Clamp;
	CString	m_strAddress;
	CString	m_strValue;
	BOOL	m_bPlcRunning;
	BOOL	m_bVirtualTNC;
	BOOL	m_ControlReady;
  long  m_lSimId;
	BOOL	m_bHSCIConfiguration;
  CString	m_strInstance;
	//}}AFX_DATA


	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CIOSimDemoDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
  
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CIOSimDemoDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnGetAddressName();
	afx_msg void OnGetAddressClamp();
	afx_msg void OnSelchangeHw();
	afx_msg void OnGetValue();
	afx_msg void OnSetValue();
	afx_msg void OnStatus();
	afx_msg void OnSignalsimulation();
	afx_msg void OnSetHsciValue();
	afx_msg void OnSplc();
	afx_msg void OnChangeName();
	afx_msg void OnToggleValue();
	afx_msg void OnSelectInstance();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

protected:
  void SetValue(long lAddressOffset, int Type, LPCSTR szValue);
  void RefreshClampLists(BOOL IsHSCIConfiguration);
	void GetAddressByName(bool quiet);
	static BOOL IsBinaryType(int iType);
	static BOOL IsLogicalName(CString strName);
	void EnableGetSetToggle(BOOL fIsBinary, BOOL fEnable = TRUE);
	void RefreshAddress(unsigned long lAddressOffset, int iType);
  int GetType(CString &strType);
	void ShowError(int nError);
  virtual void OnOK();
public:
  CButton m_ButtonGetAddressByName;
//  afx_msg void OnClose();
  virtual void PostNcDestroy();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_IOSimDemoDLG_H__CA53C32E_4157_4B8A_9174_53BC1FF78EAD__INCLUDED_)
