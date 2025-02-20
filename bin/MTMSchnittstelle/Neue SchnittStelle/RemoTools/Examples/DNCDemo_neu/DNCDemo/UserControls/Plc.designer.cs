namespace DNC_CSharp_Demo.UserControls
{
  partial class Plc
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
            this.CommunicationTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SendGroupBox = new System.Windows.Forms.GroupBox();
            this.SendTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SendStringGroupBox = new System.Windows.Forms.GroupBox();
            this.SendStringTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SendStringTextBox = new System.Windows.Forms.TextBox();
            this.StringCheckBox = new System.Windows.Forms.CheckBox();
            this.SendDataTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SendDWordGroupBox = new System.Windows.Forms.GroupBox();
            this.SendDWordTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SendDWordListView = new System.Windows.Forms.ListView();
            this.SendDWordColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SendDWordActionsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AddDWordButton = new System.Windows.Forms.Button();
            this.DWordNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RemoveDWordButton = new System.Windows.Forms.Button();
            this.DWordCheckBox = new System.Windows.Forms.CheckBox();
            this.SendMarkerGroupBox = new System.Windows.Forms.GroupBox();
            this.SendMarkerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SendMarkerListView = new System.Windows.Forms.ListView();
            this.SendMarkerColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SendMarkerActionsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RemoveMarkerButton = new System.Windows.Forms.Button();
            this.AddMarkerButton = new System.Windows.Forms.Button();
            this.MarkerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.MarkerCheckBox = new System.Windows.Forms.CheckBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.ReceiveGroupBox = new System.Windows.Forms.GroupBox();
            this.ReceiveTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ReceiveStringGroupBox = new System.Windows.Forms.GroupBox();
            this.ReceiveStringTextBox = new System.Windows.Forms.TextBox();
            this.ReceiveDataTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ReceiveDWordGroupBox = new System.Windows.Forms.GroupBox();
            this.ReceiveDWordListView = new System.Windows.Forms.ListView();
            this.ReceiveDWordColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReceiveMarkerGroupBox = new System.Windows.Forms.GroupBox();
            this.ReceiveMarkerListView = new System.Windows.Forms.ListView();
            this.ReceiveMarkerColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CncTypeLabel = new System.Windows.Forms.Label();
            this.CommunicationTableLayoutPanel.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SendGroupBox.SuspendLayout();
            this.SendTableLayoutPanel.SuspendLayout();
            this.SendStringGroupBox.SuspendLayout();
            this.SendStringTableLayoutPanel.SuspendLayout();
            this.SendDataTableLayoutPanel.SuspendLayout();
            this.SendDWordGroupBox.SuspendLayout();
            this.SendDWordTableLayoutPanel.SuspendLayout();
            this.SendDWordActionsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DWordNumericUpDown)).BeginInit();
            this.SendMarkerGroupBox.SuspendLayout();
            this.SendMarkerTableLayoutPanel.SuspendLayout();
            this.SendMarkerActionsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MarkerNumericUpDown)).BeginInit();
            this.ReceiveGroupBox.SuspendLayout();
            this.ReceiveTableLayoutPanel.SuspendLayout();
            this.ReceiveStringGroupBox.SuspendLayout();
            this.ReceiveDataTableLayoutPanel.SuspendLayout();
            this.ReceiveDWordGroupBox.SuspendLayout();
            this.ReceiveMarkerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // CommunicationTableLayoutPanel
            // 
            this.CommunicationTableLayoutPanel.ColumnCount = 1;
            this.CommunicationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.CommunicationTableLayoutPanel.Controls.Add(this.MainTableLayoutPanel, 0, 1);
            this.CommunicationTableLayoutPanel.Controls.Add(this.CncTypeLabel, 0, 0);
            this.CommunicationTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CommunicationTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.CommunicationTableLayoutPanel.Name = "CommunicationTableLayoutPanel";
            this.CommunicationTableLayoutPanel.RowCount = 2;
            this.CommunicationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.CommunicationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.CommunicationTableLayoutPanel.Size = new System.Drawing.Size(1000, 500);
            this.CommunicationTableLayoutPanel.TabIndex = 0;
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 2;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.Controls.Add(this.SendGroupBox, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ReceiveGroupBox, 1, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(3, 28);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(994, 469);
            this.MainTableLayoutPanel.TabIndex = 2;
            // 
            // SendGroupBox
            // 
            this.SendGroupBox.Controls.Add(this.SendTableLayoutPanel);
            this.SendGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendGroupBox.Location = new System.Drawing.Point(3, 3);
            this.SendGroupBox.Name = "SendGroupBox";
            this.SendGroupBox.Size = new System.Drawing.Size(491, 463);
            this.SendGroupBox.TabIndex = 3;
            this.SendGroupBox.TabStop = false;
            this.SendGroupBox.Text = "Send (counter = 0)";
            // 
            // SendTableLayoutPanel
            // 
            this.SendTableLayoutPanel.ColumnCount = 1;
            this.SendTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendTableLayoutPanel.Controls.Add(this.SendStringGroupBox, 0, 1);
            this.SendTableLayoutPanel.Controls.Add(this.SendDataTableLayoutPanel, 0, 2);
            this.SendTableLayoutPanel.Controls.Add(this.SendButton, 0, 0);
            this.SendTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendTableLayoutPanel.Location = new System.Drawing.Point(3, 25);
            this.SendTableLayoutPanel.Name = "SendTableLayoutPanel";
            this.SendTableLayoutPanel.RowCount = 3;
            this.SendTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.SendTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.SendTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendTableLayoutPanel.Size = new System.Drawing.Size(485, 435);
            this.SendTableLayoutPanel.TabIndex = 0;
            // 
            // SendStringGroupBox
            // 
            this.SendStringGroupBox.Controls.Add(this.SendStringTableLayoutPanel);
            this.SendStringGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendStringGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendStringGroupBox.Location = new System.Drawing.Point(3, 33);
            this.SendStringGroupBox.Name = "SendStringGroupBox";
            this.SendStringGroupBox.Size = new System.Drawing.Size(479, 49);
            this.SendStringGroupBox.TabIndex = 1;
            this.SendStringGroupBox.TabStop = false;
            this.SendStringGroupBox.Text = "STRING (counter = 0)";
            // 
            // SendStringTableLayoutPanel
            // 
            this.SendStringTableLayoutPanel.ColumnCount = 2;
            this.SendStringTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.SendStringTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendStringTableLayoutPanel.Controls.Add(this.SendStringTextBox, 1, 0);
            this.SendStringTableLayoutPanel.Controls.Add(this.StringCheckBox, 0, 0);
            this.SendStringTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendStringTableLayoutPanel.Location = new System.Drawing.Point(3, 18);
            this.SendStringTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SendStringTableLayoutPanel.Name = "SendStringTableLayoutPanel";
            this.SendStringTableLayoutPanel.RowCount = 1;
            this.SendStringTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendStringTableLayoutPanel.Size = new System.Drawing.Size(473, 28);
            this.SendStringTableLayoutPanel.TabIndex = 0;
            // 
            // SendStringTextBox
            // 
            this.SendStringTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendStringTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendStringTextBox.Location = new System.Drawing.Point(73, 2);
            this.SendStringTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.SendStringTextBox.Name = "SendStringTextBox";
            this.SendStringTextBox.Size = new System.Drawing.Size(400, 24);
            this.SendStringTextBox.TabIndex = 0;
            this.SendStringTextBox.TextChanged += new System.EventHandler(this.SendStringTextBox_TextChanged);
            // 
            // StringCheckBox
            // 
            this.StringCheckBox.AutoSize = true;
            this.StringCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StringCheckBox.Location = new System.Drawing.Point(3, 3);
            this.StringCheckBox.Name = "StringCheckBox";
            this.StringCheckBox.Size = new System.Drawing.Size(64, 22);
            this.StringCheckBox.TabIndex = 1;
            this.StringCheckBox.Text = "send?";
            this.StringCheckBox.UseVisualStyleBackColor = true;
            // 
            // SendDataTableLayoutPanel
            // 
            this.SendDataTableLayoutPanel.ColumnCount = 2;
            this.SendDataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendDataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendDataTableLayoutPanel.Controls.Add(this.SendDWordGroupBox, 0, 0);
            this.SendDataTableLayoutPanel.Controls.Add(this.SendMarkerGroupBox, 0, 0);
            this.SendDataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendDataTableLayoutPanel.Location = new System.Drawing.Point(0, 85);
            this.SendDataTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SendDataTableLayoutPanel.Name = "SendDataTableLayoutPanel";
            this.SendDataTableLayoutPanel.RowCount = 1;
            this.SendDataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendDataTableLayoutPanel.Size = new System.Drawing.Size(485, 350);
            this.SendDataTableLayoutPanel.TabIndex = 2;
            // 
            // SendDWordGroupBox
            // 
            this.SendDWordGroupBox.Controls.Add(this.SendDWordTableLayoutPanel);
            this.SendDWordGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendDWordGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendDWordGroupBox.Location = new System.Drawing.Point(245, 3);
            this.SendDWordGroupBox.Name = "SendDWordGroupBox";
            this.SendDWordGroupBox.Size = new System.Drawing.Size(237, 344);
            this.SendDWordGroupBox.TabIndex = 2;
            this.SendDWordGroupBox.TabStop = false;
            this.SendDWordGroupBox.Text = "(counter = 0)";
            // 
            // SendDWordTableLayoutPanel
            // 
            this.SendDWordTableLayoutPanel.ColumnCount = 1;
            this.SendDWordTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendDWordTableLayoutPanel.Controls.Add(this.SendDWordListView, 0, 1);
            this.SendDWordTableLayoutPanel.Controls.Add(this.SendDWordActionsTableLayoutPanel, 0, 0);
            this.SendDWordTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendDWordTableLayoutPanel.Location = new System.Drawing.Point(3, 18);
            this.SendDWordTableLayoutPanel.Name = "SendDWordTableLayoutPanel";
            this.SendDWordTableLayoutPanel.RowCount = 2;
            this.SendDWordTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.SendDWordTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendDWordTableLayoutPanel.Size = new System.Drawing.Size(231, 323);
            this.SendDWordTableLayoutPanel.TabIndex = 1;
            // 
            // SendDWordListView
            // 
            this.SendDWordListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SendDWordColumnHeader});
            this.SendDWordListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendDWordListView.GridLines = true;
            this.SendDWordListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SendDWordListView.LabelEdit = true;
            this.SendDWordListView.Location = new System.Drawing.Point(3, 37);
            this.SendDWordListView.MultiSelect = false;
            this.SendDWordListView.Name = "SendDWordListView";
            this.SendDWordListView.Size = new System.Drawing.Size(225, 283);
            this.SendDWordListView.TabIndex = 4;
            this.SendDWordListView.UseCompatibleStateImageBehavior = false;
            this.SendDWordListView.View = System.Windows.Forms.View.Details;
            // 
            // SendDWordColumnHeader
            // 
            this.SendDWordColumnHeader.Text = "DWORD";
            this.SendDWordColumnHeader.Width = 200;
            // 
            // SendDWordActionsTableLayoutPanel
            // 
            this.SendDWordActionsTableLayoutPanel.ColumnCount = 4;
            this.SendDWordActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SendDWordActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendDWordActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendDWordActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.SendDWordActionsTableLayoutPanel.Controls.Add(this.AddDWordButton, 1, 0);
            this.SendDWordActionsTableLayoutPanel.Controls.Add(this.DWordNumericUpDown, 0, 0);
            this.SendDWordActionsTableLayoutPanel.Controls.Add(this.RemoveDWordButton, 2, 0);
            this.SendDWordActionsTableLayoutPanel.Controls.Add(this.DWordCheckBox, 3, 0);
            this.SendDWordActionsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendDWordActionsTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.SendDWordActionsTableLayoutPanel.Name = "SendDWordActionsTableLayoutPanel";
            this.SendDWordActionsTableLayoutPanel.RowCount = 1;
            this.SendDWordActionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendDWordActionsTableLayoutPanel.Size = new System.Drawing.Size(225, 28);
            this.SendDWordActionsTableLayoutPanel.TabIndex = 5;
            // 
            // AddDWordButton
            // 
            this.AddDWordButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddDWordButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddDWordButton.Location = new System.Drawing.Point(40, 0);
            this.AddDWordButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddDWordButton.Name = "AddDWordButton";
            this.AddDWordButton.Size = new System.Drawing.Size(56, 28);
            this.AddDWordButton.TabIndex = 8;
            this.AddDWordButton.Text = "+";
            this.AddDWordButton.UseVisualStyleBackColor = true;
            this.AddDWordButton.Click += new System.EventHandler(this.AddDWordButton_Click);
            // 
            // DWordNumericUpDown
            // 
            this.DWordNumericUpDown.Location = new System.Drawing.Point(3, 3);
            this.DWordNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DWordNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DWordNumericUpDown.Name = "DWordNumericUpDown";
            this.DWordNumericUpDown.Size = new System.Drawing.Size(34, 22);
            this.DWordNumericUpDown.TabIndex = 7;
            this.DWordNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RemoveDWordButton
            // 
            this.RemoveDWordButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveDWordButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveDWordButton.Location = new System.Drawing.Point(96, 0);
            this.RemoveDWordButton.Margin = new System.Windows.Forms.Padding(0);
            this.RemoveDWordButton.Name = "RemoveDWordButton";
            this.RemoveDWordButton.Size = new System.Drawing.Size(56, 28);
            this.RemoveDWordButton.TabIndex = 5;
            this.RemoveDWordButton.Text = "-";
            this.RemoveDWordButton.UseVisualStyleBackColor = true;
            this.RemoveDWordButton.Click += new System.EventHandler(this.RemoveDWordButton_Click);
            // 
            // DWordCheckBox
            // 
            this.DWordCheckBox.AutoSize = true;
            this.DWordCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DWordCheckBox.Location = new System.Drawing.Point(155, 3);
            this.DWordCheckBox.Name = "DWordCheckBox";
            this.DWordCheckBox.Size = new System.Drawing.Size(67, 22);
            this.DWordCheckBox.TabIndex = 9;
            this.DWordCheckBox.Text = "send?";
            this.DWordCheckBox.UseVisualStyleBackColor = true;
            // 
            // SendMarkerGroupBox
            // 
            this.SendMarkerGroupBox.Controls.Add(this.SendMarkerTableLayoutPanel);
            this.SendMarkerGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendMarkerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendMarkerGroupBox.Location = new System.Drawing.Point(3, 3);
            this.SendMarkerGroupBox.Name = "SendMarkerGroupBox";
            this.SendMarkerGroupBox.Size = new System.Drawing.Size(236, 344);
            this.SendMarkerGroupBox.TabIndex = 1;
            this.SendMarkerGroupBox.TabStop = false;
            this.SendMarkerGroupBox.Text = "(counter = 0)";
            // 
            // SendMarkerTableLayoutPanel
            // 
            this.SendMarkerTableLayoutPanel.ColumnCount = 1;
            this.SendMarkerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendMarkerTableLayoutPanel.Controls.Add(this.SendMarkerListView, 0, 1);
            this.SendMarkerTableLayoutPanel.Controls.Add(this.SendMarkerActionsTableLayoutPanel, 0, 0);
            this.SendMarkerTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendMarkerTableLayoutPanel.Location = new System.Drawing.Point(3, 18);
            this.SendMarkerTableLayoutPanel.Name = "SendMarkerTableLayoutPanel";
            this.SendMarkerTableLayoutPanel.RowCount = 2;
            this.SendMarkerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.SendMarkerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendMarkerTableLayoutPanel.Size = new System.Drawing.Size(230, 323);
            this.SendMarkerTableLayoutPanel.TabIndex = 0;
            // 
            // SendMarkerListView
            // 
            this.SendMarkerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SendMarkerColumnHeader});
            this.SendMarkerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendMarkerListView.GridLines = true;
            this.SendMarkerListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SendMarkerListView.LabelEdit = true;
            this.SendMarkerListView.Location = new System.Drawing.Point(3, 37);
            this.SendMarkerListView.MultiSelect = false;
            this.SendMarkerListView.Name = "SendMarkerListView";
            this.SendMarkerListView.Size = new System.Drawing.Size(224, 283);
            this.SendMarkerListView.TabIndex = 3;
            this.SendMarkerListView.UseCompatibleStateImageBehavior = false;
            this.SendMarkerListView.View = System.Windows.Forms.View.Details;
            // 
            // SendMarkerColumnHeader
            // 
            this.SendMarkerColumnHeader.Text = "MARKER";
            this.SendMarkerColumnHeader.Width = 200;
            // 
            // SendMarkerActionsTableLayoutPanel
            // 
            this.SendMarkerActionsTableLayoutPanel.ColumnCount = 4;
            this.SendMarkerActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SendMarkerActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendMarkerActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SendMarkerActionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.SendMarkerActionsTableLayoutPanel.Controls.Add(this.RemoveMarkerButton, 2, 0);
            this.SendMarkerActionsTableLayoutPanel.Controls.Add(this.AddMarkerButton, 1, 0);
            this.SendMarkerActionsTableLayoutPanel.Controls.Add(this.MarkerNumericUpDown, 0, 0);
            this.SendMarkerActionsTableLayoutPanel.Controls.Add(this.MarkerCheckBox, 3, 0);
            this.SendMarkerActionsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendMarkerActionsTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.SendMarkerActionsTableLayoutPanel.Name = "SendMarkerActionsTableLayoutPanel";
            this.SendMarkerActionsTableLayoutPanel.RowCount = 1;
            this.SendMarkerActionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SendMarkerActionsTableLayoutPanel.Size = new System.Drawing.Size(224, 28);
            this.SendMarkerActionsTableLayoutPanel.TabIndex = 4;
            // 
            // RemoveMarkerButton
            // 
            this.RemoveMarkerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveMarkerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveMarkerButton.Location = new System.Drawing.Point(84, 0);
            this.RemoveMarkerButton.Margin = new System.Windows.Forms.Padding(0);
            this.RemoveMarkerButton.Name = "RemoveMarkerButton";
            this.RemoveMarkerButton.Size = new System.Drawing.Size(44, 28);
            this.RemoveMarkerButton.TabIndex = 4;
            this.RemoveMarkerButton.Text = "-";
            this.RemoveMarkerButton.UseVisualStyleBackColor = true;
            this.RemoveMarkerButton.Click += new System.EventHandler(this.RemoveMarkerButton_Click);
            // 
            // AddMarkerButton
            // 
            this.AddMarkerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddMarkerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddMarkerButton.Location = new System.Drawing.Point(40, 0);
            this.AddMarkerButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddMarkerButton.Name = "AddMarkerButton";
            this.AddMarkerButton.Size = new System.Drawing.Size(44, 28);
            this.AddMarkerButton.TabIndex = 5;
            this.AddMarkerButton.Text = "+";
            this.AddMarkerButton.UseVisualStyleBackColor = true;
            this.AddMarkerButton.Click += new System.EventHandler(this.AddMarkerButton_Click);
            // 
            // MarkerNumericUpDown
            // 
            this.MarkerNumericUpDown.Location = new System.Drawing.Point(3, 3);
            this.MarkerNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MarkerNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MarkerNumericUpDown.Name = "MarkerNumericUpDown";
            this.MarkerNumericUpDown.Size = new System.Drawing.Size(34, 22);
            this.MarkerNumericUpDown.TabIndex = 6;
            this.MarkerNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MarkerCheckBox
            // 
            this.MarkerCheckBox.AutoSize = true;
            this.MarkerCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MarkerCheckBox.Location = new System.Drawing.Point(131, 3);
            this.MarkerCheckBox.Name = "MarkerCheckBox";
            this.MarkerCheckBox.Size = new System.Drawing.Size(90, 22);
            this.MarkerCheckBox.TabIndex = 7;
            this.MarkerCheckBox.Text = "send?";
            this.MarkerCheckBox.UseVisualStyleBackColor = true;
            // 
            // SendButton
            // 
            this.SendButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.SendButton.Location = new System.Drawing.Point(3, 0);
            this.SendButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(479, 30);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send selected";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ReceiveGroupBox
            // 
            this.ReceiveGroupBox.Controls.Add(this.ReceiveTableLayoutPanel);
            this.ReceiveGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceiveGroupBox.Location = new System.Drawing.Point(500, 3);
            this.ReceiveGroupBox.Name = "ReceiveGroupBox";
            this.ReceiveGroupBox.Size = new System.Drawing.Size(491, 463);
            this.ReceiveGroupBox.TabIndex = 1;
            this.ReceiveGroupBox.TabStop = false;
            this.ReceiveGroupBox.Text = "Receive (counter = 0)";
            // 
            // ReceiveTableLayoutPanel
            // 
            this.ReceiveTableLayoutPanel.ColumnCount = 1;
            this.ReceiveTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ReceiveTableLayoutPanel.Controls.Add(this.ReceiveStringGroupBox, 0, 0);
            this.ReceiveTableLayoutPanel.Controls.Add(this.ReceiveDataTableLayoutPanel, 0, 1);
            this.ReceiveTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveTableLayoutPanel.Location = new System.Drawing.Point(3, 25);
            this.ReceiveTableLayoutPanel.Name = "ReceiveTableLayoutPanel";
            this.ReceiveTableLayoutPanel.RowCount = 2;
            this.ReceiveTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.ReceiveTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ReceiveTableLayoutPanel.Size = new System.Drawing.Size(485, 435);
            this.ReceiveTableLayoutPanel.TabIndex = 0;
            // 
            // ReceiveStringGroupBox
            // 
            this.ReceiveStringGroupBox.Controls.Add(this.ReceiveStringTextBox);
            this.ReceiveStringGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveStringGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceiveStringGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ReceiveStringGroupBox.Name = "ReceiveStringGroupBox";
            this.ReceiveStringGroupBox.Size = new System.Drawing.Size(479, 49);
            this.ReceiveStringGroupBox.TabIndex = 0;
            this.ReceiveStringGroupBox.TabStop = false;
            this.ReceiveStringGroupBox.Text = "STRING (counter = 0)";
            // 
            // ReceiveStringTextBox
            // 
            this.ReceiveStringTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveStringTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceiveStringTextBox.Location = new System.Drawing.Point(3, 18);
            this.ReceiveStringTextBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.ReceiveStringTextBox.Name = "ReceiveStringTextBox";
            this.ReceiveStringTextBox.Size = new System.Drawing.Size(473, 24);
            this.ReceiveStringTextBox.TabIndex = 0;
            // 
            // ReceiveDataTableLayoutPanel
            // 
            this.ReceiveDataTableLayoutPanel.ColumnCount = 2;
            this.ReceiveDataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ReceiveDataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ReceiveDataTableLayoutPanel.Controls.Add(this.ReceiveDWordGroupBox, 1, 0);
            this.ReceiveDataTableLayoutPanel.Controls.Add(this.ReceiveMarkerGroupBox, 0, 0);
            this.ReceiveDataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveDataTableLayoutPanel.Location = new System.Drawing.Point(0, 55);
            this.ReceiveDataTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ReceiveDataTableLayoutPanel.Name = "ReceiveDataTableLayoutPanel";
            this.ReceiveDataTableLayoutPanel.RowCount = 1;
            this.ReceiveDataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ReceiveDataTableLayoutPanel.Size = new System.Drawing.Size(485, 380);
            this.ReceiveDataTableLayoutPanel.TabIndex = 1;
            // 
            // ReceiveDWordGroupBox
            // 
            this.ReceiveDWordGroupBox.Controls.Add(this.ReceiveDWordListView);
            this.ReceiveDWordGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveDWordGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceiveDWordGroupBox.Location = new System.Drawing.Point(245, 3);
            this.ReceiveDWordGroupBox.Name = "ReceiveDWordGroupBox";
            this.ReceiveDWordGroupBox.Size = new System.Drawing.Size(237, 374);
            this.ReceiveDWordGroupBox.TabIndex = 1;
            this.ReceiveDWordGroupBox.TabStop = false;
            this.ReceiveDWordGroupBox.Text = "(counter = 0)";
            // 
            // ReceiveDWordListView
            // 
            this.ReceiveDWordListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ReceiveDWordColumnHeader});
            this.ReceiveDWordListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveDWordListView.GridLines = true;
            this.ReceiveDWordListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ReceiveDWordListView.Location = new System.Drawing.Point(3, 18);
            this.ReceiveDWordListView.MultiSelect = false;
            this.ReceiveDWordListView.Name = "ReceiveDWordListView";
            this.ReceiveDWordListView.Size = new System.Drawing.Size(231, 353);
            this.ReceiveDWordListView.TabIndex = 1;
            this.ReceiveDWordListView.UseCompatibleStateImageBehavior = false;
            this.ReceiveDWordListView.View = System.Windows.Forms.View.Details;
            // 
            // ReceiveDWordColumnHeader
            // 
            this.ReceiveDWordColumnHeader.Text = "DWORD";
            this.ReceiveDWordColumnHeader.Width = 200;
            // 
            // ReceiveMarkerGroupBox
            // 
            this.ReceiveMarkerGroupBox.Controls.Add(this.ReceiveMarkerListView);
            this.ReceiveMarkerGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveMarkerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceiveMarkerGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ReceiveMarkerGroupBox.Name = "ReceiveMarkerGroupBox";
            this.ReceiveMarkerGroupBox.Size = new System.Drawing.Size(236, 374);
            this.ReceiveMarkerGroupBox.TabIndex = 0;
            this.ReceiveMarkerGroupBox.TabStop = false;
            this.ReceiveMarkerGroupBox.Text = "(counter = 0)";
            // 
            // ReceiveMarkerListView
            // 
            this.ReceiveMarkerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ReceiveMarkerColumnHeader});
            this.ReceiveMarkerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveMarkerListView.GridLines = true;
            this.ReceiveMarkerListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ReceiveMarkerListView.Location = new System.Drawing.Point(3, 18);
            this.ReceiveMarkerListView.MultiSelect = false;
            this.ReceiveMarkerListView.Name = "ReceiveMarkerListView";
            this.ReceiveMarkerListView.Size = new System.Drawing.Size(230, 353);
            this.ReceiveMarkerListView.TabIndex = 0;
            this.ReceiveMarkerListView.UseCompatibleStateImageBehavior = false;
            this.ReceiveMarkerListView.View = System.Windows.Forms.View.Details;
            // 
            // ReceiveMarkerColumnHeader
            // 
            this.ReceiveMarkerColumnHeader.Text = "MARKER";
            this.ReceiveMarkerColumnHeader.Width = 200;
            // 
            // CncTypeLabel
            // 
            this.CncTypeLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CncTypeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CncTypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CncTypeLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CncTypeLabel.Location = new System.Drawing.Point(3, 0);
            this.CncTypeLabel.Name = "CncTypeLabel";
            this.CncTypeLabel.Size = new System.Drawing.Size(994, 25);
            this.CncTypeLabel.TabIndex = 0;
            this.CncTypeLabel.Text = "CNC Type";
            this.CncTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Plc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CommunicationTableLayoutPanel);
            this.Name = "Plc";
            this.Size = new System.Drawing.Size(1000, 500);
            this.Load += new System.EventHandler(this.Plc_Load);
            this.CommunicationTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.SendGroupBox.ResumeLayout(false);
            this.SendTableLayoutPanel.ResumeLayout(false);
            this.SendStringGroupBox.ResumeLayout(false);
            this.SendStringTableLayoutPanel.ResumeLayout(false);
            this.SendStringTableLayoutPanel.PerformLayout();
            this.SendDataTableLayoutPanel.ResumeLayout(false);
            this.SendDWordGroupBox.ResumeLayout(false);
            this.SendDWordTableLayoutPanel.ResumeLayout(false);
            this.SendDWordActionsTableLayoutPanel.ResumeLayout(false);
            this.SendDWordActionsTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DWordNumericUpDown)).EndInit();
            this.SendMarkerGroupBox.ResumeLayout(false);
            this.SendMarkerTableLayoutPanel.ResumeLayout(false);
            this.SendMarkerActionsTableLayoutPanel.ResumeLayout(false);
            this.SendMarkerActionsTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MarkerNumericUpDown)).EndInit();
            this.ReceiveGroupBox.ResumeLayout(false);
            this.ReceiveTableLayoutPanel.ResumeLayout(false);
            this.ReceiveStringGroupBox.ResumeLayout(false);
            this.ReceiveStringGroupBox.PerformLayout();
            this.ReceiveDataTableLayoutPanel.ResumeLayout(false);
            this.ReceiveDWordGroupBox.ResumeLayout(false);
            this.ReceiveMarkerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel CommunicationTableLayoutPanel;
    private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
    private System.Windows.Forms.GroupBox SendGroupBox;
    private System.Windows.Forms.TableLayoutPanel SendTableLayoutPanel;
    private System.Windows.Forms.GroupBox SendStringGroupBox;
    private System.Windows.Forms.TableLayoutPanel SendStringTableLayoutPanel;
    private System.Windows.Forms.TextBox SendStringTextBox;
    private System.Windows.Forms.TableLayoutPanel SendDataTableLayoutPanel;
    private System.Windows.Forms.GroupBox SendDWordGroupBox;
    private System.Windows.Forms.TableLayoutPanel SendDWordTableLayoutPanel;
    private System.Windows.Forms.ListView SendDWordListView;
    private System.Windows.Forms.ColumnHeader SendDWordColumnHeader;
    private System.Windows.Forms.TableLayoutPanel SendDWordActionsTableLayoutPanel;
    private System.Windows.Forms.Button AddDWordButton;
    private System.Windows.Forms.NumericUpDown DWordNumericUpDown;
    private System.Windows.Forms.Button RemoveDWordButton;
    private System.Windows.Forms.GroupBox SendMarkerGroupBox;
    private System.Windows.Forms.TableLayoutPanel SendMarkerTableLayoutPanel;
    private System.Windows.Forms.ListView SendMarkerListView;
    private System.Windows.Forms.ColumnHeader SendMarkerColumnHeader;
    private System.Windows.Forms.TableLayoutPanel SendMarkerActionsTableLayoutPanel;
    private System.Windows.Forms.Button RemoveMarkerButton;
    private System.Windows.Forms.Button AddMarkerButton;
    private System.Windows.Forms.NumericUpDown MarkerNumericUpDown;
    private System.Windows.Forms.GroupBox ReceiveGroupBox;
    private System.Windows.Forms.TableLayoutPanel ReceiveTableLayoutPanel;
    private System.Windows.Forms.GroupBox ReceiveStringGroupBox;
    private System.Windows.Forms.TextBox ReceiveStringTextBox;
    private System.Windows.Forms.TableLayoutPanel ReceiveDataTableLayoutPanel;
    private System.Windows.Forms.GroupBox ReceiveDWordGroupBox;
    private System.Windows.Forms.ListView ReceiveDWordListView;
    private System.Windows.Forms.ColumnHeader ReceiveDWordColumnHeader;
    private System.Windows.Forms.GroupBox ReceiveMarkerGroupBox;
    private System.Windows.Forms.ListView ReceiveMarkerListView;
    private System.Windows.Forms.ColumnHeader ReceiveMarkerColumnHeader;
    private System.Windows.Forms.Label CncTypeLabel;
    private System.Windows.Forms.CheckBox StringCheckBox;
    private System.Windows.Forms.CheckBox DWordCheckBox;
    private System.Windows.Forms.CheckBox MarkerCheckBox;
    private System.Windows.Forms.Button SendButton;

  }
}
