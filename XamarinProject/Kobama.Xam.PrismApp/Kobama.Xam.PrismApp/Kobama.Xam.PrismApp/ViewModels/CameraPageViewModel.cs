// <copyright file="CameraPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Gallary;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Camera Page
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase" />
    public class CameraPageViewModel : ViewModelBase
    {
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

        private IDeviceService device;

        private IGallaryService gallary;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="camera">The camera.</param>
        /// <param name="gallary">The Gallary Service </param>
        /// <param name="device">The device.</param>
        public CameraPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            IGallaryService gallary,
            IDeviceService device)
            : base(navigationService)
        {
            this.gallary = gallary;

            this.TitleLensButton = "Front";
            this.camera = camera;
            this.camera.ImageMode = ImageAvailableMode.Auto;
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

            this.CommandShot = new DelegateCommand(() =>
            {
                this.camera.TakePicture();
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
        /// Gets the command shot.
        /// </summary>
        /// <value>The command shot.</value>
        public DelegateCommand CommandShot { get; }

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
            this.Logger.CalledMethod($"width:{size.Width} height:{size.Height}");

            try
            {
                this.gallary.SaveImage(image, size, string.Empty, "photo");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message);
            }
        }
    }
}
