// ------------------------------------------------------------------------------------------------
// <copyright file="AxisPosition.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>08.03.2016</date>
// <summary>
// This file contains a user control which is used to show axis infirmations
// and set position values.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A user control to show an set axes specific values like the axisId, the axisName and a position.
    /// </summary>
    public partial class AxisPosition : UserControl
    {
        #region "fields"
        /// <summary>
        /// Axis identifier property. 
        /// </summary>
        private int axisId;

        /// <summary>
        /// Axis name property. 
        /// </summary>
        private string axisName;

        /// <summary>
        /// Initial position for the AxisPositionTextBox.
        /// </summary>
        private double initialPosition = 0.0;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisPosition"/> class.
        /// </summary>
        /// <param name="axisId">Id of the axis.</param>
        /// <param name="axisName">Name of the axis.</param>
        /// <param name="initialPosition">Initial axis position in the specified channel.</param>
        /// <param name="axisInChannel">Is this axis contained in the specified channel.</param>
        public AxisPosition(int axisId, string axisName, double initialPosition, bool axisInChannel)
        {
            this.InitializeComponent();

            this.axisId = axisId;
            this.axisName = axisName;
            this.initialPosition = initialPosition;

            this.AxisInChannel = axisInChannel;
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets the axis identification property.
        /// </summary>
        /// <value>The Id of the axis.</value>
        public int AxisId
        {
            get
            {
                return this.axisId;
            }
        }

        /// <summary>
        /// Gets the axis position property. 
        /// </summary>
        /// <value>The position for the axis.</value>
        public double Position
        {
            get
            {
                double position = 0.0;
                if (!double.TryParse(AxisPositionTextBox.Text, out position))
                {
                    throw new FormatException();
                }

                return position;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this axis is contained in this channel.
        /// </summary>
        /// <value>Is this axis contained in this channel.</value>
        public bool AxisInChannel
        {
            get
            {
                if (AxisIdLabel.BackColor == Color.Green)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (value)
                {
                    AxisIdLabel.BackColor = Color.Green;
                }
                else
                {
                    AxisIdLabel.BackColor = SystemColors.ControlLight;
                }
            }
        }

        /// <summary>
        /// Gets the JHAxisPosition helper object.
        /// </summary>
        /// <value>JHAxisPosition helper object.</value>
        public HeidenhainDNCLib.JHAxisPosition JhAxisPosition
        {
            get
            {
                HeidenhainDNCLib.JHAxisPosition axisPos = new HeidenhainDNCLib.JHAxisPosition();
                axisPos.lAxisId = this.AxisId;

                axisPos.dPosition = double.Parse(AxisPositionTextBox.Text);

                return axisPos;
            }
        }
        #endregion

        #region "event handler"
        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AxisPosition_Load(object sender, EventArgs e)
        {
            AxisIdLabel.Text = string.Format("AxisId: {0}", this.axisId);
            AxisNameLabel.Text = string.Format("AxisName: {0}", this.axisName);
            AxisPositionTextBox.Text = string.Format("1.0000").Replace(".", MainForm.DecimalSeparator);
        }
        #endregion
    }
}
