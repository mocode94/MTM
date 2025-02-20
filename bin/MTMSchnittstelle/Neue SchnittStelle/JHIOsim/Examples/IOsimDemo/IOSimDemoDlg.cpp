// IOSimDemoDlg.cpp : implementation file
//

#include "stdafx.h"
#include "IOSimDemo.h"
#include "IOSimDemoDlg.h"

#include "IOsim\JHIOsim.h"

//#define TESTHSCI

#ifdef TESTHSCI
#include "IOsim\JHIOmem.h"
#include "IOsim\JHIOsimIntern.h"
#endif

// test of explicit lock / access / unlock instead of the ...Wait() functions:
#define LOCK_EXPLICITLY

// test of explicit load of the DLL
// #define TEST_DLL_LOAD_EXPLICITLY

#define TEST_VIRTUAL_TNC


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CIOSimDemoDlg dialog

CIOSimDemoDlg::CIOSimDemoDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CIOSimDemoDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CIOSimDemoDlg)
	m_strName = _T("");
	m_strType = _T("");
	m_Clamp = 1;
	m_strAddress = _T("");
	m_strValue = _T("");
	m_bPlcRunning = FALSE;
	m_bVirtualTNC = FALSE;
	m_ControlReady = FALSE;
  m_lSimId = 100;
	m_bHSCIConfiguration = FALSE;
  m_strInstance = _T("");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}


void CIOSimDemoDlg::DoDataExchange(CDataExchange* pDX)
{
  CDialog::DoDataExchange(pDX);
  //{{AFX_DATA_MAP(CIOSimDemoDlg)
  DDX_Control(pDX, IDC_TOGGLE_VALUE, m_ButtonToggle);
  DDX_Control(pDX, IDC_SPLC, m_CheckSPLC);
  DDX_Control(pDX, IDC_SIGNAL_ID, m_ComboSignalId);
  DDX_Control(pDX, IDC_SET_HSCI_VALUE, m_ButtonSetHSCI);
  DDX_Control(pDX, IDC_SET_VALUE, m_ButtonSet);
  DDX_Control(pDX, IDC_GET_VALUE, m_ButtonGet);
  DDX_Control(pDX, IDC_CONNECTOR, m_ComboConnector);
  DDX_Control(pDX, IDC_HW, m_ComboHardware);
  DDX_Text(pDX, IDC_NAME, m_strName);
  DDX_Text(pDX, IDC_TYPE, m_strType);
  DDX_Text(pDX, IDC_CLAMP, m_Clamp);
  DDV_MinMaxUInt(pDX, m_Clamp, 1, 256);
  DDX_Text(pDX, IDC_ADDRESS, m_strAddress);
  DDX_Text(pDX, IDC_VALUE, m_strValue);
  DDX_Check(pDX, IDC_PLCRUNNING, m_bPlcRunning);
  DDX_Check(pDX, IDC_VIRTUALTNC, m_bVirtualTNC);
  DDX_Check(pDX, IDC_CONTROLREADY, m_ControlReady);
  DDX_Text(pDX, IDC_SIM_ID, m_lSimId);
  DDX_Check(pDX, IDC_HSCICONFIGURATION, m_bHSCIConfiguration);
  DDX_Text(pDX, IDC_INSTANCE_NAME, m_strInstance);
  //}}AFX_DATA_MAP
  DDX_Control(pDX, IDC_GET_ADDRESS_NAME, m_ButtonGetAddressByName);
}

BEGIN_MESSAGE_MAP(CIOSimDemoDlg, CDialog)
	//{{AFX_MSG_MAP(CIOSimDemoDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_GET_ADDRESS_NAME, OnGetAddressName)
	ON_BN_CLICKED(IDC_GET_ADDRESS_CLAMP, OnGetAddressClamp)
	ON_CBN_SELCHANGE(IDC_HW, OnSelchangeHw)
	ON_BN_CLICKED(IDC_GET_VALUE, OnGetValue)
	ON_BN_CLICKED(IDC_SET_VALUE, OnSetValue)
	ON_BN_CLICKED(IDC_STATUS, OnStatus)
	ON_BN_CLICKED(IDC_SIGNALSIMULATION, OnSignalsimulation)
	ON_BN_CLICKED(IDC_SET_HSCI_VALUE, OnSetHsciValue)
	ON_BN_CLICKED(IDC_SPLC, OnSplc)
	ON_EN_CHANGE(IDC_NAME, OnChangeName)
	ON_BN_CLICKED(IDC_TOGGLE_VALUE, OnToggleValue)
	ON_BN_CLICKED(IDC_SELECT_INSTANCE, OnSelectInstance)
	//}}AFX_MSG_MAP
