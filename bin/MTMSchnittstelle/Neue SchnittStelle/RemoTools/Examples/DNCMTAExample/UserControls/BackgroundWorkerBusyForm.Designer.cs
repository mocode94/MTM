namespace DNC_CSharp_Demo.UserControls
{
    partial class BackgroundWorkerBusyForm
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
            this.HardAbortButton = new System.Windows.Forms.Button();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HardAbortTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.WaitingProgressBar = new System.Windows.Forms.ProgressBar();
            this.MainTableLayoutPanel.SuspendLayout();
            this.HardAbortTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // HardAbortButton
            // 
            this.HardAbortButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HardAbortButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HardAbortButton.Location = new System.Drawing.Point(86, 3);
            this.HardAbortButton.Name = "HardAbortButton";
            this.HardAbortButton.Size = new System.Drawing.Size(194, 24);
            this.HardAbortButton.TabIndex = 0;
            this.HardAbortButton.Text = "!!! HARD ABORT !!!";
            this.HardAbortButton.UseVisualStyleBackColor = true;
            this.HardAbortButton.Click += new System.EventHandler(this.HardAbortButton_Click);
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.HardAbortTableLayoutPanel, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.InfoLabel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.WaitingProgressBar, 0, 1);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 3;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(373, 126);
            this.MainTableLayoutPanel.TabIndex = 1;
            // 
            // HardAbortTableLayoutPanel
            // 
            this.HardAbortTableLayoutPanel.ColumnCount = 3;
            this.HardAbortTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.HardAbortTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.HardAbortTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.HardAbortTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.HardAbortTableLayoutPanel.Controls.Add(this.HardAbortButton, 1, 0);
            this.HardAbortTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HardAbortTableLayoutPanel.Location = new System.Drawing.Point(3, 93);
            this.HardAbortTableLayoutPanel.Name = "HardAbortTableLayoutPanel";
            this.HardAbortTableLayoutPanel.RowCount = 1;
            this.HardAbortTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HardAbortTableLayoutPanel.Size = new System.Drawing.Size(367, 30);
            this.HardAbortTableLayoutPanel.TabIndex = 0;
            // 
            // InfoLabel
            // 
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.Location = new System.Drawing.Point(3, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(367, 60);
            this.InfoLabel.TabIndex = 1;
            this.InfoLabel.Text = "A Background worker is still busy. Waiting for the worker to finish.";
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WaitingProgressBar
            // 
            this.WaitingProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaitingProgressBar.Location = new System.Drawing.Point(3, 63);
            this.WaitingProgressBar.Name = "WaitingProgressBar";
            this.WaitingProgressBar.Size = new System.Drawing.Size(367, 24);
            this.WaitingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.WaitingProgressBar.TabIndex = 2;
            this.WaitingProgressBar.Value = 100;
            // 
            // BackgroundWorkerBusyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 126);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "BackgroundWorkerBusyForm";
            this.Text = "Waiting ...";
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.HardAbortTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button HardAbortButton;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel HardAbortTableLayoutPanel;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ProgressBar WaitingProgressBar;
    }
}