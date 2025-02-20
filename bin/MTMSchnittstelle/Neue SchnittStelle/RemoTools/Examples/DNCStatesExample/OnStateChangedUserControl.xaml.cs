// -----------------------------------------------------------------------
// <copyright file="OnStateChangedUserControl.xaml.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright (c) DR. JOHANNES HEIDENHAIN GmbH. All Rights Reserved.
// </copyright>
// <author>Marco Hayler</author>
// <date>07.10.2015</date>
// -----------------------------------------------------------------------

namespace DNC_CSharp_OnStateChanged
{
    using System;
    using System.Windows.Media;
    using HeidenhainDNCLib;

    /// <summary>
    /// Class to show the XAML state machine.
    /// </summary>
    public partial class OnStateChangedUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnStateChangedUserControl"/> class.
        /// </summary>
        public OnStateChangedUserControl()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        /// <summary>
        /// Update GUI if state has changed.
        /// </summary>
        /// <param name="state">New state.</param>
        public void SetState(DNC_STATE state)
        {
            switch (state)
            {
                case DNC_STATE.DNC_STATE_NOT_INITIALIZED:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.Green;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_HOST_IS_NOT_AVAILABLE:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.Green;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_HOST_IS_AVAILABLE:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.Green;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_WAITING_PERMISSION:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.Green;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_DNC_IS_AVAILABLE:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.Green;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_MACHINE_IS_BOOTED:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.Green;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_MACHINE_IS_INITIALIZING:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.Green;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_MACHINE_IS_AVAILABLE:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.Green;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_MACHINE_IS_SHUTTING_DOWN:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.Green;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_DNC_IS_STOPPED:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.Green;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_HOST_IS_STOPPED:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.Green;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.AliceBlue;
                    break;
                case DNC_STATE.DNC_STATE_NO_PERMISSION:
                    DNC_STATE_NOT_INITIALIZED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_NOT_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_WAITING_PERMISSION.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_BOOTED.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_INITIALIZING.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_AVAILABLE.Background = Brushes.AliceBlue;
                    DNC_STATE_MACHINE_IS_SHUTTING_DOWN.Background = Brushes.AliceBlue;
                    DNC_STATE_DNC_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_HOST_IS_STOPPED.Background = Brushes.AliceBlue;
                    DNC_STATE_NO_PERMISSION.Background = Brushes.Green;
                    break;
            }
        }
    }
}