//  ON_WM_CLOSE()
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CIOSimDemoDlg message handlers: MFC stuff


BOOL CIOSimDemoDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	


#ifdef TEST_DLL_LOAD_EXPLICITLY

  HMODULE hDll = LoadLibrary("JHIOsim.dll");
  
  if (hDll)
  {
    int nResult;

    IO_FUNC_PTRS IOSim;
  
    GET_IO_PROC_ADDRESSES(hDll, IOSim)

    nResult = IOSim.LockMemory(100);
    if (nResult == JHIO_SUCCESS)
      nResult = IOSim.UnlockMemory();
  
    FreeLibrary(hDll);
  }
#endif


  OnStatus();

  RefreshClampLists(JHIOIsHSCIConfiguration());

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CIOSimDemoDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CIOSimDemoDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

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

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CIOSimDemoDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}



/////////////////////////////////////////////////////////////////////////////
// CIOSimDemoDlg message handlers: Application specific


/////////////////////////////////////////////////////////////////////////////
// OnGetAddressName

void CIOSimDemoDlg::OnGetAddressName() 
{
  GetAddressByName(false);
}

void CIOSimDemoDlg::GetAddressByName(bool quiet) 
{

  int nResult;
  
  unsigned long lAddressOffset;
  int  iType;
  BOOL IsSPLCMode;

  UpdateData();

  IsSPLCMode = m_CheckSPLC.GetCheck();

  if (IsSPLCMode)
    nResult = JHIOGetSPLCAddressByName (m_strName, &lAddressOffset, &iType);
  else
    nResult = JHIOGetAddressByName (m_strName, &lAddressOffset, &iType);
	
  if (nResult == JHIO_ERROR_NOINIT)
  {
    if (!quiet)
    {
      if (IsSPLCMode)
        MessageBox("SPLC program is not running.\n\n"

                   "Please configure the SPLC program in "
                   "- virtualTNC (Programming station with virtualTNC dongle) or \n"
                   "- iTNC 530 programming station in demo mode (without any dongle)" , NULL, MB_ICONEXCLAMATION);
      else
        MessageBox("Shared memory or PLC map file not initialized.\n\n"

                   "Please run \n"
                   "- VirtualTNCsim application (with PLC map file name set) or \n"
                   "- virtualTNC (Programming station with virtualTNC dongle) or \n"
                   "- iTNC 530 programming station in demo mode (without any dongle)" , NULL, MB_ICONEXCLAMATION);

    }
  }
  else if (nResult != JHIO_SUCCESS)
  {
    if (!quiet)
      ShowError(nResult);
  }
  else
  {
    RefreshAddress(lAddressOffset, iType);

    EnableGetSetToggle(IsBinaryType(iType));
    m_ButtonGetAddressByName.EnableWindow(false);
  }
}


/////////////////////////////////////////////////////////////////////////////
// OnGetAddressClamp

void CIOSimDemoDlg::OnGetAddressClamp() 
{
  int nResult;
  
  if (!UpdateData())
    return;


  unsigned long lAddressOffset;
  int iType;

  int iConnector;
  int iHardware;
  CString strConnector;
  iHardware = static_cast<int>(m_ComboHardware.GetItemData(m_ComboHardware.GetCurSel()));
  iConnector = static_cast<int>(m_ComboConnector.GetItemData(m_ComboConnector.GetCurSel()));

  nResult = JHIOGetAddressByClamp(iHardware, iConnector, m_Clamp, &lAddressOffset, &iType);

  if (nResult == JHIO_ERROR_NOINIT)
    MessageBox( "Shared memory not inizialized. \n\n"

                "Please run \n"
                "- VirtualTNCsim application or \n"
                "- virtualTNC (Programming station with virtualTNC dongle) or \n"
                "- Programming station in demo mode (without any dongle)" , NULL, MB_ICONEXCLAMATION);
  else if (nResult != JHIO_SUCCESS)
    ShowError(nResult);
  else
  {
    RefreshAddress(lAddressOffset, iType);
    EnableGetSetToggle(IsBinaryType(iType));
  }
}


