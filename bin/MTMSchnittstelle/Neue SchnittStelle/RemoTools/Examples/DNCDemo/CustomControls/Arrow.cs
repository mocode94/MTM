// ------------------------------------------------------------------------------------------------
// <copyright file="Arrow.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This class creates a new Toolbox control element arrow
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Toolbox control element arrow
    /// </summary>
    public class Arrow : Control
    {
        /// <summary>
        /// Redraw user defined control element
        /// -> arrow
        /// </summary>
        /// <param name="e">The parameter is not used.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class
            base.OnPaint(e);

            // get the graphics object - use to draw
            Graphics g = e.Graphics;

            // sets the rendering quality for this graphics
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Control adjustment
            g.TranslateTransform(this.Width / 2, this.Height / 2);
            this.BackColor = SystemColors.ControlLightLight;

            // create arrow
            Pen p = new Pen(Color.Black, 10);
            p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            g.DrawLine(p, 0, -30, 0, 30);

            // release all ressources used by Pen
            p.Dispose();
        }
    }
}
