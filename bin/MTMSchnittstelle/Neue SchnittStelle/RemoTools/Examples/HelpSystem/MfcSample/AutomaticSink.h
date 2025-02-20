// AutomaticSink.h : Declaration of the CAutomaticSink

#pragma once
#include "resource.h"       // main symbols

#include "MfcSample.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CAutomaticSink

class ATL_NO_VTABLE CAutomaticSink :
    public CComObjectRootEx< CComSingleThreadModel >,
    public CComCoClass< CAutomaticSink, &CLSID_AutomaticSink >,
    public IAutomaticSink,
    public HeidenhainDNCLib::_IJHAutomaticEvents2//,
//	public HeidenhainDNCLib::_IJHAutomaticEvents
{
public:
    CAutomaticSink()
        : m_pWindow( 0 )
    {
    }

    DECLARE_REGISTRY_RESOURCEID(IDR_AUTOMATICSINK)

    DECLARE_NOT_AGGREGATABLE( CAutomaticSink )

    BEGIN_COM_MAP( CAutomaticSink )
        COM_INTERFACE_ENTRY( IAutomaticSink )
        COM_INTERFACE_ENTRY( HeidenhainDNCLib::_IJHAutomaticEvents2 )
        COM_INTERFACE_ENTRY( HeidenhainDNCLib::_IJHAutomaticEvents )
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
    // The main dialog
    CWnd* m_pWindow;

public:
    // Set a handle to the window to send the message to
    void SetWindow( CWnd* pWnd )
    {
        m_pWindow = pWnd;
    }


    // _IJHAutomaticEvents2 Methods
public:
    STDMETHOD(OnExecutionMessage)(long lChannel, VARIANT varNumericValue, BSTR bstrValue)
    {
        return E_NOTIMPL;
    }
    STDMETHOD(OnProgramChanged)(long lChannel, DATE dTimeStamp, BSTR bstrNewProgram)
    {
        return E_NOTIMPL;
    }
    STDMETHOD(OnToolChanged)(long lChannel, HeidenhainDNCLib::IJHToolId * pidToolOut, HeidenhainDNCLib::IJHToolId * pidToolIn, DATE dTimeStamp)
    {
        return E_NOTIMPL;
    }

    // _IJHAutomaticEvents Methods
public:
    STDMETHOD(OnProgramStatusChanged)(HeidenhainDNCLib::DNC_EVT_PROGRAM programEvent)
    {
        if( m_pWindow != 0 )
        {
            (void) m_pWindow->PostMessage( WM_PROGSTAT_CHANGED, 0, (LPARAM) programEvent ) ;
            return S_OK;
        }
        return E_NOTIMPL;
    }
    STDMETHOD(OnDncModeChanged)(HeidenhainDNCLib::DNC_MODE newDncMode)
    {
        return E_NOTIMPL;
    }
};

OBJECT_ENTRY_AUTO(__uuidof(AutomaticSink), CAutomaticSink)
