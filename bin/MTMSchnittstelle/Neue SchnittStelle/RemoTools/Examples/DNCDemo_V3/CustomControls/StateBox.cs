// ------------------------------------------------------------------------------------------------
// <copyright file="StateBox.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Josef Kamml</author>
// <date>19.04.2017</date>
// <summary>
// This class creates a new Toolbox control element state box.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Toolbox control element status box.
    /// </summary>
    public class StateBox : Control
    {
        /// <summary>
        /// overwrite base method
        /// redraw user defined control element
        /// -> state box
        /// </summary>
        /// <param name="e">The paint event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class
            base.OnPaint(e);

            // get the graphics object - use to draw
            Graphics graphics = e.Graphics;

            // sets the rendering quality for this graphics
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Control adjustment
            this.Height = 35;
            this.Width = 80;
            Pen pen = new Pen(Color.Black, 10);

            // Draw "Statusbox"
            graphics.DrawRectangle(pen, 0, 0, this.Width, this.Height);

            // release all ressources used by Pen
            pen.Dispose();
        }
    }
}
