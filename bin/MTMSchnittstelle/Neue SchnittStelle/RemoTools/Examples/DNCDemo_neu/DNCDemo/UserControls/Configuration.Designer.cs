namespace DNC_CSharp_Demo.UserControls
{
    partial class Configuration
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
            this.ChannelInfoListView = new System.Windows.Forms.ListView();
            this.ChannelIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AxesInChannelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AxisInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.AxisInfoDataGridView = new System.Windows.Forms.DataGridView();
            this.AxisNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisPositionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SetPositionButton = new System.Windows.Forms.Button();
            this.AxisInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AxisInfoDataGridView)).BeginInit();
            this.ChannelInfoGroupBox.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChannelInfoListView
            // 
            this.ChannelInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChannelIdColumnHeader,
            this.AxesInChannelColumnHeader});
            this.ChannelInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelInfoListView.FullRowSelect = true;
            this.ChannelInfoListView.GridLines = true;
            this.ChannelInfoListView.Location = new System.Drawing.Point(3, 16);
            this.ChannelInfoListView.Name = "ChannelInfoListView";
            this.ChannelInfoListView.Size = new System.Drawing.Size(296, 441);
            this.ChannelInfoListView.TabIndex = 1;
            this.ChannelInfoListView.UseCompatibleStateImageBehavior = false;
            this.ChannelInfoListView.View = System.Windows.Forms.View.Details;
            // 
            // ChannelIdColumnHeader
            // 
            this.ChannelIdColumnHeader.Text = "ChannelID";
            this.ChannelIdColumnHeader.Width = 80;
            // 
            // AxesInChannelColumnHeader
            // 
            this.AxesInChannelColumnHeader.Text = "AxesInChannel";
            this.AxesInChannelColumnHeader.Width = 210;
            // 
            // AxisInfoGroupBox
            // 
            this.AxisInfoGroupBox.Controls.Add(this.AxisInfoDataGridView);
            this.AxisInfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxisInfoGroupBox.Location = new System.Drawing.Point(3, 3);
            this.AxisInfoGroupBox.Name = "AxisInfoGroupBox";
            this.AxisInfoGroupBox.Size = new System.Drawing.Size(456, 431);
            this.AxisInfoGroupBox.TabIndex = 4;
            this.AxisInfoGroupBox.TabStop = false;
            this.AxisInfoGroupBox.Text = "Axis Info";
            // 
            // AxisInfoDataGridView
            // 
            this.AxisInfoDataGridView.AllowUserToAddRows = false;
            this.AxisInfoDataGridView.AllowUserToDeleteRows = false;
            this.AxisInfoDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.AxisInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AxisInfoDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AxisNameColumn,
            this.AxisIDColumn,
            this.AxisTypeColumn,
            this.AxisPositionColumn});
            this.AxisInfoDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxisInfoDataGridView.Location = new System.Drawing.Point(3, 16);
            this.AxisInfoDataGridView.Name = "AxisInfoDataGridView";
            this.AxisInfoDataGridView.RowHeadersVisible = false;
            this.AxisInfoDataGridView.Size = new System.Drawing.Size(450, 412);
            this.AxisInfoDataGridView.TabIndex = 7;
            // 
            // AxisNameColumn
            // 
            this.AxisNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AxisNameColumn.HeaderText = "AxisName";
            this.AxisNameColumn.Name = "AxisNameColumn";
            this.AxisNameColumn.ReadOnly = true;
            this.AxisNameColumn.Width = 79;
            // 
            // AxisIDColumn
            // 
            this.AxisIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AxisIDColumn.HeaderText = "AxisID";
            this.AxisIDColumn.Name = "AxisIDColumn";
            this.AxisIDColumn.ReadOnly = true;
            this.AxisIDColumn.Width = 62;
            // 
            // AxisTypeColumn
            // 
            this.AxisTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AxisTypeColumn.HeaderText = "AxisType";
            this.AxisTypeColumn.Name = "AxisTypeColumn";
            this.AxisTypeColumn.ReadOnly = true;
            this.AxisTypeColumn.Width = 75;
            // 
            // AxisPositionColumn
            // 
            this.AxisPositionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AxisPositionColumn.HeaderText = "AxisPosition";
            this.AxisPositionColumn.Name = "AxisPositionColumn";
            // 
            // ChannelInfoGroupBox
            // 
            this.ChannelInfoGroupBox.Controls.Add(this.ChannelInfoListView);
            this.ChannelInfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelInfoGroupBox.Location = new System.Drawing.Point(465, 3);
            this.ChannelInfoGroupBox.Name = "ChannelInfoGroupBox";
            this.MainTableLayoutPanel.SetRowSpan(this.ChannelInfoGroupBox, 2);
            this.ChannelInfoGroupBox.Size = new System.Drawing.Size(302, 460);
            this.ChannelInfoGroupBox.TabIndex = 5;
            this.ChannelInfoGroupBox.TabStop = false;
            this.ChannelInfoGroupBox.Text = "Channel Info";
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 2;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.MainTableLayoutPanel.Controls.Add(this.ChannelInfoGroupBox, 1, 0);
            this.MainTableLayoutPanel.Controls.Add(this.AxisInfoGroupBox, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.SetPositionButton, 0, 1);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.TabIndex = 6;
            // 
            // SetPositionButton
            // 
            this.SetPositionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetPositionButton.Location = new System.Drawing.Point(3, 440);
            this.SetPositionButton.Name = "SetPositionButton";
            this.SetPositionButton.Size = new System.Drawing.Size(456, 23);
            this.SetPositionButton.TabIndex = 7;
            this.SetPositionButton.Text = "SetPosition (JHVirtualMachine)";
            this.SetPositionButton.UseVisualStyleBackColor = true;
            this.SetPositionButton.Click += new System.EventHandler(this.SetPositionButton_Click);
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "Configuration";
            this.Size = new System.Drawing.Size(770, 466);
            this.Load += new System.EventHandler(this.Configuration_Load);
            this.AxisInfoGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AxisInfoDataGridView)).EndInit();
            this.ChannelInfoGroupBox.ResumeLayout(false);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView ChannelInfoListView;
        private System.Windows.Forms.ColumnHeader ChannelIdColumnHeader;
        private System.Windows.Forms.ColumnHeader AxesInChannelColumnHeader;
        private System.Windows.Forms.GroupBox AxisInfoGroupBox;
        private System.Windows.Forms.GroupBox ChannelInfoGroupBox;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.Button SetPositionButton;
        private System.Windows.Forms.DataGridView AxisInfoDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisPositionColumn;
    }
}
