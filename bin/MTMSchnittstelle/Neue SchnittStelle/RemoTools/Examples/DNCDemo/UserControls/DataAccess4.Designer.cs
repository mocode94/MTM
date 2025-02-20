namespace DNC_CSharp_Demo.UserControls
{
    partial class DataAccess4
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
            this.AddFullIdentTextBox = new System.Windows.Forms.TextBox();
            this.AddStoragePathTextBox = new System.Windows.Forms.TextBox();
            this.AddDataEntryButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddDataEntryGroupBox = new System.Windows.Forms.GroupBox();
            this.AddDataEntryTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AddFullIdentInfoLabel = new System.Windows.Forms.Label();
            this.AddStoragePathInfoLabel = new System.Windows.Forms.Label();
            this.RemoveDataEntryGroupBox = new System.Windows.Forms.GroupBox();
            this.RemoveDataEntryTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RemoveFullIdentTextBox = new System.Windows.Forms.TextBox();
            this.RemoveFullIdentInfoLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AddDataEntryGroupBox.SuspendLayout();
            this.AddDataEntryTableLayoutPanel.SuspendLayout();
            this.RemoveDataEntryGroupBox.SuspendLayout();
            this.RemoveDataEntryTableLayoutPanel.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddFullIdentTextBox
            // 
            this.AddFullIdentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddFullIdentTextBox.Location = new System.Drawing.Point(3, 21);
            this.AddFullIdentTextBox.Name = "AddFullIdentTextBox";
            this.AddFullIdentTextBox.Size = new System.Drawing.Size(282, 20);
            this.AddFullIdentTextBox.TabIndex = 0;
            this.AddFullIdentTextBox.Text = "\\CFG\\CfgLogInfoDnc";
            // 
            // AddStoragePathTextBox
            // 
            this.AddStoragePathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddStoragePathTextBox.Location = new System.Drawing.Point(3, 65);
            this.AddStoragePathTextBox.Name = "AddStoragePathTextBox";
            this.AddStoragePathTextBox.Size = new System.Drawing.Size(282, 20);
            this.AddStoragePathTextBox.TabIndex = 1;
            this.AddStoragePathTextBox.Text = "TNC:\\config\\user.cfg";
            // 
            // AddDataEntryButton
            // 
            this.AddDataEntryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddDataEntryButton.Location = new System.Drawing.Point(3, 348);
            this.AddDataEntryButton.Name = "AddDataEntryButton";
            this.AddDataEntryButton.Size = new System.Drawing.Size(282, 24);
            this.AddDataEntryButton.TabIndex = 2;
            this.AddDataEntryButton.Text = "Add";
            this.AddDataEntryButton.UseVisualStyleBackColor = true;
            this.AddDataEntryButton.Click += new System.EventHandler(this.AddDataEntryButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveButton.Location = new System.Drawing.Point(3, 348);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(282, 24);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddDataEntryGroupBox
            // 
            this.AddDataEntryGroupBox.Controls.Add(this.AddDataEntryTableLayoutPanel);
            this.AddDataEntryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddDataEntryGroupBox.Location = new System.Drawing.Point(3, 3);
            this.AddDataEntryGroupBox.Name = "AddDataEntryGroupBox";
            this.AddDataEntryGroupBox.Size = new System.Drawing.Size(294, 394);
            this.AddDataEntryGroupBox.TabIndex = 4;
            this.AddDataEntryGroupBox.TabStop = false;
            this.AddDataEntryGroupBox.Text = "Add data entry";
            // 
            // AddDataEntryTableLayoutPanel
            // 
            this.AddDataEntryTableLayoutPanel.ColumnCount = 1;
            this.AddDataEntryTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AddDataEntryTableLayoutPanel.Controls.Add(this.AddStoragePathTextBox, 0, 3);
            this.AddDataEntryTableLayoutPanel.Controls.Add(this.AddFullIdentTextBox, 0, 1);
            this.AddDataEntryTableLayoutPanel.Controls.Add(this.AddDataEntryButton, 0, 5);
            this.AddDataEntryTableLayoutPanel.Controls.Add(this.AddFullIdentInfoLabel, 0, 0);
            this.AddDataEntryTableLayoutPanel.Controls.Add(this.AddStoragePathInfoLabel, 0, 2);
            this.AddDataEntryTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddDataEntryTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.AddDataEntryTableLayoutPanel.Name = "AddDataEntryTableLayoutPanel";
            this.AddDataEntryTableLayoutPanel.RowCount = 6;
            this.AddDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.AddDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.AddDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.AddDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.AddDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AddDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.AddDataEntryTableLayoutPanel.Size = new System.Drawing.Size(288, 375);
            this.AddDataEntryTableLayoutPanel.TabIndex = 0;
            // 
            // AddFullIdentInfoLabel
            // 
            this.AddFullIdentInfoLabel.AutoSize = true;
            this.AddFullIdentInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddFullIdentInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddFullIdentInfoLabel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.AddFullIdentInfoLabel.Location = new System.Drawing.Point(3, 0);
            this.AddFullIdentInfoLabel.Name = "AddFullIdentInfoLabel";
            this.AddFullIdentInfoLabel.Size = new System.Drawing.Size(282, 18);
            this.AddFullIdentInfoLabel.TabIndex = 3;
            this.AddFullIdentInfoLabel.Text = "Full ident:";
            this.AddFullIdentInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddStoragePathInfoLabel
            // 
            this.AddStoragePathInfoLabel.AutoSize = true;
            this.AddStoragePathInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddStoragePathInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddStoragePathInfoLabel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.AddStoragePathInfoLabel.Location = new System.Drawing.Point(3, 44);
            this.AddStoragePathInfoLabel.Name = "AddStoragePathInfoLabel";
            this.AddStoragePathInfoLabel.Size = new System.Drawing.Size(282, 18);
            this.AddStoragePathInfoLabel.TabIndex = 4;
            this.AddStoragePathInfoLabel.Text = "Storage path:";
            // 
            // RemoveDataEntryGroupBox
            // 
            this.RemoveDataEntryGroupBox.Controls.Add(this.RemoveDataEntryTableLayoutPanel);
            this.RemoveDataEntryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveDataEntryGroupBox.Location = new System.Drawing.Point(303, 3);
            this.RemoveDataEntryGroupBox.Name = "RemoveDataEntryGroupBox";
            this.RemoveDataEntryGroupBox.Size = new System.Drawing.Size(294, 394);
            this.RemoveDataEntryGroupBox.TabIndex = 5;
            this.RemoveDataEntryGroupBox.TabStop = false;
            this.RemoveDataEntryGroupBox.Text = "Remove data entry";
            // 
            // RemoveDataEntryTableLayoutPanel
            // 
            this.RemoveDataEntryTableLayoutPanel.ColumnCount = 1;
            this.RemoveDataEntryTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RemoveDataEntryTableLayoutPanel.Controls.Add(this.RemoveFullIdentTextBox, 0, 1);
            this.RemoveDataEntryTableLayoutPanel.Controls.Add(this.RemoveButton, 0, 3);
            this.RemoveDataEntryTableLayoutPanel.Controls.Add(this.RemoveFullIdentInfoLabel, 0, 0);
            this.RemoveDataEntryTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveDataEntryTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.RemoveDataEntryTableLayoutPanel.Name = "RemoveDataEntryTableLayoutPanel";
            this.RemoveDataEntryTableLayoutPanel.RowCount = 4;
            this.RemoveDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.RemoveDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.RemoveDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RemoveDataEntryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.RemoveDataEntryTableLayoutPanel.Size = new System.Drawing.Size(288, 375);
            this.RemoveDataEntryTableLayoutPanel.TabIndex = 0;
            // 
            // RemoveFullIdentTextBox
            // 
            this.RemoveFullIdentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFullIdentTextBox.Location = new System.Drawing.Point(3, 21);
            this.RemoveFullIdentTextBox.Name = "RemoveFullIdentTextBox";
            this.RemoveFullIdentTextBox.Size = new System.Drawing.Size(282, 20);
            this.RemoveFullIdentTextBox.TabIndex = 5;
            this.RemoveFullIdentTextBox.Text = "\\CFG\\CfgLogInfoDnc";
            // 
            // RemoveFullIdentInfoLabel
            // 
            this.RemoveFullIdentInfoLabel.AutoSize = true;
            this.RemoveFullIdentInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFullIdentInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveFullIdentInfoLabel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.RemoveFullIdentInfoLabel.Location = new System.Drawing.Point(3, 0);
            this.RemoveFullIdentInfoLabel.Name = "RemoveFullIdentInfoLabel";
            this.RemoveFullIdentInfoLabel.Size = new System.Drawing.Size(282, 18);
            this.RemoveFullIdentInfoLabel.TabIndex = 6;
            this.RemoveFullIdentInfoLabel.Text = "Full ident:";
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 2;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.Controls.Add(this.AddDataEntryGroupBox, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.RemoveDataEntryGroupBox, 1, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(600, 400);
            this.MainTableLayoutPanel.TabIndex = 6;
            // 
            // DataAccess4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "DataAccess4";
            this.Size = new System.Drawing.Size(600, 400);
            this.AddDataEntryGroupBox.ResumeLayout(false);
            this.AddDataEntryTableLayoutPanel.ResumeLayout(false);
            this.AddDataEntryTableLayoutPanel.PerformLayout();
            this.RemoveDataEntryGroupBox.ResumeLayout(false);
            this.RemoveDataEntryTableLayoutPanel.ResumeLayout(false);
            this.RemoveDataEntryTableLayoutPanel.PerformLayout();
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox AddFullIdentTextBox;
        private System.Windows.Forms.TextBox AddStoragePathTextBox;
        private System.Windows.Forms.Button AddDataEntryButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.GroupBox AddDataEntryGroupBox;
        private System.Windows.Forms.TableLayoutPanel AddDataEntryTableLayoutPanel;
        private System.Windows.Forms.Label AddFullIdentInfoLabel;
        private System.Windows.Forms.Label AddStoragePathInfoLabel;
        private System.Windows.Forms.GroupBox RemoveDataEntryGroupBox;
        private System.Windows.Forms.TableLayoutPanel RemoveDataEntryTableLayoutPanel;
        private System.Windows.Forms.TextBox RemoveFullIdentTextBox;
        private System.Windows.Forms.Label RemoveFullIdentInfoLabel;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;

    }
}
