// <copyright file="AzureFaceApiDetectPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using System.IO;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.PrismApp.Settings;
    using Microsoft.ProjectOxford.Face;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure face API detect page view model.
    /// </summary>
    public class AzureFaceApiDetectPageViewModel : CameraPageViewModel
    {
        private IAzureFaceApiService faceApiService;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiDetectPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="gallary">Gallary.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Setting Service</param>
        /// <param name="faceApiService">Face Api Service</param>
        public AzureFaceApiDetectPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            Plugin.Gallary.IGallaryService gallary,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IAzureFaceApiService faceApiService)
            : base(
                navigationService,
                camera,
                gallary,
                device)
        {
            this.Dialog = dialog;
            this.Settings = settings;
            this.faceApiService = faceApiService;
            this.FaceApiRoot = this.Settings.AzureFaceApiRoot;
            this.FaceApiKey = this.Settings.AzureFaceApiKey;

            this.Logger.Debug($"Face API Root: {this.FaceApiRoot}");
            this.Logger.Debug($"Face API Key: {this.FaceApiKey}");
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
        /// Gets the face API root.
        /// </summary>
        /// <value>The face API root.</value>
        protected string FaceApiRoot { get; private set; }

        /// <summary>
        /// Gets the face API key.
        /// </summary>
        /// <value>The face API key.</value>
        protected string FaceApiKey { get; private set; }

        /// <summary>
        /// Gets the face group identifier.
        /// </summary>
        /// <value>The face group identifier.</value>
        protected string FaceGroupId { get; private set; } = "myfriends";

        /// <summary>
        /// Events the handler saved image.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="size">Size.</param>
        protected override void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.DeviceService.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var result = await this.faceApiService.Detect(
                        image,
                        returnFaceAttributes: new[] { FaceAttributeType.Smile });

                    if (result.Length == 0)
                    {
                        await this.Dialog.DisplayAlertAsync("Error", $"The face can not be found in the image.", "OK");
                        return;
                    }

                    await this.Dialog.DisplayAlertAsync("Smile point", $"Your smile point is {result[0].FaceAttributes.Smile * 100}", "OK");
                }
                catch (Exception ex)
                {
                    await this.Dialog.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            });
        }
    }
}
