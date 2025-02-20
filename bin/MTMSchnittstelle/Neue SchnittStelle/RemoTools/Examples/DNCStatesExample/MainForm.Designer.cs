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
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.AutoReconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.StatusTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ControlTextLabel = new System.Windows.Forms.Label();
            this.ApplicationTextLabel = new System.Windows.Forms.Label();
            this.ApplicationStatusLabel = new System.Windows.Forms.Label();
            this.ControlStatusLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel.SuspendLayout();
            this.ConnectionTableLayoutPanel.SuspendLayout();
            this.StatusTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.AutoSize = true;
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.ConnectionTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.StatusTableLayoutPanel, 0, 2);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 3;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(584, 926);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // ConnectionTableLayoutPanel
            // 
            this.ConnectionTableLayoutPanel.ColumnCount = 5;
            this.ConnectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.ConnectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ConnectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.ConnectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ConnectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ConnectionTableLayoutPanel.Controls.Add(this.ConfigureConnectionButton, 0, 0);
            this.ConnectionTableLayoutPanel.Controls.Add(this.ConnectionListComboBox, 3, 0);
            this.ConnectionTableLayoutPanel.Controls.Add(this.DisconnectButton, 4, 0);
            this.ConnectionTableLayoutPanel.Controls.Add(this.ConnectButton, 1, 0);
            this.ConnectionTableLayoutPanel.Controls.Add(this.AutoReconnectCheckBox, 2, 0);
            this.ConnectionTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ConnectionTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ConnectionTableLayoutPanel.Name = "ConnectionTableLayoutPanel";
            this.ConnectionTableLayoutPanel.RowCount = 1;
            this.ConnectionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ConnectionTableLayoutPanel.Size = new System.Drawing.Size(584, 36);
            this.ConnectionTableLayoutPanel.TabIndex = 0;
            // 
            // ConfigureConnectionButton
            // 
            this.ConfigureConnectionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfigureConnectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigureConnectionButton.Location = new System.Drawing.Point(4, 5);
            this.ConfigureConnectionButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConfigureConnectionButton.Name = "ConfigureConnectionButton";
            this.ConfigureConnectionButton.Size = new System.Drawing.Size(152, 26);
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
            this.ConnectionListComboBox.Location = new System.Drawing.Point(384, 5);
            this.ConnectionListComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConnectionListComboBox.Name = "ConnectionListComboBox";
            this.ConnectionListComboBox.Size = new System.Drawing.Size(96, 24);
            this.ConnectionListComboBox.TabIndex = 1;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisconnectButton.Enabled = false;
            this.DisconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectButton.Location = new System.Drawing.Point(488, 5);
            this.DisconnectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(92, 26);
            this.DisconnectButton.TabIndex = 2;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(164, 5);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(92, 26);
            this.ConnectButton.TabIndex = 1;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // AutoReconnectCheckBox
            // 
            this.AutoReconnectCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoReconnectCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AutoReconnectCheckBox.Location = new System.Drawing.Point(264, 8);
            this.AutoReconnectCheckBox.Margin = new System.Windows.Forms.Padding(4, 8, 4, 5);
            this.AutoReconnectCheckBox.Name = "AutoReconnectCheckBox";
            this.AutoReconnectCheckBox.Size = new System.Drawing.Size(112, 23);
            this.AutoReconnectCheckBox.TabIndex = 3;
            this.AutoReconnectCheckBox.Text = "auto reconnect";
            this.AutoReconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // StatusTableLayoutPanel
            // 
            this.StatusTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.StatusTableLayoutPanel.ColumnCount = 2;
            this.StatusTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.StatusTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StatusTableLayoutPanel.Controls.Add(this.ControlTextLabel, 0, 1);
            this.StatusTableLayoutPanel.Controls.Add(this.ApplicationTextLabel, 0, 0);
            this.StatusTableLayoutPanel.Controls.Add(this.ApplicationStatusLabel, 1, 0);
            this.StatusTableLayoutPanel.Controls.Add(this.ControlStatusLabel, 1, 1);
            this.StatusTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusTableLayoutPanel.Location = new System.Drawing.Point(0, 886);
            this.StatusTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.StatusTableLayoutPanel.Name = "StatusTableLayoutPanel";
            this.StatusTableLayoutPanel.RowCount = 2;
            this.StatusTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.StatusTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.StatusTableLayoutPanel.Size = new System.Drawing.Size(584, 40);
            this.StatusTableLayoutPanel.TabIndex = 2;
            // 
            // ControlTextLabel
            // 
            this.ControlTextLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ControlTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlTextLabel.Location = new System.Drawing.Point(1, 20);
            this.ControlTextLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlTextLabel.Name = "ControlTextLabel";
            this.ControlTextLabel.Size = new System.Drawing.Size(180, 19);
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
            this.ApplicationTextLabel.Size = new System.Drawing.Size(180, 18);
            this.ApplicationTextLabel.TabIndex = 2;
            this.ApplicationTextLabel.Text = "Application Status:";
            this.ApplicationTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ApplicationStatusLabel
            // 
            this.ApplicationStatusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.ApplicationStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ApplicationStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationStatusLabel.Location = new System.Drawing.Point(182, 1);
            this.ApplicationStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ApplicationStatusLabel.Name = "ApplicationStatusLabel";
            this.ApplicationStatusLabel.Size = new System.Drawing.Size(401, 18);
            this.ApplicationStatusLabel.TabIndex = 1;
            this.ApplicationStatusLabel.Text = "Application Status";
            this.ApplicationStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ControlStatusLabel
            // 
            this.ControlStatusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.ControlStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlStatusLabel.Location = new System.Drawing.Point(182, 20);
            this.ControlStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlStatusLabel.Name = "ControlStatusLabel";
            this.ControlStatusLabel.Size = new System.Drawing.Size(401, 19);
            this.ControlStatusLabel.TabIndex = 0;
            this.ControlStatusLabel.Text = "Control Status";
            this.ControlStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 926);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DNC_CSharp_OnStateChanged";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ConnectionTableLayoutPanel.ResumeLayout(false);
            this.StatusTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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

