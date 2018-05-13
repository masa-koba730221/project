// <copyright file="AzureFaceApiDetectPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using System.IO;
    using Kobama.Xam.Plugin.Camera;
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
        /// <summary>
        /// The dialog.
        /// </summary>
        private readonly IPageDialogService dialog;

        private readonly ISettingsService settings;

        private readonly string faceApiRoot;
        private readonly string faceApiKey;

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
        public AzureFaceApiDetectPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            Plugin.Gallary.IGallaryService gallary,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings)
            : base(
                navigationService,
                camera,
                gallary,
                device)
        {
            this.dialog = dialog;
            this.settings = settings;
            this.faceApiRoot = this.settings.AzureFaceApiRoot;
            this.faceApiKey = this.settings.AzureFaceApiKey;

            this.Logger.Debug($"Face API Root: {this.faceApiRoot}");
            this.Logger.Debug($"Face API Key: {this.faceApiKey}");
        }

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
                    using (var strm = new MemoryStream(image))
                    {
                        var client = new Microsoft.ProjectOxford.Face.FaceServiceClient(this.faceApiKey, this.faceApiRoot);
                        var result = await client.DetectAsync(
                            strm,
                            returnFaceAttributes: new[] { FaceAttributeType.Smile });

                        if (result.Length == 0)
                        {
                            await this.dialog.DisplayAlertAsync("Error", $"The face can not be found in the image.", "OK");
                            return;
                        }

                        await this.dialog.DisplayAlertAsync("Smile point", $"Your smile point is {result[0].FaceAttributes.Smile * 100}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await this.dialog.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            });
        }
    }
}
