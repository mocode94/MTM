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
            this.GeneralInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GeneralInfoLabel
            // 
            this.GeneralInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralInfoLabel.Location = new System.Drawing.Point(0, 0);
            this.GeneralInfoLabel.Name = "GeneralInfoLabel";
            this.GeneralInfoLabel.Size = new System.Drawing.Size(550, 300);
            this.GeneralInfoLabel.TabIndex = 0;
            this.GeneralInfoLabel.Text = "I am a user control";
            this.GeneralInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DncInterfaceUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GeneralInfoLabel);
            this.Name = "DncInterfaceUserControl";
            this.Size = new System.Drawing.Size(550, 300);
            this.Load += new System.EventHandler(this.DncInterface_Load);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label GeneralInfoLabel;



  }
}
