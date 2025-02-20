// ------------------------------------------------------------------------------------------------
// <copyright file="BackgroundWorkerBusyForm.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>07.10.2015</date>
// <summary>
// The form is showed as long as the worker is busy or the button hard cancel is pressed.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// The form is showed as long as the worker is busy or the button hard cancel is pressed.
    /// </summary>
    public partial class BackgroundWorkerBusyForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkerBusyForm"/> class.
        /// </summary>
        /// <param name="worker">This background worker has to be observed.</param>
        public BackgroundWorkerBusyForm(BackgroundWorker worker)
        {
            this.InitializeComponent();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
        }

        /// <summary>
        /// Close the form if the background worker has finished.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Hard abort the worker. This can cause an RCW error.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void HardAbortButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
