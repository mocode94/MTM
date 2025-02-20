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
            this.TabCtrlInterfaces = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // TabCtrlInterfaces
            // 
            this.TabCtrlInterfaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabCtrlInterfaces.HotTrack = true;
            this.TabCtrlInterfaces.Location = new System.Drawing.Point(0, 0);
            this.TabCtrlInterfaces.Multiline = true;
            this.TabCtrlInterfaces.Name = "TabCtrlInterfaces";
            this.TabCtrlInterfaces.SelectedIndex = 0;
            this.TabCtrlInterfaces.Size = new System.Drawing.Size(800, 500);
            this.TabCtrlInterfaces.TabIndex = 4;
            // 
            // DncInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabCtrlInterfaces);
            this.Name = "DncInterface";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.DncInterface_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabCtrlInterfaces;
    }
}