/////////////////////////////////////////////////////////////////////////////
// OnSelchangeHw

void CIOSimDemoDlg::OnSelchangeHw() 
{
	CString strHW;
  
  m_ComboHardware.GetWindowText(strHW);

  m_ComboConnector.ResetContent();

  switch (m_ComboHardware.GetItemData(m_ComboHardware.GetCurSel()))
  {
  case JHIO_HW_LE:
    m_ComboConnector.AddString("Internal (ADC)");
    m_ComboConnector.AddString("X8/X9 (DAC)");
    m_ComboConnector.AddString("X12 (TS)");
    m_ComboConnector.AddString("X13 (TS)");
    m_ComboConnector.AddString("X41 (Out)");
    m_ComboConnector.AddString("X42 (In)");
    m_ComboConnector.AddString("X45 (ADC Feed/Speed)");
    m_ComboConnector.AddString("X46 (Panel In/Out)");
    m_ComboConnector.AddString("X48 (ADC)");

    m_ComboConnector.SetItemData(0, JHIO_INTERN);
    m_ComboConnector.SetItemData(1, JHIO_LE_X8_X9);
    m_ComboConnector.SetItemData(2, JHIO_TS_X12);
    m_ComboConnector.SetItemData(3, JHIO_TT_X13);
    m_ComboConnector.SetItemData(4, JHIO_LE_X41);
    m_ComboConnector.SetItemData(5, JHIO_LE_X42);
    m_ComboConnector.SetItemData(6, JHIO_LE_X45);
    m_ComboConnector.SetItemData(7, JHIO_LE_X46);
    m_ComboConnector.SetItemData(8, JHIO_LE_X48);
  
    break;
  
  case JHIO_HW_PL410_1:
  case JHIO_HW_PL410_2:
  case JHIO_HW_PL410_3:
  case JHIO_HW_PL410_4:
    m_ComboConnector.AddString("X3 (PL4XX In)");      m_ComboConnector.SetItemData(0, JHIO_PL4XX_X3);
    m_ComboConnector.AddString("X4 (PL4XX In)");      m_ComboConnector.SetItemData(1, JHIO_PL4XX_X4);
    m_ComboConnector.AddString("X5 (PL410 In)");      m_ComboConnector.SetItemData(2, JHIO_PL410_X5);
    m_ComboConnector.AddString("X6 (PL410 In)");      m_ComboConnector.SetItemData(3, JHIO_PL410_X6);
    m_ComboConnector.AddString("X7 (PL4XX Out)");     m_ComboConnector.SetItemData(4, JHIO_PL4XX_X7);
    m_ComboConnector.AddString("X8 (PL4XX Out)");     m_ComboConnector.SetItemData(5, JHIO_PL4XX_X8);
    m_ComboConnector.AddString("X15-22 (PL410 ADC)"); m_ComboConnector.SetItemData(6, JHIO_PL410_X15_22);
    break;
  
  case JHIO_HW_PL510_1:
  case JHIO_HW_PL510_2:
  case JHIO_HW_PL510_3:
  case JHIO_HW_PL510_4:
    m_ComboConnector.AddString("X4 (slot 0 In)");   m_ComboConnector.SetItemData(0, JHIO_PL510_1_X4);
    m_ComboConnector.AddString("X5 (slot 0 In)");   m_ComboConnector.SetItemData(1, JHIO_PL510_1_X5);
    m_ComboConnector.AddString("X4 (slot 1 In)");   m_ComboConnector.SetItemData(2, JHIO_PL510_2_X4);
    m_ComboConnector.AddString("X5 (slot 1 In)");   m_ComboConnector.SetItemData(3, JHIO_PL510_2_X5);
    m_ComboConnector.AddString("X4 (slot 2 In)");   m_ComboConnector.SetItemData(4, JHIO_PL510_3_X4);
    m_ComboConnector.AddString("X5 (slot 2 In)");   m_ComboConnector.SetItemData(5, JHIO_PL510_3_X5);
    m_ComboConnector.AddString("X4 (slot 3 In)");   m_ComboConnector.SetItemData(6, JHIO_PL510_4_X4);
    m_ComboConnector.AddString("X5 (slot 3 In)");   m_ComboConnector.SetItemData(7, JHIO_PL510_4_X5);

    m_ComboConnector.AddString("X6 (slot 0 Out)");     m_ComboConnector.SetItemData(8, JHIO_PL510_1_X6);
    m_ComboConnector.AddString("X6 (slot 1 Out)");     m_ComboConnector.SetItemData(9, JHIO_PL510_2_X6);
    m_ComboConnector.AddString("X6 (slot 2 Out)");     m_ComboConnector.SetItemData(10, JHIO_PL510_3_X6);
    m_ComboConnector.AddString("X6 (slot 3 Out)");     m_ComboConnector.SetItemData(11, JHIO_PL510_4_X6);

    m_ComboConnector.AddString("X15-22 (slot 0 ADC)"); m_ComboConnector.SetItemData(12, JHIO_PL510_1_X15_22);
    m_ComboConnector.AddString("X15-22 (slot 1 ADC)"); m_ComboConnector.SetItemData(13, JHIO_PL510_2_X15_22);
    m_ComboConnector.AddString("X15-22 (slot 2 ADC)"); m_ComboConnector.SetItemData(14, JHIO_PL510_3_X15_22);
    m_ComboConnector.AddString("X15-22 (slot 3 ADC)"); m_ComboConnector.SetItemData(15, JHIO_PL510_4_X15_22);
    break;
  
  case JHIO_HW_HW410:
    m_ComboConnector.AddString("Inputs");  m_ComboConnector.SetItemData(0, HW410_INPUTS);
    m_ComboConnector.AddString("Outputs"); m_ComboConnector.SetItemData(1, HW410_OUTPUTS);
    break;

  case JHIO_HW_CC:
    m_ComboConnector.AddString("X150");    m_ComboConnector.SetItemData(0, JHIO_CC_X150);
    m_ComboConnector.AddString("X151");    m_ComboConnector.SetItemData(1, JHIO_CC_X151);
    break;

  case JHIO_HW_HSCI_MOP:
    m_ComboConnector.AddString("MOP1");    m_ComboConnector.SetItemData(0, JHIO_MOP_1);
    m_ComboConnector.AddString("MOP2");    m_ComboConnector.SetItemData(1, JHIO_MOP_2);
    m_ComboConnector.AddString("MOP3");    m_ComboConnector.SetItemData(2, JHIO_MOP_3);
    m_ComboConnector.AddString("MOP4");    m_ComboConnector.SetItemData(3, JHIO_MOP_4);
    m_ComboConnector.AddString("MOP5");    m_ComboConnector.SetItemData(4, JHIO_MOP_5);
    m_ComboConnector.AddString("MOP6");    m_ComboConnector.SetItemData(5, JHIO_MOP_6);
    m_ComboConnector.AddString("MOP7");    m_ComboConnector.SetItemData(6, JHIO_MOP_7);
    m_ComboConnector.AddString("MOP8");    m_ComboConnector.SetItemData(7, JHIO_MOP_8);
    m_ComboConnector.AddString("MOP9");    m_ComboConnector.SetItemData(8, JHIO_MOP_9);
    m_ComboConnector.AddString("MOP10");   m_ComboConnector.SetItemData(9, JHIO_MOP_10);
    break;

  case JHIO_HW_HSCI_PL620x:
    m_ComboConnector.AddString("X112 PL1");   m_ComboConnector.SetItemData(0, JHIO_X112_PL_1);
    m_ComboConnector.AddString("X113 PL1");   m_ComboConnector.SetItemData(1, JHIO_X113_PL_1);
    break;
  }

  m_ComboConnector.SetCurSel(0);
}


