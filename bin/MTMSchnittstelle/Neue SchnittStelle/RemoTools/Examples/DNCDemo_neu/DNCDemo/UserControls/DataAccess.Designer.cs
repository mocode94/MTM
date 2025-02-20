namespace DNC_CSharp_Demo.UserControls
{
    partial class DataAccess
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DataSelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.DataSelectionTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DataSelectionBackButton = new System.Windows.Forms.Button();
            this.GetDataEntryButton = new System.Windows.Forms.Button();
            this.SubscribeButton = new System.Windows.Forms.Button();
            this.DataSelectionTextBox = new System.Windows.Forms.TextBox();
            this.LoggingGroupBox = new System.Windows.Forms.GroupBox();
            this.FileLoggingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LoggingCheckBox = new System.Windows.Forms.CheckBox();
            this.SelectLogfileButton = new System.Windows.Forms.Button();
            this.LogFilePathTextBox = new System.Windows.Forms.TextBox();
            this.EditLogFileButton = new System.Windows.Forms.Button();
            this.DataAccessTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.InterfaceTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AccessModeGroupBox = new System.Windows.Forms.GroupBox();
            this.AccessModeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SetAccessModeButton = new System.Windows.Forms.Button();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.AccessModeComboBox = new System.Windows.Forms.ComboBox();
            this.InterfaceGroupBox = new System.Windows.Forms.GroupBox();
            this.InterfaceComboBox = new System.Windows.Forms.ComboBox();
            this.MainTableLayoutPanel.SuspendLayout();
            this.DataSelectionGroupBox.SuspendLayout();
            this.DataSelectionTableLayoutPanel.SuspendLayout();
            this.LoggingGroupBox.SuspendLayout();
            this.FileLoggingTableLayoutPanel.SuspendLayout();
            this.InterfaceTableLayoutPanel.SuspendLayout();
            this.AccessModeGroupBox.SuspendLayout();
            this.AccessModeTableLayoutPanel.SuspendLayout();
            this.InterfaceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.DataSelectionGroupBox, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.LoggingGroupBox, 0, 3);
            this.MainTableLayoutPanel.Controls.Add(this.DataAccessTableLayoutPanel, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.InterfaceTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 4;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(800, 600);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // DataSelectionGroupBox
            // 
            this.DataSelectionGroupBox.Controls.Add(this.DataSelectionTableLayoutPanel);
            this.DataSelectionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSelectionGroupBox.Location = new System.Drawing.Point(3, 59);
            this.DataSelectionGroupBox.Name = "DataSelectionGroupBox";
            this.DataSelectionGroupBox.Size = new System.Drawing.Size(794, 50);
            this.DataSelectionGroupBox.TabIndex = 1;
            this.DataSelectionGroupBox.TabStop = false;
            this.DataSelectionGroupBox.Text = "DataSelection";
            // 
            // DataSelectionTableLayoutPanel
            // 
            this.DataSelectionTableLayoutPanel.ColumnCount = 4;
            this.DataSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DataSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.DataSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.DataSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.DataSelectionTableLayoutPanel.Controls.Add(this.DataSelectionBackButton, 0, 0);
            this.DataSelectionTableLayoutPanel.Controls.Add(this.GetDataEntryButton, 2, 0);
            this.DataSelectionTableLayoutPanel.Controls.Add(this.SubscribeButton, 3, 0);
            this.DataSelectionTableLayoutPanel.Controls.Add(this.DataSelectionTextBox, 0, 0);
            this.DataSelectionTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSelectionTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.DataSelectionTableLayoutPanel.Name = "DataSelectionTableLayoutPanel";
            this.DataSelectionTableLayoutPanel.RowCount = 1;
            this.DataSelectionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DataSelectionTableLayoutPanel.Size = new System.Drawing.Size(788, 31);
            this.DataSelectionTableLayoutPanel.TabIndex = 0;
            // 
            // DataSelectionBackButton
            // 
            this.DataSelectionBackButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSelectionBackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataSelectionBackButton.Location = new System.Drawing.Point(501, 3);
            this.DataSelectionBackButton.Name = "DataSelectionBackButton";
            this.DataSelectionBackButton.Size = new System.Drawing.Size(44, 25);
            this.DataSelectionBackButton.TabIndex = 5;
            this.DataSelectionBackButton.Text = "..";
            this.DataSelectionBackButton.UseVisualStyleBackColor = true;
            this.DataSelectionBackButton.Click += new System.EventHandler(this.DataSelectionBackButton_Click);
            // 
            // GetDataEntryButton
            // 
            this.GetDataEntryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetDataEntryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetDataEntryButton.Location = new System.Drawing.Point(551, 3);
            this.GetDataEntryButton.Name = "GetDataEntryButton";
            this.GetDataEntryButton.Size = new System.Drawing.Size(114, 25);
            this.GetDataEntryButton.TabIndex = 3;
            this.GetDataEntryButton.Text = "GetDataEntry";
            this.GetDataEntryButton.UseVisualStyleBackColor = true;
            this.GetDataEntryButton.Click += new System.EventHandler(this.GetDataEntryButton_Click);
            // 
            // SubscribeButton
            // 
            this.SubscribeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubscribeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubscribeButton.Location = new System.Drawing.Point(671, 3);
            this.SubscribeButton.Name = "SubscribeButton";
            this.SubscribeButton.Size = new System.Drawing.Size(114, 25);
            this.SubscribeButton.TabIndex = 4;
            this.SubscribeButton.Text = "Subscribe";
            this.SubscribeButton.UseVisualStyleBackColor = true;
            this.SubscribeButton.Click += new System.EventHandler(this.SubscribeButton_Click);
            // 
            // DataSelectionTextBox
            // 
            this.DataSelectionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSelectionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataSelectionTextBox.Location = new System.Drawing.Point(3, 4);
            this.DataSelectionTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.DataSelectionTextBox.Name = "DataSelectionTextBox";
            this.DataSelectionTextBox.Size = new System.Drawing.Size(492, 22);
            this.DataSelectionTextBox.TabIndex = 2;
            this.DataSelectionTextBox.Text = "\\";
            this.DataSelectionTextBox.TextChanged += new System.EventHandler(this.DataSelectionTextBox_TextChanged);
            // 
            // LoggingGroupBox
            // 
            this.LoggingGroupBox.Controls.Add(this.FileLoggingTableLayoutPanel);
            this.LoggingGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoggingGroupBox.Location = new System.Drawing.Point(3, 553);
            this.LoggingGroupBox.Name = "LoggingGroupBox";
            this.LoggingGroupBox.Size = new System.Drawing.Size(794, 44);
            this.LoggingGroupBox.TabIndex = 3;
            this.LoggingGroupBox.TabStop = false;
            this.LoggingGroupBox.Text = "Logging of subscribed data (Choose path of IJHDataAccessX.log file)";
            // 
            // FileLoggingTableLayoutPanel
            // 
            this.FileLoggingTableLayoutPanel.ColumnCount = 4;
            this.FileLoggingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.FileLoggingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.FileLoggingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FileLoggingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.FileLoggingTableLayoutPanel.Controls.Add(this.LoggingCheckBox, 0, 0);
            this.FileLoggingTableLayoutPanel.Controls.Add(this.SelectLogfileButton, 1, 0);
            this.FileLoggingTableLayoutPanel.Controls.Add(this.LogFilePathTextBox, 2, 0);
            this.FileLoggingTableLayoutPanel.Controls.Add(this.EditLogFileButton, 3, 0);
            this.FileLoggingTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileLoggingTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.FileLoggingTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FileLoggingTableLayoutPanel.Name = "FileLoggingTableLayoutPanel";
            this.FileLoggingTableLayoutPanel.RowCount = 1;
            this.FileLoggingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FileLoggingTableLayoutPanel.Size = new System.Drawing.Size(788, 25);
            this.FileLoggingTableLayoutPanel.TabIndex = 2;
            // 
            // LoggingCheckBox
            // 
            this.LoggingCheckBox.AutoSize = true;
            this.LoggingCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoggingCheckBox.Enabled = false;
            this.LoggingCheckBox.Location = new System.Drawing.Point(3, 3);
            this.LoggingCheckBox.Name = "LoggingCheckBox";
            this.LoggingCheckBox.Size = new System.Drawing.Size(94, 19);
            this.LoggingCheckBox.TabIndex = 0;
            this.LoggingCheckBox.Text = "Logging";
            this.LoggingCheckBox.UseVisualStyleBackColor = true;
            this.LoggingCheckBox.CheckedChanged += new System.EventHandler(this.LoggingCheckBox_CheckedChanged);
            // 
            // SelectLogfileButton
            // 
            this.SelectLogfileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectLogfileButton.Location = new System.Drawing.Point(103, 3);
            this.SelectLogfileButton.Name = "SelectLogfileButton";
            this.SelectLogfileButton.Size = new System.Drawing.Size(24, 19);
            this.SelectLogfileButton.TabIndex = 1;
            this.SelectLogfileButton.Text = "...";
            this.SelectLogfileButton.UseVisualStyleBackColor = true;
            this.SelectLogfileButton.Click += new System.EventHandler(this.SelectLogfileButton_Click);
            // 
            // LogFilePathTextBox
            // 
            this.LogFilePathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogFilePathTextBox.Enabled = false;
            this.LogFilePathTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogFilePathTextBox.Location = new System.Drawing.Point(133, 3);
            this.LogFilePathTextBox.Name = "LogFilePathTextBox";
            this.LogFilePathTextBox.Size = new System.Drawing.Size(621, 21);
            this.LogFilePathTextBox.TabIndex = 2;
            // 
            // EditLogFileButton
            // 
            this.EditLogFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditLogFileButton.Image = global::DNC_CSharp_Demo.Properties.Resources.sk_bearbeiten;
            this.EditLogFileButton.Location = new System.Drawing.Point(760, 3);
            this.EditLogFileButton.Name = "EditLogFileButton";
            this.EditLogFileButton.Size = new System.Drawing.Size(25, 19);
            this.EditLogFileButton.TabIndex = 3;
            this.EditLogFileButton.UseVisualStyleBackColor = true;
            this.EditLogFileButton.Click += new System.EventHandler(this.EditLogFileButton_Click);
            // 
            // DataAccessTableLayoutPanel
            // 
            this.DataAccessTableLayoutPanel.ColumnCount = 1;
            this.DataAccessTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DataAccessTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataAccessTableLayoutPanel.Location = new System.Drawing.Point(0, 112);
            this.DataAccessTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DataAccessTableLayoutPanel.Name = "DataAccessTableLayoutPanel";
            this.DataAccessTableLayoutPanel.RowCount = 1;
            this.DataAccessTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DataAccessTableLayoutPanel.Size = new System.Drawing.Size(800, 438);
            this.DataAccessTableLayoutPanel.TabIndex = 4;
            // 
            // InterfaceTableLayoutPanel
            // 
            this.InterfaceTableLayoutPanel.ColumnCount = 2;
            this.InterfaceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.InterfaceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.InterfaceTableLayoutPanel.Controls.Add(this.AccessModeGroupBox, 1, 0);
            this.InterfaceTableLayoutPanel.Controls.Add(this.InterfaceGroupBox, 0, 0);
            this.InterfaceTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterfaceTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.InterfaceTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.InterfaceTableLayoutPanel.Name = "InterfaceTableLayoutPanel";
            this.InterfaceTableLayoutPanel.RowCount = 1;
            this.InterfaceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.InterfaceTableLayoutPanel.Size = new System.Drawing.Size(800, 56);
            this.InterfaceTableLayoutPanel.TabIndex = 5;
            // 
            // AccessModeGroupBox
            // 
            this.AccessModeGroupBox.Controls.Add(this.AccessModeTableLayoutPanel);
            this.AccessModeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccessModeGroupBox.Location = new System.Drawing.Point(153, 3);
            this.AccessModeGroupBox.Name = "AccessModeGroupBox";
            this.AccessModeGroupBox.Size = new System.Drawing.Size(644, 50);
            this.AccessModeGroupBox.TabIndex = 0;
            this.AccessModeGroupBox.TabStop = false;
            this.AccessModeGroupBox.Text = "AccessMode";
            // 
            // AccessModeTableLayoutPanel
            // 
            this.AccessModeTableLayoutPanel.ColumnCount = 3;
            this.AccessModeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AccessModeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.AccessModeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.AccessModeTableLayoutPanel.Controls.Add(this.SetAccessModeButton, 2, 0);
            this.AccessModeTableLayoutPanel.Controls.Add(this.PasswordTextBox, 1, 0);
            this.AccessModeTableLayoutPanel.Controls.Add(this.AccessModeComboBox, 0, 0);
            this.AccessModeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccessModeTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.AccessModeTableLayoutPanel.Name = "AccessModeTableLayoutPanel";
            this.AccessModeTableLayoutPanel.RowCount = 1;
            this.AccessModeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AccessModeTableLayoutPanel.Size = new System.Drawing.Size(638, 31);
            this.AccessModeTableLayoutPanel.TabIndex = 0;
            // 
            // SetAccessModeButton
            // 
            this.SetAccessModeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetAccessModeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetAccessModeButton.Location = new System.Drawing.Point(591, 3);
            this.SetAccessModeButton.Name = "SetAccessModeButton";
            this.SetAccessModeButton.Size = new System.Drawing.Size(44, 25);
            this.SetAccessModeButton.TabIndex = 0;
            this.SetAccessModeButton.Text = "Set";
            this.SetAccessModeButton.UseVisualStyleBackColor = true;
            this.SetAccessModeButton.Click += new System.EventHandler(this.SetAccessModeButton_Click);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(511, 4);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(74, 22);
            this.PasswordTextBox.TabIndex = 1;
            this.PasswordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AccessModeComboBox
            // 
            this.AccessModeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccessModeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccessModeComboBox.FormattingEnabled = true;
            this.AccessModeComboBox.Location = new System.Drawing.Point(3, 3);
            this.AccessModeComboBox.Name = "AccessModeComboBox";
            this.AccessModeComboBox.Size = new System.Drawing.Size(502, 24);
            this.AccessModeComboBox.TabIndex = 2;
            // 
            // InterfaceGroupBox
            // 
            this.InterfaceGroupBox.Controls.Add(this.InterfaceComboBox);
            this.InterfaceGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterfaceGroupBox.Location = new System.Drawing.Point(3, 3);
            this.InterfaceGroupBox.Name = "InterfaceGroupBox";
            this.InterfaceGroupBox.Size = new System.Drawing.Size(144, 50);
            this.InterfaceGroupBox.TabIndex = 1;
            this.InterfaceGroupBox.TabStop = false;
            this.InterfaceGroupBox.Text = "Interface";
            // 
            // InterfaceComboBox
            // 
            this.InterfaceComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterfaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InterfaceComboBox.FormattingEnabled = true;
            this.InterfaceComboBox.Location = new System.Drawing.Point(3, 16);
            this.InterfaceComboBox.Name = "InterfaceComboBox";
            this.InterfaceComboBox.Size = new System.Drawing.Size(138, 24);
            this.InterfaceComboBox.TabIndex = 1;
            this.InterfaceComboBox.SelectedIndexChanged += new System.EventHandler(this.InterfaceComboBox_SelectedIndexChanged);
            // 
            // DataAccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "DataAccess";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.DataAccess_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.DataSelectionGroupBox.ResumeLayout(false);
            this.DataSelectionTableLayoutPanel.ResumeLayout(false);
            this.DataSelectionTableLayoutPanel.PerformLayout();
            this.LoggingGroupBox.ResumeLayout(false);
            this.FileLoggingTableLayoutPanel.ResumeLayout(false);
            this.FileLoggingTableLayoutPanel.PerformLayout();
            this.InterfaceTableLayoutPanel.ResumeLayout(false);
            this.AccessModeGroupBox.ResumeLayout(false);
            this.AccessModeTableLayoutPanel.ResumeLayout(false);
            this.AccessModeTableLayoutPanel.PerformLayout();
            this.InterfaceGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.GroupBox AccessModeGroupBox;
        private System.Windows.Forms.TableLayoutPanel AccessModeTableLayoutPanel;
        private System.Windows.Forms.Button SetAccessModeButton;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.ComboBox AccessModeComboBox;
        private System.Windows.Forms.GroupBox DataSelectionGroupBox;
        private System.Windows.Forms.TableLayoutPanel DataSelectionTableLayoutPanel;
        private System.Windows.Forms.Button GetDataEntryButton;
        private System.Windows.Forms.Button SubscribeButton;
        private System.Windows.Forms.Button DataSelectionBackButton;
        private System.Windows.Forms.GroupBox LoggingGroupBox;
        private System.Windows.Forms.TableLayoutPanel FileLoggingTableLayoutPanel;
        private System.Windows.Forms.CheckBox LoggingCheckBox;
        private System.Windows.Forms.Button SelectLogfileButton;
        private System.Windows.Forms.TextBox LogFilePathTextBox;
        private System.Windows.Forms.Button EditLogFileButton;
        private System.Windows.Forms.TableLayoutPanel DataAccessTableLayoutPanel;
        private System.Windows.Forms.ComboBox InterfaceComboBox;
        private System.Windows.Forms.TableLayoutPanel InterfaceTableLayoutPanel;
        private System.Windows.Forms.GroupBox InterfaceGroupBox;
        internal System.Windows.Forms.TextBox DataSelectionTextBox;
    }
}
