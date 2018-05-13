// -----------------------------------------------------------------------
// <copyright file="QRCodeReaderPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.QRCode;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Main page view model.
    /// </summary>
    public class QRCodeReaderPageViewModel : ViewModelBase
    {
        private readonly IQRCodeControl qRCodeService;

        private readonly IDeviceService device;

        private readonly ICameraControl camera;

        /// <summary>
        /// The title lens button.
        /// </summary>
        private string titleLensButton;

        /// <summary>
        /// The lens mode.
        /// </summary>
        private CameraLens lensMode = CameraLens.Rear;

        private bool isDecoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeReaderPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="qrCode">QR Code</param>
        /// <param name="device">Device Service</param>
        public QRCodeReaderPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            IQRCodeControl qrCode,
            IDeviceService device)
            : base(navigationService)
        {
            this.device = device;

            this.Title = "QR Code Reader";
            Result = string.Empty;
            this.isDecoding = true;
            this.TitleLensButton = "Front";
            this.qRCodeService = qrCode;
            this.camera = camera;
            this.camera.ImageMode = ImageAvailableMode.EachFrame;
            this.camera.CallabckOpened += this.EventHandelerCameraOpened;
            this.camera.CallbackSavedImage += this.EventHandlerSavedImage;
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
        /// Gets or sets the title lens button.
        /// </summary>
        /// <value>The title lens button.</value>
        public string TitleLensButton
        {
            get { return this.titleLensButton; }
            set { this.SetProperty(ref this.titleLensButton, value); }
        }

        /// <summary>
        /// Gets the command change lens.
        /// </summary>
        /// <value>The command change lens.</value>
        public DelegateCommand CommandChangeLens { get; }

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
            this.camera.OnDestroy();
        }

        /// <summary>
        /// Events the handeler camera opened.
        /// </summary>
        private void EventHandelerCameraOpened()
        {
            this.Logger.CalledMethod();

            var sizes = this.camera.GetSizeList();
            foreach (var size in sizes)
            {
                this.Logger.Debug($"Size width:{size.Width}, height:{size.Height}");
            }

            var ranges = this.camera.GetFpsRangeList();
            foreach (var range in ranges)
            {
                this.Logger.Debug($"Range lower:{range.Lower} upper:{range.Upper}");
            }
        }

        /// <summary>
        /// Events the handler saved image.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        private void EventHandlerSavedImage(byte[] image, Size size)
        {
            if (!this.isDecoding)
            {
                return;
            }

            var result = this.qRCodeService.Decode(image, size);
            if (result != null)
            {
                this.isDecoding = false;

                this.Logger.Debug($"QR Code : {result}");
                Result = result;
                this.device.BeginInvokeOnMainThread(async () =>
                {
                    await this.NavigationService.GoBackAsync();
                });
            }
        }
    }
}