/////////////////////////////////////////////////////////////////////////////
// OnGetValue

void CIOSimDemoDlg::OnGetValue() 
{
	int nResult;
  
  long lAddressOffset = atol(m_strAddress);

  int Type = GetType(m_strType);
  if (Type == -1)
    return;
   

#ifdef LOCK_EXPLICITLY
  nResult = JHIOLockMemory(1000);
      
  if (nResult != JHIO_SUCCESS)
  {
    ShowError(nResult);
    return;
  }
#endif

  switch (Type)
  {
    case JHIO_TYPE_INPUT:
    case JHIO_TYPE_OUTPUT:
      BOOL fValue;
#ifdef LOCK_EXPLICITLY
      nResult = JHIOGetLogicValue(lAddressOffset, &fValue);
#else
      nResult = JHIOGetLogicValueWait(lAddressOffset, &fValue, 1000);
#endif      
      if (nResult == JHIO_SUCCESS)
        m_strValue = fValue ? "1" : "0";
      break;

    case JHIO_TYPE_INPUTBYTE:
    case JHIO_TYPE_OUTPUTBYTE:
      BYTE bValue;
#ifdef LOCK_EXPLICITLY
      nResult = JHIOGetByteValue(lAddressOffset, &bValue);
#else
      nResult = JHIOGetByteValueWait(lAddressOffset, &bValue, 1000);
#endif      
      if (nResult == JHIO_SUCCESS)
        m_strValue.Format("%d", bValue);
      
      break;

    case JHIO_TYPE_INPUTWORD:
    case JHIO_TYPE_OUTPUTWORD:
      short siValue;
#ifdef LOCK_EXPLICITLY
      nResult = JHIOGetShortValue(lAddressOffset, &siValue);
#else
      nResult = JHIOGetShortValueWait(lAddressOffset, &siValue, 1000);
#endif      
      if (nResult == JHIO_SUCCESS)
        m_strValue.Format("%d", siValue);
      
      break;

    case JHIO_TYPE_INPUTDWORD:
    case JHIO_TYPE_OUTPUTDWORD:
      long lValue;
#ifdef LOCK_EXPLICITLY
      nResult = JHIOGetLongValue(lAddressOffset, &lValue);
#else
      nResult = JHIOGetLongValueWait(lAddressOffset, &lValue, 1000);
#endif

      if (nResult == JHIO_SUCCESS)
        m_strValue.Format("%d", lValue);
      
      break;
    
    case JHIO_TYPE_INPUTFLOAT:
    case JHIO_TYPE_OUTPUTFLOAT:
      float flValue;
#ifdef LOCK_EXPLICITLY
      nResult = JHIOGetFloatValue(lAddressOffset, &flValue);
#else
      nResult = JHIOGetFloatValueWait(lAddressOffset, &flValue, 1000);
#endif      
      if (nResult == JHIO_SUCCESS)
        m_strValue.Format("%f", flValue);
      
      break;
  }

#ifdef LOCK_EXPLICITLY
  JHIOUnlockMemory();
#endif

  if (nResult != JHIO_SUCCESS)
    ShowError(nResult);
  else
    UpdateData(FALSE);
}


