namespace DNC_CSharp_Demo.UserControls
{
    partial class DncInterface
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

            // All backgroung workers and threads hat use Invoke or BeginInvoke to write to the gui
            // has to be stopped bevore the gui is disposed.
            this.Stop();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DncInterface));
            this.ThreadingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ThreadAGroupBox = new System.Windows.Forms.GroupBox();
            this.ThreadATableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ReadToolTableButton = new System.Windows.Forms.Button();
            this.ThreadAStateLabel = new System.Windows.Forms.Label();
            this.ToolTableGroupBox = new System.Windows.Forms.GroupBox();
            this.ToolTableListView = new System.Windows.Forms.ListView();
            this.DataAccessGoupBox = new System.Windows.Forms.GroupBox();
            this.ThreadALoggingTextBox = new System.Windows.Forms.TextBox();
            this.ThreadBGroupBox = new System.Windows.Forms.GroupBox();
            this.ThreadBTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PollingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StartPollingButton = new System.Windows.Forms.Button();
            this.StopPollingButton = new System.Windows.Forms.Button();
            this.LogPollingGroupBox = new System.Windows.Forms.GroupBox();
            this.ThreadBLoggingTextBox = new System.Windows.Forms.TextBox();
            this.ThreadBStateLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.ThreadingTableLayoutPanel.SuspendLayout();
            this.ThreadAGroupBox.SuspendLayout();
            this.ThreadATableLayoutPanel.SuspendLayout();
            this.ToolTableGroupBox.SuspendLayout();
            this.DataAccessGoupBox.SuspendLayout();
            this.ThreadBGroupBox.SuspendLayout();
            this.ThreadBTableLayoutPanel.SuspendLayout();
            this.PollingTableLayoutPanel.SuspendLayout();
            this.LogPollingGroupBox.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ThreadingTableLayoutPanel
            // 
            this.ThreadingTableLayoutPanel.ColumnCount = 2;
            this.ThreadingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ThreadingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ThreadingTableLayoutPanel.Controls.Add(this.ThreadAGroupBox, 0, 0);
            this.ThreadingTableLayoutPanel.Controls.Add(this.ThreadBGroupBox, 1, 0);
            this.ThreadingTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadingTableLayoutPanel.Location = new System.Drawing.Point(3, 83);
            this.ThreadingTableLayoutPanel.Name = "ThreadingTableLayoutPanel";
            this.ThreadingTableLayoutPanel.RowCount = 1;
            this.ThreadingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ThreadingTableLayoutPanel.Size = new System.Drawing.Size(794, 514);
            this.ThreadingTableLayoutPanel.TabIndex = 0;
            // 
            // ThreadAGroupBox
            // 
            this.ThreadAGroupBox.Controls.Add(this.ThreadATableLayoutPanel);
            this.ThreadAGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadAGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ThreadAGroupBox.Name = "ThreadAGroupBox";
            this.ThreadAGroupBox.Size = new System.Drawing.Size(391, 508);
            this.ThreadAGroupBox.TabIndex = 0;
            this.ThreadAGroupBox.TabStop = false;
            this.ThreadAGroupBox.Text = "Thread A";
            // 
            // ThreadATableLayoutPanel
            // 
            this.ThreadATableLayoutPanel.ColumnCount = 1;
            this.ThreadATableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ThreadATableLayoutPanel.Controls.Add(this.ReadToolTableButton, 0, 1);
            this.ThreadATableLayoutPanel.Controls.Add(this.ThreadAStateLabel, 0, 0);
            this.ThreadATableLayoutPanel.Controls.Add(this.ToolTableGroupBox, 0, 2);
            this.ThreadATableLayoutPanel.Controls.Add(this.DataAccessGoupBox, 0, 3);
            this.ThreadATableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadATableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.ThreadATableLayoutPanel.Name = "ThreadATableLayoutPanel";
            this.ThreadATableLayoutPanel.RowCount = 4;
            this.ThreadATableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ThreadATableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ThreadATableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ThreadATableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ThreadATableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ThreadATableLayoutPanel.Size = new System.Drawing.Size(385, 489);
            this.ThreadATableLayoutPanel.TabIndex = 0;
            // 
            // ReadToolTableButton
            // 
            this.ReadToolTableButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReadToolTableButton.Location = new System.Drawing.Point(3, 23);
            this.ReadToolTableButton.Name = "ReadToolTableButton";
            this.ReadToolTableButton.Size = new System.Drawing.Size(379, 34);
            this.ReadToolTableButton.TabIndex = 0;
            this.ReadToolTableButton.Text = "Read tool table at once";
            this.ReadToolTableButton.UseVisualStyleBackColor = true;
            this.ReadToolTableButton.Click += new System.EventHandler(this.ReadToolTableButton_Click);
            // 
            // ThreadAStateLabel
            // 
            this.ThreadAStateLabel.BackColor = System.Drawing.Color.Yellow;
            this.ThreadAStateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadAStateLabel.Location = new System.Drawing.Point(3, 0);
            this.ThreadAStateLabel.Name = "ThreadAStateLabel";
            this.ThreadAStateLabel.Size = new System.Drawing.Size(379, 20);
            this.ThreadAStateLabel.TabIndex = 3;
            this.ThreadAStateLabel.Text = "stopped ...";
            this.ThreadAStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolTableGroupBox
            // 
            this.ToolTableGroupBox.Controls.Add(this.ToolTableListView);
            this.ToolTableGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolTableGroupBox.Location = new System.Drawing.Point(3, 63);
            this.ToolTableGroupBox.Name = "ToolTableGroupBox";
            this.ToolTableGroupBox.Size = new System.Drawing.Size(379, 323);
            this.ToolTableGroupBox.TabIndex = 4;
            this.ToolTableGroupBox.TabStop = false;
            this.ToolTableGroupBox.Text = "Tool table";
            // 
            // ToolTableListView
            // 
            this.ToolTableListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolTableListView.FullRowSelect = true;
            this.ToolTableListView.GridLines = true;
            this.ToolTableListView.Location = new System.Drawing.Point(3, 16);
            this.ToolTableListView.Name = "ToolTableListView";
            this.ToolTableListView.Size = new System.Drawing.Size(373, 304);
            this.ToolTableListView.TabIndex = 0;
            this.ToolTableListView.UseCompatibleStateImageBehavior = false;
            this.ToolTableListView.View = System.Windows.Forms.View.Details;
            // 
            // DataAccessGoupBox
            // 
            this.DataAccessGoupBox.Controls.Add(this.ThreadALoggingTextBox);
            this.DataAccessGoupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataAccessGoupBox.Location = new System.Drawing.Point(3, 392);
            this.DataAccessGoupBox.Name = "DataAccessGoupBox";
            this.DataAccessGoupBox.Size = new System.Drawing.Size(379, 94);
            this.DataAccessGoupBox.TabIndex = 7;
            this.DataAccessGoupBox.TabStop = false;
            this.DataAccessGoupBox.Text = "Thread A logging...";
            // 
            // ThreadALoggingTextBox
            // 
            this.ThreadALoggingTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadALoggingTextBox.Location = new System.Drawing.Point(3, 16);
            this.ThreadALoggingTextBox.Multiline = true;
            this.ThreadALoggingTextBox.Name = "ThreadALoggingTextBox";
            this.ThreadALoggingTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ThreadALoggingTextBox.Size = new System.Drawing.Size(373, 75);
            this.ThreadALoggingTextBox.TabIndex = 0;
            // 
            // ThreadBGroupBox
            // 
            this.ThreadBGroupBox.Controls.Add(this.ThreadBTableLayoutPanel);
            this.ThreadBGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadBGroupBox.Location = new System.Drawing.Point(400, 3);
            this.ThreadBGroupBox.Name = "ThreadBGroupBox";
            this.ThreadBGroupBox.Size = new System.Drawing.Size(391, 508);
            this.ThreadBGroupBox.TabIndex = 1;
            this.ThreadBGroupBox.TabStop = false;
            this.ThreadBGroupBox.Text = "Thread B";
            // 
            // ThreadBTableLayoutPanel
            // 
            this.ThreadBTableLayoutPanel.ColumnCount = 1;
            this.ThreadBTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ThreadBTableLayoutPanel.Controls.Add(this.PollingTableLayoutPanel, 0, 1);
            this.ThreadBTableLayoutPanel.Controls.Add(this.LogPollingGroupBox, 0, 2);
            this.ThreadBTableLayoutPanel.Controls.Add(this.ThreadBStateLabel, 0, 0);
            this.ThreadBTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadBTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.ThreadBTableLayoutPanel.Name = "ThreadBTableLayoutPanel";
            this.ThreadBTableLayoutPanel.RowCount = 3;
            this.ThreadBTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ThreadBTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ThreadBTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ThreadBTableLayoutPanel.Size = new System.Drawing.Size(385, 489);
            this.ThreadBTableLayoutPanel.TabIndex = 0;
            // 
            // PollingTableLayoutPanel
            // 
            this.PollingTableLayoutPanel.ColumnCount = 2;
            this.PollingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PollingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PollingTableLayoutPanel.Controls.Add(this.StartPollingButton, 0, 0);
            this.PollingTableLayoutPanel.Controls.Add(this.StopPollingButton, 1, 0);
            this.PollingTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PollingTableLayoutPanel.Location = new System.Drawing.Point(3, 23);
            this.PollingTableLayoutPanel.Name = "PollingTableLayoutPanel";
            this.PollingTableLayoutPanel.RowCount = 1;
            this.PollingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PollingTableLayoutPanel.Size = new System.Drawing.Size(379, 34);
            this.PollingTableLayoutPanel.TabIndex = 0;
            // 
            // StartPollingButton
            // 
            this.StartPollingButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartPollingButton.Location = new System.Drawing.Point(3, 3);
            this.StartPollingButton.Name = "StartPollingButton";
            this.StartPollingButton.Size = new System.Drawing.Size(183, 28);
            this.StartPollingButton.TabIndex = 0;
            this.StartPollingButton.Text = "Start polling";
            this.StartPollingButton.UseVisualStyleBackColor = true;
            this.StartPollingButton.Click += new System.EventHandler(this.StartPollingButton_Click);
            // 
            // StopPollingButton
            // 
            this.StopPollingButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StopPollingButton.Enabled = false;
            this.StopPollingButton.Location = new System.Drawing.Point(192, 3);
            this.StopPollingButton.Name = "StopPollingButton";
            this.StopPollingButton.Size = new System.Drawing.Size(184, 28);
            this.StopPollingButton.TabIndex = 1;
            this.StopPollingButton.Text = "Stop polling";
            this.StopPollingButton.UseVisualStyleBackColor = true;
            this.StopPollingButton.Click += new System.EventHandler(this.StopPollingButton_Click);
            // 
            // LogPollingGroupBox
            // 
            this.LogPollingGroupBox.Controls.Add(this.ThreadBLoggingTextBox);
            this.LogPollingGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogPollingGroupBox.Location = new System.Drawing.Point(3, 63);
            this.LogPollingGroupBox.Name = "LogPollingGroupBox";
            this.LogPollingGroupBox.Size = new System.Drawing.Size(379, 423);
            this.LogPollingGroupBox.TabIndex = 5;
            this.LogPollingGroupBox.TabStop = false;
            this.LogPollingGroupBox.Text = "Thread B logging...";
            // 
            // ThreadBLoggingTextBox
            // 
            this.ThreadBLoggingTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadBLoggingTextBox.Location = new System.Drawing.Point(3, 16);
            this.ThreadBLoggingTextBox.Multiline = true;
            this.ThreadBLoggingTextBox.Name = "ThreadBLoggingTextBox";
            this.ThreadBLoggingTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ThreadBLoggingTextBox.Size = new System.Drawing.Size(373, 404);
            this.ThreadBLoggingTextBox.TabIndex = 0;
            // 
            // ThreadBStateLabel
            // 
            this.ThreadBStateLabel.BackColor = System.Drawing.Color.Yellow;
            this.ThreadBStateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadBStateLabel.Location = new System.Drawing.Point(3, 0);
            this.ThreadBStateLabel.Name = "ThreadBStateLabel";
            this.ThreadBStateLabel.Size = new System.Drawing.Size(379, 20);
            this.ThreadBStateLabel.TabIndex = 6;
            this.ThreadBStateLabel.Text = "stopped...";
            this.ThreadBStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.ThreadingTableLayoutPanel, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.InfoLabel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(800, 600);
            this.MainTableLayoutPanel.TabIndex = 1;
            // 
            // InfoLabel
            // 
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.Location = new System.Drawing.Point(3, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(794, 80);
            this.InfoLabel.TabIndex = 1;
            this.InfoLabel.Text = resources.GetString("InfoLabel.Text");
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DncInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "DncInterface";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.DncInterface_Load);
            this.ThreadingTableLayoutPanel.ResumeLayout(false);
            this.ThreadAGroupBox.ResumeLayout(false);
            this.ThreadATableLayoutPanel.ResumeLayout(false);
            this.ToolTableGroupBox.ResumeLayout(false);
            this.DataAccessGoupBox.ResumeLayout(false);
            this.DataAccessGoupBox.PerformLayout();
            this.ThreadBGroupBox.ResumeLayout(false);
            this.ThreadBTableLayoutPanel.ResumeLayout(false);
            this.PollingTableLayoutPanel.ResumeLayout(false);
            this.LogPollingGroupBox.ResumeLayout(false);
            this.LogPollingGroupBox.PerformLayout();
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ThreadingTableLayoutPanel;
        private System.Windows.Forms.GroupBox ThreadAGroupBox;
        private System.Windows.Forms.TableLayoutPanel ThreadATableLayoutPanel;
        private System.Windows.Forms.Button ReadToolTableButton;
        private System.Windows.Forms.GroupBox ThreadBGroupBox;
        private System.Windows.Forms.TableLayoutPanel ThreadBTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel PollingTableLayoutPanel;
        private System.Windows.Forms.Button StartPollingButton;
        private System.Windows.Forms.Button StopPollingButton;
        private System.Windows.Forms.GroupBox LogPollingGroupBox;
        private System.Windows.Forms.TextBox ThreadBLoggingTextBox;
        private System.Windows.Forms.Label ThreadAStateLabel;
        private System.Windows.Forms.Label ThreadBStateLabel;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.GroupBox ToolTableGroupBox;
        private System.Windows.Forms.ListView ToolTableListView;
        private System.Windows.Forms.GroupBox DataAccessGoupBox;
        private System.Windows.Forms.TextBox ThreadALoggingTextBox;




    }
}
