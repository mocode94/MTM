// ------------------------------------------------------------------------------------------------
// <copyright file="InfoPanel.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a user control which can be used to show informations to the user.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Use this to show some information instead of the interface example user control.
    /// </summary>
    public partial class InfoPanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoPanel"/> class.
        /// Fills this user control to its parent.
        /// </summary>
        /// <param name="infoMessage">The message to show on the "empty" user control.</param>
        public InfoPanel(string infoMessage = "No message defined")
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;

            // Show message
            GeneralInfoLabel.Text = infoMessage;
        }
    }
}
