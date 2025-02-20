namespace DNC_CSharp_Demo.UserControls
{
    partial class ProcessData
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
            this.RefreshButton = new System.Windows.Forms.Button();
            this.TimesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MachineUpTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.MachineUpTimeTextBox = new System.Windows.Forms.TextBox();
            this.NcUpTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.NcUpTimeTextBox = new System.Windows.Forms.TextBox();
            this.SpindleRunningTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.SpindleRunningTimeTextBox = new System.Windows.Forms.TextBox();
            this.MachineRunningTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.MachineRunningTimeTextBox = new System.Windows.Forms.TextBox();
            this.MainTableLayoutPanel.SuspendLayout();
            this.TimesTableLayoutPanel.SuspendLayout();
            this.MachineUpTimeGroupBox.SuspendLayout();
            this.NcUpTimeGroupBox.SuspendLayout();
            this.SpindleRunningTimeGroupBox.SuspendLayout();
            this.MachineRunningTimeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.RefreshButton, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.TimesTableLayoutPanel, 0, 1);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.TabIndex = 9;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(3, 3);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(764, 200);
            this.RefreshButton.TabIndex = 0;
            this.RefreshButton.Text = "R e f r e s h";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // TimesTableLayoutPanel
            // 
            this.TimesTableLayoutPanel.ColumnCount = 2;
            this.TimesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TimesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TimesTableLayoutPanel.Controls.Add(this.MachineUpTimeGroupBox, 0, 1);
            this.TimesTableLayoutPanel.Controls.Add(this.NcUpTimeGroupBox, 1, 1);
            this.TimesTableLayoutPanel.Controls.Add(this.SpindleRunningTimeGroupBox, 1, 0);
            this.TimesTableLayoutPanel.Controls.Add(this.MachineRunningTimeGroupBox, 0, 0);
            this.TimesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TimesTableLayoutPanel.Location = new System.Drawing.Point(3, 209);
            this.TimesTableLayoutPanel.Name = "TimesTableLayoutPanel";
            this.TimesTableLayoutPanel.RowCount = 2;
            this.TimesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TimesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TimesTableLayoutPanel.Size = new System.Drawing.Size(764, 254);
            this.TimesTableLayoutPanel.TabIndex = 12;
            // 
            // MachineUpTimeGroupBox
            // 
            this.MachineUpTimeGroupBox.Controls.Add(this.MachineUpTimeTextBox);
            this.MachineUpTimeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MachineUpTimeGroupBox.Location = new System.Drawing.Point(3, 130);
            this.MachineUpTimeGroupBox.Name = "MachineUpTimeGroupBox";
            this.MachineUpTimeGroupBox.Size = new System.Drawing.Size(376, 121);
            this.MachineUpTimeGroupBox.TabIndex = 9;
            this.MachineUpTimeGroupBox.TabStop = false;
            this.MachineUpTimeGroupBox.Text = "MachineUpTime";
            // 
            // MachineUpTimeTextBox
            // 
            this.MachineUpTimeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MachineUpTimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MachineUpTimeTextBox.Location = new System.Drawing.Point(3, 16);
            this.MachineUpTimeTextBox.Name = "MachineUpTimeTextBox";
            this.MachineUpTimeTextBox.Size = new System.Drawing.Size(370, 98);
            this.MachineUpTimeTextBox.TabIndex = 2;
            this.MachineUpTimeTextBox.Text = "0:00";
            this.MachineUpTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NcUpTimeGroupBox
            // 
            this.NcUpTimeGroupBox.Controls.Add(this.NcUpTimeTextBox);
            this.NcUpTimeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NcUpTimeGroupBox.Location = new System.Drawing.Point(385, 130);
            this.NcUpTimeGroupBox.Name = "NcUpTimeGroupBox";
            this.NcUpTimeGroupBox.Size = new System.Drawing.Size(376, 121);
            this.NcUpTimeGroupBox.TabIndex = 10;
            this.NcUpTimeGroupBox.TabStop = false;
            this.NcUpTimeGroupBox.Text = "NcUpTime";
            // 
            // NcUpTimeTextBox
            // 
            this.NcUpTimeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NcUpTimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NcUpTimeTextBox.Location = new System.Drawing.Point(3, 16);
            this.NcUpTimeTextBox.Name = "NcUpTimeTextBox";
            this.NcUpTimeTextBox.Size = new System.Drawing.Size(370, 98);
            this.NcUpTimeTextBox.TabIndex = 1;
            this.NcUpTimeTextBox.Text = "0:00";
            this.NcUpTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SpindleRunningTimeGroupBox
            // 
            this.SpindleRunningTimeGroupBox.Controls.Add(this.SpindleRunningTimeTextBox);
            this.SpindleRunningTimeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpindleRunningTimeGroupBox.Location = new System.Drawing.Point(385, 3);
            this.SpindleRunningTimeGroupBox.Name = "SpindleRunningTimeGroupBox";
            this.SpindleRunningTimeGroupBox.Size = new System.Drawing.Size(376, 121);
            this.SpindleRunningTimeGroupBox.TabIndex = 8;
            this.SpindleRunningTimeGroupBox.TabStop = false;
            this.SpindleRunningTimeGroupBox.Text = "SpindleRunningTime (1st spindle)";
            // 
            // SpindleRunningTimeTextBox
            // 
            this.SpindleRunningTimeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpindleRunningTimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpindleRunningTimeTextBox.Location = new System.Drawing.Point(3, 16);
            this.SpindleRunningTimeTextBox.Name = "SpindleRunningTimeTextBox";
            this.SpindleRunningTimeTextBox.Size = new System.Drawing.Size(370, 98);
            this.SpindleRunningTimeTextBox.TabIndex = 4;
            this.SpindleRunningTimeTextBox.Text = "0:00";
            this.SpindleRunningTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MachineRunningTimeGroupBox
            // 
            this.MachineRunningTimeGroupBox.Controls.Add(this.MachineRunningTimeTextBox);
            this.MachineRunningTimeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MachineRunningTimeGroupBox.Location = new System.Drawing.Point(3, 3);
            this.MachineRunningTimeGroupBox.Name = "MachineRunningTimeGroupBox";
            this.MachineRunningTimeGroupBox.Size = new System.Drawing.Size(376, 121);
            this.MachineRunningTimeGroupBox.TabIndex = 11;
            this.MachineRunningTimeGroupBox.TabStop = false;
            this.MachineRunningTimeGroupBox.Text = "MachineRunningTime";
            // 
            // MachineRunningTimeTextBox
            // 
            this.MachineRunningTimeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MachineRunningTimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MachineRunningTimeTextBox.Location = new System.Drawing.Point(3, 16);
            this.MachineRunningTimeTextBox.Name = "MachineRunningTimeTextBox";
            this.MachineRunningTimeTextBox.Size = new System.Drawing.Size(370, 98);
            this.MachineRunningTimeTextBox.TabIndex = 3;
            this.MachineRunningTimeTextBox.Text = "0:00";
            this.MachineRunningTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ProcessData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "ProcessData";
            this.Size = new System.Drawing.Size(770, 466);
            this.Load += new System.EventHandler(this.UCProcessData_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.TimesTableLayoutPanel.ResumeLayout(false);
            this.MachineUpTimeGroupBox.ResumeLayout(false);
            this.MachineUpTimeGroupBox.PerformLayout();
            this.NcUpTimeGroupBox.ResumeLayout(false);
            this.NcUpTimeGroupBox.PerformLayout();
            this.SpindleRunningTimeGroupBox.ResumeLayout(false);
            this.SpindleRunningTimeGroupBox.PerformLayout();
            this.MachineRunningTimeGroupBox.ResumeLayout(false);
            this.MachineRunningTimeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.TableLayoutPanel TimesTableLayoutPanel;
        private System.Windows.Forms.GroupBox SpindleRunningTimeGroupBox;
        private System.Windows.Forms.TextBox SpindleRunningTimeTextBox;
        private System.Windows.Forms.GroupBox MachineRunningTimeGroupBox;
        private System.Windows.Forms.TextBox MachineRunningTimeTextBox;
        private System.Windows.Forms.GroupBox MachineUpTimeGroupBox;
        private System.Windows.Forms.TextBox MachineUpTimeTextBox;
        private System.Windows.Forms.GroupBox NcUpTimeGroupBox;
        private System.Windows.Forms.TextBox NcUpTimeTextBox;
    }
}
