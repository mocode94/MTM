// ------------------------------------------------------------------------------------------------
// <copyright file="Utils.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a static class which provides common tools.
// - CheckDncVersion
//   Can be used to check, if the installed HeidenhainDNC COM version meets the minimum
//   requirement of your application.
// - ShowException & ShowComException can be used to show error messages in a consistent manner.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// This class provides static methods to support your application with some common utils.
    /// </summary>
    public static class Utils
    {
        #region "public static methods"
        /// <summary>
        /// Use this method to check if the installed HeidenhainDNC COM version meets the minimum requirement of your application.
        /// You can find out the minimum version requirement of your application by checking the compatibility tables in the HeidenhainDNC API description.
        /// </summary>
        /// <param name="version">Minimum required version as string like "1.5.3.0".</param>
        /// <returns>Return the version string of the installed HeidenhainDNC COM component.</returns>
        public static string CheckDncVersion(string version)
        {
            Version requiredVersion = null;
            string versionString = CheckDncVersionString(version);

            if (!string.IsNullOrEmpty(versionString))
            {
                requiredVersion = new Version(versionString);
            }

            return CheckDncVersion(requiredVersion);
        }

        /// <summary>
        /// Use this method to check if the installed HeidenhainDNC COM version meets the minimum requirement of your application.
        /// You can find out the minimum version requirement of your application by checking the compatibility tables in the HeidenhainDNC API description.
        /// </summary>
        /// <param name="version">Minimum required version as integer array.</param>
        /// <returns>Return the version string of the installed HeidenhainDNC COM component.</returns>
        public static string CheckDncVersion(int[] version)
        {
            Version requiredVersion = null;
            if (version.Length == 4)
            {
                requiredVersion = new Version(version[0], version[1], version[2], version[3]);
            }

            return CheckDncVersion(requiredVersion);
        }

        /// <summary>
        /// Use this method to check if the installed HeidenhainDNC COM version meets the minimum requirement of your application.
        /// You can find out the minimum version requirement of your application by checking the compatibility tables in the HeidenhainDNC API description.
        /// </summary>
        /// <param name="mayor">Minimum required mayor version.</param>
        /// <param name="minor">Minimum required minor version.</param>
        /// <param name="build">Minimum required build version.</param>
        /// <param name="revision">Minimum required revision version.</param>
        /// <returns>Return the version string of the installed HeidenhainDNC COM component.</returns>
        public static string CheckDncVersion(int mayor = 0, int minor = 0, int build = 0, int revision = 0)
        {
            Version requiredVersion = null;
            if (!(mayor < 0 || minor < 0 || build < 0 || revision < 0))
            {
                requiredVersion = new Version(mayor, minor, build, revision);
            }

            return CheckDncVersion(requiredVersion);
        }

        /// <summary>
        /// Use this method to check if the installed HeidenhainDNC COM version meets the minimum requirement of your application.
        /// You can find out the minimum version requirement of your application by checking the compatibility tables in the HeidenhainDNC API description.
        /// </summary>
        /// <param name="requiredVersion">Required version as System.Version data type.</param>
        /// <returns>Return the version string of the installed HeidenhainDNC COM component.</returns>
        public static string CheckDncVersion(Version requiredVersion)
        {
            string presentVersionComInterface = string.Empty;
            string requiredVersionComInterface = string.Empty;

#if MACHINE_IN_PROCESS
            HeidenhainDNCLib.JHMachineInProcess machine = null;
#else
            HeidenhainDNCLib.JHMachine machine = null;
#endif
            try
            {
#if MACHINE_IN_PROCESS
                machine = new HeidenhainDNCLib.JHMachineInProcess();
#else
                machine = new HeidenhainDNCLib.JHMachine();
#endif
                // Get the version strings for the message box an the return value
                presentVersionComInterface = machine.GetVersionComInterface();
                presentVersionComInterface = CheckDncVersionString(presentVersionComInterface);
                requiredVersionComInterface = requiredVersion.ToString();

                Version presentVersion = new Version(presentVersionComInterface);

                // check if installed COM component meets minimum application requirements
                if (requiredVersion > presentVersion)
                {
                    DncVersionNotSufficingMessageBox(presentVersionComInterface, requiredVersionComInterface);
                }
            }
            catch (COMException)
            {
                ErrorUsingDncMessageBox();
            }
            catch (Exception)
            {
                ErrorVerifyDncVersionMessageBox();
            }
            finally
            {
                if (machine != null)
                {
                    Marshal.ReleaseComObject(machine);
                }
            }

            return presentVersionComInterface;
        }

        /// <summary>
        /// Get the nck version of the connected tnc.
        /// NCK_Version(0, 0) if software identification number is not the nck type "597110".
        /// </summary>
        /// <param name="machine">IJHMachine object. The connection to the control has to be established.</param>
        /// <returns>The nck version of the connected tnc.</returns>
#if MACHINE_IN_PROCESS
        public static NCK_Version GetNckVersion(HeidenhainDNCLib.JHMachineInProcess machine)
#else
        public static NCK_Version GetNckVersion(HeidenhainDNCLib.JHMachine machine)
#endif
        {
            NCK_Version presentVersion = new NCK_Version(0, 0);
            HeidenhainDNCLib.JHVersion version = null;
            HeidenhainDNCLib.IJHSoftwareVersionList softwareVersionList = null;
            HeidenhainDNCLib.IJHSoftwareVersion softwareVersion = null;

            try
            {
                version = machine.GetInterface(HeidenhainDNCLib.DNC_INTERFACE_OBJECT.DNC_INTERFACE_JHVERSION);
                softwareVersionList = version.GetVersionNcSoftware();

                for (int i = 0; i < softwareVersionList.Count; i++)
                {
                    softwareVersion = softwareVersionList[i];

                    if (softwareVersion.softwareType == HeidenhainDNCLib.DNC_SW_TYPE.DNC_SW_TYPE_NCKERN)
                    {
                        string fullTncVersion = softwareVersion.bstrIdentNr;
                        string[] tncVersion = fullTncVersion.Split(' ');
                        string identNumber = tncVersion[0];
                        string mileStone = string.Empty;
                        string serviePack = string.Empty;

                        // 597110 is the identification number of the nck base software.
                        if (identNumber == "597110")
                        {
                            if (tncVersion.Length >= 2)
                            {
                                mileStone = tncVersion[1];
                                presentVersion.MileStone = Convert.ToInt32(mileStone);
                            }

                            if (tncVersion.Length >= 3)
                            {
                                serviePack = tncVersion[2].Replace("SP", string.Empty);
                                presentVersion.ServicePack = Convert.ToInt32(serviePack);
                            }
                        }
                    }

                    Marshal.ReleaseComObject(softwareVersion);
                }
            }
            catch
            {
                ErrorVerifyNckVersionMessageBox();
            }
            finally
            {
                if (version != null)
                {
                    Marshal.ReleaseComObject(version);
                }

                if (softwareVersionList != null)
                {
                    Marshal.ReleaseComObject(softwareVersionList);
                }

                if (softwareVersion != null)
                {
                    Marshal.ReleaseComObject(softwareVersion);
                }
            }

            return presentVersion;
        }

        /// ===== message boxes for external use ==================================================
        /// <summary>
        /// Specialized message box for a consistent com exception notification.
        /// </summary>
        /// <param name="errorCode">HResult error code.</param>
        /// <param name="className">Name of the class where the error occurred.</param>
        /// <param name="methodName">Name of the method where the error occurred.</param>
        public static void ShowComException(int errorCode, string className = null, string methodName = null)
        {
            // --- build caption for COM exception message box ----------------------
            // --- build message caption
            string messageBoxCaption = "COM Exception";
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(methodName))
            {
                messageBoxCaption = className + " --> " + methodName;
            }

            // --- build text for COM exception message box
            HeidenhainDNCLib.DNC_HRESULT hresult = (HeidenhainDNCLib.DNC_HRESULT)errorCode;
            string exceptionMessage = "System.Runtime.InteropServices.COMException!" + Environment.NewLine + Environment.NewLine;
            exceptionMessage += "HResult: 0x" + Convert.ToString(errorCode, 16) + Environment.NewLine;
            exceptionMessage += Enum.GetName(typeof(HeidenhainDNCLib.DNC_HRESULT), hresult);

            // --- show message box
            MessageBox.Show(exceptionMessage, messageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Specialized message box for a consistent "non com" exception notification.
        /// </summary>
        /// <param name="ex">The caught System.Exception.</param>
        /// <param name="className">Name of the class where the error occurred.</param>
        /// <param name="methodName">Name of the method where the error occurred.</param>
        public static void ShowException(Exception ex, string className = null, string methodName = null)
        {
            // --- build caption for common exception message box -------------------
            // --- get HResult
            int hresult = 0;
            bool hresultOk = false;
            try
            {
                hresult = Marshal.GetHRForException(ex);
                hresultOk = true;
            }
            catch
            {
                hresultOk = false;
            }

            // --- build message caption
            string messageBoxCaption = "Exception";
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(methodName))
            {
                messageBoxCaption = className + " --> " + methodName;
            }

            // --- build text for COM exception message box
            string exceptionMessage = "Common Exception (System.Exception)!" + Environment.NewLine + Environment.NewLine;
            if (hresultOk)
            {
                exceptionMessage += "HResult: 0x" + string.Format("{0:x}", hresult) + Environment.NewLine;
            }

            exceptionMessage += ex.Message + Environment.NewLine;

            // --- show message box
            MessageBox.Show(exceptionMessage, messageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Specialized message box for a consistent warning messages.
        /// Warning! You are changing data on the control now!
        /// </summary>
        /// <returns>The dialog result, Use this to decide whether the program run should be canceled.</returns>
        public static DialogResult WorkingOnTheControlWarning()
        {
            return MessageBox.Show(
                              "You are changing data on the control now!" + Environment.NewLine +
                              "Do not use the SDK examples on productive systems." + Environment.NewLine + Environment.NewLine + 
                              "We recommend using the examples with programming stations only.",
                              "Attention!",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning);
        }
        #endregion

        #region "private static methods"
        /// <summary>
        /// Check version string. Following formats are allowed "1.5.2.3" | "1,3,2,0".
        /// </summary>
        /// <param name="version">The minimum required version for the application.</param>
        /// <returns>The version of the HeidenhainDNC COM component in format "1.5.3.0".</returns>
        private static string CheckDncVersionString(string version)
        {
            // Convert version string from old format to new format (if necessary)
            string versionString = version;
            if (version.Contains(","))
            {
                versionString = versionString.Replace(',', '.');
            }

            // Check structure of version string
            // --> 1.5.3.0 ==> Major=1, Minor=5, Build=3, Revision=0 
            string[] versionStringArray = versionString.Split('.');
            int arrayLength = versionStringArray.Length;

            if (arrayLength != 4)
            {
                return string.Empty;
            }

            // Check if all of the substrings are numeric
            for (int i = 0; i < arrayLength; i++)
            {
                int versionNumber = 0;
                if (!int.TryParse(versionStringArray[i], out versionNumber))
                {
                    return string.Empty;
                }
            }

            return versionString;
        }

        //// ===== Internal version control specific Message Boxes ================================

        /// <summary>
        /// Standard message for an error while verify the COM version.
        /// </summary>
        private static void ErrorVerifyDncVersionMessageBox()
        {
            // --- something went wrong by verifying HeidenhainDNC version -------- 
            DialogResult dr;
            dr = MessageBox.Show(
                            "Problem verifying HeidenhainDNC version!" + Environment.NewLine +
                            "Possibly the application can't be executed properly." + Environment.NewLine +
                            "Do you want to proceed anyway?" + Environment.NewLine,
                            "Verify HeidenhainDNC version",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Standard message for an error trying to use the HeidenhainDNC COM component.
        /// </summary>
        private static void ErrorUsingDncMessageBox()
        {
            // --- close application, because can't use HeidenhainDNC COM component (reinstall ?)
            MessageBox.Show(
                       "Can't use HeidenhainDNC COM interface!" + Environment.NewLine +
                       "The application has to be closed." + Environment.NewLine +
                       "Please reinstall HeidenhainDNC.",
                       "Verify HeidenhainDNC version",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
            Application.Exit();
        }

        /// <summary>
        /// Standard message if version of installed HeidenhainDNC COM component does not meet minimum requirement of the application.
        /// </summary>
        /// <param name="presentVersionComInterface">Version of the installed HeidenhainDNC COM component.</param>
        /// <param name="requiredVersionComInterface">Minimum required HeidenhainDNC COM component version for the application.</param>
        private static void DncVersionNotSufficingMessageBox(string presentVersionComInterface, string requiredVersionComInterface)
        {
            // --- close application, because HeidenhainDNC version dos not meer minimum application requirement
            DialogResult dr;
            dr = MessageBox.Show(
                        "Your HeidenhainDNC version doesn't meet the minimum application requirement! " +
                        "Please install the latest version of HeidenhainDNC." + Environment.NewLine + Environment.NewLine +
                        "Installed version\t: " + presentVersionComInterface + Environment.NewLine +
                        "Required version\t: " + requiredVersionComInterface + Environment.NewLine + Environment.NewLine +
                        "This application probably won't run properly!" + Environment.NewLine + Environment.NewLine +
                        "Do you want to proceed anyway?",
                        "Verify the installed HeidenhainDNC version.",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.No)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Standard message for an error while verify the nck version.
        /// </summary>
        private static void ErrorVerifyNckVersionMessageBox()
        {
            // --- something went wrong by verifying the nck version of the connected tnc -------- 
            DialogResult dr;
            dr = MessageBox.Show(
                            "Problem verifying the nck version of the connected tnc!" + Environment.NewLine +
                            "Perhaps your TNC is running a test software, anyhow your application may not work properly." + 
                            Environment.NewLine + Environment.NewLine + 
                            "Do you want to proceed anyway?" + Environment.NewLine,
                            "Verify TNC Software version.",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            if (dr == System.Windows.Forms.DialogResult.No)
            {
                Application.Exit();
            }
        }
        //// ======================================================================================
        #endregion

        #region "structs"
        /// <summary>
        /// The nck base software is organized in milestones and service packs.
        /// </summary>
        public struct NCK_Version
        {
            /// <summary>
            /// Main version step of the nck software.
            /// </summary>
            public int MileStone;
            
            /// <summary>
            /// Service pack for the nck software.
            /// </summary>
            public int ServicePack;

            /// <summary>
            /// Initializes a new instance of the <see cref="NCK_Version"/> struct.
            /// </summary>
            /// <param name="mlst">The nck Milestone.</param>
            /// <param name="sp">The nck service pack.</param>
            public NCK_Version(int mlst, int sp)
            {
                this.MileStone = mlst;
                this.ServicePack = sp;
            }
        }
        #endregion
    }
}
