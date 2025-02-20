namespace DNC_CSharp_Demo.UserControls
{
    partial class AxisPosition
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
            this.AxisIdLabel = new System.Windows.Forms.Label();
            this.AxisNameLabel = new System.Windows.Forms.Label();
            this.AxisPositionTextBox = new System.Windows.Forms.TextBox();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 3;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.AxisIdLabel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.AxisNameLabel, 1, 0);
            this.MainTableLayoutPanel.Controls.Add(this.AxisPositionTextBox, 2, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(238, 25);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // AxisIdLabel
            // 
            this.AxisIdLabel.AutoSize = true;
            this.AxisIdLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.AxisIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AxisIdLabel.Location = new System.Drawing.Point(3, 0);
            this.AxisIdLabel.Name = "AxisIdLabel";
            this.AxisIdLabel.Size = new System.Drawing.Size(42, 25);
            this.AxisIdLabel.TabIndex = 0;
            this.AxisIdLabel.Text = "AxisId:";
            this.AxisIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AxisNameLabel
            // 
            this.AxisNameLabel.AutoSize = true;
            this.AxisNameLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.AxisNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AxisNameLabel.Location = new System.Drawing.Point(73, 0);
            this.AxisNameLabel.Name = "AxisNameLabel";
            this.AxisNameLabel.Size = new System.Drawing.Size(66, 25);
            this.AxisNameLabel.TabIndex = 1;
            this.AxisNameLabel.Text = "AxisName:";
            this.AxisNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AxisPositionTextBox
            // 
            this.AxisPositionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxisPositionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AxisPositionTextBox.Location = new System.Drawing.Point(163, 3);
            this.AxisPositionTextBox.Name = "AxisPositionTextBox";
            this.AxisPositionTextBox.Size = new System.Drawing.Size(72, 21);
            this.AxisPositionTextBox.TabIndex = 2;
            this.AxisPositionTextBox.Text = "0.0000";
            // 
            // AxisPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AxisPosition";
            this.Size = new System.Drawing.Size(238, 25);
            this.Load += new System.EventHandler(this.AxisPosition_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.Label AxisIdLabel;
        private System.Windows.Forms.Label AxisNameLabel;
        private System.Windows.Forms.TextBox AxisPositionTextBox;
    }
}
