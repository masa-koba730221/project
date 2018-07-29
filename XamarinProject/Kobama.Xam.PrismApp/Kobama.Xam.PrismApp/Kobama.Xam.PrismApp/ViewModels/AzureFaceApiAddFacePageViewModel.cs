// <copyright file="AzureFaceApiAddFacePageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using System.IO;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Gallary;
    using Kobama.Xam.PrismApp.Settings;
    using Microsoft.ProjectOxford.Face;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure Face Api Registration Camera Page
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase" />
    public class AzureFaceApiAddFacePageViewModel : AzureFaceApiCameraViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiAddFacePageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="gallary">Gallary.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="faceApiService">Face Api Service</param>
        public AzureFaceApiAddFacePageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            IGallaryService gallary,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IAzureFaceApiService faceApiService)
            : base(navigationService, camera, gallary, device, dialog, settings, faceApiService)
        {
        }

        /// <summary>
        /// Gets or sets the person group name.
        /// </summary>
        /// <value>The person group name</value>
        public static string PersonGroupName { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        /// <value>The name of the person.</value>
        public static string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>The person identifier.</value>
        public static string PersonId { get; set; }

        /// <summary>
        /// Events the handler saved image.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        protected override void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.Logger.CalledMethod($"Image size: width:{size.Width} height:{size.Height}");

            this.DeviceService.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var resultDetect = await this.FaceApi.Detect(image);

                    if (resultDetect.Length == 0)
                    {
                        await this.Dialog.DisplayAlertAsync("Error", $"The face can not be found in the image.", "OK");
                        return;
                    }

                    var resultAdd = await this.FaceApi.AddFace(this.PersonGroupId, System.Guid.Parse(PersonId), image);
                    if (resultAdd == null)
                    {
                        await this.Dialog.DisplayAlertAsync("Error", $"The face can not be added.", "OK");
                        return;
                    }

                    this.Logger.CalledMethod($"Face ID: {resultAdd.PersistedFaceId}");

                    await this.FaceApi.StartTraining(this.PersonGroupId);

                    await this.NavigationService.GoBackAsync();
                }
                catch (Exception ex)
                {
                    await this.Dialog.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            });
        }
    }
}
