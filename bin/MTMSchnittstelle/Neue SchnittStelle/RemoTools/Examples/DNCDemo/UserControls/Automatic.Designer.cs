namespace DNC_CSharp_Demo.UserControls
{
    partial class Automatic
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
            this.OverrideInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.GetOverrideInfoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.GetSetOverrideTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.GetOverrideInfoButton = new System.Windows.Forms.Button();
            this.GetRapidGroupBox = new System.Windows.Forms.GroupBox();
            this.RapidTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SetRapidButton = new System.Windows.Forms.Button();
            this.RapidTextBox = new System.Windows.Forms.TextBox();
            this.GetSpeedGroupBox = new System.Windows.Forms.GroupBox();
            this.SpeedTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SetSpeedButton = new System.Windows.Forms.Button();
            this.SpeedTextBox = new System.Windows.Forms.TextBox();
            this.GetFeedGroupBox = new System.Windows.Forms.GroupBox();
            this.FeedTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FeedTextBox = new System.Windows.Forms.TextBox();
            this.SetFeedButton = new System.Windows.Forms.Button();
            this.ProgramHandlingGroupBox = new System.Windows.Forms.GroupBox();
            this.ProgramHandlingControlsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ProgramHandlingSelectTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StartProgramButton = new System.Windows.Forms.Button();
            this.StartBlockNumberNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SelectProgramButton = new System.Windows.Forms.Button();
            this.SelectProgramTextBox = new System.Windows.Forms.TextBox();
            this.ProgramStartStopCancelTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.NCStartButton = new System.Windows.Forms.Button();
            this.StopProgramButton = new System.Windows.Forms.Button();
            this.CancelProgramButton = new System.Windows.Forms.Button();
            this.ChannelNumberGroupBox = new System.Windows.Forms.GroupBox();
            this.ChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.CutterLocationGroupBox = new System.Windows.Forms.GroupBox();
            this.GetCutterLocationTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CutterLocationListView = new System.Windows.Forms.ListView();
            this.ChoordNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PositionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UnitColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GetCutterLocationButton = new System.Windows.Forms.Button();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeadTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecutionPointGroupBox = new System.Windows.Forms.GroupBox();
            this.ExecutionPointTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecutionPointTextBox = new System.Windows.Forms.TextBox();
            this.GetExecutionPointButton = new System.Windows.Forms.Button();
            this.OverrideInfoGroupBox.SuspendLayout();
            this.GetOverrideInfoTableLayoutPanel.SuspendLayout();
            this.GetSetOverrideTableLayoutPanel.SuspendLayout();
            this.GetRapidGroupBox.SuspendLayout();
            this.RapidTableLayoutPanel.SuspendLayout();
            this.GetSpeedGroupBox.SuspendLayout();
            this.SpeedTableLayoutPanel.SuspendLayout();
            this.GetFeedGroupBox.SuspendLayout();
            this.FeedTableLayoutPanel.SuspendLayout();
            this.ProgramHandlingGroupBox.SuspendLayout();
            this.ProgramHandlingControlsTableLayoutPanel.SuspendLayout();
            this.ProgramHandlingSelectTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartBlockNumberNumericUpDown)).BeginInit();
            this.ProgramStartStopCancelTableLayoutPanel.SuspendLayout();
            this.ChannelNumberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChannelNumericUpDown)).BeginInit();
            this.CutterLocationGroupBox.SuspendLayout();
            this.GetCutterLocationTableLayoutPanel.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.HeadTableLayoutPanel.SuspendLayout();
            this.ExecutionPointGroupBox.SuspendLayout();
            this.ExecutionPointTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OverrideInfoGroupBox
            // 
            this.OverrideInfoGroupBox.Controls.Add(this.GetOverrideInfoTableLayoutPanel);
            this.OverrideInfoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OverrideInfoGroupBox.Location = new System.Drawing.Point(73, 3);
            this.OverrideInfoGroupBox.Name = "OverrideInfoGroupBox";
            this.OverrideInfoGroupBox.Size = new System.Drawing.Size(688, 68);
            this.OverrideInfoGroupBox.TabIndex = 0;
            this.OverrideInfoGroupBox.TabStop = false;
            this.OverrideInfoGroupBox.Text = "OverrideInfo";
            // 
            // GetOverrideInfoTableLayoutPanel
            // 
            this.GetOverrideInfoTableLayoutPanel.ColumnCount = 4;
            this.GetOverrideInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.GetOverrideInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.GetOverrideInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.GetOverrideInfoTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.GetOverrideInfoTableLayoutPanel.Controls.Add(this.GetSetOverrideTableLayoutPanel, 0, 0);
            this.GetOverrideInfoTableLayoutPanel.Controls.Add(this.GetRapidGroupBox, 3, 0);
            this.GetOverrideInfoTableLayoutPanel.Controls.Add(this.GetSpeedGroupBox, 2, 0);
            this.GetOverrideInfoTableLayoutPanel.Controls.Add(this.GetFeedGroupBox, 1, 0);
            this.GetOverrideInfoTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetOverrideInfoTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.GetOverrideInfoTableLayoutPanel.Name = "GetOverrideInfoTableLayoutPanel";
            this.GetOverrideInfoTableLayoutPanel.RowCount = 1;
            this.GetOverrideInfoTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GetOverrideInfoTableLayoutPanel.Size = new System.Drawing.Size(682, 49);
            this.GetOverrideInfoTableLayoutPanel.TabIndex = 1;
            // 
            // GetSetOverrideTableLayoutPanel
            // 
            this.GetSetOverrideTableLayoutPanel.ColumnCount = 1;
            this.GetSetOverrideTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GetSetOverrideTableLayoutPanel.Controls.Add(this.GetOverrideInfoButton, 0, 0);
            this.GetSetOverrideTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetSetOverrideTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.GetSetOverrideTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.GetSetOverrideTableLayoutPanel.Name = "GetSetOverrideTableLayoutPanel";
            this.GetSetOverrideTableLayoutPanel.RowCount = 1;
            this.GetSetOverrideTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GetSetOverrideTableLayoutPanel.Size = new System.Drawing.Size(56, 49);
            this.GetSetOverrideTableLayoutPanel.TabIndex = 2;
            // 
            // GetOverrideInfoButton
            // 
            this.GetOverrideInfoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetOverrideInfoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetOverrideInfoButton.Location = new System.Drawing.Point(0, 0);
            this.GetOverrideInfoButton.Margin = new System.Windows.Forms.Padding(0);
            this.GetOverrideInfoButton.Name = "GetOverrideInfoButton";
            this.GetOverrideInfoButton.Size = new System.Drawing.Size(56, 49);
            this.GetOverrideInfoButton.TabIndex = 0;
            this.GetOverrideInfoButton.Text = "GET";
            this.GetOverrideInfoButton.UseVisualStyleBackColor = true;
            this.GetOverrideInfoButton.Click += new System.EventHandler(this.GetOverrideInfoButton_Click);
            // 
            // GetRapidGroupBox
            // 
            this.GetRapidGroupBox.Controls.Add(this.RapidTableLayoutPanel);
            this.GetRapidGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetRapidGroupBox.Location = new System.Drawing.Point(475, 3);
            this.GetRapidGroupBox.Name = "GetRapidGroupBox";
            this.GetRapidGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.GetRapidGroupBox.Size = new System.Drawing.Size(204, 43);
            this.GetRapidGroupBox.TabIndex = 1;
            this.GetRapidGroupBox.TabStop = false;
            this.GetRapidGroupBox.Text = "Rapid (- %)";
            // 
            // RapidTableLayoutPanel
            // 
            this.RapidTableLayoutPanel.ColumnCount = 2;
            this.RapidTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RapidTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.RapidTableLayoutPanel.Controls.Add(this.SetRapidButton, 1, 0);
            this.RapidTableLayoutPanel.Controls.Add(this.RapidTextBox, 0, 0);
            this.RapidTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RapidTableLayoutPanel.Location = new System.Drawing.Point(5, 18);
            this.RapidTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.RapidTableLayoutPanel.Name = "RapidTableLayoutPanel";
            this.RapidTableLayoutPanel.RowCount = 1;
            this.RapidTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RapidTableLayoutPanel.Size = new System.Drawing.Size(194, 20);
            this.RapidTableLayoutPanel.TabIndex = 1;
            // 
            // SetRapidButton
            // 
            this.SetRapidButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetRapidButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetRapidButton.Location = new System.Drawing.Point(154, 0);
            this.SetRapidButton.Margin = new System.Windows.Forms.Padding(0);
            this.SetRapidButton.Name = "SetRapidButton";
            this.SetRapidButton.Size = new System.Drawing.Size(40, 20);
            this.SetRapidButton.TabIndex = 3;
            this.SetRapidButton.Text = "SET";
            this.SetRapidButton.UseVisualStyleBackColor = true;
            this.SetRapidButton.Click += new System.EventHandler(this.SetRapidButton_Click);
            // 
            // RapidTextBox
            // 
            this.RapidTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RapidTextBox.Location = new System.Drawing.Point(0, 0);
            this.RapidTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.RapidTextBox.Name = "RapidTextBox";
            this.RapidTextBox.Size = new System.Drawing.Size(154, 20);
            this.RapidTextBox.TabIndex = 0;
            // 
            // GetSpeedGroupBox
            // 
            this.GetSpeedGroupBox.Controls.Add(this.SpeedTableLayoutPanel);
            this.GetSpeedGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetSpeedGroupBox.Location = new System.Drawing.Point(267, 3);
            this.GetSpeedGroupBox.Name = "GetSpeedGroupBox";
            this.GetSpeedGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.GetSpeedGroupBox.Size = new System.Drawing.Size(202, 43);
            this.GetSpeedGroupBox.TabIndex = 3;
            this.GetSpeedGroupBox.TabStop = false;
            this.GetSpeedGroupBox.Text = "Speed (- %)";
            // 
            // SpeedTableLayoutPanel
            // 
            this.SpeedTableLayoutPanel.ColumnCount = 2;
            this.SpeedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SpeedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SpeedTableLayoutPanel.Controls.Add(this.SetSpeedButton, 1, 0);
            this.SpeedTableLayoutPanel.Controls.Add(this.SpeedTextBox, 0, 0);
            this.SpeedTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpeedTableLayoutPanel.Location = new System.Drawing.Point(5, 18);
            this.SpeedTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SpeedTableLayoutPanel.Name = "SpeedTableLayoutPanel";
            this.SpeedTableLayoutPanel.RowCount = 1;
            this.SpeedTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SpeedTableLayoutPanel.Size = new System.Drawing.Size(192, 20);
            this.SpeedTableLayoutPanel.TabIndex = 1;
            // 
            // SetSpeedButton
            // 
            this.SetSpeedButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetSpeedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetSpeedButton.Location = new System.Drawing.Point(152, 0);
            this.SetSpeedButton.Margin = new System.Windows.Forms.Padding(0);
            this.SetSpeedButton.Name = "SetSpeedButton";
            this.SetSpeedButton.Size = new System.Drawing.Size(40, 20);
            this.SetSpeedButton.TabIndex = 2;
            this.SetSpeedButton.Text = "SET";
            this.SetSpeedButton.UseVisualStyleBackColor = true;
            this.SetSpeedButton.Click += new System.EventHandler(this.SetSpeedButton_Click);
            // 
            // SpeedTextBox
            // 
            this.SpeedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpeedTextBox.Location = new System.Drawing.Point(0, 0);
            this.SpeedTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.SpeedTextBox.Name = "SpeedTextBox";
            this.SpeedTextBox.Size = new System.Drawing.Size(152, 20);
            this.SpeedTextBox.TabIndex = 0;
            // 
            // GetFeedGroupBox
            // 
            this.GetFeedGroupBox.Controls.Add(this.FeedTableLayoutPanel);
            this.GetFeedGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetFeedGroupBox.Location = new System.Drawing.Point(59, 3);
            this.GetFeedGroupBox.Name = "GetFeedGroupBox";
            this.GetFeedGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.GetFeedGroupBox.Size = new System.Drawing.Size(202, 43);
            this.GetFeedGroupBox.TabIndex = 2;
            this.GetFeedGroupBox.TabStop = false;
            this.GetFeedGroupBox.Text = "Feed (- %)";
            // 
            // FeedTableLayoutPanel
            // 
            this.FeedTableLayoutPanel.ColumnCount = 2;
            this.FeedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FeedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.FeedTableLayoutPanel.Controls.Add(this.FeedTextBox, 0, 0);
            this.FeedTableLayoutPanel.Controls.Add(this.SetFeedButton, 1, 0);
            this.FeedTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FeedTableLayoutPanel.Location = new System.Drawing.Point(5, 18);
            this.FeedTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FeedTableLayoutPanel.Name = "FeedTableLayoutPanel";
            this.FeedTableLayoutPanel.RowCount = 1;
            this.FeedTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FeedTableLayoutPanel.Size = new System.Drawing.Size(192, 20);
            this.FeedTableLayoutPanel.TabIndex = 1;
            // 
            // FeedTextBox
            // 
            this.FeedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FeedTextBox.Location = new System.Drawing.Point(0, 0);
            this.FeedTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.FeedTextBox.Name = "FeedTextBox";
            this.FeedTextBox.Size = new System.Drawing.Size(152, 20);
            this.FeedTextBox.TabIndex = 0;
            // 
            // SetFeedButton
            // 
            this.SetFeedButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetFeedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetFeedButton.Location = new System.Drawing.Point(152, 0);
            this.SetFeedButton.Margin = new System.Windows.Forms.Padding(0);
            this.SetFeedButton.Name = "SetFeedButton";
            this.SetFeedButton.Size = new System.Drawing.Size(40, 20);
            this.SetFeedButton.TabIndex = 1;
            this.SetFeedButton.Text = "SET";
            this.SetFeedButton.UseVisualStyleBackColor = true;
            this.SetFeedButton.Click += new System.EventHandler(this.SetFeedButton_Click);
            // 
            // ProgramHandlingGroupBox
            // 
            this.ProgramHandlingGroupBox.Controls.Add(this.ProgramHandlingControlsTableLayoutPanel);
            this.ProgramHandlingGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgramHandlingGroupBox.Location = new System.Drawing.Point(3, 83);
            this.ProgramHandlingGroupBox.Name = "ProgramHandlingGroupBox";
            this.ProgramHandlingGroupBox.Size = new System.Drawing.Size(764, 93);
            this.ProgramHandlingGroupBox.TabIndex = 1;
            this.ProgramHandlingGroupBox.TabStop = false;
            this.ProgramHandlingGroupBox.Text = "ProgramHandling";
            // 
            // ProgramHandlingControlsTableLayoutPanel
            // 
            this.ProgramHandlingControlsTableLayoutPanel.ColumnCount = 1;
            this.ProgramHandlingControlsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ProgramHandlingControlsTableLayoutPanel.Controls.Add(this.ProgramHandlingSelectTableLayoutPanel, 0, 0);
            this.ProgramHandlingControlsTableLayoutPanel.Controls.Add(this.ProgramStartStopCancelTableLayoutPanel, 0, 1);
            this.ProgramHandlingControlsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgramHandlingControlsTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.ProgramHandlingControlsTableLayoutPanel.Name = "ProgramHandlingControlsTableLayoutPanel";
            this.ProgramHandlingControlsTableLayoutPanel.RowCount = 2;
            this.ProgramHandlingControlsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.ProgramHandlingControlsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ProgramHandlingControlsTableLayoutPanel.Size = new System.Drawing.Size(758, 74);
            this.ProgramHandlingControlsTableLayoutPanel.TabIndex = 5;
            // 
            // ProgramHandlingSelectTableLayoutPanel
            // 
            this.ProgramHandlingSelectTableLayoutPanel.ColumnCount = 4;
            this.ProgramHandlingSelectTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.ProgramHandlingSelectTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ProgramHandlingSelectTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ProgramHandlingSelectTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ProgramHandlingSelectTableLayoutPanel.Controls.Add(this.StartProgramButton, 0, 0);
            this.ProgramHandlingSelectTableLayoutPanel.Controls.Add(this.StartBlockNumberNumericUpDown, 2, 0);
            this.ProgramHandlingSelectTableLayoutPanel.Controls.Add(this.SelectProgramButton, 3, 0);
            this.ProgramHandlingSelectTableLayoutPanel.Controls.Add(this.SelectProgramTextBox, 1, 0);
            this.ProgramHandlingSelectTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgramHandlingSelectTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ProgramHandlingSelectTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramHandlingSelectTableLayoutPanel.Name = "ProgramHandlingSelectTableLayoutPanel";
            this.ProgramHandlingSelectTableLayoutPanel.RowCount = 1;
            this.ProgramHandlingSelectTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ProgramHandlingSelectTableLayoutPanel.Size = new System.Drawing.Size(758, 27);
            this.ProgramHandlingSelectTableLayoutPanel.TabIndex = 0;
            // 
            // StartProgramButton
            // 
            this.StartProgramButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartProgramButton.Location = new System.Drawing.Point(3, 3);
            this.StartProgramButton.Name = "StartProgramButton";
            this.StartProgramButton.Size = new System.Drawing.Size(114, 21);
            this.StartProgramButton.TabIndex = 5;
            this.StartProgramButton.Text = "Start program";
            this.StartProgramButton.UseVisualStyleBackColor = true;
            this.StartProgramButton.Click += new System.EventHandler(this.StartProgramButton_Click);
            // 
            // StartBlockNumberNumericUpDown
            // 
            this.StartBlockNumberNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartBlockNumberNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartBlockNumberNumericUpDown.Location = new System.Drawing.Point(581, 3);
            this.StartBlockNumberNumericUpDown.Name = "StartBlockNumberNumericUpDown";
            this.StartBlockNumberNumericUpDown.Size = new System.Drawing.Size(74, 22);
            this.StartBlockNumberNumericUpDown.TabIndex = 4;
            // 
            // SelectProgramButton
            // 
            this.SelectProgramButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectProgramButton.Location = new System.Drawing.Point(658, 0);
            this.SelectProgramButton.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.SelectProgramButton.Name = "SelectProgramButton";
            this.SelectProgramButton.Size = new System.Drawing.Size(94, 27);
            this.SelectProgramButton.TabIndex = 3;
            this.SelectProgramButton.Text = "Select";
            this.SelectProgramButton.UseVisualStyleBackColor = true;
            this.SelectProgramButton.Click += new System.EventHandler(this.SelectProgramButton_Click);
            // 
            // SelectProgramTextBox
            // 
            this.SelectProgramTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectProgramTextBox.Location = new System.Drawing.Point(126, 3);
            this.SelectProgramTextBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.SelectProgramTextBox.Name = "SelectProgramTextBox";
            this.SelectProgramTextBox.Size = new System.Drawing.Size(449, 20);
            this.SelectProgramTextBox.TabIndex = 1;
            // 
            // ProgramStartStopCancelTableLayoutPanel
            // 
            this.ProgramStartStopCancelTableLayoutPanel.ColumnCount = 3;
            this.ProgramStartStopCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ProgramStartStopCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ProgramStartStopCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ProgramStartStopCancelTableLayoutPanel.Controls.Add(this.NCStartButton, 0, 0);
            this.ProgramStartStopCancelTableLayoutPanel.Controls.Add(this.StopProgramButton, 1, 0);
            this.ProgramStartStopCancelTableLayoutPanel.Controls.Add(this.CancelProgramButton, 2, 0);
            this.ProgramStartStopCancelTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgramStartStopCancelTableLayoutPanel.Location = new System.Drawing.Point(3, 30);
            this.ProgramStartStopCancelTableLayoutPanel.Name = "ProgramStartStopCancelTableLayoutPanel";
            this.ProgramStartStopCancelTableLayoutPanel.RowCount = 1;
            this.ProgramStartStopCancelTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ProgramStartStopCancelTableLayoutPanel.Size = new System.Drawing.Size(752, 41);
            this.ProgramStartStopCancelTableLayoutPanel.TabIndex = 1;
            // 
            // NCStartButton
            // 
            this.NCStartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NCStartButton.Location = new System.Drawing.Point(3, 3);
            this.NCStartButton.Name = "NCStartButton";
            this.NCStartButton.Size = new System.Drawing.Size(244, 35);
            this.NCStartButton.TabIndex = 5;
            this.NCStartButton.Text = "NC-Start";
            this.NCStartButton.UseVisualStyleBackColor = true;
            this.NCStartButton.Click += new System.EventHandler(this.NCStartButton_Click);
            // 
            // StopProgramButton
            // 
            this.StopProgramButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StopProgramButton.Location = new System.Drawing.Point(253, 3);
            this.StopProgramButton.Name = "StopProgramButton";
            this.StopProgramButton.Size = new System.Drawing.Size(244, 35);
            this.StopProgramButton.TabIndex = 3;
            this.StopProgramButton.Text = "Stop";
            this.StopProgramButton.UseVisualStyleBackColor = true;
            this.StopProgramButton.Click += new System.EventHandler(this.StopProgramButton_Click);
            // 
            // CancelProgramButton
            // 
            this.CancelProgramButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelProgramButton.Location = new System.Drawing.Point(503, 3);
            this.CancelProgramButton.Name = "CancelProgramButton";
            this.CancelProgramButton.Size = new System.Drawing.Size(246, 35);
            this.CancelProgramButton.TabIndex = 4;
            this.CancelProgramButton.Text = "Cancel";
            this.CancelProgramButton.UseVisualStyleBackColor = true;
            this.CancelProgramButton.Click += new System.EventHandler(this.CancelProgramButton_Click);
            // 
            // ChannelNumberGroupBox
            // 
            this.ChannelNumberGroupBox.Controls.Add(this.ChannelNumericUpDown);
            this.ChannelNumberGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelNumberGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ChannelNumberGroupBox.Name = "ChannelNumberGroupBox";
            this.ChannelNumberGroupBox.Size = new System.Drawing.Size(64, 68);
            this.ChannelNumberGroupBox.TabIndex = 0;
            this.ChannelNumberGroupBox.TabStop = false;
            this.ChannelNumberGroupBox.Text = "Channel";
            // 
            // ChannelNumericUpDown
            // 
            this.ChannelNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChannelNumericUpDown.Location = new System.Drawing.Point(3, 16);
            this.ChannelNumericUpDown.Name = "ChannelNumericUpDown";
            this.ChannelNumericUpDown.Size = new System.Drawing.Size(58, 22);
            this.ChannelNumericUpDown.TabIndex = 0;
            // 
            // CutterLocationGroupBox
            // 
            this.CutterLocationGroupBox.Controls.Add(this.GetCutterLocationTableLayoutPanel);
            this.CutterLocationGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CutterLocationGroupBox.Location = new System.Drawing.Point(3, 182);
            this.CutterLocationGroupBox.Name = "CutterLocationGroupBox";
            this.CutterLocationGroupBox.Size = new System.Drawing.Size(764, 207);
            this.CutterLocationGroupBox.TabIndex = 2;
            this.CutterLocationGroupBox.TabStop = false;
            this.CutterLocationGroupBox.Text = "GetCutterLocation";
            // 
            // GetCutterLocationTableLayoutPanel
            // 
            this.GetCutterLocationTableLayoutPanel.ColumnCount = 1;
            this.GetCutterLocationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GetCutterLocationTableLayoutPanel.Controls.Add(this.CutterLocationListView, 0, 1);
            this.GetCutterLocationTableLayoutPanel.Controls.Add(this.GetCutterLocationButton, 0, 0);
            this.GetCutterLocationTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetCutterLocationTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.GetCutterLocationTableLayoutPanel.Name = "GetCutterLocationTableLayoutPanel";
            this.GetCutterLocationTableLayoutPanel.RowCount = 2;
            this.GetCutterLocationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.GetCutterLocationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GetCutterLocationTableLayoutPanel.Size = new System.Drawing.Size(758, 188);
            this.GetCutterLocationTableLayoutPanel.TabIndex = 0;
            // 
            // CutterLocationListView
            // 
            this.CutterLocationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChoordNameColumnHeader,
            this.PositionColumnHeader,
            this.UnitColumnHeader});
            this.CutterLocationListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CutterLocationListView.FullRowSelect = true;
            this.CutterLocationListView.GridLines = true;
            this.CutterLocationListView.HideSelection = false;
            this.CutterLocationListView.Location = new System.Drawing.Point(3, 33);
            this.CutterLocationListView.Name = "CutterLocationListView";
            this.CutterLocationListView.Size = new System.Drawing.Size(752, 152);
            this.CutterLocationListView.TabIndex = 0;
            this.CutterLocationListView.UseCompatibleStateImageBehavior = false;
            this.CutterLocationListView.View = System.Windows.Forms.View.Details;
            // 
            // ChoordNameColumnHeader
            // 
            this.ChoordNameColumnHeader.Text = "Choord";
            // 
            // PositionColumnHeader
            // 
            this.PositionColumnHeader.Text = "Position";
            // 
            // UnitColumnHeader
            // 
            this.UnitColumnHeader.Text = "Unit";
            // 
            // GetCutterLocationButton
            // 
            this.GetCutterLocationButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetCutterLocationButton.Location = new System.Drawing.Point(3, 3);
            this.GetCutterLocationButton.Name = "GetCutterLocationButton";
            this.GetCutterLocationButton.Size = new System.Drawing.Size(752, 24);
            this.GetCutterLocationButton.TabIndex = 1;
            this.GetCutterLocationButton.Text = "GetCutterLocation";
            this.GetCutterLocationButton.UseVisualStyleBackColor = true;
            this.GetCutterLocationButton.Click += new System.EventHandler(this.GetCutterLocationButton_Click);
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.CutterLocationGroupBox, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.HeadTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ProgramHandlingGroupBox, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.ExecutionPointGroupBox, 0, 3);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 4;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 213F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.TabIndex = 3;
            // 
            // HeadTableLayoutPanel
            // 
            this.HeadTableLayoutPanel.ColumnCount = 2;
            this.HeadTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.HeadTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HeadTableLayoutPanel.Controls.Add(this.ChannelNumberGroupBox, 0, 0);
            this.HeadTableLayoutPanel.Controls.Add(this.OverrideInfoGroupBox, 1, 0);
            this.HeadTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeadTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.HeadTableLayoutPanel.Name = "HeadTableLayoutPanel";
            this.HeadTableLayoutPanel.RowCount = 1;
            this.HeadTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.HeadTableLayoutPanel.Size = new System.Drawing.Size(764, 74);
            this.HeadTableLayoutPanel.TabIndex = 3;
            // 
            // ExecutionPointGroupBox
            // 
            this.ExecutionPointGroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ExecutionPointGroupBox.Controls.Add(this.ExecutionPointTableLayoutPanel);
            this.ExecutionPointGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecutionPointGroupBox.Location = new System.Drawing.Point(3, 395);
            this.ExecutionPointGroupBox.Name = "ExecutionPointGroupBox";
            this.ExecutionPointGroupBox.Size = new System.Drawing.Size(764, 68);
            this.ExecutionPointGroupBox.TabIndex = 4;
            this.ExecutionPointGroupBox.TabStop = false;
            this.ExecutionPointGroupBox.Text = "GetExecutionPoint";
            // 
            // ExecutionPointTableLayoutPanel
            // 
            this.ExecutionPointTableLayoutPanel.ColumnCount = 2;
            this.ExecutionPointTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.51187F));
            this.ExecutionPointTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.48813F));
            this.ExecutionPointTableLayoutPanel.Controls.Add(this.ExecutionPointTextBox, 0, 0);
            this.ExecutionPointTableLayoutPanel.Controls.Add(this.GetExecutionPointButton, 0, 0);
            this.ExecutionPointTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecutionPointTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.ExecutionPointTableLayoutPanel.Name = "ExecutionPointTableLayoutPanel";
            this.ExecutionPointTableLayoutPanel.RowCount = 1;
            this.ExecutionPointTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ExecutionPointTableLayoutPanel.Size = new System.Drawing.Size(758, 49);
            this.ExecutionPointTableLayoutPanel.TabIndex = 2;
            // 
            // ExecutionPointTextBox
            // 
            this.ExecutionPointTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecutionPointTextBox.Location = new System.Drawing.Point(115, 3);
            this.ExecutionPointTextBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.ExecutionPointTextBox.Multiline = true;
            this.ExecutionPointTextBox.Name = "ExecutionPointTextBox";
            this.ExecutionPointTextBox.Size = new System.Drawing.Size(640, 43);
            this.ExecutionPointTextBox.TabIndex = 3;
            // 
            // GetExecutionPointButton
            // 
            this.GetExecutionPointButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetExecutionPointButton.Location = new System.Drawing.Point(3, 3);
            this.GetExecutionPointButton.Name = "GetExecutionPointButton";
            this.GetExecutionPointButton.Size = new System.Drawing.Size(103, 43);
            this.GetExecutionPointButton.TabIndex = 2;
            this.GetExecutionPointButton.Text = "GetExecutionPoint";
            this.GetExecutionPointButton.UseVisualStyleBackColor = true;
            this.GetExecutionPointButton.Click += new System.EventHandler(this.GetExecutionPointButton_Click);
            // 
            // Automatic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "Automatic";
            this.Size = new System.Drawing.Size(770, 466);
            this.Load += new System.EventHandler(this.Automatic_Load);
            this.OverrideInfoGroupBox.ResumeLayout(false);
            this.GetOverrideInfoTableLayoutPanel.ResumeLayout(false);
            this.GetSetOverrideTableLayoutPanel.ResumeLayout(false);
            this.GetRapidGroupBox.ResumeLayout(false);
            this.RapidTableLayoutPanel.ResumeLayout(false);
            this.RapidTableLayoutPanel.PerformLayout();
            this.GetSpeedGroupBox.ResumeLayout(false);
            this.SpeedTableLayoutPanel.ResumeLayout(false);
            this.SpeedTableLayoutPanel.PerformLayout();
            this.GetFeedGroupBox.ResumeLayout(false);
            this.FeedTableLayoutPanel.ResumeLayout(false);
            this.FeedTableLayoutPanel.PerformLayout();
            this.ProgramHandlingGroupBox.ResumeLayout(false);
            this.ProgramHandlingControlsTableLayoutPanel.ResumeLayout(false);
            this.ProgramHandlingSelectTableLayoutPanel.ResumeLayout(false);
            this.ProgramHandlingSelectTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartBlockNumberNumericUpDown)).EndInit();
            this.ProgramStartStopCancelTableLayoutPanel.ResumeLayout(false);
            this.ChannelNumberGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChannelNumericUpDown)).EndInit();
            this.CutterLocationGroupBox.ResumeLayout(false);
            this.GetCutterLocationTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.HeadTableLayoutPanel.ResumeLayout(false);
            this.ExecutionPointGroupBox.ResumeLayout(false);
            this.ExecutionPointTableLayoutPanel.ResumeLayout(false);
            this.ExecutionPointTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox OverrideInfoGroupBox;
        private System.Windows.Forms.TableLayoutPanel GetOverrideInfoTableLayoutPanel;
        private System.Windows.Forms.Button GetOverrideInfoButton;
        private System.Windows.Forms.GroupBox GetRapidGroupBox;
        private System.Windows.Forms.GroupBox GetSpeedGroupBox;
        private System.Windows.Forms.GroupBox GetFeedGroupBox;
        private System.Windows.Forms.TextBox SpeedTextBox;
        private System.Windows.Forms.TextBox FeedTextBox;
        private System.Windows.Forms.TextBox RapidTextBox;
        private System.Windows.Forms.TableLayoutPanel GetSetOverrideTableLayoutPanel;
        private System.Windows.Forms.GroupBox ProgramHandlingGroupBox;
        private System.Windows.Forms.GroupBox ChannelNumberGroupBox;
        private System.Windows.Forms.NumericUpDown ChannelNumericUpDown;
        private System.Windows.Forms.GroupBox CutterLocationGroupBox;
        private System.Windows.Forms.TableLayoutPanel GetCutterLocationTableLayoutPanel;
        private System.Windows.Forms.ListView CutterLocationListView;
        private System.Windows.Forms.ColumnHeader ChoordNameColumnHeader;
        private System.Windows.Forms.ColumnHeader PositionColumnHeader;
        private System.Windows.Forms.ColumnHeader UnitColumnHeader;
        private System.Windows.Forms.Button GetCutterLocationButton;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ProgramHandlingControlsTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ProgramHandlingSelectTableLayoutPanel;
        private System.Windows.Forms.TextBox SelectProgramTextBox;
        private System.Windows.Forms.TableLayoutPanel ProgramStartStopCancelTableLayoutPanel;
        private System.Windows.Forms.Button StopProgramButton;
        private System.Windows.Forms.Button CancelProgramButton;
        private System.Windows.Forms.TableLayoutPanel HeadTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel FeedTableLayoutPanel;
        private System.Windows.Forms.Button SetFeedButton;
        private System.Windows.Forms.TableLayoutPanel RapidTableLayoutPanel;
        private System.Windows.Forms.Button SetRapidButton;
        private System.Windows.Forms.TableLayoutPanel SpeedTableLayoutPanel;
        private System.Windows.Forms.Button SetSpeedButton;
        private System.Windows.Forms.GroupBox ExecutionPointGroupBox;
        private System.Windows.Forms.TableLayoutPanel ExecutionPointTableLayoutPanel;
        private System.Windows.Forms.TextBox ExecutionPointTextBox;
        private System.Windows.Forms.Button GetExecutionPointButton;
        private System.Windows.Forms.NumericUpDown StartBlockNumberNumericUpDown;
        private System.Windows.Forms.Button SelectProgramButton;
        private System.Windows.Forms.Button NCStartButton;
        private System.Windows.Forms.Button StartProgramButton;
    }
}
