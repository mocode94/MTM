// ------------------------------------------------------------------------------------------------
// <copyright file="DataAccess4.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the users own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>26.07.2016</date>
// <summary>
// This file contains a class which shows how to handle the
// IJHDataAccess4 interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This user control is used to show the IJHDataAccess4 functionalities.
    /// </summary>
    public partial class DataAccess4 : UserControl
    {
        #region "type defs"
        #endregion

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
        /// Object of HeidenhainDNC IJHDataAccess interface.
        /// </summary>
        private HeidenhainDNCLib.JHDataAccess dataAccess = null;

        /// <summary>
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccess4"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="mainForm">Reference to the main application Form.</param>
        public DataAccess4(MainForm mainForm)
        {
            this.InitializeComponent();
            this.Disposed += new EventHandler(this.DataAccess4_Disposed);

            this.Dock = DockStyle.Fill;
            this.machine = mainForm.Machine;
        }
        #endregion

        #region "delegates"
        #endregion

        #region "Enums"
        #endregion

        #region "properties"
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
                this.dataAccess = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHDATAACCESS);

                //// --- Subscribe for the event(s) -------------------------------------------------

                this.dataAccess.SetAccessMode(HeidenhainDNCLib.DNC_ACCESS_MODE.DNC_ACCESS_MODE_CFGDATAACCESS, string.Empty);

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
        /// <summary>
        /// Add a new data entry at given data selection. Only supported for \CFG.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AddDataEntryButton_Click(object sender, EventArgs e)
        {
            try
            {
                string fullIdent = AddFullIdentTextBox.Text;
                string storagePath = AddStoragePathTextBox.Text;

                this.dataAccess.AddDataEntry(fullIdent, storagePath);
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
        }

        /// <summary>
        /// /// Add a new data entry at given data selection. Only supported for \CFG.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string fullIdent = RemoveFullIdentTextBox.Text;

                this.dataAccess.RemoveDataEntry(fullIdent);
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
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DataAccess4_Disposed(object sender, EventArgs e)
        {
            if (this.dataAccess != null)
            {
                Marshal.ReleaseComObject(this.dataAccess);
            }
        }
        #endregion

        #region "private methods"
        #endregion
    }
}
