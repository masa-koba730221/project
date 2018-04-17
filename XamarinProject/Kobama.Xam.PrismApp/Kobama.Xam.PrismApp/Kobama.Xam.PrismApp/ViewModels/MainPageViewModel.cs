// -----------------------------------------------------------------------
//  <copyright file="MainPageViewModel.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using Kobama.Xam.Plugin.Log;

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.QRCode;
    using Prism.Commands;
    using Prism.Navigation;

    /// <summary>
    /// Main page view model.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The log.
        /// </summary>
        private Logger log = new Logger(nameof(MainPageViewModel));

        /// <summary>
        /// The camera.
        /// </summary>
        private ICameraControl camera;

        /// <summary>
        /// The title lens button.
        /// </summary>
        private string titleLensButton;

        /// <summary>
        /// The lens mode.
        /// </summary>
        private CameraLens lensMode = CameraLens.Rear;

        /// <summary>
        /// The QR Code service.
        /// </summary>
        private IQRCodeControl qRCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.PrimsApp.ViewModels.MainPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="qrCode">QR Code</param>
        public MainPageViewModel(INavigationService navigationService, 
                                 ICameraControl camera, 
                                IQRCodeControl qrCode
                                )
            : base(navigationService)
        {
            this.Title = "Camera Preview Page";
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
                    this.log.CallMethod("to Front");
                    this.TitleLensButton = "Front";
                    this.camera.ChangeLens(CameraLens.Front);
                    this.lensMode = CameraLens.Front;
                }
                else
                {
                    this.log.CallMethod("to Rear");
                    this.TitleLensButton = "Rear";
                    this.camera.ChangeLens(CameraLens.Rear);
                    this.lensMode = CameraLens.Rear;
                }
            });
        }

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
            this.log.CallMethod();

            var sizes = this.camera.GetSizeList();
            foreach (var size in sizes)
            {
                log.Debug($"Size width:{size.Width}, height:{size.Height}");
            }

            var ranges = this.camera.GetFpsRangeList();
            foreach (var range in ranges)
            {
                log.Debug($"Range lower:{range.Lower} upper:{range.Upper}");
            }
        }

        /// <summary>
        /// Events the handler saved image.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        private void EventHandlerSavedImage(byte[] image, Size size)
        {
            var result = this.qRCodeService.Decode(image, size);
            if (result != null)
            {
                log.Debug($"QR Code : {result}");
            }
        }
    }
}
