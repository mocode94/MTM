namespace DNC_CSharp_Demo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ConnectionTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ConfigureConnectionButton = new System.Windows.Forms.Button();
            this.ConnectionListComboBox = new System.Windows.Forms.ComboBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.AutoReconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.StatusTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ControlTextLabel = new System.Windows.Forms.Label();
            this.ApplicationTextLabel = new System.Windows.Forms.Label();
            this.ApplicationStatusLabel = new System.Windows.Forms.Label();
            this.ControlStatusLabel = new System.Windows.Forms.Label();
            this.StatusTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 864F));
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 3;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(850, 441);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // ConnectionTableLayoutPanel
            // 
            this.ConnectionTableLayoutPanel.ColumnCount = 5;
            this.ConnectionTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ConnectionTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ConnectionTableLayoutPanel.Name = "ConnectionTableLayoutPanel";
            this.ConnectionTableLayoutPanel.RowCount = 1;
            this.ConnectionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ConnectionTableLayoutPanel.Size = new System.Drawing.Size(864, 29);
            this.ConnectionTableLayoutPanel.TabIndex = 0;
            // 
            // ConfigureConnectionButton
            // 
            this.ConfigureConnectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigureConnectionButton.Location = new System.Drawing.Point(3, 3);
            this.ConfigureConnectionButton.Name = "ConfigureConnectionButton";
            this.ConfigureConnectionButton.Size = new System.Drawing.Size(150, 23);
            this.ConfigureConnectionButton.TabIndex = 0;
            this.ConfigureConnectionButton.Text = "Configure Connection";
            this.ConfigureConnectionButton.UseVisualStyleBackColor = true;
            this.ConfigureConnectionButton.Click += new System.EventHandler(this.ConfigureConnectionButton_Click);
            // 
            // ConnectionListComboBox
            // 
            this.ConnectionListComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionListComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionListComboBox.FormattingEnabled = true;
            this.ConnectionListComboBox.Location = new System.Drawing.Point(359, 3);
            this.ConnectionListComboBox.Name = "ConnectionListComboBox";
            this.ConnectionListComboBox.Size = new System.Drawing.Size(416, 24);
            this.ConnectionListComboBox.TabIndex = 1;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(159, 3);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(74, 23);
            this.ConnectButton.TabIndex = 1;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectButton.Location = new System.Drawing.Point(781, 3);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(80, 23);
            this.DisconnectButton.TabIndex = 2;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // AutoReconnectCheckBox
            // 
            this.AutoReconnectCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoReconnectCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AutoReconnectCheckBox.Location = new System.Drawing.Point(239, 5);
            this.AutoReconnectCheckBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.AutoReconnectCheckBox.Name = "AutoReconnectCheckBox";
            this.AutoReconnectCheckBox.Size = new System.Drawing.Size(114, 21);
            this.AutoReconnectCheckBox.TabIndex = 3;
            this.AutoReconnectCheckBox.Text = "auto reconnect";
            this.AutoReconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // StatusTableLayoutPanel
            // 
            this.StatusTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.StatusTableLayoutPanel.ColumnCount = 2;
            this.StatusTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.StatusTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StatusTableLayoutPanel.Controls.Add(this.ControlTextLabel, 0, 1);
            this.StatusTableLayoutPanel.Controls.Add(this.ApplicationTextLabel, 0, 0);
            this.StatusTableLayoutPanel.Controls.Add(this.ApplicationStatusLabel, 1, 0);
            this.StatusTableLayoutPanel.Controls.Add(this.ControlStatusLabel, 1, 1);
            this.StatusTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusTableLayoutPanel.Location = new System.Drawing.Point(0, 618);
            this.StatusTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.StatusTableLayoutPanel.Name = "StatusTableLayoutPanel";
            this.StatusTableLayoutPanel.RowCount = 2;
            this.StatusTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.StatusTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.StatusTableLayoutPanel.Size = new System.Drawing.Size(864, 44);
            this.StatusTableLayoutPanel.TabIndex = 2;
            // 
            // ControlTextLabel
            // 
            this.ControlTextLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ControlTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlTextLabel.Location = new System.Drawing.Point(1, 22);
            this.ControlTextLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlTextLabel.Name = "ControlTextLabel";
            this.ControlTextLabel.Size = new System.Drawing.Size(120, 21);
            this.ControlTextLabel.TabIndex = 3;
            this.ControlTextLabel.Text = "Control Status:";
            this.ControlTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ApplicationTextLabel
            // 
            this.ApplicationTextLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ApplicationTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ApplicationTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationTextLabel.Location = new System.Drawing.Point(1, 1);
            this.ApplicationTextLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ApplicationTextLabel.Name = "ApplicationTextLabel";
            this.ApplicationTextLabel.Size = new System.Drawing.Size(120, 20);
            this.ApplicationTextLabel.TabIndex = 2;
            this.ApplicationTextLabel.Text = "Application Status:";
            this.ApplicationTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ApplicationStatusLabel
            // 
            this.ApplicationStatusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.ApplicationStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ApplicationStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationStatusLabel.Location = new System.Drawing.Point(122, 1);
            this.ApplicationStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ApplicationStatusLabel.Name = "ApplicationStatusLabel";
            this.ApplicationStatusLabel.Size = new System.Drawing.Size(741, 20);
            this.ApplicationStatusLabel.TabIndex = 1;
            this.ApplicationStatusLabel.Text = "Application Status";
            this.ApplicationStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ControlStatusLabel
            // 
            this.ControlStatusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.ControlStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlStatusLabel.Location = new System.Drawing.Point(122, 22);
            this.ControlStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlStatusLabel.Name = "ControlStatusLabel";
            this.ControlStatusLabel.Size = new System.Drawing.Size(741, 21);
            this.ControlStatusLabel.TabIndex = 0;
            this.ControlStatusLabel.Text = "Control Status";
            this.ControlStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 441);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "DNC_CSharp_Template";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.StatusTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ConnectionTableLayoutPanel;
        private System.Windows.Forms.Button ConfigureConnectionButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.ComboBox ConnectionListComboBox;
        private System.Windows.Forms.TableLayoutPanel StatusTableLayoutPanel;
        private System.Windows.Forms.Label ApplicationStatusLabel;
        private System.Windows.Forms.Label ControlStatusLabel;
        private System.Windows.Forms.Label ControlTextLabel;
        private System.Windows.Forms.Label ApplicationTextLabel;
        private System.Windows.Forms.CheckBox AutoReconnectCheckBox;
    }
}