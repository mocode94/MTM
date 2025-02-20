namespace DNC_CSharp_Demo.UserControls
{
    partial class DataAccess2
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
            this.SubscriptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.SubscribeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SubscriptionsListView = new System.Windows.Forms.ListView();
            this.HandleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataSelectionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeStampColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChildPropertySplitContainer = new System.Windows.Forms.SplitContainer();
            this.PropertyGroupBox = new System.Windows.Forms.GroupBox();
            this.PropertyTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.VarValueTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.VarValueTextBox = new System.Windows.Forms.TextBox();
            this.SetDataEntryButton = new System.Windows.Forms.Button();
            this.VarValueLabel = new System.Windows.Forms.Label();
            this.IsReadOnlyLabel = new System.Windows.Forms.Label();
            this.PropertyListListView = new System.Windows.Forms.ListView();
            this.PropKindColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VarValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VarValueTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChildGroupBox = new System.Windows.Forms.GroupBox();
            this.ChildListListBox = new System.Windows.Forms.ListBox();
            this.DataAccessInterfaceInfoLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SubscriptionsGroupBox.SuspendLayout();
            this.SubscribeTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChildPropertySplitContainer)).BeginInit();
            this.ChildPropertySplitContainer.Panel1.SuspendLayout();
            this.ChildPropertySplitContainer.Panel2.SuspendLayout();
            this.ChildPropertySplitContainer.SuspendLayout();
            this.PropertyGroupBox.SuspendLayout();
            this.PropertyTableLayoutPanel.SuspendLayout();
            this.VarValueTableLayoutPanel.SuspendLayout();
            this.ChildGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.SubscriptionsGroupBox, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.ChildPropertySplitContainer, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.DataAccessInterfaceInfoLabel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 3;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.31683F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.68317F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(782, 606);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // SubscriptionsGroupBox
            // 
            this.SubscriptionsGroupBox.Controls.Add(this.SubscribeTableLayoutPanel);
            this.SubscriptionsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubscriptionsGroupBox.Location = new System.Drawing.Point(3, 423);
            this.SubscriptionsGroupBox.Name = "SubscriptionsGroupBox";
            this.SubscriptionsGroupBox.Size = new System.Drawing.Size(776, 180);
            this.SubscriptionsGroupBox.TabIndex = 5;
            this.SubscriptionsGroupBox.TabStop = false;
            this.SubscriptionsGroupBox.Text = "Subscriptions (right click to unsubscribe)";
            // 
            // SubscribeTableLayoutPanel
            // 
            this.SubscribeTableLayoutPanel.ColumnCount = 1;
            this.SubscribeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubscribeTableLayoutPanel.Controls.Add(this.SubscriptionsListView, 0, 0);
            this.SubscribeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubscribeTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.SubscribeTableLayoutPanel.Name = "SubscribeTableLayoutPanel";
            this.SubscribeTableLayoutPanel.RowCount = 1;
            this.SubscribeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubscribeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.SubscribeTableLayoutPanel.Size = new System.Drawing.Size(770, 161);
            this.SubscribeTableLayoutPanel.TabIndex = 0;
            // 
            // SubscriptionsListView
            // 
            this.SubscriptionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.HandleColumnHeader,
            this.DataSelectionColumnHeader,
            this.DataColumnHeader,
            this.TimeStampColumnHeader});
            this.SubscriptionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubscriptionsListView.FullRowSelect = true;
            this.SubscriptionsListView.GridLines = true;
            this.SubscriptionsListView.HideSelection = false;
            this.SubscriptionsListView.Location = new System.Drawing.Point(3, 3);
            this.SubscriptionsListView.Name = "SubscriptionsListView";
            this.SubscriptionsListView.Size = new System.Drawing.Size(764, 155);
            this.SubscriptionsListView.TabIndex = 0;
            this.SubscriptionsListView.UseCompatibleStateImageBehavior = false;
            this.SubscriptionsListView.View = System.Windows.Forms.View.Details;
            this.SubscriptionsListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SubscriptionsListView_MouseClick);
            // 
            // HandleColumnHeader
            // 
            this.HandleColumnHeader.Text = "Handle";
            // 
            // DataSelectionColumnHeader
            // 
            this.DataSelectionColumnHeader.Text = "DataSelection";
            // 
            // DataColumnHeader
            // 
            this.DataColumnHeader.Text = "Data";
            // 
            // TimeStampColumnHeader
            // 
            this.TimeStampColumnHeader.Text = "TimeStamp";
            // 
            // ChildPropertySplitContainer
            // 
            this.ChildPropertySplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ChildPropertySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildPropertySplitContainer.Location = new System.Drawing.Point(3, 23);
            this.ChildPropertySplitContainer.Name = "ChildPropertySplitContainer";
            this.ChildPropertySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ChildPropertySplitContainer.Panel1
            // 
            this.ChildPropertySplitContainer.Panel1.Controls.Add(this.PropertyGroupBox);
            // 
            // ChildPropertySplitContainer.Panel2
            // 
            this.ChildPropertySplitContainer.Panel2.Controls.Add(this.ChildGroupBox);
            this.ChildPropertySplitContainer.Size = new System.Drawing.Size(776, 394);
            this.ChildPropertySplitContainer.SplitterDistance = 200;
            this.ChildPropertySplitContainer.TabIndex = 0;
            // 
            // PropertyGroupBox
            // 
            this.PropertyGroupBox.Controls.Add(this.PropertyTableLayoutPanel);
            this.PropertyGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGroupBox.Location = new System.Drawing.Point(0, 0);
            this.PropertyGroupBox.Name = "PropertyGroupBox";
            this.PropertyGroupBox.Size = new System.Drawing.Size(772, 196);
            this.PropertyGroupBox.TabIndex = 4;
            this.PropertyGroupBox.TabStop = false;
            this.PropertyGroupBox.Text = "PropertyList";
            // 
            // PropertyTableLayoutPanel
            // 
            this.PropertyTableLayoutPanel.ColumnCount = 1;
            this.PropertyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PropertyTableLayoutPanel.Controls.Add(this.VarValueTableLayoutPanel, 0, 0);
            this.PropertyTableLayoutPanel.Controls.Add(this.PropertyListListView, 0, 1);
            this.PropertyTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.PropertyTableLayoutPanel.Name = "PropertyTableLayoutPanel";
            this.PropertyTableLayoutPanel.RowCount = 2;
            this.PropertyTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.PropertyTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PropertyTableLayoutPanel.Size = new System.Drawing.Size(766, 177);
            this.PropertyTableLayoutPanel.TabIndex = 0;
            // 
            // VarValueTableLayoutPanel
            // 
            this.VarValueTableLayoutPanel.ColumnCount = 4;
            this.VarValueTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.VarValueTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VarValueTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.VarValueTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.VarValueTableLayoutPanel.Controls.Add(this.VarValueTextBox, 1, 0);
            this.VarValueTableLayoutPanel.Controls.Add(this.SetDataEntryButton, 3, 0);
            this.VarValueTableLayoutPanel.Controls.Add(this.VarValueLabel, 0, 0);
            this.VarValueTableLayoutPanel.Controls.Add(this.IsReadOnlyLabel, 2, 0);
            this.VarValueTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VarValueTableLayoutPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VarValueTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.VarValueTableLayoutPanel.Name = "VarValueTableLayoutPanel";
            this.VarValueTableLayoutPanel.RowCount = 1;
            this.VarValueTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VarValueTableLayoutPanel.Size = new System.Drawing.Size(760, 31);
            this.VarValueTableLayoutPanel.TabIndex = 0;
            // 
            // VarValueTextBox
            // 
            this.VarValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VarValueTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VarValueTextBox.Location = new System.Drawing.Point(88, 4);
            this.VarValueTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.VarValueTextBox.Name = "VarValueTextBox";
            this.VarValueTextBox.Size = new System.Drawing.Size(449, 22);
            this.VarValueTextBox.TabIndex = 5;
            // 
            // SetDataEntryButton
            // 
            this.SetDataEntryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetDataEntryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetDataEntryButton.Location = new System.Drawing.Point(643, 3);
            this.SetDataEntryButton.Name = "SetDataEntryButton";
            this.SetDataEntryButton.Size = new System.Drawing.Size(114, 25);
            this.SetDataEntryButton.TabIndex = 4;
            this.SetDataEntryButton.Text = "SetDataEntry";
            this.SetDataEntryButton.UseVisualStyleBackColor = true;
            this.SetDataEntryButton.Click += new System.EventHandler(this.SetDataEntryButton_Click);
            // 
            // VarValueLabel
            // 
            this.VarValueLabel.Location = new System.Drawing.Point(3, 0);
            this.VarValueLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.VarValueLabel.Name = "VarValueLabel";
            this.VarValueLabel.Size = new System.Drawing.Size(79, 28);
            this.VarValueLabel.TabIndex = 6;
            this.VarValueLabel.Text = "VarValue:";
            this.VarValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IsReadOnlyLabel
            // 
            this.IsReadOnlyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IsReadOnlyLabel.Location = new System.Drawing.Point(545, 5);
            this.IsReadOnlyLabel.Margin = new System.Windows.Forms.Padding(5);
            this.IsReadOnlyLabel.Name = "IsReadOnlyLabel";
            this.IsReadOnlyLabel.Size = new System.Drawing.Size(90, 21);
            this.IsReadOnlyLabel.TabIndex = 7;
            this.IsReadOnlyLabel.Text = "R/W";
            this.IsReadOnlyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PropertyListListView
            // 
            this.PropertyListListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PropKindColumnHeader,
            this.VarValueColumnHeader,
            this.VarValueTypeColumnHeader});
            this.PropertyListListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyListListView.GridLines = true;
            this.PropertyListListView.HideSelection = false;
            this.PropertyListListView.Location = new System.Drawing.Point(3, 40);
            this.PropertyListListView.Name = "PropertyListListView";
            this.PropertyListListView.ShowGroups = false;
            this.PropertyListListView.Size = new System.Drawing.Size(760, 134);
            this.PropertyListListView.TabIndex = 1;
            this.PropertyListListView.UseCompatibleStateImageBehavior = false;
            this.PropertyListListView.View = System.Windows.Forms.View.Details;
            // 
            // PropKindColumnHeader
            // 
            this.PropKindColumnHeader.Text = "PropKind";
            this.PropKindColumnHeader.Width = 300;
            // 
            // VarValueColumnHeader
            // 
            this.VarValueColumnHeader.Text = "VarValue";
            this.VarValueColumnHeader.Width = 200;
            // 
            // VarValueTypeColumnHeader
            // 
            this.VarValueTypeColumnHeader.Text = "VarValueType";
            this.VarValueTypeColumnHeader.Width = 150;
            // 
            // ChildGroupBox
            // 
            this.ChildGroupBox.Controls.Add(this.ChildListListBox);
            this.ChildGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ChildGroupBox.Name = "ChildGroupBox";
            this.ChildGroupBox.Size = new System.Drawing.Size(772, 186);
            this.ChildGroupBox.TabIndex = 5;
            this.ChildGroupBox.TabStop = false;
            this.ChildGroupBox.Text = "ChildList";
            // 
            // ChildListListBox
            // 
            this.ChildListListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildListListBox.FormattingEnabled = true;
            this.ChildListListBox.Location = new System.Drawing.Point(3, 16);
            this.ChildListListBox.Name = "ChildListListBox";
            this.ChildListListBox.Size = new System.Drawing.Size(766, 167);
            this.ChildListListBox.TabIndex = 0;
            this.ChildListListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChildListListBox_MouseClick);
            this.ChildListListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChildListListBox_MouseDoubleClick);
            // 
            // DataAccessInterfaceInfoLabel
            // 
            this.DataAccessInterfaceInfoLabel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.DataAccessInterfaceInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataAccessInterfaceInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataAccessInterfaceInfoLabel.Location = new System.Drawing.Point(3, 0);
            this.DataAccessInterfaceInfoLabel.Name = "DataAccessInterfaceInfoLabel";
            this.DataAccessInterfaceInfoLabel.Size = new System.Drawing.Size(776, 20);
            this.DataAccessInterfaceInfoLabel.TabIndex = 6;
            this.DataAccessInterfaceInfoLabel.Text = "IJHDataAccess2";
            this.DataAccessInterfaceInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataAccess2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "DataAccess2";
            this.Size = new System.Drawing.Size(782, 606);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.SubscriptionsGroupBox.ResumeLayout(false);
            this.SubscribeTableLayoutPanel.ResumeLayout(false);
            this.ChildPropertySplitContainer.Panel1.ResumeLayout(false);
            this.ChildPropertySplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChildPropertySplitContainer)).EndInit();
            this.ChildPropertySplitContainer.ResumeLayout(false);
            this.PropertyGroupBox.ResumeLayout(false);
            this.PropertyTableLayoutPanel.ResumeLayout(false);
            this.VarValueTableLayoutPanel.ResumeLayout(false);
            this.VarValueTableLayoutPanel.PerformLayout();
            this.ChildGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.SplitContainer ChildPropertySplitContainer;
        private System.Windows.Forms.GroupBox PropertyGroupBox;
        private System.Windows.Forms.TableLayoutPanel PropertyTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel VarValueTableLayoutPanel;
        private System.Windows.Forms.TextBox VarValueTextBox;
        private System.Windows.Forms.Button SetDataEntryButton;
        private System.Windows.Forms.Label VarValueLabel;
        private System.Windows.Forms.Label IsReadOnlyLabel;
        private System.Windows.Forms.ListView PropertyListListView;
        private System.Windows.Forms.ColumnHeader PropKindColumnHeader;
        private System.Windows.Forms.ColumnHeader VarValueColumnHeader;
        private System.Windows.Forms.ColumnHeader VarValueTypeColumnHeader;
        private System.Windows.Forms.GroupBox ChildGroupBox;
        private System.Windows.Forms.ListBox ChildListListBox;
        private System.Windows.Forms.GroupBox SubscriptionsGroupBox;
        private System.Windows.Forms.TableLayoutPanel SubscribeTableLayoutPanel;
        private System.Windows.Forms.ListView SubscriptionsListView;
        private System.Windows.Forms.ColumnHeader HandleColumnHeader;
        private System.Windows.Forms.ColumnHeader DataSelectionColumnHeader;
        private System.Windows.Forms.ColumnHeader DataColumnHeader;
        private System.Windows.Forms.ColumnHeader TimeStampColumnHeader;
        private System.Windows.Forms.Label DataAccessInterfaceInfoLabel;
    }
}
