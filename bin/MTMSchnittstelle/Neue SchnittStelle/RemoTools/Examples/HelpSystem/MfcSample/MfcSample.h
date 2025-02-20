// MfcSample.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
    #error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols
#include "MfcSample_i.h"

// Application specific window messages
#define WM_CONNECTIONSTATE_CHANGED ( WM_APP + 10 )
#define WM_PROGSTAT_CHANGED ( WM_APP + 11 )


// CMfcSampleApp:
// See MfcSample.cpp for the implementation of this class
//

class CMfcSampleApp : public CWinApp
{
public:
    CMfcSampleApp();

// Overrides
    public:
    virtual BOOL InitInstance();

// Implementation

    DECLARE_MESSAGE_MAP()
    BOOL ExitInstance( void );
};

extern CMfcSampleApp theApp;