<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VbSampleDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim exitButton As System.Windows.Forms.Button
        Me.programStateTextBox = New System.Windows.Forms.TextBox
        Me.label3 = New System.Windows.Forms.Label
        Me.connectionStateTextBox = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.configureConnectionsButton = New System.Windows.Forms.Button
        Me.disconnectButton = New System.Windows.Forms.Button
        Me.connectButton = New System.Windows.Forms.Button
        Me.connectionList = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        exitButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'exitButton
        '
        exitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        exitButton.DialogResult = System.Windows.Forms.DialogResult.OK
        exitButton.Location = New System.Drawing.Point(321, 165)
        exitButton.Name = "exitButton"
        exitButton.Size = New System.Drawing.Size(75, 23)
        exitButton.TabIndex = 10
        exitButton.Text = "Exit"
        exitButton.UseVisualStyleBackColor = True
        AddHandler exitButton.Click, AddressOf Me.exitButton_Click
        '
        'programStateTextBox
        '
        Me.programStateTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.programStateTextBox.Location = New System.Drawing.Point(11, 168)
        Me.programStateTextBox.Name = "programStateTextBox"
        Me.programStateTextBox.ReadOnly = True
        Me.programStateTextBox.Size = New System.Drawing.Size(288, 20)
        Me.programStateTextBox.TabIndex = 19
        Me.programStateTextBox.TabStop = False
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(8, 152)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(75, 13)
        Me.label3.TabIndex = 18
        Me.label3.Text = "Program state:"
        '
        'connectionStateTextBox
        '
        Me.connectionStateTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.connectionStateTextBox.Location = New System.Drawing.Point(11, 119)
        Me.connectionStateTextBox.Name = "connectionStateTextBox"
        Me.connectionStateTextBox.ReadOnly = True
        Me.connectionStateTextBox.Size = New System.Drawing.Size(288, 20)
        Me.connectionStateTextBox.TabIndex = 17
        Me.connectionStateTextBox.TabStop = False
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(8, 103)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(90, 13)
        Me.label2.TabIndex = 16
        Me.label2.Text = "Connection state:"
        '
        'configureConnectionsButton
        '
        Me.configureConnectionsButton.Location = New System.Drawing.Point(11, 66)
        Me.configureConnectionsButton.MinimumSize = New System.Drawing.Size(100, 23)
        Me.configureConnectionsButton.Name = "configureConnectionsButton"
        Me.configureConnectionsButton.Size = New System.Drawing.Size(131, 23)
        Me.configureConnectionsButton.TabIndex = 14
        Me.configureConnectionsButton.Text = "Configure Connections"
        Me.configureConnectionsButton.UseVisualStyleBackColor = True
        '
        'disconnectButton
        '
        Me.disconnectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.disconnectButton.Location = New System.Drawing.Point(321, 66)
        Me.disconnectButton.Name = "disconnectButton"
        Me.disconnectButton.Size = New System.Drawing.Size(75, 23)
        Me.disconnectButton.TabIndex = 15
        Me.disconnectButton.Text = "Disconnect"
        Me.disconnectButton.UseVisualStyleBackColor = True
        '
        'connectButton
        '
        Me.connectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.connectButton.Location = New System.Drawing.Point(321, 24)
        Me.connectButton.Name = "connectButton"
        Me.connectButton.Size = New System.Drawing.Size(75, 23)
        Me.connectButton.TabIndex = 11
        Me.connectButton.Text = "Connect"
        Me.connectButton.UseVisualStyleBackColor = True
        '
        'connectionList
        '
        Me.connectionList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.connectionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.connectionList.FormattingEnabled = True
        Me.connectionList.Location = New System.Drawing.Point(11, 24)
        Me.connectionList.Name = "connectionList"
        Me.connectionList.Size = New System.Drawing.Size(288, 21)
        Me.connectionList.TabIndex = 13
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(8, 8)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(51, 13)
        Me.label1.TabIndex = 12
        Me.label1.Text = "Machine:"
        '
        'VbSampleDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(404, 196)
        Me.Controls.Add(exitButton)
        Me.Controls.Add(Me.programStateTextBox)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.connectionStateTextBox)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.configureConnectionsButton)
        Me.Controls.Add(Me.disconnectButton)
        Me.Controls.Add(Me.connectButton)
        Me.Controls.Add(Me.connectionList)
        Me.Controls.Add(Me.label1)
        Me.Name = "VbSampleDialog"
        Me.Text = "VB Example - Not connected"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents programStateTextBox As System.Windows.Forms.TextBox
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents connectionStateTextBox As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents configureConnectionsButton As System.Windows.Forms.Button
    Private WithEvents disconnectButton As System.Windows.Forms.Button
    Private WithEvents connectButton As System.Windows.Forms.Button
    Private WithEvents connectionList As System.Windows.Forms.ComboBox
    Private WithEvents label1 As System.Windows.Forms.Label

End Class
