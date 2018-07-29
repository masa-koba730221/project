// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiViewModelBase.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Kobama.Xam.PrismApp.Settings;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure face API view model base.
    /// </summary>
    public class AzureFaceApiViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiViewModelBase"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="faceApiService">Face Api Service</param>
        public AzureFaceApiViewModelBase(
            INavigationService navigationService,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IAzureFaceApiService faceApiService)
            : base(navigationService)
        {
            this.Dialog = dialog;
            this.Device = device;
            this.Settings = settings;
            this.FaceApi = faceApiService;
        }

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>The device.</value>
        protected IDeviceService Device { get; private set; }

        /// <summary>
        /// Gets the dialog.
        /// </summary>
        /// <value>The dialog.</value>
        protected IPageDialogService Dialog { get; private set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        protected ISettingsService Settings { get; private set; }

        /// <summary>
        /// Gets the face API.
        /// </summary>
        /// <value>The face API.</value>
        protected IAzureFaceApiService FaceApi { get; private set; }

        /// <summary>
        /// Gets the face group identifier.
        /// </summary>
        /// <value>The face group identifier.</value>
        protected string PersonGroupId { get; private set; } = "myfriends";
    }
}
