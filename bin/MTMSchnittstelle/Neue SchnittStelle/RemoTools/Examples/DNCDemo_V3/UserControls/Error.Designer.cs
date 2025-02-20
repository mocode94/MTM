namespace DNC_CSharp_Demo.UserControls
{
    partial class Error
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
            this.ErrorListTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ClearAllErrorsOnTncButton = new System.Windows.Forms.Button();
            this.ModifyListTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ClearListButton = new System.Windows.Forms.Button();
            this.SaveListButton = new System.Windows.Forms.Button();
            this.ErrorListView = new System.Windows.Forms.ListView();
            this.ChannelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ErrorNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TextColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClassColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArrivedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GoneColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MainTableLayoutPanel.SuspendLayout();
            this.ErrorListTableLayoutPanel.SuspendLayout();
            this.ModifyListTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.ErrorListTableLayoutPanel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 1;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // ErrorListTableLayoutPanel
            // 
            this.ErrorListTableLayoutPanel.ColumnCount = 1;
            this.ErrorListTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ErrorListTableLayoutPanel.Controls.Add(this.ClearAllErrorsOnTncButton, 0, 0);
            this.ErrorListTableLayoutPanel.Controls.Add(this.ModifyListTableLayoutPanel, 0, 2);
            this.ErrorListTableLayoutPanel.Controls.Add(this.ErrorListView, 0, 1);
            this.ErrorListTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorListTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ErrorListTableLayoutPanel.Name = "ErrorListTableLayoutPanel";
            this.ErrorListTableLayoutPanel.RowCount = 3;
            this.ErrorListTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.ErrorListTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ErrorListTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ErrorListTableLayoutPanel.Size = new System.Drawing.Size(764, 460);
            this.ErrorListTableLayoutPanel.TabIndex = 0;
            // 
            // ClearAllErrorsOnTncButton
            // 
            this.ClearAllErrorsOnTncButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClearAllErrorsOnTncButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearAllErrorsOnTncButton.Location = new System.Drawing.Point(3, 3);
            this.ClearAllErrorsOnTncButton.Name = "ClearAllErrorsOnTncButton";
            this.ClearAllErrorsOnTncButton.Size = new System.Drawing.Size(758, 29);
            this.ClearAllErrorsOnTncButton.TabIndex = 0;
            this.ClearAllErrorsOnTncButton.Text = "Clear all errors on TNC";
            this.ClearAllErrorsOnTncButton.UseVisualStyleBackColor = true;
            this.ClearAllErrorsOnTncButton.Click += new System.EventHandler(this.ClearAllErrorsOnTncButton_Click);
            // 
            // ModifyListTableLayoutPanel
            // 
            this.ModifyListTableLayoutPanel.ColumnCount = 3;
            this.ModifyListTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ModifyListTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ModifyListTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ModifyListTableLayoutPanel.Controls.Add(this.ClearListButton, 0, 0);
            this.ModifyListTableLayoutPanel.Controls.Add(this.SaveListButton, 2, 0);
            this.ModifyListTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModifyListTableLayoutPanel.Location = new System.Drawing.Point(3, 427);
            this.ModifyListTableLayoutPanel.Name = "ModifyListTableLayoutPanel";
            this.ModifyListTableLayoutPanel.RowCount = 1;
            this.ModifyListTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ModifyListTableLayoutPanel.Size = new System.Drawing.Size(758, 30);
            this.ModifyListTableLayoutPanel.TabIndex = 1;
            // 
            // ClearListButton
            // 
            this.ClearListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClearListButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearListButton.Location = new System.Drawing.Point(3, 3);
            this.ClearListButton.Name = "ClearListButton";
            this.ClearListButton.Size = new System.Drawing.Size(144, 24);
            this.ClearListButton.TabIndex = 1;
            this.ClearListButton.Text = "Clear List";
            this.ClearListButton.UseVisualStyleBackColor = true;
            this.ClearListButton.Click += new System.EventHandler(this.ClearListButton_Click);
            // 
            // SaveListButton
            // 
            this.SaveListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveListButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveListButton.Location = new System.Drawing.Point(611, 3);
            this.SaveListButton.Name = "SaveListButton";
            this.SaveListButton.Size = new System.Drawing.Size(144, 24);
            this.SaveListButton.TabIndex = 2;
            this.SaveListButton.Text = "Save List";
            this.SaveListButton.UseVisualStyleBackColor = true;
            this.SaveListButton.Click += new System.EventHandler(this.SaveListButton_Click);
            // 
            // ErrorListView
            // 
            this.ErrorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChannelColumnHeader,
            this.ErrorNumberColumnHeader,
            this.TextColumnHeader,
            this.GroupColumnHeader,
            this.ClassColumnHeader,
            this.ArrivedColumnHeader,
            this.GoneColumnHeader});
            this.ErrorListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorListView.FullRowSelect = true;
            this.ErrorListView.GridLines = true;
            this.ErrorListView.Location = new System.Drawing.Point(3, 38);
            this.ErrorListView.Name = "ErrorListView";
            this.ErrorListView.Size = new System.Drawing.Size(758, 383);
            this.ErrorListView.TabIndex = 2;
            this.ErrorListView.UseCompatibleStateImageBehavior = false;
            this.ErrorListView.View = System.Windows.Forms.View.Details;
            // 
            // ChannelColumnHeader
            // 
            this.ChannelColumnHeader.Text = "Channel";
            this.ChannelColumnHeader.Width = 55;
            // 
            // ErrorNumberColumnHeader
            // 
            this.ErrorNumberColumnHeader.Text = "Number";
            this.ErrorNumberColumnHeader.Width = 75;
            // 
            // TextColumnHeader
            // 
            this.TextColumnHeader.Text = "Text";
            this.TextColumnHeader.Width = 250;
            // 
            // GroupColumnHeader
            // 
            this.GroupColumnHeader.Text = "Group";
            this.GroupColumnHeader.Width = 130;
            // 
            // ClassColumnHeader
            // 
            this.ClassColumnHeader.Text = "Class";
            this.ClassColumnHeader.Width = 100;
            // 
            // ArrivedColumnHeader
            // 
            this.ArrivedColumnHeader.Text = "Arrived";
            this.ArrivedColumnHeader.Width = 120;
            // 
            // GoneColumnHeader
            // 
            this.GoneColumnHeader.Tag = "";
            this.GoneColumnHeader.Text = "Gone";
            this.GoneColumnHeader.Width = 120;
            // 
            // Error
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "Error";
            this.Size = new System.Drawing.Size(770, 466);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ErrorListTableLayoutPanel.ResumeLayout(false);
            this.ModifyListTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ErrorListTableLayoutPanel;
        private System.Windows.Forms.Button ClearAllErrorsOnTncButton;
        private System.Windows.Forms.TableLayoutPanel ModifyListTableLayoutPanel;
        private System.Windows.Forms.Button ClearListButton;
        private System.Windows.Forms.Button SaveListButton;
        private System.Windows.Forms.ListView ErrorListView;
        private System.Windows.Forms.ColumnHeader ChannelColumnHeader;
        private System.Windows.Forms.ColumnHeader ErrorNumberColumnHeader;
        private System.Windows.Forms.ColumnHeader GroupColumnHeader;
        private System.Windows.Forms.ColumnHeader ClassColumnHeader;
        private System.Windows.Forms.ColumnHeader TextColumnHeader;
        private System.Windows.Forms.ColumnHeader ArrivedColumnHeader;
        private System.Windows.Forms.ColumnHeader GoneColumnHeader;
    }
}
