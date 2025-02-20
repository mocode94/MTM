namespace DNC_CSharp_Demo.UserControls
{
    partial class ProgressDiagram
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
            this.PrepareWorkingFolderLabel = new System.Windows.Forms.Label();
            this.PrepareToolDataLabel = new System.Windows.Forms.Label();
            this.UniversalFunctionLabel = new System.Windows.Forms.Label();
            this.UniversalFinishedStateBox = new DNC_CSharp_Demo.StateBox();
            this.PrepareToolDataStateBox = new DNC_CSharp_Demo.StateBox();
            this.PrepareWorkingFolderStateBox = new DNC_CSharp_Demo.StateBox();
            this.arrow_2 = new DNC_CSharp_Demo.Arrow();
            this.arrow_1 = new DNC_CSharp_Demo.Arrow();
            this.SuspendLayout();
            // 
            // PrepareWorkingFolderLabel
            // 
            this.PrepareWorkingFolderLabel.AutoSize = true;
            this.PrepareWorkingFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrepareWorkingFolderLabel.Location = new System.Drawing.Point(3, 19);
            this.PrepareWorkingFolderLabel.Name = "PrepareWorkingFolderLabel";
            this.PrepareWorkingFolderLabel.Size = new System.Drawing.Size(141, 13);
            this.PrepareWorkingFolderLabel.TabIndex = 11;
            this.PrepareWorkingFolderLabel.Text = "Prepare Working Folder";
            // 
            // PrepareToolDataLabel
            // 
            this.PrepareToolDataLabel.AutoSize = true;
            this.PrepareToolDataLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrepareToolDataLabel.Location = new System.Drawing.Point(3, 159);
            this.PrepareToolDataLabel.Name = "PrepareToolDataLabel";
            this.PrepareToolDataLabel.Size = new System.Drawing.Size(111, 13);
            this.PrepareToolDataLabel.TabIndex = 12;
            this.PrepareToolDataLabel.Text = "Prepare Tool Data";
            // 
            // UniversalFunctionLabel
            // 
            this.UniversalFunctionLabel.AutoSize = true;
            this.UniversalFunctionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UniversalFunctionLabel.Location = new System.Drawing.Point(3, 302);
            this.UniversalFunctionLabel.Name = "UniversalFunctionLabel";
            this.UniversalFunctionLabel.Size = new System.Drawing.Size(19, 13);
            this.UniversalFunctionLabel.TabIndex = 13;
            this.UniversalFunctionLabel.Text = "---";
            // 
            // UniversalFinishedStateBox
            // 
            this.UniversalFinishedStateBox.Location = new System.Drawing.Point(170, 292);
            this.UniversalFinishedStateBox.Name = "UniversalFinishedStateBox";
            this.UniversalFinishedStateBox.Size = new System.Drawing.Size(80, 35);
            this.UniversalFinishedStateBox.TabIndex = 22;
            // 
            // PrepareToolDataStateBox
            // 
            this.PrepareToolDataStateBox.Location = new System.Drawing.Point(170, 153);
            this.PrepareToolDataStateBox.Name = "PrepareToolDataStateBox";
            this.PrepareToolDataStateBox.Size = new System.Drawing.Size(80, 35);
            this.PrepareToolDataStateBox.TabIndex = 21;
            // 
            // PrepareWorkingFolderStateBox
            // 
            this.PrepareWorkingFolderStateBox.Location = new System.Drawing.Point(170, 14);
            this.PrepareWorkingFolderStateBox.Name = "PrepareWorkingFolderStateBox";
            this.PrepareWorkingFolderStateBox.Size = new System.Drawing.Size(80, 35);
            this.PrepareWorkingFolderStateBox.TabIndex = 20;
            // 
            // arrow_2
            // 
            this.arrow_2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.arrow_2.Location = new System.Drawing.Point(199, 201);
            this.arrow_2.Name = "arrow_2";
            this.arrow_2.Size = new System.Drawing.Size(25, 80);
            this.arrow_2.TabIndex = 19;
            // 
            // arrow_1
            // 
            this.arrow_1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.arrow_1.Location = new System.Drawing.Point(199, 61);
            this.arrow_1.Name = "arrow_1";
            this.arrow_1.Size = new System.Drawing.Size(25, 80);
            this.arrow_1.TabIndex = 18;
            // 
            // ProgressDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.UniversalFinishedStateBox);
            this.Controls.Add(this.PrepareToolDataStateBox);
            this.Controls.Add(this.PrepareWorkingFolderStateBox);
            this.Controls.Add(this.arrow_2);
            this.Controls.Add(this.arrow_1);
            this.Controls.Add(this.UniversalFunctionLabel);
            this.Controls.Add(this.PrepareToolDataLabel);
            this.Controls.Add(this.PrepareWorkingFolderLabel);
            this.Name = "ProgressDiagram";
            this.Size = new System.Drawing.Size(280, 333);
            this.Load += new System.EventHandler(this.ImportToolUsageDiagram_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PrepareWorkingFolderLabel;
        private System.Windows.Forms.Label PrepareToolDataLabel;
        private System.Windows.Forms.Label UniversalFunctionLabel;
        private Arrow arrow_1;
        private Arrow arrow_2;
        private StateBox PrepareWorkingFolderStateBox;
        private StateBox PrepareToolDataStateBox;
        private StateBox UniversalFinishedStateBox;
       

    }
}
