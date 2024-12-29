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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DncInterface));
            this.ReadTableButton = new System.Windows.Forms.Button();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OutputTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ControlTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ClearAllButton = new System.Windows.Forms.Button();
            this.ClearStatisticsButton = new System.Windows.Forms.Button();
            this.ReadingTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.DataAccessTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.ClearTableButton = new System.Windows.Forms.Button();
            this.ReadEverythingButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.ReadColumnNamesButton = new System.Windows.Forms.Button();
            this.LoadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.OutputSplitContainer = new System.Windows.Forms.SplitContainer();
            this.TableGroupBox = new System.Windows.Forms.GroupBox();
            this.StatisticsGroupBox = new System.Windows.Forms.GroupBox();
            this.TableListView = new System.Windows.Forms.ListView();
            this.StatisticsListView = new System.Windows.Forms.ListView();
            this.JobColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DurationColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CellByCellRadioButton = new System.Windows.Forms.RadioButton();
            this.OnTheWholeRadioButton = new System.Windows.Forms.RadioButton();
            this.LineByLineRadioButton = new System.Windows.Forms.RadioButton();
            this.DataAccess3RadioButton = new System.Windows.Forms.RadioButton();
            this.DataAccess2RadioButton = new System.Windows.Forms.RadioButton();
            this.ToolListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddRowIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel.SuspendLayout();
            this.ControlTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutputSplitContainer)).BeginInit();
            this.OutputSplitContainer.Panel1.SuspendLayout();
            this.OutputSplitContainer.Panel2.SuspendLayout();
            this.OutputSplitContainer.SuspendLayout();
            this.ToolListContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // ReadTableButton
            // 
            this.ReadTableButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReadTableButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadTableButton.Location = new System.Drawing.Point(3, 203);
            this.ReadTableButton.Name = "ReadTableButton";
            this.ReadTableButton.Size = new System.Drawing.Size(118, 44);
            this.ReadTableButton.TabIndex = 0;
            this.ReadTableButton.Text = "Read table";
            this.ReadTableButton.UseVisualStyleBackColor = true;
            this.ReadTableButton.Click += new System.EventHandler(this.ReadToolTableButton_Click);
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 2;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.OutputTableLayoutPanel, 1, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ControlTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(979, 604);
            this.MainTableLayoutPanel.TabIndex = 6;
            // 
            // OutputTableLayoutPanel
            // 
            this.OutputTableLayoutPanel.ColumnCount = 1;
            this.OutputTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputTableLayoutPanel.Location = new System.Drawing.Point(133, 3);
            this.OutputTableLayoutPanel.Name = "OutputTableLayoutPanel";
            this.OutputTableLayoutPanel.RowCount = 2;
            this.OutputTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.OutputTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.OutputTableLayoutPanel.Size = new System.Drawing.Size(843, 598);
            this.OutputTableLayoutPanel.TabIndex = 0;
            // 
            // ControlTableLayoutPanel
            // 
            this.ControlTableLayoutPanel.ColumnCount = 1;
            this.ControlTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ControlTableLayoutPanel.Controls.Add(this.ClearAllButton, 0, 9);
            this.ControlTableLayoutPanel.Controls.Add(this.ClearStatisticsButton, 0, 8);
            this.ControlTableLayoutPanel.Controls.Add(this.ReadingTypeGroupBox, 0, 1);
            this.ControlTableLayoutPanel.Controls.Add(this.DataAccessTypeGroupBox, 0, 0);
            this.ControlTableLayoutPanel.Controls.Add(this.ClearTableButton, 0, 7);
            this.ControlTableLayoutPanel.Controls.Add(this.ReadEverythingButton, 0, 4);
            this.ControlTableLayoutPanel.Controls.Add(this.ReadTableButton, 0, 3);
            this.ControlTableLayoutPanel.Controls.Add(this.StopButton, 0, 5);
            this.ControlTableLayoutPanel.Controls.Add(this.ReadColumnNamesButton, 0, 2);
            this.ControlTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ControlTableLayoutPanel.Name = "ControlTableLayoutPanel";
            this.ControlTableLayoutPanel.RowCount = 10;
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ControlTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ControlTableLayoutPanel.Size = new System.Drawing.Size(124, 598);
            this.ControlTableLayoutPanel.TabIndex = 1;
            // 
            // ClearAllButton
            // 
            this.ClearAllButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClearAllButton.Location = new System.Drawing.Point(3, 571);
            this.ClearAllButton.Name = "ClearAllButton";
            this.ClearAllButton.Size = new System.Drawing.Size(118, 24);
            this.ClearAllButton.TabIndex = 8;
            this.ClearAllButton.Text = "Clear all";
            this.ClearAllButton.UseVisualStyleBackColor = true;
            this.ClearAllButton.Click += new System.EventHandler(this.ClearAllButton_Click);
            // 
            // ClearStatisticsButton
            // 
            this.ClearStatisticsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClearStatisticsButton.Location = new System.Drawing.Point(3, 541);
            this.ClearStatisticsButton.Name = "ClearStatisticsButton";
            this.ClearStatisticsButton.Size = new System.Drawing.Size(118, 24);
            this.ClearStatisticsButton.TabIndex = 7;
            this.ClearStatisticsButton.Text = "Clear statistics";
            this.ClearStatisticsButton.UseVisualStyleBackColor = true;
            this.ClearStatisticsButton.Click += new System.EventHandler(this.ClearStatisticsButton_Click);
            // 
            // ReadingTypeGroupBox
            // 
            this.ReadingTypeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReadingTypeGroupBox.Location = new System.Drawing.Point(3, 73);
            this.ReadingTypeGroupBox.Name = "ReadingTypeGroupBox";
            this.ReadingTypeGroupBox.Size = new System.Drawing.Size(118, 94);
            this.ReadingTypeGroupBox.TabIndex = 1;
            this.ReadingTypeGroupBox.TabStop = false;
            this.ReadingTypeGroupBox.Text = "Reading mode";
            // 
            // DataAccessTypeGroupBox
            // 
            this.DataAccessTypeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataAccessTypeGroupBox.Location = new System.Drawing.Point(3, 3);
            this.DataAccessTypeGroupBox.Name = "DataAccessTypeGroupBox";
            this.DataAccessTypeGroupBox.Size = new System.Drawing.Size(118, 64);
            this.DataAccessTypeGroupBox.TabIndex = 3;
            this.DataAccessTypeGroupBox.TabStop = false;
            this.DataAccessTypeGroupBox.Text = "Choose interface";
            // 
            // ClearTableButton
            // 
            this.ClearTableButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClearTableButton.Location = new System.Drawing.Point(3, 511);
            this.ClearTableButton.Name = "ClearTableButton";
            this.ClearTableButton.Size = new System.Drawing.Size(118, 24);
            this.ClearTableButton.TabIndex = 6;
            this.ClearTableButton.Text = "Clear table";
            this.ClearTableButton.UseVisualStyleBackColor = true;
            this.ClearTableButton.Click += new System.EventHandler(this.ClearToolTableButton_Click);
            // 
            // ReadEverythingButton
            // 
            this.ReadEverythingButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReadEverythingButton.Location = new System.Drawing.Point(3, 253);
            this.ReadEverythingButton.Name = "ReadEverythingButton";
            this.ReadEverythingButton.Size = new System.Drawing.Size(118, 44);
            this.ReadEverythingButton.TabIndex = 5;
            this.ReadEverythingButton.Text = "Read all types with all interfaces";
            this.ReadEverythingButton.UseVisualStyleBackColor = true;
            this.ReadEverythingButton.Click += new System.EventHandler(this.ReadEverythingButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StopButton.Location = new System.Drawing.Point(3, 303);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(118, 24);
            this.StopButton.TabIndex = 9;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ReadColumnNamesButton
            // 
            this.ReadColumnNamesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReadColumnNamesButton.Location = new System.Drawing.Point(3, 173);
            this.ReadColumnNamesButton.Name = "ReadColumnNamesButton";
            this.ReadColumnNamesButton.Size = new System.Drawing.Size(118, 24);
            this.ReadColumnNamesButton.TabIndex = 10;
            this.ReadColumnNamesButton.Text = "Read column names";
            this.ReadColumnNamesButton.UseVisualStyleBackColor = true;
            this.ReadColumnNamesButton.Click += new System.EventHandler(this.ReadColumnNamesButton_Click);
            // 
            // LoadingProgressBar
            // 
            this.LoadingProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingProgressBar.Location = new System.Drawing.Point(3, 571);
            this.LoadingProgressBar.Name = "LoadingProgressBar";
            this.LoadingProgressBar.Size = new System.Drawing.Size(837, 24);
            this.LoadingProgressBar.TabIndex = 5;
            // 
            // OutputSplitContainer
            // 
            this.OutputSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.OutputSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.OutputSplitContainer.Name = "OutputSplitContainer";
            this.OutputSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // OutputSplitContainer.Panel1
            // 
            this.OutputSplitContainer.Panel1.Controls.Add(this.TableGroupBox);
            // 
            // OutputSplitContainer.Panel2
            // 
            this.OutputSplitContainer.Panel2.Controls.Add(this.StatisticsGroupBox);
            this.OutputSplitContainer.Size = new System.Drawing.Size(837, 562);
            this.OutputSplitContainer.SplitterDistance = 358;
            this.OutputSplitContainer.TabIndex = 6;
            // 
            // TableGroupBox
            // 
            this.TableGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableGroupBox.Location = new System.Drawing.Point(0, 0);
            this.TableGroupBox.Name = "TableGroupBox";
            this.TableGroupBox.Size = new System.Drawing.Size(833, 354);
            this.TableGroupBox.TabIndex = 0;
            this.TableGroupBox.TabStop = false;
            this.TableGroupBox.Text = "Table: ";
            // 
            // StatisticsGroupBox
            // 
            this.StatisticsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatisticsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.StatisticsGroupBox.Name = "StatisticsGroupBox";
            this.StatisticsGroupBox.Size = new System.Drawing.Size(833, 196);
            this.StatisticsGroupBox.TabIndex = 0;
            this.StatisticsGroupBox.TabStop = false;
            this.StatisticsGroupBox.Text = "Statistics";
            // 
            // TableListView
            // 
            this.TableListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableListView.FullRowSelect = true;
            this.TableListView.GridLines = true;
            this.TableListView.HideSelection = false;
            this.TableListView.Location = new System.Drawing.Point(3, 16);
            this.TableListView.MultiSelect = false;
            this.TableListView.Name = "TableListView";
            this.TableListView.Size = new System.Drawing.Size(827, 335);
            this.TableListView.TabIndex = 0;
            this.TableListView.UseCompatibleStateImageBehavior = false;
            this.TableListView.View = System.Windows.Forms.View.Details;
            this.TableListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TableListView_MouseClick);
            // 
            // StatisticsListView
            // 
            this.StatisticsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.JobColumnHeader,
            this.DurationColumnHeader});
            this.StatisticsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatisticsListView.GridLines = true;
            this.StatisticsListView.HideSelection = false;
            this.StatisticsListView.Location = new System.Drawing.Point(3, 16);
            this.StatisticsListView.Name = "StatisticsListView";
            this.StatisticsListView.Scrollable = false;
            this.StatisticsListView.Size = new System.Drawing.Size(827, 177);
            this.StatisticsListView.TabIndex = 1;
            this.StatisticsListView.UseCompatibleStateImageBehavior = false;
            this.StatisticsListView.View = System.Windows.Forms.View.Details;
            // 
            // JobColumnHeader
            // 
            this.JobColumnHeader.Text = "Job";
            // 
            // DurationColumnHeader
            // 
            this.DurationColumnHeader.Text = "Duration [ms]";
            // 
            // CellByCellRadioButton
            // 
            this.CellByCellRadioButton.AutoSize = true;
            this.CellByCellRadioButton.Location = new System.Drawing.Point(6, 19);
            this.CellByCellRadioButton.Name = "CellByCellRadioButton";
            this.CellByCellRadioButton.Size = new System.Drawing.Size(75, 17);
            this.CellByCellRadioButton.TabIndex = 1;
            this.CellByCellRadioButton.TabStop = true;
            this.CellByCellRadioButton.Text = "Cell by cell";
            this.CellByCellRadioButton.UseVisualStyleBackColor = true;
            // 
            // OnTheWholeRadioButton
            // 
            this.OnTheWholeRadioButton.AutoSize = true;
            this.OnTheWholeRadioButton.Checked = true;
            this.OnTheWholeRadioButton.Location = new System.Drawing.Point(6, 65);
            this.OnTheWholeRadioButton.Name = "OnTheWholeRadioButton";
            this.OnTheWholeRadioButton.Size = new System.Drawing.Size(88, 17);
            this.OnTheWholeRadioButton.TabIndex = 3;
            this.OnTheWholeRadioButton.TabStop = true;
            this.OnTheWholeRadioButton.Text = "On the whole";
            this.OnTheWholeRadioButton.UseVisualStyleBackColor = true;
            // 
            // LineByLineRadioButton
            // 
            this.LineByLineRadioButton.AutoSize = true;
            this.LineByLineRadioButton.Location = new System.Drawing.Point(6, 42);
            this.LineByLineRadioButton.Name = "LineByLineRadioButton";
            this.LineByLineRadioButton.Size = new System.Drawing.Size(78, 17);
            this.LineByLineRadioButton.TabIndex = 2;
            this.LineByLineRadioButton.Text = "Line by line";
            this.LineByLineRadioButton.UseVisualStyleBackColor = true;
            // 
            // DataAccess3RadioButton
            // 
            this.DataAccess3RadioButton.AutoSize = true;
            this.DataAccess3RadioButton.Checked = true;
            this.DataAccess3RadioButton.Location = new System.Drawing.Point(12, 41);
            this.DataAccess3RadioButton.Name = "DataAccess3RadioButton";
            this.DataAccess3RadioButton.Size = new System.Drawing.Size(102, 17);
            this.DataAccess3RadioButton.TabIndex = 1;
            this.DataAccess3RadioButton.TabStop = true;
            this.DataAccess3RadioButton.Text = "JHDataAccess3";
            this.DataAccess3RadioButton.UseVisualStyleBackColor = true;
            // 
            // DataAccess2RadioButton
            // 
            this.DataAccess2RadioButton.AutoSize = true;
            this.DataAccess2RadioButton.Location = new System.Drawing.Point(12, 19);
            this.DataAccess2RadioButton.Name = "DataAccess2RadioButton";
            this.DataAccess2RadioButton.Size = new System.Drawing.Size(102, 17);
            this.DataAccess2RadioButton.TabIndex = 0;
            this.DataAccess2RadioButton.Text = "JHDataAccess2";
            this.DataAccess2RadioButton.UseVisualStyleBackColor = true;
            this.DataAccess2RadioButton.CheckedChanged += new System.EventHandler(this.DataAccess2Button_CheckedChanged);
            // 
            // ToolListContextMenuStrip
            // 
            this.ToolListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditRowToolStripMenuItem,
            this.DeleteRowToolStripMenuItem,
            this.AddRowToolStripMenuItem,
            this.AddRowIndexToolStripMenuItem});
            this.ToolListContextMenuStrip.Name = "cmsToolList";
            this.ToolListContextMenuStrip.Size = new System.Drawing.Size(152, 92);
            // 
            // EditRowToolStripMenuItem
            // 
            this.EditRowToolStripMenuItem.Name = "EditRowToolStripMenuItem";
            this.EditRowToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.EditRowToolStripMenuItem.Text = "&Edit row";
            this.EditRowToolStripMenuItem.Click += new System.EventHandler(this.EditRowToolStripMenuItem_Click);
            // 
            // DeleteRowToolStripMenuItem
            // 
            this.DeleteRowToolStripMenuItem.Name = "DeleteRowToolStripMenuItem";
            this.DeleteRowToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.DeleteRowToolStripMenuItem.Text = "&Delete row";
            this.DeleteRowToolStripMenuItem.Click += new System.EventHandler(this.DeleteRowToolStripMenuItem_Click);
            // 
            // AddRowToolStripMenuItem
            // 
            this.AddRowToolStripMenuItem.Name = "AddRowToolStripMenuItem";
            this.AddRowToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.AddRowToolStripMenuItem.Text = "&Add row";
            this.AddRowToolStripMenuItem.Click += new System.EventHandler(this.AddRowToolStripMenuItem_Click);
            // 
            // AddRowIndexToolStripMenuItem
            // 
            this.AddRowIndexToolStripMenuItem.Name = "AddRowIndexToolStripMenuItem";
            this.AddRowIndexToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.AddRowIndexToolStripMenuItem.Text = "Add row &index";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(92, 114);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(442, 125);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(368, 114);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(367, 125);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(53, 102);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(150, 151);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(368, 114);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(128, 126);
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(653, 102);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(148, 151);
            this.pictureBox5.TabIndex = 5;
            this.pictureBox5.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(311, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 37);
            this.label2.TabIndex = 7;
            this.label2.Text = "Please Wait ......";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // DncInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "DncInterface";
            this.Size = new System.Drawing.Size(866, 480);
            this.Load += new System.EventHandler(this.ReadTableUserControl_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ControlTableLayoutPanel.ResumeLayout(false);
            this.OutputSplitContainer.Panel1.ResumeLayout(false);
            this.OutputSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OutputSplitContainer)).EndInit();
            this.OutputSplitContainer.ResumeLayout(false);
            this.ToolListContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadTableButton;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ControlTableLayoutPanel;
        private System.Windows.Forms.GroupBox ReadingTypeGroupBox;
        private System.Windows.Forms.RadioButton CellByCellRadioButton;
        private System.Windows.Forms.RadioButton OnTheWholeRadioButton;
        private System.Windows.Forms.RadioButton LineByLineRadioButton;
        private System.Windows.Forms.GroupBox DataAccessTypeGroupBox;
        private System.Windows.Forms.RadioButton DataAccess3RadioButton;
        private System.Windows.Forms.RadioButton DataAccess2RadioButton;
        private System.Windows.Forms.TableLayoutPanel OutputTableLayoutPanel;
        private System.Windows.Forms.ProgressBar LoadingProgressBar;
        private System.Windows.Forms.SplitContainer OutputSplitContainer;
        private System.Windows.Forms.ListView TableListView;
        private System.Windows.Forms.GroupBox TableGroupBox;
        private System.Windows.Forms.GroupBox StatisticsGroupBox;
        private System.Windows.Forms.ListView StatisticsListView;
        private System.Windows.Forms.ColumnHeader JobColumnHeader;
        private System.Windows.Forms.ColumnHeader DurationColumnHeader;
        private System.Windows.Forms.Button ReadEverythingButton;
        private System.Windows.Forms.Button ClearTableButton;
        private System.Windows.Forms.Button ClearStatisticsButton;
        private System.Windows.Forms.Button ClearAllButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button ReadColumnNamesButton;
        private System.Windows.Forms.ContextMenuStrip ToolListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddRowIndexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddRowToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label2;
    }
}
