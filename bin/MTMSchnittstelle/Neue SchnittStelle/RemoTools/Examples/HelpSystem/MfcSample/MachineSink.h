// MachineSink.h : Declaration of the CMachineSink

#pragma once
#include "resource.h"       // main symbols

#include "MfcSample.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CMachineSink

class ATL_NO_VTABLE CMachineSink :
    public CComObjectRootEx< CComSingleThreadModel >,
    public CComCoClass< CMachineSink, &CLSID_MachineSink >,
    public IMachineSink,
    public HeidenhainDNCLib::_IJHMachineEvents2
{
public:
    CMachineSink()
        : m_pWindow( 0 )
    {
    }

    DECLARE_REGISTRY_RESOURCEID( IDR_MACHINESINK )

    DECLARE_NOT_AGGREGATABLE( CMachineSink )

    BEGIN_COM_MAP( CMachineSink )
        COM_INTERFACE_ENTRY( IMachineSink)
        COM_INTERFACE_ENTRY( HeidenhainDNCLib::_IJHMachineEvents2 )
    END_COM_MAP()



    DECLARE_PROTECT_FINAL_CONSTRUCT()

    HRESULT FinalConstruct()
    {
        return S_OK;
    }

    void FinalRelease()
    {
    }

private:
    // The window that receives the message
    CWnd* m_pWindow;

public:
    // Set a handle to the window to send the message to
    void SetWindow( CWnd* pWnd )
    {
        m_pWindow = pWnd;
    }

    // _IJHMachineEvents2 Methods
public:
    STDMETHOD(OnStateChanged)(HeidenhainDNCLib::DNC_EVT_STATE eventValue)
    {
        if( m_pWindow != 0 )
        {
            (void) m_pWindow->PostMessage( WM_CONNECTIONSTATE_CHANGED, 0, (LPARAM) eventValue ) ;
            return S_OK;
        }
        return E_NOTIMPL;
    }
};

OBJECT_ENTRY_AUTO( __uuidof(MachineSink), CMachineSink )
