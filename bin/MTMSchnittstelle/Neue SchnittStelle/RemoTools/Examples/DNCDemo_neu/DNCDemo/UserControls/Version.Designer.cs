namespace DNC_CSharp_Demo.UserControls
{
    partial class Version
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
            this.ControlTypeTextBox = new System.Windows.Forms.TextBox();
            this.NcSoftwareGroupBox = new System.Windows.Forms.GroupBox();
            this.NcVersionListView = new System.Windows.Forms.ListView();
            this.IndentNrColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SoftwareTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HeadTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SikGroupBox = new System.Windows.Forms.GroupBox();
            this.SikTextBox = new System.Windows.Forms.TextBox();
            this.ComInterfaceGroupBox = new System.Windows.Forms.GroupBox();
            this.ComInterfaceTextBox = new System.Windows.Forms.TextBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel.SuspendLayout();
            this.NcSoftwareGroupBox.SuspendLayout();
            this.HeadTableLayoutPanel.SuspendLayout();
            this.SikGroupBox.SuspendLayout();
            this.ComInterfaceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.ControlTypeTextBox, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.NcSoftwareGroupBox, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.HeadTableLayoutPanel, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.InfoLabel, 0, 3);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 4;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.TabIndex = 10;
            // 
            // ControlTypeTextBox
            // 
            this.ControlTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlTypeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlTypeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlTypeTextBox.Location = new System.Drawing.Point(3, 3);
            this.ControlTypeTextBox.Name = "ControlTypeTextBox";
            this.ControlTypeTextBox.ReadOnly = true;
            this.ControlTypeTextBox.Size = new System.Drawing.Size(764, 22);
            this.ControlTypeTextBox.TabIndex = 5;
            this.ControlTypeTextBox.TabStop = false;
            this.ControlTypeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NcSoftwareGroupBox
            // 
            this.NcSoftwareGroupBox.Controls.Add(this.NcVersionListView);
            this.NcSoftwareGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NcSoftwareGroupBox.Location = new System.Drawing.Point(3, 89);
            this.NcSoftwareGroupBox.Name = "NcSoftwareGroupBox";
            this.NcSoftwareGroupBox.Size = new System.Drawing.Size(764, 184);
            this.NcSoftwareGroupBox.TabIndex = 4;
            this.NcSoftwareGroupBox.TabStop = false;
            this.NcSoftwareGroupBox.Text = "NC Software";
            // 
            // NcVersionListView
            // 
            this.NcVersionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IndentNrColumnHeader,
            this.SoftwareTypeColumnHeader,
            this.DescriptionColumnHeader});
            this.NcVersionListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NcVersionListView.FullRowSelect = true;
            this.NcVersionListView.GridLines = true;
            this.NcVersionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.NcVersionListView.Location = new System.Drawing.Point(3, 16);
            this.NcVersionListView.Name = "NcVersionListView";
            this.NcVersionListView.ShowGroups = false;
            this.NcVersionListView.Size = new System.Drawing.Size(758, 165);
            this.NcVersionListView.TabIndex = 0;
            this.NcVersionListView.TabStop = false;
            this.NcVersionListView.UseCompatibleStateImageBehavior = false;
            this.NcVersionListView.View = System.Windows.Forms.View.Details;
            // 
            // IndentNrColumnHeader
            // 
            this.IndentNrColumnHeader.Tag = "";
            this.IndentNrColumnHeader.Text = "IdentNr";
            this.IndentNrColumnHeader.Width = 180;
            // 
            // SoftwareTypeColumnHeader
            // 
            this.SoftwareTypeColumnHeader.Text = "SoftwareType";
            this.SoftwareTypeColumnHeader.Width = 100;
            // 
            // DescriptionColumnHeader
            // 
            this.DescriptionColumnHeader.Text = "Description";
            this.DescriptionColumnHeader.Width = 264;
            // 
            // HeadTableLayoutPanel
            // 
            this.HeadTableLayoutPanel.ColumnCount = 2;
            this.HeadTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.HeadTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.HeadTableLayoutPanel.Controls.Add(this.SikGroupBox, 0, 0);
            this.HeadTableLayoutPanel.Controls.Add(this.ComInterfaceGroupBox, 0, 0);
            this.HeadTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeadTableLayoutPanel.Location = new System.Drawing.Point(3, 29);
            this.HeadTableLayoutPanel.Name = "HeadTableLayoutPanel";
            this.HeadTableLayoutPanel.RowCount = 1;
            this.HeadTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.HeadTableLayoutPanel.Size = new System.Drawing.Size(764, 54);
            this.HeadTableLayoutPanel.TabIndex = 3;
            // 
            // SikGroupBox
            // 
            this.SikGroupBox.Controls.Add(this.SikTextBox);
            this.SikGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SikGroupBox.Location = new System.Drawing.Point(385, 3);
            this.SikGroupBox.Name = "SikGroupBox";
            this.SikGroupBox.Size = new System.Drawing.Size(376, 48);
            this.SikGroupBox.TabIndex = 10;
            this.SikGroupBox.TabStop = false;
            this.SikGroupBox.Text = "SIK";
            // 
            // SikTextBox
            // 
            this.SikTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SikTextBox.Location = new System.Drawing.Point(3, 16);
            this.SikTextBox.Name = "SikTextBox";
            this.SikTextBox.Size = new System.Drawing.Size(370, 20);
            this.SikTextBox.TabIndex = 2;
            // 
            // ComInterfaceGroupBox
            // 
            this.ComInterfaceGroupBox.Controls.Add(this.ComInterfaceTextBox);
            this.ComInterfaceGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComInterfaceGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ComInterfaceGroupBox.Name = "ComInterfaceGroupBox";
            this.ComInterfaceGroupBox.Size = new System.Drawing.Size(376, 48);
            this.ComInterfaceGroupBox.TabIndex = 9;
            this.ComInterfaceGroupBox.TabStop = false;
            this.ComInterfaceGroupBox.Text = "COM Interface";
            // 
            // ComInterfaceTextBox
            // 
            this.ComInterfaceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComInterfaceTextBox.Location = new System.Drawing.Point(3, 16);
            this.ComInterfaceTextBox.Name = "ComInterfaceTextBox";
            this.ComInterfaceTextBox.Size = new System.Drawing.Size(370, 20);
            this.ComInterfaceTextBox.TabIndex = 1;
            // 
            // InfoLabel
            // 
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.Location = new System.Drawing.Point(3, 276);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(764, 190);
            this.InfoLabel.TabIndex = 6;
            this.InfoLabel.Text = "The supported functionality depends on the type and version of the connected NC control.\r\n" +
                                  "Please refer to the compatibility tables in the SDK help system for more information!";
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Version
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "Version";
            this.Size = new System.Drawing.Size(770, 466);
            this.Load += new System.EventHandler(this.UCVersion_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.PerformLayout();
            this.NcSoftwareGroupBox.ResumeLayout(false);
            this.HeadTableLayoutPanel.ResumeLayout(false);
            this.SikGroupBox.ResumeLayout(false);
            this.SikGroupBox.PerformLayout();
            this.ComInterfaceGroupBox.ResumeLayout(false);
            this.ComInterfaceGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.GroupBox NcSoftwareGroupBox;
        private System.Windows.Forms.ListView NcVersionListView;
        private System.Windows.Forms.ColumnHeader IndentNrColumnHeader;
        private System.Windows.Forms.ColumnHeader SoftwareTypeColumnHeader;
        private System.Windows.Forms.ColumnHeader DescriptionColumnHeader;
        private System.Windows.Forms.TableLayoutPanel HeadTableLayoutPanel;
        private System.Windows.Forms.GroupBox SikGroupBox;
        private System.Windows.Forms.TextBox SikTextBox;
        private System.Windows.Forms.GroupBox ComInterfaceGroupBox;
        private System.Windows.Forms.TextBox ComInterfaceTextBox;
        private System.Windows.Forms.TextBox ControlTypeTextBox;
        private System.Windows.Forms.Label InfoLabel;

    }
}
