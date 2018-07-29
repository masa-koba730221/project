// -----------------------------------------------------------------------
// <copyright file="QRCodeReaderPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Gallary;
    using Kobama.Xam.Plugin.QRCode;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Main page view model.
    /// </summary>
    public class QRCodeReaderPageViewModel : CameraPageViewModel
    {
        private readonly IQRCodeControl qRCodeService;

        private readonly IDeviceService device;

        private readonly ICameraControl camera;

        private CameraLens lensMode = CameraLens.Rear;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeReaderPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="qrCode">QR Code</param>
        /// <param name="device">Device Service</param>
        /// <param name="gallary">Gallary Service</param>
        public QRCodeReaderPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            IQRCodeControl qrCode,
            IDeviceService device,
            IGallaryService gallary)
            : base(navigationService, camera, gallary, device)
        {
            this.device = device;

            this.Title = "QR Code Reader";
            Result = string.Empty;
            this.TitleLensButton = "Front";
            this.qRCodeService = qrCode;
            this.qRCodeService.ResultQRCodeCallback += this.QRCodeService_ResultQRCodeCallback;
            this.camera = camera;

            this.lensMode = this.camera.Lens;
            this.CommandChangeLens = new DelegateCommand(() =>
            {
                if (this.lensMode == CameraLens.Rear)
                {
                    this.Logger.CalledMethod("to Front");
                    this.TitleLensButton = "Front";
                    this.camera.ChangeLens(CameraLens.Front);
                    this.lensMode = CameraLens.Front;
                }
                else
                {
                    this.Logger.CalledMethod("to Rear");
                    this.TitleLensButton = "Rear";
                    this.camera.ChangeLens(CameraLens.Rear);
                    this.lensMode = CameraLens.Rear;
                }
            });
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public static string Result { get; set; }

        /// <summary>
        /// On the resume.
        /// </summary>
        public override void OnResume()
        {
            base.OnResume();
            this.camera.OnResume();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public override void OnSleep()
        {
            this.camera.OnPause();
            base.OnSleep();
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public override void Destroy()
        {
            this.OnDisappearing();
            this.camera.OnDestroy();
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();
            this.CameraService.CallbackSavedImage += this.EventHandlerSavedImage;
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public override void OnDisappearing()
        {
            this.CameraService.CallbackSavedImage -= this.EventHandlerSavedImage;
            base.OnDisappearing();
        }

        /// <summary>
        /// Result QR Code callback.
        /// </summary>
        /// <param name="result">Result.</param>
        protected void QRCodeService_ResultQRCodeCallback(string result)
        {
            Result = result;

            if (string.IsNullOrEmpty(Result))
            {
                return;
            }

            this.device.BeginInvokeOnMainThread(async () =>
            {
                await this.NavigationService.GoBackAsync();
            });
        }

        /// <summary>
        /// Event Handler Saved Image
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Save</param>
        protected override void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.Logger.CalledMethod();

            this.qRCodeService.Decode(image, size);
        }
    }
}
