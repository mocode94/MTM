namespace DNC_CSharp_Demo.UserControls
{
    partial class FileSystem
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
            this.components = new System.ComponentModel.Container();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AttributeSelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.AttributeSelectionTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ResetAttribSelectionButton = new System.Windows.Forms.Button();
            this.SetAttributeSelectionButton = new System.Windows.Forms.Button();
            this.AttributeSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.AttributeStateGroupBox = new System.Windows.Forms.GroupBox();
            this.AttributeStateTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ResetAttribStateButton = new System.Windows.Forms.Button();
            this.SetAttributeStateButton = new System.Windows.Forms.Button();
            this.AttributeStateComboBox = new System.Windows.Forms.ComboBox();
            this.AccessModeGroupBox = new System.Windows.Forms.GroupBox();
            this.AccessModeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SetAccessModeButton = new System.Windows.Forms.Button();
            this.AccessModeComboBox = new System.Windows.Forms.ComboBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.GeneralInfoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PathGroupBox = new System.Windows.Forms.GroupBox();
            this.PathTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BackButton = new System.Windows.Forms.Button();
            this.GetPathButton = new System.Windows.Forms.Button();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.DiskInfoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TotalSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.TotalSizeLabel = new System.Windows.Forms.Label();
            this.FreeSpaceGroupBox = new System.Windows.Forms.GroupBox();
            this.FreeSpaceLabel = new System.Windows.Forms.Label();
            this.FolderViewGroupBox = new System.Windows.Forms.GroupBox();
            this.FolderListView = new System.Windows.Forms.ListView();
            this.FilenameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileSizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeStampColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttribReadonlyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttribHiddenColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttribDirColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttribSystemColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttribModifiedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttribLockedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FolderViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReceiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TransmitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyPathToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainTableLayoutPanel.SuspendLayout();
            this.AttributeSelectionGroupBox.SuspendLayout();
            this.AttributeSelectionTableLayoutPanel.SuspendLayout();
            this.AttributeStateGroupBox.SuspendLayout();
            this.AttributeStateTableLayoutPanel.SuspendLayout();
            this.AccessModeGroupBox.SuspendLayout();
            this.AccessModeTableLayoutPanel.SuspendLayout();
            this.GeneralInfoTableLayoutPanel.SuspendLayout();
            this.PathGroupBox.SuspendLayout();
            this.PathTableLayoutPanel.SuspendLayout();
            this.DiskInfoTableLayoutPanel.SuspendLayout();
            this.TotalSizeGroupBox.SuspendLayout();
            this.FreeSpaceGroupBox.SuspendLayout();
            this.FolderViewGroupBox.SuspendLayout();
            this.FolderViewContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.AttributeSelectionGroupBox, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.AttributeStateGroupBox, 0, 3);
            this.MainTableLayoutPanel.Controls.Add(this.AccessModeGroupBox, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.GeneralInfoTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.FolderViewGroupBox, 0, 4);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 5;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(784, 562);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // AttributeSelectionGroupBox
            // 
            this.AttributeSelectionGroupBox.Controls.Add(this.AttributeSelectionTableLayoutPanel);
            this.AttributeSelectionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeSelectionGroupBox.Location = new System.Drawing.Point(3, 107);
            this.AttributeSelectionGroupBox.Name = "AttributeSelectionGroupBox";
            this.AttributeSelectionGroupBox.Size = new System.Drawing.Size(778, 46);
            this.AttributeSelectionGroupBox.TabIndex = 0;
            this.AttributeSelectionGroupBox.TabStop = false;
            this.AttributeSelectionGroupBox.Text = "AttributeSelection";
            // 
            // AttributeSelectionTableLayoutPanel
            // 
            this.AttributeSelectionTableLayoutPanel.ColumnCount = 3;
            this.AttributeSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.AttributeSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.AttributeSelectionTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AttributeSelectionTableLayoutPanel.Controls.Add(this.ResetAttribSelectionButton, 0, 0);
            this.AttributeSelectionTableLayoutPanel.Controls.Add(this.SetAttributeSelectionButton, 0, 0);
            this.AttributeSelectionTableLayoutPanel.Controls.Add(this.AttributeSelectionComboBox, 2, 0);
            this.AttributeSelectionTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeSelectionTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.AttributeSelectionTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AttributeSelectionTableLayoutPanel.Name = "AttributeSelectionTableLayoutPanel";
            this.AttributeSelectionTableLayoutPanel.RowCount = 1;
            this.AttributeSelectionTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AttributeSelectionTableLayoutPanel.Size = new System.Drawing.Size(772, 27);
            this.AttributeSelectionTableLayoutPanel.TabIndex = 1;
            // 
            // ResetAttribSelectionButton
            // 
            this.ResetAttribSelectionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetAttribSelectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetAttribSelectionButton.Location = new System.Drawing.Point(53, 3);
            this.ResetAttribSelectionButton.Name = "ResetAttribSelectionButton";
            this.ResetAttribSelectionButton.Size = new System.Drawing.Size(64, 21);
            this.ResetAttribSelectionButton.TabIndex = 1;
            this.ResetAttribSelectionButton.Text = "RESET";
            this.ResetAttribSelectionButton.UseVisualStyleBackColor = true;
            this.ResetAttribSelectionButton.Click += new System.EventHandler(this.ResetAttribSelectionButton_Click);
            // 
            // SetAttributeSelectionButton
            // 
            this.SetAttributeSelectionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetAttributeSelectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetAttributeSelectionButton.Location = new System.Drawing.Point(3, 3);
            this.SetAttributeSelectionButton.Name = "SetAttributeSelectionButton";
            this.SetAttributeSelectionButton.Size = new System.Drawing.Size(44, 21);
            this.SetAttributeSelectionButton.TabIndex = 0;
            this.SetAttributeSelectionButton.Text = "SET";
            this.SetAttributeSelectionButton.UseVisualStyleBackColor = true;
            this.SetAttributeSelectionButton.Click += new System.EventHandler(this.SetAttributeSelectionButton_Click);
            // 
            // AttributeSelectionComboBox
            // 
            this.AttributeSelectionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeSelectionComboBox.FormattingEnabled = true;
            this.AttributeSelectionComboBox.Location = new System.Drawing.Point(123, 3);
            this.AttributeSelectionComboBox.Name = "AttributeSelectionComboBox";
            this.AttributeSelectionComboBox.Size = new System.Drawing.Size(646, 21);
            this.AttributeSelectionComboBox.TabIndex = 0;
            // 
            // AttributeStateGroupBox
            // 
            this.AttributeStateGroupBox.Controls.Add(this.AttributeStateTableLayoutPanel);
            this.AttributeStateGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeStateGroupBox.Location = new System.Drawing.Point(3, 159);
            this.AttributeStateGroupBox.Name = "AttributeStateGroupBox";
            this.AttributeStateGroupBox.Size = new System.Drawing.Size(778, 46);
            this.AttributeStateGroupBox.TabIndex = 1;
            this.AttributeStateGroupBox.TabStop = false;
            this.AttributeStateGroupBox.Text = "AttributeState";
            // 
            // AttributeStateTableLayoutPanel
            // 
            this.AttributeStateTableLayoutPanel.ColumnCount = 3;
            this.AttributeStateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.AttributeStateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.AttributeStateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AttributeStateTableLayoutPanel.Controls.Add(this.ResetAttribStateButton, 0, 0);
            this.AttributeStateTableLayoutPanel.Controls.Add(this.SetAttributeStateButton, 0, 0);
            this.AttributeStateTableLayoutPanel.Controls.Add(this.AttributeStateComboBox, 2, 0);
            this.AttributeStateTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeStateTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.AttributeStateTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AttributeStateTableLayoutPanel.Name = "AttributeStateTableLayoutPanel";
            this.AttributeStateTableLayoutPanel.RowCount = 1;
            this.AttributeStateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AttributeStateTableLayoutPanel.Size = new System.Drawing.Size(772, 27);
            this.AttributeStateTableLayoutPanel.TabIndex = 2;
            // 
            // ResetAttribStateButton
            // 
            this.ResetAttribStateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetAttribStateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetAttribStateButton.Location = new System.Drawing.Point(53, 3);
            this.ResetAttribStateButton.Name = "ResetAttribStateButton";
            this.ResetAttribStateButton.Size = new System.Drawing.Size(64, 21);
            this.ResetAttribStateButton.TabIndex = 2;
            this.ResetAttribStateButton.Text = "RESET";
            this.ResetAttribStateButton.UseVisualStyleBackColor = true;
            this.ResetAttribStateButton.Click += new System.EventHandler(this.ResetAttribStateButton_Click);
            // 
            // SetAttributeStateButton
            // 
            this.SetAttributeStateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetAttributeStateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetAttributeStateButton.Location = new System.Drawing.Point(3, 3);
            this.SetAttributeStateButton.Name = "SetAttributeStateButton";
            this.SetAttributeStateButton.Size = new System.Drawing.Size(44, 21);
            this.SetAttributeStateButton.TabIndex = 0;
            this.SetAttributeStateButton.Text = "SET";
            this.SetAttributeStateButton.UseVisualStyleBackColor = true;
            this.SetAttributeStateButton.Click += new System.EventHandler(this.SetAttributeStateButton_Click);
            // 
            // AttributeStateComboBox
            // 
            this.AttributeStateComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeStateComboBox.FormattingEnabled = true;
            this.AttributeStateComboBox.Location = new System.Drawing.Point(123, 3);
            this.AttributeStateComboBox.Name = "AttributeStateComboBox";
            this.AttributeStateComboBox.Size = new System.Drawing.Size(646, 21);
            this.AttributeStateComboBox.TabIndex = 0;
            // 
            // AccessModeGroupBox
            // 
            this.AccessModeGroupBox.Controls.Add(this.AccessModeTableLayoutPanel);
            this.AccessModeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccessModeGroupBox.Location = new System.Drawing.Point(3, 55);
            this.AccessModeGroupBox.Name = "AccessModeGroupBox";
            this.AccessModeGroupBox.Size = new System.Drawing.Size(778, 46);
            this.AccessModeGroupBox.TabIndex = 4;
            this.AccessModeGroupBox.TabStop = false;
            this.AccessModeGroupBox.Text = "AccessMode";
            // 
            // AccessModeTableLayoutPanel
            // 
            this.AccessModeTableLayoutPanel.ColumnCount = 3;
            this.AccessModeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.AccessModeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AccessModeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.AccessModeTableLayoutPanel.Controls.Add(this.SetAccessModeButton, 0, 0);
            this.AccessModeTableLayoutPanel.Controls.Add(this.AccessModeComboBox, 1, 0);
            this.AccessModeTableLayoutPanel.Controls.Add(this.PasswordTextBox, 2, 0);
            this.AccessModeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccessModeTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.AccessModeTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AccessModeTableLayoutPanel.Name = "AccessModeTableLayoutPanel";
            this.AccessModeTableLayoutPanel.RowCount = 1;
            this.AccessModeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AccessModeTableLayoutPanel.Size = new System.Drawing.Size(772, 27);
            this.AccessModeTableLayoutPanel.TabIndex = 0;
            // 
            // SetAccessModeButton
            // 
            this.SetAccessModeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetAccessModeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetAccessModeButton.Location = new System.Drawing.Point(3, 3);
            this.SetAccessModeButton.Name = "SetAccessModeButton";
            this.SetAccessModeButton.Size = new System.Drawing.Size(44, 21);
            this.SetAccessModeButton.TabIndex = 2;
            this.SetAccessModeButton.Text = "SET";
            this.SetAccessModeButton.UseVisualStyleBackColor = true;
            this.SetAccessModeButton.Click += new System.EventHandler(this.SetAccessModeButton_Click);
            // 
            // AccessModeComboBox
            // 
            this.AccessModeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccessModeComboBox.FormattingEnabled = true;
            this.AccessModeComboBox.Location = new System.Drawing.Point(53, 3);
            this.AccessModeComboBox.Name = "AccessModeComboBox";
            this.AccessModeComboBox.Size = new System.Drawing.Size(616, 21);
            this.AccessModeComboBox.TabIndex = 3;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordTextBox.Location = new System.Drawing.Point(675, 3);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(94, 20);
            this.PasswordTextBox.TabIndex = 4;
            this.PasswordTextBox.Text = "password";
            this.PasswordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // GeneralInfoTableLayoutPanel
            // 
            this.GeneralInfoTableLayoutPanel.ColumnCount = 2;
            this.GeneralInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GeneralInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.GeneralInfoTableLayoutPanel.Controls.Add(this.PathGroupBox, 0, 0);
            this.GeneralInfoTableLayoutPanel.Controls.Add(this.DiskInfoTableLayoutPanel, 1, 0);
            this.GeneralInfoTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralInfoTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.GeneralInfoTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.GeneralInfoTableLayoutPanel.Name = "GeneralInfoTableLayoutPanel";
            this.GeneralInfoTableLayoutPanel.RowCount = 1;
            this.GeneralInfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GeneralInfoTableLayoutPanel.Size = new System.Drawing.Size(784, 52);
            this.GeneralInfoTableLayoutPanel.TabIndex = 5;
            // 
            // PathGroupBox
            // 
            this.PathGroupBox.Controls.Add(this.PathTableLayoutPanel);
            this.PathGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PathGroupBox.Location = new System.Drawing.Point(3, 3);
            this.PathGroupBox.Name = "PathGroupBox";
            this.PathGroupBox.Size = new System.Drawing.Size(558, 46);
            this.PathGroupBox.TabIndex = 3;
            this.PathGroupBox.TabStop = false;
            this.PathGroupBox.Text = "Path";
            // 
            // PathTableLayoutPanel
            // 
            this.PathTableLayoutPanel.ColumnCount = 3;
            this.PathTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.PathTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.PathTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PathTableLayoutPanel.Controls.Add(this.BackButton, 0, 0);
            this.PathTableLayoutPanel.Controls.Add(this.GetPathButton, 0, 0);
            this.PathTableLayoutPanel.Controls.Add(this.PathTextBox, 2, 0);
            this.PathTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PathTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.PathTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PathTableLayoutPanel.Name = "PathTableLayoutPanel";
            this.PathTableLayoutPanel.RowCount = 1;
            this.PathTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PathTableLayoutPanel.Size = new System.Drawing.Size(552, 27);
            this.PathTableLayoutPanel.TabIndex = 1;
            // 
            // BackButton
            // 
            this.BackButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackButton.Location = new System.Drawing.Point(53, 3);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(64, 21);
            this.BackButton.TabIndex = 2;
            this.BackButton.Text = "BACK";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // GetPathButton
            // 
            this.GetPathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetPathButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetPathButton.Location = new System.Drawing.Point(3, 3);
            this.GetPathButton.Name = "GetPathButton";
            this.GetPathButton.Size = new System.Drawing.Size(44, 21);
            this.GetPathButton.TabIndex = 1;
            this.GetPathButton.Text = "GET";
            this.GetPathButton.UseVisualStyleBackColor = true;
            this.GetPathButton.Click += new System.EventHandler(this.GetPathButton_Click);
            // 
            // PathTextBox
            // 
            this.PathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PathTextBox.Location = new System.Drawing.Point(123, 3);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(426, 20);
            this.PathTextBox.TabIndex = 0;
            // 
            // DiskInfoTableLayoutPanel
            // 
            this.DiskInfoTableLayoutPanel.ColumnCount = 2;
            this.DiskInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DiskInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DiskInfoTableLayoutPanel.Controls.Add(this.TotalSizeGroupBox, 0, 0);
            this.DiskInfoTableLayoutPanel.Controls.Add(this.FreeSpaceGroupBox, 1, 0);
            this.DiskInfoTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DiskInfoTableLayoutPanel.Location = new System.Drawing.Point(567, 3);
            this.DiskInfoTableLayoutPanel.Name = "DiskInfoTableLayoutPanel";
            this.DiskInfoTableLayoutPanel.RowCount = 1;
            this.DiskInfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DiskInfoTableLayoutPanel.Size = new System.Drawing.Size(214, 46);
            this.DiskInfoTableLayoutPanel.TabIndex = 4;
            // 
            // TotalSizeGroupBox
            // 
            this.TotalSizeGroupBox.Controls.Add(this.TotalSizeLabel);
            this.TotalSizeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TotalSizeGroupBox.Location = new System.Drawing.Point(3, 3);
            this.TotalSizeGroupBox.Name = "TotalSizeGroupBox";
            this.TotalSizeGroupBox.Size = new System.Drawing.Size(101, 40);
            this.TotalSizeGroupBox.TabIndex = 0;
            this.TotalSizeGroupBox.TabStop = false;
            this.TotalSizeGroupBox.Text = "Total size";
            // 
            // TotalSizeLabel
            // 
            this.TotalSizeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TotalSizeLabel.Location = new System.Drawing.Point(3, 16);
            this.TotalSizeLabel.Name = "TotalSizeLabel";
            this.TotalSizeLabel.Size = new System.Drawing.Size(95, 21);
            this.TotalSizeLabel.TabIndex = 0;
            this.TotalSizeLabel.Text = "Total Size";
            this.TotalSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FreeSpaceGroupBox
            // 
            this.FreeSpaceGroupBox.Controls.Add(this.FreeSpaceLabel);
            this.FreeSpaceGroupBox.Location = new System.Drawing.Point(110, 3);
            this.FreeSpaceGroupBox.Name = "FreeSpaceGroupBox";
            this.FreeSpaceGroupBox.Size = new System.Drawing.Size(101, 40);
            this.FreeSpaceGroupBox.TabIndex = 1;
            this.FreeSpaceGroupBox.TabStop = false;
            this.FreeSpaceGroupBox.Text = "Free space";
            // 
            // FreeSpaceLabel
            // 
            this.FreeSpaceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FreeSpaceLabel.Location = new System.Drawing.Point(3, 16);
            this.FreeSpaceLabel.Name = "FreeSpaceLabel";
            this.FreeSpaceLabel.Size = new System.Drawing.Size(95, 21);
            this.FreeSpaceLabel.TabIndex = 1;
            this.FreeSpaceLabel.Text = "Free Space";
            this.FreeSpaceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FolderViewGroupBox
            // 
            this.FolderViewGroupBox.Controls.Add(this.FolderListView);
            this.FolderViewGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderViewGroupBox.Location = new System.Drawing.Point(3, 211);
            this.FolderViewGroupBox.Name = "FolderViewGroupBox";
            this.FolderViewGroupBox.Size = new System.Drawing.Size(778, 348);
            this.FolderViewGroupBox.TabIndex = 6;
            this.FolderViewGroupBox.TabStop = false;
            this.FolderViewGroupBox.Text = "Right click for additional functions!";
            // 
            // FolderListView
            // 
            this.FolderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FilenameColumnHeader,
            this.FileSizeColumnHeader,
            this.TimeStampColumnHeader,
            this.AttribReadonlyColumnHeader,
            this.AttribHiddenColumnHeader,
            this.AttribDirColumnHeader,
            this.AttribSystemColumnHeader,
            this.AttribModifiedColumnHeader,
            this.AttribLockedColumnHeader});
            this.FolderListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderListView.FullRowSelect = true;
            this.FolderListView.GridLines = true;
            this.FolderListView.Location = new System.Drawing.Point(3, 16);
            this.FolderListView.Name = "FolderListView";
            this.FolderListView.Size = new System.Drawing.Size(772, 329);
            this.FolderListView.TabIndex = 4;
            this.FolderListView.UseCompatibleStateImageBehavior = false;
            this.FolderListView.View = System.Windows.Forms.View.Details;
            this.FolderListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FolderListView_MouseClick);
            this.FolderListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FolderListView_MouseDoubleClick);
            // 
            // FilenameColumnHeader
            // 
            this.FilenameColumnHeader.Text = "FileName";
            this.FilenameColumnHeader.Width = 200;
            // 
            // FileSizeColumnHeader
            // 
            this.FileSizeColumnHeader.Text = "FileSize";
            this.FileSizeColumnHeader.Width = 80;
            // 
            // TimeStampColumnHeader
            // 
            this.TimeStampColumnHeader.Text = "TimeStamp";
            this.TimeStampColumnHeader.Width = 120;
            // 
            // AttribReadonlyColumnHeader
            // 
            this.AttribReadonlyColumnHeader.Text = "ReadOnly";
            // 
            // AttribHiddenColumnHeader
            // 
            this.AttribHiddenColumnHeader.Text = "Hidden";
            // 
            // AttribDirColumnHeader
            // 
            this.AttribDirColumnHeader.Text = "Dir";
            // 
            // AttribSystemColumnHeader
            // 
            this.AttribSystemColumnHeader.Text = "System";
            // 
            // AttribModifiedColumnHeader
            // 
            this.AttribModifiedColumnHeader.Text = "Modified";
            // 
            // AttribLockedColumnHeader
            // 
            this.AttribLockedColumnHeader.Text = "Locked";
            // 
            // FolderViewContextMenuStrip
            // 
            this.FolderViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteToolStripMenuItem,
            this.ReceiveToolStripMenuItem,
            this.TransmitToolStripMenuItem,
            this.CopyPathToClipboardToolStripMenuItem});
            this.FolderViewContextMenuStrip.Name = "cmsFolderView";
            this.FolderViewContextMenuStrip.Size = new System.Drawing.Size(197, 92);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.DeleteToolStripMenuItem.Text = "Delete";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // ReceiveToolStripMenuItem
            // 
            this.ReceiveToolStripMenuItem.Name = "ReceiveToolStripMenuItem";
            this.ReceiveToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.ReceiveToolStripMenuItem.Text = "Receive";
            this.ReceiveToolStripMenuItem.Click += new System.EventHandler(this.ReceiveToolStripMenuItem_Click);
            // 
            // TransmitToolStripMenuItem
            // 
            this.TransmitToolStripMenuItem.Name = "TransmitToolStripMenuItem";
            this.TransmitToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.TransmitToolStripMenuItem.Text = "Transmit";
            this.TransmitToolStripMenuItem.Click += new System.EventHandler(this.TransmitToolStripMenuItem_Click);
            // 
            // CopyPathToClipboardToolStripMenuItem
            // 
            this.CopyPathToClipboardToolStripMenuItem.Name = "CopyPathToClipboardToolStripMenuItem";
            this.CopyPathToClipboardToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.CopyPathToClipboardToolStripMenuItem.Text = "Copy path to clipboard";
            this.CopyPathToClipboardToolStripMenuItem.Click += new System.EventHandler(this.CopyPathToClipboardToolStripMenuItem_Click);
            // 
            // FileSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "FileSystem";
            this.Size = new System.Drawing.Size(784, 562);
            this.Load += new System.EventHandler(this.FileSystem_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.AttributeSelectionGroupBox.ResumeLayout(false);
            this.AttributeSelectionTableLayoutPanel.ResumeLayout(false);
            this.AttributeStateGroupBox.ResumeLayout(false);
            this.AttributeStateTableLayoutPanel.ResumeLayout(false);
            this.AccessModeGroupBox.ResumeLayout(false);
            this.AccessModeTableLayoutPanel.ResumeLayout(false);
            this.AccessModeTableLayoutPanel.PerformLayout();
            this.GeneralInfoTableLayoutPanel.ResumeLayout(false);
            this.PathGroupBox.ResumeLayout(false);
            this.PathTableLayoutPanel.ResumeLayout(false);
            this.PathTableLayoutPanel.PerformLayout();
            this.DiskInfoTableLayoutPanel.ResumeLayout(false);
            this.TotalSizeGroupBox.ResumeLayout(false);
            this.FreeSpaceGroupBox.ResumeLayout(false);
            this.FolderViewGroupBox.ResumeLayout(false);
            this.FolderViewContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.GroupBox AttributeSelectionGroupBox;
        private System.Windows.Forms.ComboBox AttributeSelectionComboBox;
        private System.Windows.Forms.GroupBox AttributeStateGroupBox;
        private System.Windows.Forms.ComboBox AttributeStateComboBox;
        private System.Windows.Forms.TableLayoutPanel AttributeSelectionTableLayoutPanel;
        private System.Windows.Forms.Button SetAttributeSelectionButton;
        private System.Windows.Forms.TableLayoutPanel AttributeStateTableLayoutPanel;
        private System.Windows.Forms.Button SetAttributeStateButton;
        private System.Windows.Forms.Button ResetAttribSelectionButton;
        private System.Windows.Forms.Button ResetAttribStateButton;
        private System.Windows.Forms.GroupBox AccessModeGroupBox;
        private System.Windows.Forms.TableLayoutPanel AccessModeTableLayoutPanel;
        private System.Windows.Forms.Button SetAccessModeButton;
        private System.Windows.Forms.ComboBox AccessModeComboBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TableLayoutPanel GeneralInfoTableLayoutPanel;
        private System.Windows.Forms.GroupBox PathGroupBox;
        private System.Windows.Forms.TableLayoutPanel PathTableLayoutPanel;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button GetPathButton;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.TableLayoutPanel DiskInfoTableLayoutPanel;
        private System.Windows.Forms.GroupBox TotalSizeGroupBox;
        private System.Windows.Forms.Label TotalSizeLabel;
        private System.Windows.Forms.GroupBox FreeSpaceGroupBox;
        private System.Windows.Forms.Label FreeSpaceLabel;
        private System.Windows.Forms.ContextMenuStrip FolderViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReceiveToolStripMenuItem;
        private System.Windows.Forms.GroupBox FolderViewGroupBox;
        private System.Windows.Forms.ListView FolderListView;
        private System.Windows.Forms.ColumnHeader FilenameColumnHeader;
        private System.Windows.Forms.ColumnHeader FileSizeColumnHeader;
        private System.Windows.Forms.ColumnHeader TimeStampColumnHeader;
        private System.Windows.Forms.ColumnHeader AttribReadonlyColumnHeader;
        private System.Windows.Forms.ColumnHeader AttribHiddenColumnHeader;
        private System.Windows.Forms.ColumnHeader AttribDirColumnHeader;
        private System.Windows.Forms.ColumnHeader AttribSystemColumnHeader;
        private System.Windows.Forms.ColumnHeader AttribModifiedColumnHeader;
        private System.Windows.Forms.ColumnHeader AttribLockedColumnHeader;
        private System.Windows.Forms.ToolStripMenuItem TransmitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyPathToClipboardToolStripMenuItem;

    }
}
