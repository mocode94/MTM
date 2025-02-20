// ------------------------------------------------------------------------------------------------
// <copyright file="Version.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which shows how to use the IJHVersion interface.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class shows how to use the IJHVersion interface.
    /// </summary>
    public partial class Version : UserControl
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
        /// Object of HeidenhainDNC IJHVersion interface.
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
        /// Has all HeidenhainDNC interfaces and events initialized correctly.
        /// </summary>
        private bool initOkay = false;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public Version(MainForm parentForm)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.Version_Disposed);

            this.machine = parentForm.Machine;
            this.isNck = parentForm.IsNck;
            this.cncType = parentForm.CncType;
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
                // --- get interfaces here ----------------------------------------------------------------
                this.version = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHVERSION);

                //// --- advise events here -----------------------------------------------------------------

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
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void UCVersion_Load(object sender, EventArgs e)
        {
            if (!this.initOkay)
            {
                return;
            }

            this.UpdateGui();
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Version_Disposed(object sender, EventArgs e)
        {
            if (this.version != null)
            {
                //// --- 1. unadvice all event handlers here ------------------------------------------------

                // --- 2. release interfaces here ---------------------------------------------------------
                Marshal.ReleaseComObject(this.version);
            }

#if !DEBUG
     /* In this application we release all interfaces, event handlers and JH helper objects
      * explicitly using the Marshal.ReleaseComObject(<COM Object>) mechanism.
      * If we try to disconnect before all objects are released, we get an error in the windows
      * event log. With the information from the windows event log, we can find the orphan object.
      * Therefore in DEBUG built we do not use the garbage collector for releasing all COM objects.
      * 
      * But for the release software built we use the garbage collector to avoid memory leaks,
      * even when a object was not explicitly released my Marshall.ReleaseComObject
      */
   
      /*  MSDN: How to: Handle Events Raised by a COM Source
      *   Note that COM objects that raise events within a .NET client require two Garbage Collector (GC) collections before they are released.
      *   This is caused by the reference cycle that occurs between COM objects and managed clients.
      *   If you need to explicitly release a COM object you should call the Collect method twice.
      */
      GC.Collect();
      GC.Collect();
      GC.WaitForPendingFinalizers();
#endif
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Common functions to update GUI.
        /// </summary>
        private void UpdateGui()
        {
            HeidenhainDNCLib.JHSoftwareVersionList softwareVersionList = null;
            HeidenhainDNCLib.JHSoftwareVersion softwareVersion = null;
            try
            {
                NcVersionListView.Items.Clear();

                // fetch the infomations and put it to display
                ComInterfaceTextBox.Text = this.version.GetVersionComInterface();
                SikTextBox.Text = this.version.GetSik();

                // some informations are organized in lists
                // if you want to get the informations, you have to unpack the list. See example below.
                softwareVersionList = this.version.GetVersionNcSoftware();
                for (int i = 0; i < softwareVersionList.Count; i++)
                {
                    softwareVersion = softwareVersionList[i];

                    // data of first column
                    ListViewItem versionItem = new ListViewItem(softwareVersion.bstrIdentNr);
                    if (softwareVersion.softwareType == HeidenhainDNCLib.DNC_SW_TYPE.DNC_SW_TYPE_MC)
                    {
                        versionItem.BackColor = Color.Aqua;
                    }

                    // data of second and third column
                    versionItem.SubItems.Add(Enum.GetName(typeof(HeidenhainDNCLib.DNC_SW_TYPE), softwareVersion.softwareType));
                    versionItem.SubItems.Add(softwareVersion.bstrDescription);

                    // Add item to ListViewControl (show the new row)
                    NcVersionListView.Items.Add(versionItem);
                    NcVersionListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

                    if (softwareVersion != null)
                    {
                        Marshal.ReleaseComObject(softwareVersion);
                        softwareVersion = null;
                    }
                }

                // show control type
                string controlType = this.cncType.ToString();
                ControlTypeTextBox.Text = this.isNck ? (controlType + " (NCK)") : (controlType + " (non NCK)");
            }
            catch (COMException cex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, strClassName, strMethodName);
            }
            catch (Exception ex)
            {
                string strClassName = this.GetType().Name;
                string strMethodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, strClassName, strMethodName);
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