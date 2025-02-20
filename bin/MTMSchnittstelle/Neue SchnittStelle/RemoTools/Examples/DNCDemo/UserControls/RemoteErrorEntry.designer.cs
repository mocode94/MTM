namespace DNC_CSharp_Demo.UserControls
{
    partial class RemoteErrorEntry
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
            this.ActionButton = new System.Windows.Forms.Button();
            this.ErrorMessageLabel = new System.Windows.Forms.Label();
            this.ErrorClassLabel = new System.Windows.Forms.Label();
            this.ErrorNumberLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.RemoteAckCheckBox = new System.Windows.Forms.CheckBox();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.MainTableLayoutPanel.ColumnCount = 6;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.MainTableLayoutPanel.Controls.Add(this.ActionButton, 4, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ErrorMessageLabel, 3, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ErrorClassLabel, 2, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ErrorNumberLabel, 1, 0);
            this.MainTableLayoutPanel.Controls.Add(this.DeleteButton, 5, 0);
            this.MainTableLayoutPanel.Controls.Add(this.RemoteAckCheckBox, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(780, 30);
            this.MainTableLayoutPanel.TabIndex = 1;
            // 
            // ActionButton
            // 
            this.ActionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionButton.Location = new System.Drawing.Point(638, 4);
            this.ActionButton.Name = "ActionButton";
            this.ActionButton.Size = new System.Drawing.Size(64, 22);
            this.ActionButton.TabIndex = 8;
            this.ActionButton.Text = "Raise";
            this.ActionButton.UseVisualStyleBackColor = true;
            this.ActionButton.Click += new System.EventHandler(this.ActionButton_Click);
            // 
            // ErrorMessageLabel
            // 
            this.ErrorMessageLabel.AutoSize = true;
            this.ErrorMessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorMessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorMessageLabel.Location = new System.Drawing.Point(362, 1);
            this.ErrorMessageLabel.Name = "ErrorMessageLabel";
            this.ErrorMessageLabel.Size = new System.Drawing.Size(269, 28);
            this.ErrorMessageLabel.TabIndex = 2;
            this.ErrorMessageLabel.Text = "Error Message";
            this.ErrorMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ErrorClassLabel
            // 
            this.ErrorClassLabel.AutoSize = true;
            this.ErrorClassLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorClassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorClassLabel.Location = new System.Drawing.Point(151, 1);
            this.ErrorClassLabel.Name = "ErrorClassLabel";
            this.ErrorClassLabel.Size = new System.Drawing.Size(204, 28);
            this.ErrorClassLabel.TabIndex = 1;
            this.ErrorClassLabel.Text = "Error Class";
            this.ErrorClassLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ErrorNumberLabel
            // 
            this.ErrorNumberLabel.AutoSize = true;
            this.ErrorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorNumberLabel.Location = new System.Drawing.Point(40, 1);
            this.ErrorNumberLabel.Name = "ErrorNumberLabel";
            this.ErrorNumberLabel.Size = new System.Drawing.Size(104, 28);
            this.ErrorNumberLabel.TabIndex = 0;
            this.ErrorNumberLabel.Text = "Error Number";
            this.ErrorNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteButton.Location = new System.Drawing.Point(709, 4);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(67, 22);
            this.DeleteButton.TabIndex = 9;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // RemoteAckCheckBox
            // 
            this.RemoteAckCheckBox.AutoSize = true;
            this.RemoteAckCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RemoteAckCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoteAckCheckBox.Enabled = false;
            this.RemoteAckCheckBox.Location = new System.Drawing.Point(4, 4);
            this.RemoteAckCheckBox.Name = "RemoteAckCheckBox";
            this.RemoteAckCheckBox.Size = new System.Drawing.Size(29, 22);
            this.RemoteAckCheckBox.TabIndex = 10;
            this.RemoteAckCheckBox.UseVisualStyleBackColor = true;
            // 
            // RemoteErrorEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "RemoteErrorEntry";
            this.Size = new System.Drawing.Size(780, 30);
            this.Load += new System.EventHandler(this.RemoteErrorEntry_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.Button ActionButton;
        private System.Windows.Forms.Label ErrorMessageLabel;
        private System.Windows.Forms.Label ErrorClassLabel;
        private System.Windows.Forms.Label ErrorNumberLabel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.CheckBox RemoteAckCheckBox;
    }
}
