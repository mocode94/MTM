import sys
import traceback


# COM support
import comtypes
import comtypes.client

# load TLB
# by UUID
#    tlb_id = comtypes.GUID("{14B95319-AEF9-492A-A878-CA18FEB1F5BF}")
#    comtypes.client.GetModule((tlb_id,1,7))
# or
# by DLL
comtypes.client.GetModule("C:\Program Files (x86)\Common Files\Heidenhain Shared\HeidenhainDNC.dll")

import comtypes.gen.HeidenhainDNCLib as HDNC

try:
    # Create COM object 'JHMachineInProcess'
    jhmachine = comtypes.client.CreateObject("HeidenhainDnc.JHMachineInProcess")

    # get COM interface 'IJHMachine4'
    ijhmachine = jhmachine.QueryInterface(HDNC.IJHMachine4)
    #help(ijhmachine)

    # call method on interface
    comversion = ijhmachine.GetVersionComInterface()
    print(comversion)

    # launch HeidenhainDNC connection dialog (returns selected connection)
    connection = ijhmachine.ConfigureConnection( HDNC.DNC_CONFIGURE_MODE_ALL)
    print(connection)

    if connection:
        # connect to selected machine
        ijhmachine.Connect( connection )

        # get actual state of connection
        state = ijhmachine.GetState()
        print(state)

        jhversion = ijhmachine.GetInterface( HDNC.DNC_INTERFACE_JHVERSION )
        ijhversion = jhversion.QueryInterface(HDNC.IJHVersion)
        swVersions = ijhversion.GetVersionNcSoftware()
        for jj, swVersion in enumerate(swVersions):
            print( jj, swVersion.bstrSoftwareType, swVersion.bstrIdentNr, swVersion.bstrDescription  )

        swVersions.Release()
        ijhversion.Release()
        jhversion = None

        ijhmachine.Disconnect()
        ijhmachine.Release()
        jhmachine = None
    else:
        print('no connection selected')

    print('success')

except:
    # the values returned are (type, value, traceback). 
    # - type:       gets the exception type of the exception being handled (a class object); 
    # - value:      gets the exception parameter (its associated value or the second argument to raise, which is always a class instance if the exception type is a class object); 
    # - traceback:  gets a traceback object (see the Reference Manual) which encapsulates the call stack at the point where the exception originally occurred.
    print("\n*****  aborted due to exception: " , sys.exc_info()[0], sys.exc_info()[1])
    traceback.print_tb(sys.exc_info()[2])