/////////////////////////////////////////////////////////////////////////////
// OnSetValue

void CIOSimDemoDlg::OnSetValue() 
{
  UpdateData();

  long lAddressOffset = atol(m_strAddress);

  int Type = GetType(m_strType);
  if (Type == -1)
  {
    MessageBox("Invalid data type"); 
    return;
  }

  SetValue(lAddressOffset, Type, m_strValue);

}



void CIOSimDemoDlg::SetValue(long lAddressOffset, int Type, LPCSTR szValue) 
{
  int nResult;
  int Value;
  
#ifdef LOCK_EXPLICITLY
  nResult = JHIOLockMemory(1000);
  if (nResult != JHIO_SUCCESS)
  {
    ShowError(nResult);
    return;
  }
#endif

  switch (Type)
  {
    case JHIO_TYPE_INPUT:
      
      BOOL fValue;
      
      if (sscanf(szValue, "%d", &Value) != 1 || (Value != 0 && Value != 1))
        MessageBox("Invalid value"); 
      fValue = (Value != 0);

#ifdef LOCK_EXPLICITLY
      nResult = JHIOSetLogicValue(lAddressOffset, fValue);
#else
      nResult = JHIOSetLogicValueWait(lAddressOffset, fValue, 1000);
#endif      
      break;

    case JHIO_TYPE_INPUTBYTE:
      if (sscanf(szValue, "%d", &Value) != 1 || Value < -128 || Value > 127) 
      {
        MessageBox("Invalid value"); 
        break;
      }

#ifdef LOCK_EXPLICITLY
      nResult = JHIOSetByteValue(lAddressOffset, (BYTE)Value);
#else
      nResult = JHIOSetByteValueWait(lAddressOffset, (BYTE)Value, 1000);
#endif      
      break;

    case JHIO_TYPE_INPUTWORD:
      if (sscanf(szValue, "%d", &Value) != 1 || Value < -32768 || Value > 32767)
      {
        MessageBox("Invalid value"); 
        break;
      }

#ifdef LOCK_EXPLICITLY
      nResult = JHIOSetShortValue(lAddressOffset, (short)Value);
#else
      nResult = JHIOSetShortValueWait(lAddressOffset, (short)Value, 1000);
#endif      
      break;

    case JHIO_TYPE_INPUTDWORD:
      if (sscanf(szValue, "%d", &Value) != 1)
      {
        MessageBox("Invalid value"); 
        break;
      }

#ifdef LOCK_EXPLICITLY
      nResult = JHIOSetLongValue(lAddressOffset, Value);
#else
      nResult = JHIOSetLongValueWait(lAddressOffset, Value, 1000);
#endif      
      break;

    case JHIO_TYPE_INPUTFLOAT:
      float flValue;
      if (sscanf(szValue, "%f", &flValue) != 1)
      {
        MessageBox("Invalid value"); 
        break;
      }
#ifdef LOCK_EXPLICITLY
      nResult = JHIOSetFloatValue(lAddressOffset, flValue);
#else
      nResult = JHIOSetFloatValueWait(lAddressOffset, flValue, 1000);
#endif      
      break;
  }
  
#ifdef LOCK_EXPLICITLY
  JHIOUnlockMemory();
#endif

  if (nResult != JHIO_SUCCESS)
    ShowError(nResult);
}


