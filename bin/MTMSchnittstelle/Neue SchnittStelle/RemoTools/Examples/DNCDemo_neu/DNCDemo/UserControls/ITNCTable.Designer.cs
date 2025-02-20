namespace DNC_CSharp_Demo.UserControls
{
    partial class ITNCTable
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
            this.ToolTableGroupBox = new System.Windows.Forms.GroupBox();
            this.ToolTableListView = new System.Windows.Forms.ListView();
            this.ControlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TransmitTablePartGroupBox = new System.Windows.Forms.GroupBox();
            this.TransmitTablePartTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TransmitFromFileButton = new System.Windows.Forms.Button();
            this.TransmitFromListViewButton = new System.Windows.Forms.Button();
            this.QuerryGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.QuerryTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ReceiveTableLinesButton = new System.Windows.Forms.Button();
            this.DeleteRecordButton = new System.Windows.Forms.Button();
            this.QuerryComboBox = new System.Windows.Forms.ComboBox();
            this.MainTableLayoutPanel.SuspendLayout();
            this.ToolTableGroupBox.SuspendLayout();
            this.ControlTableLayoutPanel.SuspendLayout();
            this.TransmitTablePartGroupBox.SuspendLayout();
            this.TransmitTablePartTableLayoutPanel.SuspendLayout();
            this.QuerryGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.QuerryTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.ToolTableGroupBox, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.ControlTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(680, 350);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // ToolTableGroupBox
            // 
            this.ToolTableGroupBox.Controls.Add(this.ToolTableListView);
            this.ToolTableGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolTableGroupBox.Location = new System.Drawing.Point(3, 103);
            this.ToolTableGroupBox.Name = "ToolTableGroupBox";
            this.ToolTableGroupBox.Size = new System.Drawing.Size(674, 244);
            this.ToolTableGroupBox.TabIndex = 0;
            this.ToolTableGroupBox.TabStop = false;
            this.ToolTableGroupBox.Text = "Tool Table";
            // 
            // ToolTableListView
            // 
            this.ToolTableListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolTableListView.FullRowSelect = true;
            this.ToolTableListView.GridLines = true;
            this.ToolTableListView.LabelEdit = true;
            this.ToolTableListView.Location = new System.Drawing.Point(3, 16);
            this.ToolTableListView.MultiSelect = false;
            this.ToolTableListView.Name = "ToolTableListView";
            this.ToolTableListView.Size = new System.Drawing.Size(668, 225);
            this.ToolTableListView.TabIndex = 0;
            this.ToolTableListView.UseCompatibleStateImageBehavior = false;
            this.ToolTableListView.View = System.Windows.Forms.View.Details;
            // 
            // ControlTableLayoutPanel
            // 
            this.ControlTableLayoutPanel.ColumnCount = 2;
            this.ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.ControlTableLayoutPanel.Controls.Add(this.TransmitTablePartGroupBox, 0, 0);
            this.ControlTableLayoutPanel.Controls.Add(this.QuerryGroupBox, 1, 0);
            this.ControlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ControlTableLayoutPanel.Name = "ControlTableLayoutPanel";
            this.ControlTableLayoutPanel.RowCount = 1;
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ControlTableLayoutPanel.Size = new System.Drawing.Size(674, 94);
            this.ControlTableLayoutPanel.TabIndex = 1;
            // 
            // TransmitTablePartGroupBox
            // 
            this.TransmitTablePartGroupBox.Controls.Add(this.TransmitTablePartTableLayoutPanel);
            this.TransmitTablePartGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TransmitTablePartGroupBox.Location = new System.Drawing.Point(3, 3);
            this.TransmitTablePartGroupBox.Name = "TransmitTablePartGroupBox";
            this.TransmitTablePartGroupBox.Size = new System.Drawing.Size(196, 88);
            this.TransmitTablePartGroupBox.TabIndex = 0;
            this.TransmitTablePartGroupBox.TabStop = false;
            this.TransmitTablePartGroupBox.Text = "TransmitTablePart()";
            // 
            // TransmitTablePartTableLayoutPanel
            // 
            this.TransmitTablePartTableLayoutPanel.ColumnCount = 1;
            this.TransmitTablePartTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TransmitTablePartTableLayoutPanel.Controls.Add(this.TransmitFromFileButton, 0, 0);
            this.TransmitTablePartTableLayoutPanel.Controls.Add(this.TransmitFromListViewButton, 0, 1);
            this.TransmitTablePartTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TransmitTablePartTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.TransmitTablePartTableLayoutPanel.Name = "TransmitTablePartTableLayoutPanel";
            this.TransmitTablePartTableLayoutPanel.RowCount = 2;
            this.TransmitTablePartTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TransmitTablePartTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TransmitTablePartTableLayoutPanel.Size = new System.Drawing.Size(190, 69);
            this.TransmitTablePartTableLayoutPanel.TabIndex = 0;
            // 
            // TransmitFromFileButton
            // 
            this.TransmitFromFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TransmitFromFileButton.Location = new System.Drawing.Point(3, 3);
            this.TransmitFromFileButton.Name = "TransmitFromFileButton";
            this.TransmitFromFileButton.Size = new System.Drawing.Size(184, 28);
            this.TransmitFromFileButton.TabIndex = 0;
            this.TransmitFromFileButton.Text = "Transmit Table Part From File";
            this.TransmitFromFileButton.UseVisualStyleBackColor = true;
            this.TransmitFromFileButton.Click += new System.EventHandler(this.TransmitFromFileButton_Click);
            // 
            // TransmitFromListViewButton
            // 
            this.TransmitFromListViewButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TransmitFromListViewButton.Location = new System.Drawing.Point(3, 37);
            this.TransmitFromListViewButton.Name = "TransmitFromListViewButton";
            this.TransmitFromListViewButton.Size = new System.Drawing.Size(184, 29);
            this.TransmitFromListViewButton.TabIndex = 1;
            this.TransmitFromListViewButton.Text = "Transmit Tool Table Below";
            this.TransmitFromListViewButton.UseVisualStyleBackColor = true;
            this.TransmitFromListViewButton.Click += new System.EventHandler(this.TransmitFromListViewButton_Click);
            // 
            // QuerryGroupBox
            // 
            this.QuerryGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.QuerryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuerryGroupBox.Location = new System.Drawing.Point(205, 3);
            this.QuerryGroupBox.Name = "QuerryGroupBox";
            this.QuerryGroupBox.Size = new System.Drawing.Size(466, 88);
            this.QuerryGroupBox.TabIndex = 1;
            this.QuerryGroupBox.TabStop = false;
            this.QuerryGroupBox.Text = "Querry";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.QuerryTableLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.QuerryComboBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 69);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // QuerryTableLayoutPanel
            // 
            this.QuerryTableLayoutPanel.ColumnCount = 2;
            this.QuerryTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.QuerryTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.QuerryTableLayoutPanel.Controls.Add(this.ReceiveTableLinesButton, 0, 0);
            this.QuerryTableLayoutPanel.Controls.Add(this.DeleteRecordButton, 1, 0);
            this.QuerryTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuerryTableLayoutPanel.Location = new System.Drawing.Point(0, 3);
            this.QuerryTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.QuerryTableLayoutPanel.Name = "QuerryTableLayoutPanel";
            this.QuerryTableLayoutPanel.RowCount = 1;
            this.QuerryTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.QuerryTableLayoutPanel.Size = new System.Drawing.Size(460, 34);
            this.QuerryTableLayoutPanel.TabIndex = 0;
            // 
            // ReceiveTableLinesButton
            // 
            this.ReceiveTableLinesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveTableLinesButton.Location = new System.Drawing.Point(3, 3);
            this.ReceiveTableLinesButton.Name = "ReceiveTableLinesButton";
            this.ReceiveTableLinesButton.Size = new System.Drawing.Size(224, 28);
            this.ReceiveTableLinesButton.TabIndex = 0;
            this.ReceiveTableLinesButton.Text = "Receive Table Lines";
            this.ReceiveTableLinesButton.UseVisualStyleBackColor = true;
            this.ReceiveTableLinesButton.Click += new System.EventHandler(this.ReceiveTableLinesButton_Click);
            // 
            // DeleteRecordButton
            // 
            this.DeleteRecordButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteRecordButton.Location = new System.Drawing.Point(233, 3);
            this.DeleteRecordButton.Name = "DeleteRecordButton";
            this.DeleteRecordButton.Size = new System.Drawing.Size(224, 28);
            this.DeleteRecordButton.TabIndex = 1;
            this.DeleteRecordButton.Text = "Delete Record";
            this.DeleteRecordButton.UseVisualStyleBackColor = true;
            this.DeleteRecordButton.Click += new System.EventHandler(this.DeleteRecordButton_Click);
            // 
            // QuerryComboBox
            // 
            this.QuerryComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuerryComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuerryComboBox.FormattingEnabled = true;
            this.QuerryComboBox.Location = new System.Drawing.Point(3, 43);
            this.QuerryComboBox.Name = "QuerryComboBox";
            this.QuerryComboBox.Size = new System.Drawing.Size(454, 23);
            this.QuerryComboBox.TabIndex = 1;
            // 
            // ITNCTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "ITNCTable";
            this.Size = new System.Drawing.Size(680, 350);
            this.Load += new System.EventHandler(this.ITNCTable_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ToolTableGroupBox.ResumeLayout(false);
            this.ControlTableLayoutPanel.ResumeLayout(false);
            this.TransmitTablePartGroupBox.ResumeLayout(false);
            this.TransmitTablePartTableLayoutPanel.ResumeLayout(false);
            this.QuerryGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.QuerryTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.GroupBox ToolTableGroupBox;
        private System.Windows.Forms.ListView ToolTableListView;
        private System.Windows.Forms.TableLayoutPanel ControlTableLayoutPanel;
        private System.Windows.Forms.GroupBox TransmitTablePartGroupBox;
        private System.Windows.Forms.TableLayoutPanel TransmitTablePartTableLayoutPanel;
        private System.Windows.Forms.Button TransmitFromFileButton;
        private System.Windows.Forms.Button TransmitFromListViewButton;
        private System.Windows.Forms.GroupBox QuerryGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel QuerryTableLayoutPanel;
        private System.Windows.Forms.Button ReceiveTableLinesButton;
        private System.Windows.Forms.Button DeleteRecordButton;
        private System.Windows.Forms.ComboBox QuerryComboBox;

    }
}
