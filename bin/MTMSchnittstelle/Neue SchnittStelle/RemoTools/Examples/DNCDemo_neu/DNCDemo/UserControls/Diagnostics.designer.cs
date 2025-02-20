namespace DNC_CSharp_Demo.UserControls
{
    partial class Diagnostics
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
            this.ServiceFileGroupBox = new System.Windows.Forms.GroupBox();
            this.ServiceFileTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ServiceFileNameTextBox = new System.Windows.Forms.TextBox();
            this.ServiceFileButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StartCreateServiceFileButton = new System.Windows.Forms.Button();
            this.ReceiveServieFileButton = new System.Windows.Forms.Button();
            this.ScreenShotTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PreviewScreenShotPictureBox = new System.Windows.Forms.PictureBox();
            this.MakeScreenShotActiontableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ScreenShotFileNameTextBox = new System.Windows.Forms.TextBox();
            this.MakeScreenShotButton = new System.Windows.Forms.Button();
            this.ScreenShotGroupBox = new System.Windows.Forms.GroupBox();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RightSideTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ServiceFileGroupBox.SuspendLayout();
            this.ServiceFileTableLayoutPanel.SuspendLayout();
            this.ServiceFileButtonsTableLayoutPanel.SuspendLayout();
            this.ScreenShotTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewScreenShotPictureBox)).BeginInit();
            this.MakeScreenShotActiontableLayoutPanel.SuspendLayout();
            this.ScreenShotGroupBox.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.RightSideTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServiceFileGroupBox
            // 
            this.ServiceFileGroupBox.Controls.Add(this.ServiceFileTableLayoutPanel);
            this.ServiceFileGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServiceFileGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ServiceFileGroupBox.Name = "ServiceFileGroupBox";
            this.ServiceFileGroupBox.Size = new System.Drawing.Size(758, 76);
            this.ServiceFileGroupBox.TabIndex = 1;
            this.ServiceFileGroupBox.TabStop = false;
            this.ServiceFileGroupBox.Text = "Service File";
            // 
            // ServiceFileTableLayoutPanel
            // 
            this.ServiceFileTableLayoutPanel.ColumnCount = 1;
            this.ServiceFileTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ServiceFileTableLayoutPanel.Controls.Add(this.ServiceFileNameTextBox, 0, 1);
            this.ServiceFileTableLayoutPanel.Controls.Add(this.ServiceFileButtonsTableLayoutPanel, 0, 0);
            this.ServiceFileTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServiceFileTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.ServiceFileTableLayoutPanel.Name = "ServiceFileTableLayoutPanel";
            this.ServiceFileTableLayoutPanel.RowCount = 2;
            this.ServiceFileTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.ServiceFileTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.ServiceFileTableLayoutPanel.Size = new System.Drawing.Size(752, 57);
            this.ServiceFileTableLayoutPanel.TabIndex = 0;
            // 
            // ServiceFileNameTextBox
            // 
            this.ServiceFileNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServiceFileNameTextBox.Enabled = false;
            this.ServiceFileNameTextBox.Location = new System.Drawing.Point(3, 33);
            this.ServiceFileNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.ServiceFileNameTextBox.Name = "ServiceFileNameTextBox";
            this.ServiceFileNameTextBox.Size = new System.Drawing.Size(746, 20);
            this.ServiceFileNameTextBox.TabIndex = 2;
            // 
            // ServiceFileButtonsTableLayoutPanel
            // 
            this.ServiceFileButtonsTableLayoutPanel.ColumnCount = 2;
            this.ServiceFileButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ServiceFileButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ServiceFileButtonsTableLayoutPanel.Controls.Add(this.StartCreateServiceFileButton, 0, 0);
            this.ServiceFileButtonsTableLayoutPanel.Controls.Add(this.ReceiveServieFileButton, 1, 0);
            this.ServiceFileButtonsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServiceFileButtonsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ServiceFileButtonsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ServiceFileButtonsTableLayoutPanel.Name = "ServiceFileButtonsTableLayoutPanel";
            this.ServiceFileButtonsTableLayoutPanel.RowCount = 1;
            this.ServiceFileButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ServiceFileButtonsTableLayoutPanel.Size = new System.Drawing.Size(752, 29);
            this.ServiceFileButtonsTableLayoutPanel.TabIndex = 4;
            // 
            // StartCreateServiceFileButton
            // 
            this.StartCreateServiceFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartCreateServiceFileButton.Location = new System.Drawing.Point(3, 3);
            this.StartCreateServiceFileButton.Name = "StartCreateServiceFileButton";
            this.StartCreateServiceFileButton.Size = new System.Drawing.Size(370, 23);
            this.StartCreateServiceFileButton.TabIndex = 1;
            this.StartCreateServiceFileButton.Text = "Create Service File";
            this.StartCreateServiceFileButton.UseVisualStyleBackColor = true;
            this.StartCreateServiceFileButton.Click += new System.EventHandler(this.StartCreateServiceFileButton_Click);
            // 
            // ReceiveServieFileButton
            // 
            this.ReceiveServieFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveServieFileButton.Enabled = false;
            this.ReceiveServieFileButton.Location = new System.Drawing.Point(379, 3);
            this.ReceiveServieFileButton.Name = "ReceiveServieFileButton";
            this.ReceiveServieFileButton.Size = new System.Drawing.Size(370, 23);
            this.ReceiveServieFileButton.TabIndex = 3;
            this.ReceiveServieFileButton.Text = "Receive Service File";
            this.ReceiveServieFileButton.UseVisualStyleBackColor = true;
            this.ReceiveServieFileButton.Click += new System.EventHandler(this.ReceiveServieFileButton_Click);
            // 
            // ScreenShotTableLayoutPanel
            // 
            this.ScreenShotTableLayoutPanel.ColumnCount = 1;
            this.ScreenShotTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ScreenShotTableLayoutPanel.Controls.Add(this.PreviewScreenShotPictureBox, 0, 1);
            this.ScreenShotTableLayoutPanel.Controls.Add(this.MakeScreenShotActiontableLayoutPanel, 0, 0);
            this.ScreenShotTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreenShotTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.ScreenShotTableLayoutPanel.Name = "ScreenShotTableLayoutPanel";
            this.ScreenShotTableLayoutPanel.RowCount = 2;
            this.ScreenShotTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.ScreenShotTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ScreenShotTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ScreenShotTableLayoutPanel.Size = new System.Drawing.Size(752, 353);
            this.ScreenShotTableLayoutPanel.TabIndex = 2;
            // 
            // PreviewScreenShotPictureBox
            // 
            this.PreviewScreenShotPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewScreenShotPictureBox.Location = new System.Drawing.Point(3, 32);
            this.PreviewScreenShotPictureBox.Name = "PreviewScreenShotPictureBox";
            this.PreviewScreenShotPictureBox.Size = new System.Drawing.Size(746, 318);
            this.PreviewScreenShotPictureBox.TabIndex = 2;
            this.PreviewScreenShotPictureBox.TabStop = false;
            // 
            // MakeScreenShotActiontableLayoutPanel
            // 
            this.MakeScreenShotActiontableLayoutPanel.ColumnCount = 2;
            this.MakeScreenShotActiontableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MakeScreenShotActiontableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MakeScreenShotActiontableLayoutPanel.Controls.Add(this.ScreenShotFileNameTextBox, 0, 0);
            this.MakeScreenShotActiontableLayoutPanel.Controls.Add(this.MakeScreenShotButton, 0, 0);
            this.MakeScreenShotActiontableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MakeScreenShotActiontableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MakeScreenShotActiontableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MakeScreenShotActiontableLayoutPanel.Name = "MakeScreenShotActiontableLayoutPanel";
            this.MakeScreenShotActiontableLayoutPanel.RowCount = 1;
            this.MakeScreenShotActiontableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MakeScreenShotActiontableLayoutPanel.Size = new System.Drawing.Size(752, 29);
            this.MakeScreenShotActiontableLayoutPanel.TabIndex = 3;
            // 
            // ScreenShotFileNameTextBox
            // 
            this.ScreenShotFileNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreenShotFileNameTextBox.Enabled = false;
            this.ScreenShotFileNameTextBox.Location = new System.Drawing.Point(379, 4);
            this.ScreenShotFileNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.ScreenShotFileNameTextBox.Name = "ScreenShotFileNameTextBox";
            this.ScreenShotFileNameTextBox.Size = new System.Drawing.Size(370, 20);
            this.ScreenShotFileNameTextBox.TabIndex = 2;
            // 
            // MakeScreenShotButton
            // 
            this.MakeScreenShotButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MakeScreenShotButton.Location = new System.Drawing.Point(3, 3);
            this.MakeScreenShotButton.Name = "MakeScreenShotButton";
            this.MakeScreenShotButton.Size = new System.Drawing.Size(370, 23);
            this.MakeScreenShotButton.TabIndex = 1;
            this.MakeScreenShotButton.Text = "Make Screen Shot (auto receive)";
            this.MakeScreenShotButton.UseVisualStyleBackColor = true;
            this.MakeScreenShotButton.Click += new System.EventHandler(this.MakeScreenShotButton_Click);
            // 
            // ScreenShotGroupBox
            // 
            this.ScreenShotGroupBox.Controls.Add(this.ScreenShotTableLayoutPanel);
            this.ScreenShotGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreenShotGroupBox.Location = new System.Drawing.Point(3, 85);
            this.ScreenShotGroupBox.Name = "ScreenShotGroupBox";
            this.ScreenShotGroupBox.Size = new System.Drawing.Size(758, 372);
            this.ScreenShotGroupBox.TabIndex = 3;
            this.ScreenShotGroupBox.TabStop = false;
            this.ScreenShotGroupBox.Text = "Screen Shot";
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.RightSideTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.TabIndex = 4;
            // 
            // RightSideTableLayoutPanel
            // 
            this.RightSideTableLayoutPanel.ColumnCount = 1;
            this.RightSideTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RightSideTableLayoutPanel.Controls.Add(this.ScreenShotGroupBox, 0, 1);
            this.RightSideTableLayoutPanel.Controls.Add(this.ServiceFileGroupBox, 0, 0);
            this.RightSideTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightSideTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.RightSideTableLayoutPanel.Name = "RightSideTableLayoutPanel";
            this.RightSideTableLayoutPanel.RowCount = 2;
            this.RightSideTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.RightSideTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RightSideTableLayoutPanel.Size = new System.Drawing.Size(764, 460);
            this.RightSideTableLayoutPanel.TabIndex = 1;
            // 
            // Diagnostics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "Diagnostics";
            this.Size = new System.Drawing.Size(770, 466);
            this.Load += new System.EventHandler(this.Diagnostics_Load);
            this.ServiceFileGroupBox.ResumeLayout(false);
            this.ServiceFileTableLayoutPanel.ResumeLayout(false);
            this.ServiceFileTableLayoutPanel.PerformLayout();
            this.ServiceFileButtonsTableLayoutPanel.ResumeLayout(false);
            this.ScreenShotTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewScreenShotPictureBox)).EndInit();
            this.MakeScreenShotActiontableLayoutPanel.ResumeLayout(false);
            this.MakeScreenShotActiontableLayoutPanel.PerformLayout();
            this.ScreenShotGroupBox.ResumeLayout(false);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.RightSideTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox ServiceFileGroupBox;
        private System.Windows.Forms.TableLayoutPanel ServiceFileTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ScreenShotTableLayoutPanel;
        private System.Windows.Forms.TextBox ServiceFileNameTextBox;
        private System.Windows.Forms.PictureBox PreviewScreenShotPictureBox;
        private System.Windows.Forms.Button ReceiveServieFileButton;
        private System.Windows.Forms.GroupBox ScreenShotGroupBox;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel RightSideTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ServiceFileButtonsTableLayoutPanel;
        private System.Windows.Forms.Button StartCreateServiceFileButton;
        private System.Windows.Forms.TableLayoutPanel MakeScreenShotActiontableLayoutPanel;
        private System.Windows.Forms.TextBox ScreenShotFileNameTextBox;
        private System.Windows.Forms.Button MakeScreenShotButton;


    }
}
