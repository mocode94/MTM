' VBScript Example - Display the active CNC errors and respond to the error events
Option Explicit

Const DNC_INTERFACE_JHERROR = 2
Const DNC_CONFIGURE_MODE_ALL = 3
Const DNC_EG_NONE = 0
Const S_OK = 0

On Error Resume Next

' Create a new JHMachine object using the WScript object
Dim objJHMachine
Set objJHMachine = WScript.CreateObject("HeidenhainDNC.JHMachine")
' Use the Connections dialog to let the user select a CNC to connect to
Dim strConnectionName
objJHMachine.ConfigureConnection DNC_CONFIGURE_MODE_ALL, strConnectionName

If strConnectionName <> "" Then
    ' The user has selected a CNC, so make the connection
    objJHMachine.Connect strConnectionName
    If Err.number = S_OK Then
        ' Create the JHError object
        Dim objJHError
        Set objJHError = objJHMachine.GetInterface(DNC_INTERFACE_JHERROR)
        ' Connect the JHError event source using the WScript object
        ' The event handler methods must have the prefix "objJHError_"
        WScript.ConnectObject objJHError, "objJHError_"
        
        ' Display the active CNC errors
        Do
            Dim errorGroup, lErrorNumber, errorClass, bstrError, lChannel
            Dim strErrorMsg
            errorGroup = DNC_EG_NONE
            objJHError.GetFirstError errorGroup, lErrorNumber, errorClass, bstrError, lChannel
            If errorGroup <> DNC_EG_NONE Then
                strErrorMsg = "CNC Errors:" & vbNewLine _
                            & Hex(lErrorNumber) & ", " & bstrError & vbNewLine
                objJHError.GetNextError errorGroup, lErrorNumber, errorClass, bstrError, lChannel
                Do While errorGroup <> DNC_EG_NONE
                    strErrorMsg = strErrorMsg & Hex(lErrorNumber) & ", " & bstrError & vbNewLine
                    objJHError.GetNextError errorGroup, lErrorNumber, errorClass, bstrError, lChannel
                Loop
            Else
                strErrorMsg = "No CNC Errors" & vbCrLf
            End If
            ' Display the active CNC errors. The script ends when the user presses the Cancel button
        Loop While MSgBox(strErrorMsg & vbNewLine _
                          & "Press OK to refresh the CNC error list, Cancel to terminate the Application" _
                          , vbOKCancel + vbInformation, "VBScript Example") = vbOK

        ' Disconnect from the CNC
        objJHMachine.Disconnect
        ' Disconnect the JHError event source and release the JHError object
        WScript.DisconnectObject objJHError
        Set objJHError = Nothing
    Else
        MsgBox "Could not connect to " & strConnectionName, vbOKOnly + vbExclamation, "VBScript Example"
    End If
End If

Set objJHMachine = Nothing

' JHError OnError event handler
Sub objJHError_OnError(errorGroup, lErrorNumber, errorClass, bstrError, lChannel)
    MsgBox "New CNC Error:" & vbNewLine _
           & Hex(lErrorNumber) & ", " & bstrError
End Sub

' JHError OnErrorCleared event handler
Sub objJHError_OnErrorCleared(lErrorNumber, lChannel)
    MsgBox "CNC Error " & Hex(lErrorNumber) & " cleared"
End Sub