/////////////////////////////////////////////////////////////////////////////
//  CIOSimDemoDlg helper functions


/////////////////////////////////////////////////////////////////////////////
// EnableGetSetToggle

void CIOSimDemoDlg::EnableGetSetToggle(BOOL fIsBinary, BOOL fEnable /*=TRUE*/)
{
  m_ButtonGet.EnableWindow(fEnable);
  m_ButtonSet.EnableWindow(fEnable);
  m_ButtonToggle.EnableWindow(fIsBinary && fEnable);
}


/////////////////////////////////////////////////////////////////////////////
// RefreshAddress

void CIOSimDemoDlg::RefreshAddress(unsigned long lAddressOffset, int iType)
{
    switch (iType)
    {
      case JHIO_TYPE_INPUT: m_strType = "INPUT"; break;
      case JHIO_TYPE_OUTPUT: m_strType = "OUTPUT"; break;

      case JHIO_TYPE_INPUTBYTE: m_strType = "INPUTBYTE"; break;
      case JHIO_TYPE_INPUTWORD: m_strType = "INPUTWORD"; break;
      case JHIO_TYPE_INPUTDWORD: m_strType = "INPUTDWORD"; break;
      case JHIO_TYPE_INPUTFLOAT: m_strType = "INPUTFLOAT"; break;

      case JHIO_TYPE_OUTPUTBYTE: m_strType = "OUTPUTBYTE"; break;
      case JHIO_TYPE_OUTPUTWORD: m_strType = "OUTPUTWORD"; break;
      case JHIO_TYPE_OUTPUTDWORD: m_strType = "OUTPUTDWORD"; break;
      case JHIO_TYPE_OUTPUTFLOAT: m_strType = "OUTPUTFLOAT"; break;
    }
    
    m_strAddress.Format("%d", lAddressOffset);
    
    UpdateData(FALSE);
}


/////////////////////////////////////////////////////////////////////////////
// GetType

int CIOSimDemoDlg::GetType(CString &strType)
{
  if (m_strType == "INPUT")
    return JHIO_TYPE_INPUT;
  else if (m_strType == "OUTPUT")
    return JHIO_TYPE_OUTPUT;
  else if (m_strType == "INPUTBYTE") 
    return JHIO_TYPE_INPUTBYTE;
  else if (m_strType == "INPUTWORD") 
    return JHIO_TYPE_INPUTWORD; 
  else if (m_strType == "INPUTDWORD") 
    return JHIO_TYPE_INPUTDWORD;
  else if (m_strType == "INPUTFLOAT") 
    return JHIO_TYPE_INPUTFLOAT;
  else if (m_strType == "OUTPUTBYTE") 
    return JHIO_TYPE_OUTPUTBYTE; 
  else if (m_strType == "OUTPUTWORD") 
    return JHIO_TYPE_OUTPUTWORD; 
  else if (m_strType == "OUTPUTDWORD") 
    return JHIO_TYPE_OUTPUTDWORD;
  else if (m_strType == "OUTPUTFLOAT") 
    return JHIO_TYPE_OUTPUTFLOAT;
  else
    return (-1);
}


