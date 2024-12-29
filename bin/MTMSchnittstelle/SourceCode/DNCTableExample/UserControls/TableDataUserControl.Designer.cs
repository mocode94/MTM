namespace DNC_CSharp_Demo.UserControls
{
    partial class TableDataUserControl
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
            this.VarValueTextBox = new System.Windows.Forms.TextBox();
            this.ColumnNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // VarValueTextBox
            // 
            this.VarValueTextBox.Location = new System.Drawing.Point(6, 20);
            this.VarValueTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.VarValueTextBox.Name = "VarValueTextBox";
            this.VarValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarValueTextBox.TabIndex = 0;
            // 
            // ColumnNameLabel
            // 
            this.ColumnNameLabel.Location = new System.Drawing.Point(3, 0);
            this.ColumnNameLabel.Name = "ColumnNameLabel";
            this.ColumnNameLabel.Size = new System.Drawing.Size(103, 20);
            this.ColumnNameLabel.TabIndex = 2;
            this.ColumnNameLabel.Text = "Column Name";
            // 
            // ToolDataUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ColumnNameLabel);
            this.Controls.Add(this.VarValueTextBox);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ToolDataUserControl";
            this.Size = new System.Drawing.Size(116, 53);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox VarValueTextBox;
        private System.Windows.Forms.Label ColumnNameLabel;
    }
}
