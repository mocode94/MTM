// ------------------------------------------------------------------------------------------------
// <copyright file="DncInterface.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// Provides control elements for operating with the HeidenhainDNC interfaces
    /// </summary>
    public partial class DncInterface : UserControl
    {
        #region "fields"
        /// <summary>
        /// Reference to the main communication object.
        /// Use this to get the interfaces.
        /// </summary>
#if MACHINE_IN_PROCESS
        private HeidenhainDNCLib.JHMachineInProcess machine = null;
#else
        private HeidenhainDNCLib.JHMachine machine = null;
#endif

        /// <summary>
        /// Object of HeidenhainDNC IJHVersion interface
        /// </summary>
        private HeidenhainDNCLib.JHVersion version = null;

        /// <summary>
        /// Type of the connected CNC control.
        /// </summary>
        private HeidenhainDNCLib.DNC_CNC_TYPE cncType;

        /// <summary>
        /// Information if the connected CNC control NCK based.
        /// </summary>
        private bool isNck;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly
        /// </summary>
        private bool initOkay = false;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DncInterface"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form</param>
        public DncInterface(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.DncInterface_Disposed);

            this.cncType = parentForm.CncType;
            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// Get all interfaces and subscribe for all events here.
        /// </summary>
        /// <returns>Initialization successful.</returns>
        public bool Init()
        {
            try
            {
                // --- Get the interface Object(s) ------------------------------------------------
                this.version = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHVERSION);

                //// --- Subscribe for the event(s) -------------------------------------------------

                this.initOkay = true;
                return this.initOkay;
            }
            catch
            {
                this.initOkay = false;
                return this.initOkay;
            }
        }
        #endregion

        #region "event handler"
        // --- User control events ----------------------------------------------------------------

        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DncInterface_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            this.UpdateGUI();
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DncInterface_Disposed(object sender, EventArgs e)
        {
            if (this.version != null)
            {
                Marshal.ReleaseComObject(this.version);
            }
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGUI()
        {
            string infoText = string.Empty;
            HeidenhainDNCLib.IJHSoftwareVersionList softwareVersionList = null;
            HeidenhainDNCLib.IJHSoftwareVersion softwareVersion = null;

            infoText = "ControlType: " + Enum.GetName(typeof(HeidenhainDNCLib.DNC_CNC_TYPE), this.cncType);
            infoText += Environment.NewLine;
            infoText += "Is NCK based: " + this.isNck.ToString();
            infoText += Environment.NewLine;

            try
            {
                softwareVersionList = this.version.GetVersionNcSoftware();

                for (int i = 0; i < softwareVersionList.Count; i++)
                {
                    softwareVersion = softwareVersionList[i];

                    if (softwareVersion.softwareType == HeidenhainDNCLib.DNC_SW_TYPE.DNC_SW_TYPE_MC)
                    {
                        infoText += "MC Software version: " + softwareVersion.bstrIdentNr;
                        break;
                    }

                    if (softwareVersion != null)
                    {
                        Marshal.ReleaseComObject(softwareVersion);
                    }
                }

                GeneralInfoLabel.Text = infoText;
            }
            catch (COMException cex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
            }
            catch (Exception ex)
            {
                string className = this.GetType().Name;
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
            finally
            {
                if (softwareVersionList != null)
                {
                    Marshal.ReleaseComObject(softwareVersionList);
                }

                if (softwareVersion != null)
                {
                    Marshal.ReleaseComObject(softwareVersion);
                }
            }
        }
        #endregion
    }
}
