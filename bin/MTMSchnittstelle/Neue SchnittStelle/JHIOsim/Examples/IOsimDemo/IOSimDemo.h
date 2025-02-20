// IOSimDemo.h : main header file for the IOSimDemo application
//

#if !defined(AFX_IOSimDemo_H__7BD3BF98_51C9_4436_A8C6_86AA48128BDD__INCLUDED_)
#define AFX_IOSimDemo_H__7BD3BF98_51C9_4436_A8C6_86AA48128BDD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CIOSimDemoApp:
// See IOSimDemo.cpp for the implementation of this class
//

class CIOSimDemoApp : public CWinApp
{
public:
	CIOSimDemoApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CIOSimDemoApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CIOSimDemoApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_IOSimDemo_H__7BD3BF98_51C9_4436_A8C6_86AA48128BDD__INCLUDED_)