/////////////////////////////////////////////////////////////////////////////
// ShowError

void CIOSimDemoDlg::ShowError(int nError)
{
  CString strMsg = JHIOGetErrorString(nError);
  MessageBox(strMsg);
}


void CIOSimDemoDlg::OnStatus() 
{
  BOOL IsHSCI = JHIOIsHSCIConfiguration();
  if (m_bHSCIConfiguration != IsHSCI)   // Bei Änderung Hardware-Listen aktualisieren
    RefreshClampLists(IsHSCI);
  m_bHSCIConfiguration = IsHSCI;        // neuen Status übernehmen

  m_bPlcRunning = JHIOIsPLCRunning();
  m_bVirtualTNC = JHIOIsVirtualTNC();	
  m_ControlReady = JHIOIsControlReady();	


  m_ButtonSetHSCI.EnableWindow(m_bHSCIConfiguration);
  
  UpdateData(FALSE);
}


void CIOSimDemoDlg::OnSelectInstance() 
{
  UpdateData();
  int nResult = JHIOSelectInstance(m_strInstance);
  
  if (nResult != JHIO_SUCCESS)
    ShowError(nResult);

  // Status aktualisieren
  OnStatus();
}


void CIOSimDemoDlg::OnSignalsimulation() 
{
  int nResult;
  
  UpdateData();
  nResult = JHIOSetSimulationId(m_lSimId);

  if (nResult != JHIO_SUCCESS)
    ShowError(nResult);
}

void CIOSimDemoDlg::OnSetHsciValue() 
{
  int nResult;

  UpdateData();

  int iSignalId = static_cast<int>(m_ComboSignalId.GetItemData(m_ComboSignalId.GetCurSel()));
  long lAddressOffset = atol(m_strAddress);
  int Type = GetType(m_strType);
  if (Type != JHIO_TYPE_INPUT)
  {
    MessageBox("Please enter a logic input first"); 
    return;
  }
  
  BOOL fValue;
  int Value;
  if (sscanf(m_strValue, "%d", &Value) != 1 || (Value != 0 && Value != 1))
  {
    MessageBox("Invalid value"); 
    return;
  }
  else
    fValue = (Value != 0);

#ifdef LOCK_EXPLICITLY
  nResult = JHIOLockMemory(1000);
  if (nResult != JHIO_SUCCESS)
  {
    ShowError(nResult);
    return;
  }
#endif
  
#ifdef LOCK_EXPLICITLY
  nResult = JHIOSetHSCILogicValue(iSignalId, lAddressOffset, fValue);
#else
  nResult = JHIOSetHSCILogicValueWait(iSignalId, lAddressOffset, fValue, 1000);
#endif      


////////////////////////////////////////////////
#ifdef TESTHSCI
  
  CString Msg;


  Msg.Format("JHIOSetHSCILogicValue with Signal: %d, DataOffset: %d, Value: %d %s", 
             iSignalId, lAddressOffset, fValue, (nResult == JHIO_SUCCESS)?"succeeded":"failed");
  MessageBox(Msg);

  JHIO_HEADER Header;
  if (_JHIOInternGetHeader(&Header) == JHIO_SUCCESS)
  {
    Msg.Format("Header info: ES Offset: 0x%X, ES Len: %d", Header.lStartES, Header.lLenES);
    MessageBox(Msg);

    char Buf[2];
    if (_JHIOInternGetBlock(Buf, Header.lStartES, 2) == JHIO_SUCCESS)
    {
      int ESAVal = (unsigned)Buf[0];
      int ESBVal = (unsigned)Buf[1];
      Msg.Format("New value ES.A: 0x%X, ES.B: 0x%X", ESAVal, ESBVal);
      MessageBox(Msg);
    }
  }

#endif
////////////////////////////////////////////////



#ifdef LOCK_EXPLICITLY
  JHIOUnlockMemory();
#endif

  if (nResult != JHIO_SUCCESS)
    ShowError(nResult);
}

