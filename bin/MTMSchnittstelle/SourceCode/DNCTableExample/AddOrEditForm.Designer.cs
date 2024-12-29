namespace DNC_CSharp_Demo
{
    partial class AddOrEditForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Ok_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.TableDataFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.GeneralInfoLabel = new System.Windows.Forms.Label();
            this.SelectItemTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SelectItem_Button = new System.Windows.Forms.Button();
            this.ChooseNewItemComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 706F));
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 4;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(120, 27);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // ButtonsTableLayoutPanel
            // 
            this.ButtonsTableLayoutPanel.ColumnCount = 3;
            this.ButtonsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsTableLayoutPanel.Location = new System.Drawing.Point(0, 601);
            this.ButtonsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsTableLayoutPanel.Name = "ButtonsTableLayoutPanel";
            this.ButtonsTableLayoutPanel.RowCount = 1;
            this.ButtonsTableLayoutPanel.Size = new System.Drawing.Size(941, 37);
            this.ButtonsTableLayoutPanel.TabIndex = 0;
            // 
            // Ok_Button
            // 
            this.Ok_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ok_Button.Location = new System.Drawing.Point(679, 4);
            this.Ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Ok_Button.Name = "Ok_Button";
            this.Ok_Button.Size = new System.Drawing.Size(125, 29);
            this.Ok_Button.TabIndex = 0;
            this.Ok_Button.Text = "OK";
            this.Ok_Button.UseVisualStyleBackColor = true;
            this.Ok_Button.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cancel_Button.Location = new System.Drawing.Point(812, 4);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(125, 29);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // TableDataFlowLayoutPanel
            // 
            this.TableDataFlowLayoutPanel.AutoScroll = true;
            this.TableDataFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableDataFlowLayoutPanel.Location = new System.Drawing.Point(4, 78);
            this.TableDataFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.TableDataFlowLayoutPanel.Name = "TableDataFlowLayoutPanel";
            this.TableDataFlowLayoutPanel.Size = new System.Drawing.Size(933, 519);
            this.TableDataFlowLayoutPanel.TabIndex = 1;
            // 
            // GeneralInfoLabel
            // 
            this.GeneralInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GeneralInfoLabel.Location = new System.Drawing.Point(4, 0);
            this.GeneralInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GeneralInfoLabel.Name = "GeneralInfoLabel";
            this.GeneralInfoLabel.Size = new System.Drawing.Size(933, 37);
            this.GeneralInfoLabel.TabIndex = 2;
            this.GeneralInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectItemTableLayoutPanel
            // 
            this.SelectItemTableLayoutPanel.ColumnCount = 2;
            this.SelectItemTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectItemTableLayoutPanel.Location = new System.Drawing.Point(0, 37);
            this.SelectItemTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SelectItemTableLayoutPanel.Name = "SelectItemTableLayoutPanel";
            this.SelectItemTableLayoutPanel.RowCount = 1;
            this.SelectItemTableLayoutPanel.Size = new System.Drawing.Size(941, 37);
            this.SelectItemTableLayoutPanel.TabIndex = 3;
            // 
            // SelectItem_Button
            // 
            this.SelectItem_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectItem_Button.Location = new System.Drawing.Point(812, 4);
            this.SelectItem_Button.Margin = new System.Windows.Forms.Padding(4);
            this.SelectItem_Button.Name = "SelectItem_Button";
            this.SelectItem_Button.Size = new System.Drawing.Size(125, 29);
            this.SelectItem_Button.TabIndex = 0;
            this.SelectItem_Button.Text = "Select";
            this.SelectItem_Button.UseVisualStyleBackColor = true;
            this.SelectItem_Button.Click += new System.EventHandler(this.SelectItemButton_Click);
            // 
            // ChooseNewItemComboBox
            // 
            this.ChooseNewItemComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChooseNewItemComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseNewItemComboBox.FormattingEnabled = true;
            this.ChooseNewItemComboBox.Location = new System.Drawing.Point(13, 4);
            this.ChooseNewItemComboBox.Margin = new System.Windows.Forms.Padding(13, 4, 13, 4);
            this.ChooseNewItemComboBox.Name = "ChooseNewItemComboBox";
            this.ChooseNewItemComboBox.Size = new System.Drawing.Size(782, 24);
            this.ChooseNewItemComboBox.TabIndex = 3;
            // 
            // AddOrEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 27);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Name = "AddOrEditForm";
            this.Load += new System.EventHandler(this.FrmAddOrEdit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ButtonsTableLayoutPanel;
        private System.Windows.Forms.Button Ok_Button;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.FlowLayoutPanel TableDataFlowLayoutPanel;
        private System.Windows.Forms.Label GeneralInfoLabel;
        private System.Windows.Forms.ComboBox ChooseNewItemComboBox;
        private System.Windows.Forms.TableLayoutPanel SelectItemTableLayoutPanel;
        private System.Windows.Forms.Button SelectItem_Button;
    }
}