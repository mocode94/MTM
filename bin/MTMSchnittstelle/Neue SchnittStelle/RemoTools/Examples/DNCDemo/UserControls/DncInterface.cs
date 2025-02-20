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
// <summary>
// This user control provides a tab control to show all of the HEIDENHAIN
// interfaces in seperate tabs.
// </summary>
// ------------------------------------------------------------------------------------------------

#define JH_AUTOMATIC
#define JH_AUTOMATIC4
#define JH_AUTOMATIC_EVENTS
#define JH_AXESPOSITIONSTREAMING
#define JH_CONFIGURATION
#define JH_DATAACCESS
#define JH_DIAGNOSTIC
#define JH_ERROR
#define JH_REMOTEERROR
#define JH_FILESYSTEM
#define JH_ITNCTABLE
//#define JH_PLC
#define JH_PROCESSDATA
#define JH_VERSION

namespace DNC_CSharp_Demo.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Provides control elements for operating with the HeidenhainDNC interfaces.
    /// </summary>
    public partial class DncInterface : UserControl
    {
        #region "fields"
        /// <summary>
        /// Reference to parent form.
        /// </summary>
        private MainForm parentForm = null;

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
        /// Type of the connected CNC control.
        /// </summary>
        private HeidenhainDNCLib.DNC_CNC_TYPE cncType;

        /// <summary>
        /// Gets a value indicating whether the connected CNC control is NCK based or not.
        /// </summary>
        private bool isNck;
        private int axesPositionStreamingTabPageIndex = 0;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="DncInterface"/> class.
        /// </summary>
        /// <param name="parentForm">Reference to the parent Form.</param>
        public DncInterface(MainForm parentForm)
        {
            this.InitializeComponent();
            this.components = new Container();
            this.Dock = DockStyle.Fill;
            this.Disposed += new EventHandler(this.DncInterface_Disposed);

            this.parentForm = parentForm;
            this.machine = parentForm.Machine;
            this.cncType = parentForm.CncType;
            this.isNck = parentForm.IsNck;
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// This method is only here to guarantee the substitutability with the other examples.
        /// </summary>
        /// <returns>Returns always true.</returns>
        public bool Init()
        {
            return true;
        }
        #endregion

        #region "event handler"
        /// <summary>
        /// Dispose components container here.
        /// This also disposes all of the child user controls.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DncInterface_Disposed(object sender, EventArgs e)
        {
            this.components.Dispose();
        }

        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DncInterface_Load(object sender, EventArgs e)
        {
            this.CreateUserControls();
        }
        #endregion

        #region "private methods"
        /// <summary>
        /// Create and add all additional user controls.
        /// This method also adds the user controls to the Windows.Forms components container.
        /// This ensures, that the user controls becomes disposed when a from System.Windows.Form
        /// derived class gets disposed.
        /// </summary>
        private void CreateUserControls()
        {
            // --- JHVersion ----------------------------------------------------------------------------
            //#if JH_VERSION
            //            TabPage VersionTabPage = new TabPage("JHVersion");
            //            VersionTabPage.Name = "VersionTabPage";
            //            VersionTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(VersionTabPage);
            //            axesPositionStreamingTabPageIndex++;

            //            UserControls.Version version = new UserControls.Version(this.parentForm);
            //            if (version.Init())
            //            {
            //                this.components.Add(version);
            //                VersionTabPage.Controls.Add(version);
            //            }
            //            else
            //            {
            //                version.Dispose();

            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                components.Add(notSupported);
            //                VersionTabPage.Controls.Add(notSupported);
            //            }
            //#endif
            // --- JHConfiguration ----------------------------------------------------------------------
            //#if JH_CONFIGURATION
            //            TabPage ConfigurationTabPage = new TabPage("JHConfiguration");
            //            ConfigurationTabPage.Name = "ConfigurationTabPage";
            //            ConfigurationTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(ConfigurationTabPage);
            //            axesPositionStreamingTabPageIndex++;

            //            Configuration configuration = new Configuration(this.parentForm);
            //            if (configuration.Init())
            //            {
            //                this.components.Add(configuration);
            //                ConfigurationTabPage.Controls.Add(configuration);
            //            }
            //            else
            //            {
            //                configuration.Dispose();

            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                components.Add(notSupported);
            //                ConfigurationTabPage.Controls.Add(notSupported);
            //            }
            //#endif
            // --- JHProceccData ------------------------------------------------------------------------
            //#if JH_PROCESSDATA
            //            TabPage ProcessDataTabPage = new TabPage("JHProcessData");
            //            ProcessDataTabPage.Name = "ProcessDataTabPage";
            //            ProcessDataTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(ProcessDataTabPage);
            //            axesPositionStreamingTabPageIndex++;

            //            UserControls.ProcessData processData = new UserControls.ProcessData(this.parentForm);
            //            if (processData.Init())
            //            {
            //                components.Add(processData);
            //                ProcessDataTabPage.Controls.Add(processData);
            //            }
            //            else
            //            {
            //                processData.Dispose();

            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                components.Add(notSupported);
            //                ProcessDataTabPage.Controls.Add(notSupported);
            //            }
            //#endif
//            // --- JHError ------------------------------------------------------------------------------
//#if JH_ERROR
//            TabPage ErrorTabPage = new TabPage("JHError");
//            ErrorTabPage.Name = "ErrorTabPage";
//            ErrorTabPage.UseVisualStyleBackColor = true;
//            TabCtrlInterfaces.TabPages.Add(ErrorTabPage);
//            axesPositionStreamingTabPageIndex++;

//            UserControls.Error error = new UserControls.Error(this.parentForm);
//            if (error.Init())
//            {
//                components.Add(error);
//                ErrorTabPage.Controls.Add(error);
//            }
//            else
//            {
//                error.Dispose();

//                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
//                components.Add(notSupported);
//                ErrorTabPage.Controls.Add(notSupported);
//            }
//#endif

            // --- JHPlc --------------------------------------------------------------------------------
#if JH_PLC
            TabPage PlcTabPage = new TabPage("JHPlc");
            PlcTabPage.Name = "PlcTabPage";
            PlcTabPage.UseVisualStyleBackColor = true;
            TabCtrlInterfaces.TabPages.Add(PlcTabPage);
            axesPositionStreamingTabPageIndex++;

            UserControls.Plc plc = new UserControls.Plc(this.parentForm);
            if (plc.Init())
            {
                components.Add(plc);
                PlcTabPage.Controls.Add(plc);
            }
            else
            {
                plc.Dispose();

                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
                components.Add(notSupported);
                PlcTabPage.Controls.Add(notSupported);
            }
#endif

            // --- only supported by iTNC530 ------------------------------------------------------------
            //#if JH_ITNCTABLE
            //            TabPage ITncTableTabPage = new TabPage("JHITncTable");
            //            ITncTableTabPage.Name = "ITncTableTabPage";
            //            ITncTableTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(ITncTableTabPage);
            //            axesPositionStreamingTabPageIndex++;

            //            if (this.isNck)
            //            {
            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel(
            //                                                                       "Please use IJHDataAccess3 for accessing tables on NCK based controls." +
            //                                                                       Environment.NewLine +
            //                                                                       "Refer also to the Read Table Demo example!");
            //                components.Add(notSupported);
            //                ITncTableTabPage.Controls.Add(notSupported);
            //            }
            //            else
            //            {
            //                UserControls.ITNCTable itncTable = new UserControls.ITNCTable(this.parentForm);
            //                if (itncTable.Init())
            //                {
            //                    components.Add(itncTable);
            //                    ITncTableTabPage.Controls.Add(itncTable);
            //                }
            //                else
            //                {
            //                    itncTable.Dispose();

            //                    UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                    components.Add(notSupported);
            //                    ITncTableTabPage.Controls.Add(notSupported);
            //                }
            //            }
            //#endif
            // --- JHFileSystem -------------------------------------------------------------------------
            //#if JH_FILESYSTEM
            //            TabPage FileSystemTabPage = new TabPage("JHFileSystem");
            //            FileSystemTabPage.Name = "FileSystemTabPage";
            //            FileSystemTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(FileSystemTabPage);

            //            UserControls.FileSystem fileSystem = new UserControls.FileSystem(this.parentForm);
            //            if (fileSystem.Init())
            //            {
            //                components.Add(fileSystem);
            //                FileSystemTabPage.Controls.Add(fileSystem);
            //            }
            //            else
            //            {
            //                fileSystem.Dispose();

            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                components.Add(notSupported);
            //                FileSystemTabPage.Controls.Add(notSupported);
            //            }
            //#endif
            // --- JHAutomatic --------------------------------------------------------------------
            //#if JH_AUTOMATIC
            //TabPage AutomaticTabPage = new TabPage("JHAutomatic");
            //AutomaticTabPage.Name = "AutomaticTabPage";
            //AutomaticTabPage.UseVisualStyleBackColor = true;
            //TabCtrlInterfaces.TabPages.Add(AutomaticTabPage);

            //UserControls.Automatic automatic = new UserControls.Automatic(this.parentForm);
            //if (automatic.Init())
            //{
            //    components.Add(automatic);
            //    AutomaticTabPage.Controls.Add(automatic);
            //}
            //else
            //{
            //    automatic.Dispose();

            //    UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //    components.Add(notSupported);
            //    AutomaticTabPage.Controls.Add(notSupported);
            //}
            //#endif

            // --- JHAutomatic4 -------------------------------------------------------------------
            //#if JH_AUTOMATIC4
            //            TabPage Automatic4TabPage = new TabPage("JHAutomatic4");
            //            Automatic4TabPage.Name = "Automatic4TabPage";
            //            Automatic4TabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(Automatic4TabPage);

            //            if (this.isNck)
            //            {
            //                UserControls.Automatic4 automatic4 = new UserControls.Automatic4(this.parentForm);
            //                if (automatic4.Init())
            //                {
            //                    components.Add(automatic4);
            //                    Automatic4TabPage.Controls.Add(automatic4);
            //                }
            //                else
            //                {
            //                    automatic4.Dispose();

            //                    UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                    components.Add(notSupported);
            //                    Automatic4TabPage.Controls.Add(notSupported);
            //                }
            //            }
            //            else
            //            {
            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Only supported on NCK based controls like the TNC 640.");
            //                components.Add(notSupported);
            //                Automatic4TabPage.Controls.Add(notSupported);
            //            }
            //#endif

            // --- JHAutomaticEvents -------------------------------------------------------------------- 
            // Note: Must be after JHAutomatic, because there are more Automatic object instances and only
            //       the last instance receives the events with iTNC 530 connections
            //#if JH_AUTOMATIC_EVENTS
            //            TabPage AutomaticEventsTabPage = new TabPage("JHAutomaticEvents");
            //            AutomaticEventsTabPage.Name = "AutomaticEventsTabPage";
            //            AutomaticEventsTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(AutomaticEventsTabPage);

            //            UserControls.AutomaticEvents automaticEvents = new UserControls.AutomaticEvents(this.parentForm);
            //            if (automaticEvents.Init())
            //            {
            //                components.Add(automaticEvents);
            //                AutomaticEventsTabPage.Controls.Add(automaticEvents);
            //            }
            //            else
            //            {
            //                automaticEvents.Dispose();

            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                components.Add(notSupported);
            //                AutomaticEventsTabPage.Controls.Add(notSupported);
            //            }
            //#endif
            // --- JHDataAccess --------------------------------------------------------------------
#if JH_DATAACCESS
            TabPage DataAccessTabPage = new TabPage("JHDataAccess");
            DataAccessTabPage.Name = "DataAccessTabPage";
            DataAccessTabPage.UseVisualStyleBackColor = true;
            TabCtrlInterfaces.TabPages.Add(DataAccessTabPage);

            UserControls.DataAccess dataAccess = new UserControls.DataAccess(this.parentForm);
            if (dataAccess.Init())
            {
                components.Add(dataAccess);
                DataAccessTabPage.Controls.Add(dataAccess);
            }
            else
            {
                dataAccess.Dispose();

                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
                components.Add(notSupported);
                DataAccessTabPage.Controls.Add(notSupported);
            }
#endif

            // --- JHDiagnostic --------------------------------------------------------------------
            //#if JH_DIAGNOSTIC
            //            TabPage DiagnosticTabPage = new TabPage("JHDiagnostic");
            //            DiagnosticTabPage.Name = "DiagnosticTabPage";
            //            DiagnosticTabPage.UseVisualStyleBackColor = true;
            //            TabCtrlInterfaces.TabPages.Add(DiagnosticTabPage);

            //            UserControls.Diagnostics diagnostic = new UserControls.Diagnostics(this.parentForm);
            //            if (diagnostic.Init())
            //            {
            //                components.Add(diagnostic);
            //                DiagnosticTabPage.Controls.Add(diagnostic);
            //            }
            //            else
            //            {
            //                diagnostic.Dispose();

            //                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Only supported on NCK based controls like the TNC 640.");
            //                components.Add(notSupported);
            //                DiagnosticTabPage.Controls.Add(notSupported);
            //            }
            //#endif
#if JH_ERROR
            TabPage ErrorTabPage = new TabPage("JHError");
            ErrorTabPage.Name = "ErrorTabPage";
            ErrorTabPage.UseVisualStyleBackColor = true;
            TabCtrlInterfaces.TabPages.Add(ErrorTabPage);
            axesPositionStreamingTabPageIndex++;

            UserControls.Error error = new UserControls.Error(this.parentForm);
            if (error.Init())
            {
                components.Add(error);
                ErrorTabPage.Controls.Add(error);
            }
            else
            {
                error.Dispose();

                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
                components.Add(notSupported);
                ErrorTabPage.Controls.Add(notSupported);
            }
#endif
            // --- JHRemoteError ------------------------------------------------------------------------
#if JH_REMOTEERROR
            TabPage RemoteErrorTabPage = new TabPage("JHRemoteError");
            RemoteErrorTabPage.Name = "RemoteErrorTabPage";
            RemoteErrorTabPage.UseVisualStyleBackColor = true;
            TabCtrlInterfaces.TabPages.Add(RemoteErrorTabPage);

            UserControls.RemoteError remoteError = new UserControls.RemoteError(this.parentForm);
            if (remoteError.Init())
            {
                components.Add(remoteError);
                RemoteErrorTabPage.Controls.Add(remoteError);
            }
            else
            {
                remoteError.Dispose();

                UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Only supported on NCK based controls like the TNC 640.");
                components.Add(notSupported);
                RemoteErrorTabPage.Controls.Add(notSupported);
            }
#endif
            // --- JHError ------------------------------------------------------------------------------

            // --- only if VirtualTNC dongle is plugged -------------------------------------------------
            // this is a workaround, because there is no infomation about the Virtual TNC dongle in Heidenhain the DNC-Client
            //#if JH_AXESPOSITIONSTREAMING
            //            TabPage AxesPositionStreamingTabPage = new TabPage("JHAxesPositionStreaming");
            //            AxesPositionStreamingTabPage.Name = "AxesPositionStreamingTabPage";
            //            AxesPositionStreamingTabPage.UseVisualStyleBackColor = true;
            //            //TabCtrlInterfaces.TabPages.Add(AxesPositionStreamingTabPage);
            //            TabCtrlInterfaces.TabPages.Insert(axesPositionStreamingTabPageIndex, AxesPositionStreamingTabPage);

            //            HeidenhainDNCLib.JHAxesPositionStreaming tmpAPS = null;
            //            try
            //            {
            //                // --- 1 .try to get the interface --------------------------------------------------------
            //                tmpAPS = this.machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHAXESPOSITIONSTREAMING);

            //                // --- 2. release the interface if there was no error thrown ------------------------------
            //                Marshal.ReleaseComObject(tmpAPS);
            //                tmpAPS = null;

            //                // --- 3. add the AxesPositionStreaming UserControl ---------------------------------------
            //                // --> dongle is plugged
            //                UserControls.AxesPositionStreaming axesPositionStreaming = new UserControls.AxesPositionStreaming(this.parentForm);
            //                if (axesPositionStreaming.Init())
            //                {
            //                    components.Add(axesPositionStreaming);
            //                    AxesPositionStreamingTabPage.Controls.Add(axesPositionStreaming);
            //                }
            //                else
            //                {
            //                    axesPositionStreaming.Dispose();

            //                    UserControls.InfoPanel notSupported = new UserControls.InfoPanel("Not supported.");
            //                    components.Add(notSupported);
            //                    AxesPositionStreamingTabPage.Controls.Add(notSupported);
            //                }
            //            }
            //            catch
            //            {
            //                // 3. add the NoDonglePlugged UserControl
            //                UserControls.InfoPanel dongleNotPlugged = new UserControls.InfoPanel("Dongle not plugged.");
            //                components.Add(dongleNotPlugged);
            //                AxesPositionStreamingTabPage.Controls.Add(dongleNotPlugged);
            //            }
            //            finally
            //            {
            //                // 4. release the com object if it isn't already
            //                if (tmpAPS != null)
            //                {
            //                    Marshal.ReleaseComObject(tmpAPS);
            //                }
            //            }
            //#endif
        }
        #endregion
    }
}