void CIOSimDemoDlg::RefreshClampLists(BOOL IsHSCIConfiguration)
{
  m_ComboHardware.ResetContent();
  
  if (!IsHSCIConfiguration)
  {
    m_ComboHardware.AddString("LE");
    m_ComboHardware.AddString("1. PL4xx");
    m_ComboHardware.AddString("2. PL4xx");
    m_ComboHardware.AddString("3. PL4xx");
    m_ComboHardware.AddString("4. PL4xx");
    
    m_ComboHardware.AddString("1. PL510");
    m_ComboHardware.AddString("2. PL510");
    m_ComboHardware.AddString("3. PL510");
    m_ComboHardware.AddString("4. PL510");
    
    m_ComboHardware.AddString("Handwheel 410");
    
    m_ComboHardware.AddString("CC");
    
    
    m_ComboHardware.SetItemData(0, JHIO_HW_LE);
    m_ComboHardware.SetItemData(1, JHIO_HW_PL410_1);
    m_ComboHardware.SetItemData(2, JHIO_HW_PL410_2);
    m_ComboHardware.SetItemData(3, JHIO_HW_PL410_3);
    m_ComboHardware.SetItemData(4, JHIO_HW_PL410_4);
    m_ComboHardware.SetItemData(5, JHIO_HW_PL510_1);
    m_ComboHardware.SetItemData(6, JHIO_HW_PL510_2);
    m_ComboHardware.SetItemData(7, JHIO_HW_PL510_3);
    m_ComboHardware.SetItemData(8, JHIO_HW_PL510_4);
    m_ComboHardware.SetItemData(9, JHIO_HW_HW410);
    
    m_ComboHardware.SetItemData(10, JHIO_HW_CC);
    
  }
  else
  {
    m_ComboHardware.AddString("HSCI MOP");  m_ComboHardware.SetItemData(0, JHIO_HW_HSCI_MOP);
    m_ComboHardware.AddString("PL620x");    m_ComboHardware.SetItemData(1, JHIO_HW_HSCI_PL620x);
  }
  
  m_ComboHardware.SetCurSel(0);
  
  OnSelchangeHw();  // m_ComboConnector aktualisieren
  
  m_ComboSignalId.ResetContent();
  m_ComboSignalId.AddString("ES.A");  m_ComboSignalId.SetItemData(0, JHIO_HSCI_ES_A);
  m_ComboSignalId.AddString("ES.B");  m_ComboSignalId.SetItemData(1, JHIO_HSCI_ES_B);
  m_ComboSignalId.SetCurSel(0);
}

void CIOSimDemoDlg::OnSplc() 
{
  BOOL fEnable = !m_CheckSPLC.GetCheck();
    
  GetDlgItem(IDC_GET_ADDRESS_CLAMP)->EnableWindow(fEnable);
}

void CIOSimDemoDlg::OnChangeName() 
{
  m_ButtonGetAddressByName.EnableWindow(true);

  // bei nicht-symbolischen Namen gleich versuchen die Adresse zu ermitteln
  CString strName;	
  GetDlgItem(IDC_NAME)->GetWindowText(strName);

  if (IsLogicalName(strName))
    GetAddressByName(true);
}

BOOL CIOSimDemoDlg::IsLogicalName(CString strName)
{
  CString strAddress;
  // logischer Name?
  if (strName.Left(1) == "I" || strName.Left(1) == "O")
  {
    if (strName.Mid(1,1) == "B" || strName.Mid(1,1) == "W" || strName.Mid(1,1) == "D")
      strAddress = strName.Mid(2);
    else
      strAddress = strName.Mid(1);
    
    if (!strAddress.IsEmpty() && strAddress.SpanIncluding("0123456789") == strAddress)
      return TRUE;
  }

  return FALSE;
}

BOOL CIOSimDemoDlg::IsBinaryType(int iType)
{
  return (iType == JHIO_TYPE_INPUT ||
          iType == JHIO_TYPE_OUTPUT);
}

void CIOSimDemoDlg::OnToggleValue() 
{
  OnGetValue();

  long lAddressOffset = atol(m_strAddress);
  
  int Type = GetType(m_strType);
  if (Type == -1)
    return;

  CString NewValue;
  if (m_strValue == "0")
    NewValue = "1";
  else if (m_strValue == "1")
    NewValue = "0";
  else
  {
    MessageBox("Invalid value"); 
    return;
  }

  SetValue(lAddressOffset, Type, NewValue);

  OnGetValue();
}



void CIOSimDemoDlg::OnOK()
{
  // CDialog::OnOK(); Damit Dialog nicht mit Enter geschlossen wird!
}



void CIOSimDemoDlg::PostNcDestroy()
{
  CDialog::PostNcDestroy();
}
