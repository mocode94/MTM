// ------------------------------------------------------------------------------------------------
// <copyright file="RemoteErrorEntry.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
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
// This file contains a class which provides a user control used to show the remote error
// entries.
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
    public partial class RemoteErrorEntry : UserControl
    {
        #region "fields"
        /// <summary>
        /// The JHRemoteErrorEntry object describes a new CNC error. This helper object is used by JHRemoteError.
        /// </summary>
        private HeidenhainDNCLib.IJHRemoteErrorEntry remoteErrorEntry = null;

        /// <summary>
        /// Enum to choose the possible action of this control. The possible action depends on the state of this control.
        /// </summary>
        private RemoteErrorAction possibleAction = RemoteErrorAction.Raise;

        /// <summary>
        /// If this boolean is set, the error has to be acknowledged by the DNC application.
        /// </summary>
        private bool withRemoteAck;
        #endregion

        #region "constructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteErrorEntry"/> class.
        /// Copy some useful properties to local fields.
        /// </summary>
        /// <param name="remoteErrorEntry">The IJHRemoteErrorEntry helper object.</param>
        /// <param name="withRemoteAck">The error has to be acknowledged by the DNC application. </param>
        public RemoteErrorEntry(HeidenhainDNCLib.IJHRemoteErrorEntry remoteErrorEntry, bool withRemoteAck = false)
        {
            this.remoteErrorEntry = remoteErrorEntry;
            this.withRemoteAck = withRemoteAck;

            this.Name = Convert.ToString(remoteErrorEntry.Number);
            this.InitializeComponent();

            this.Disposed += new EventHandler(this.RemoteErrorEntry_Disposed);
        }
        #endregion

        #region "enumerations"
        /// <summary>
        /// Enum to choose the possible action of this control.
        /// </summary>
        private enum RemoteErrorAction
        {
            /// <summary>
            /// Raise the error message on the control.
            /// </summary>
            Raise,

            /// <summary>
            /// Remove the error message from the control.
            /// </summary>
            Remove
        }
        #endregion 

        #region "public methods"
        /// <summary>
        /// Show the request to remove the remote error by green coloring the action button.
        /// </summary>
        public void RequestToRemoveError()
        {
            this.ActionButton.BackColor = Color.Green;
        }
        #endregion

        #region "event handler"

        /// <summary>
        /// Raise or remove the error message from tnc. (depends on the current sate).
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ActionButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.possibleAction)
                {
                    case RemoteErrorAction.Raise:
                        RemoteAckCheckBox.Enabled = false;
                        this.remoteErrorEntry.Raise(RemoteAckCheckBox.Checked);
                        this.possibleAction = RemoteErrorAction.Remove;
                        break;
                    case RemoteErrorAction.Remove:
                        try
                        {
                            this.remoteErrorEntry.Remove();
                        }
                        catch
                        {
                            // The remote error has already been cleared on the control.
                            // So there is nothing to do in this case.
                        }
                        finally
                        {
                            RemoteAckCheckBox.Enabled = true;
                            ActionButton.BackColor = SystemColors.Control;
                            this.possibleAction = RemoteErrorAction.Raise;
                        }

                        break;
                }

                ActionButton.Text = Enum.GetName(typeof(RemoteErrorAction), this.possibleAction);
            }
            catch (COMException cex)
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowComException(cex.ErrorCode, className, methodName);
            }
            catch (Exception ex)
            {
                string className = this.GetType().ToString();
                string methodName = MethodInfo.GetCurrentMethod().Name;
                Utils.ShowException(ex, className, methodName);
            }
        }

        /// <summary>
        /// This event is fired if the form becomes loaded
        /// Initialize your GUI here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoteErrorEntry_Load(object sender, EventArgs e)
        {
            ErrorNumberLabel.Text = Convert.ToString(this.remoteErrorEntry.Number);
            ErrorClassLabel.Text = Convert.ToString(this.remoteErrorEntry.Class);
            ErrorMessageLabel.Text = this.remoteErrorEntry.Text;
            RemoteAckCheckBox.Enabled = (this.remoteErrorEntry.Class != HeidenhainDNCLib.DNC_ERROR_CLASS.DNC_EC_NOTE) ? true : false;
            RemoteAckCheckBox.Checked = this.withRemoteAck;
        }

        /// <summary>
        /// Delete this error entry.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Unsubscribe all events, release all interfaces and release all global helper objects here.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RemoteErrorEntry_Disposed(object sender, EventArgs e)
        {
            if (this.remoteErrorEntry != null)
            {
                if (this.possibleAction == RemoteErrorAction.Remove)
                {
                    try
                    {
                        this.remoteErrorEntry.Remove();
                    }
                    catch
                    {
                        // Perhaps this remote error has been cleared on the control.
                        // So there is nothing to do any more.
                    }
                }

                Marshal.ReleaseComObject(this.remoteErrorEntry);
                this.remoteErrorEntry = null;
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
    }
}