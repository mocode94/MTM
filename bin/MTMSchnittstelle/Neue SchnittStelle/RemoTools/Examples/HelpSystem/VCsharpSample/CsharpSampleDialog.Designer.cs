namespace CsharpSample
{
    partial class CsharpSampleDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button exitButton;
            this.label1 = new System.Windows.Forms.Label();
            this.connectionList = new System.Windows.Forms.ComboBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.configureConnectionsButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.serverStateTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.programStateTextBox = new System.Windows.Forms.TextBox();
            this.immediateConnectButton = new System.Windows.Forms.Button();
            this.errorsTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // exitButton
            // 
            exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            exitButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            exitButton.Location = new System.Drawing.Point(317, 94);
            exitButton.Name = "exitButton";
            exitButton.Size = new System.Drawing.Size(75, 23);
            exitButton.TabIndex = 0;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Machine:";
            // 
            // connectionList
            // 
            this.connectionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionList.FormattingEnabled = true;
            this.connectionList.Location = new System.Drawing.Point(7, 23);
            this.connectionList.Name = "connectionList";
            this.connectionList.Size = new System.Drawing.Size(288, 21);
            this.connectionList.TabIndex = 3;
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Location = new System.Drawing.Point(317, 7);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "ConnectReq";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.disconnectButton.Location = new System.Drawing.Point(317, 65);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 5;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // configureConnectionsButton
            // 
            this.configureConnectionsButton.Location = new System.Drawing.Point(7, 65);
            this.configureConnectionsButton.MinimumSize = new System.Drawing.Size(100, 23);
            this.configureConnectionsButton.Name = "configureConnectionsButton";
            this.configureConnectionsButton.Size = new System.Drawing.Size(131, 23);
            this.configureConnectionsButton.TabIndex = 4;
            this.configureConnectionsButton.Text = "Configure Connections";
            this.configureConnectionsButton.UseVisualStyleBackColor = true;
            this.configureConnectionsButton.Click += new System.EventHandler(this.configureConnectionsbutton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connection state:";
            // 
            // serverStateTextBox
            // 
            this.serverStateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverStateTextBox.Location = new System.Drawing.Point(7, 118);
            this.serverStateTextBox.Name = "serverStateTextBox";
            this.serverStateTextBox.ReadOnly = true;
            this.serverStateTextBox.Size = new System.Drawing.Size(288, 20);
            this.serverStateTextBox.TabIndex = 7;
            this.serverStateTextBox.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Program state:";
            // 
            // programStateTextBox
            // 
            this.programStateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programStateTextBox.Location = new System.Drawing.Point(7, 167);
            this.programStateTextBox.Name = "programStateTextBox";
            this.programStateTextBox.ReadOnly = true;
            this.programStateTextBox.Size = new System.Drawing.Size(288, 20);
            this.programStateTextBox.TabIndex = 9;
            this.programStateTextBox.TabStop = false;
            // 
            // immediateConnectButton
            // 
            this.immediateConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.immediateConnectButton.Location = new System.Drawing.Point(317, 36);
            this.immediateConnectButton.Name = "immediateConnectButton";
            this.immediateConnectButton.Size = new System.Drawing.Size(75, 23);
            this.immediateConnectButton.TabIndex = 10;
            this.immediateConnectButton.Text = "Connect";
            this.immediateConnectButton.UseVisualStyleBackColor = true;
            this.immediateConnectButton.Click += new System.EventHandler(this.immediateConnectButton_Click);
            // 
            // errorsTextBox
            // 
            this.errorsTextBox.AcceptsReturn = true;
            this.errorsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorsTextBox.Location = new System.Drawing.Point(7, 214);
            this.errorsTextBox.Multiline = true;
            this.errorsTextBox.Name = "errorsTextBox";
            this.errorsTextBox.ReadOnly = true;
            this.errorsTextBox.Size = new System.Drawing.Size(288, 160);
            this.errorsTextBox.TabIndex = 11;
            this.errorsTextBox.TabStop = false;
            this.errorsTextBox.WordWrap = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Errors:";
            // 
            // CsharpSampleDialog
            // 
            this.AcceptButton = exitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 386);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.errorsTextBox);
            this.Controls.Add(this.immediateConnectButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(this.programStateTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.serverStateTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.configureConnectionsButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.connectionList);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "CsharpSampleDialog";
            this.Text = "VC# Example - Not connected";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CsharpSampleDialog_FormClosing);
            this.Load += new System.EventHandler(this.CsharpSampleDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox connectionList;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button configureConnectionsButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverStateTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox programStateTextBox;
        private System.Windows.Forms.Button immediateConnectButton;
        private System.Windows.Forms.TextBox errorsTextBox;
        private System.Windows.Forms.Label label4;
    }
}

