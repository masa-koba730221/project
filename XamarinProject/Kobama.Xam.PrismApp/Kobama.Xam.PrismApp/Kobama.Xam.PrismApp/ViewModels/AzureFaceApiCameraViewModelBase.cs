// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiCameraViewModelBase.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Gallary;
    using Kobama.Xam.PrismApp.Settings;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure face API camera view model base.
    /// </summary>
    public class AzureFaceApiCameraViewModelBase : CameraPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiCameraViewModelBase"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="gallary">Gallary.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="faceApiService">Face API service.</param>
        public AzureFaceApiCameraViewModelBase(
            INavigationService navigationService,
            ICameraControl camera,
            IGallaryService gallary,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IAzureFaceApiService faceApiService)
            : base(navigationService, camera, gallary, device)
        {
            this.Dialog = dialog;
            this.Settings = settings;
            this.FaceApi = faceApiService;
        }

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
