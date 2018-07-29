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
        private string titleLensButton;
        private CameraLens lensMode = CameraLens.Rear;

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
            this.GallaryService = gallary;
            this.DeviceService = device;

            this.TitleLensButton = "Front";
            this.CameraService = camera;
            this.lensMode = camera.Lens;

            // this.CameraService.ImageMode = ImageMode.Photo;
            this.CommandChangeLens = new DelegateCommand(() =>
            {
                if (this.lensMode == CameraLens.Rear)
                {
                    this.Logger.CalledMethod("to Front");
                    this.TitleLensButton = "Front";
                    this.CameraService.ChangeLens(CameraLens.Front);
                    this.lensMode = CameraLens.Front;
                }
                else
                {
                    this.Logger.CalledMethod("to Rear");
                    this.TitleLensButton = "Rear";
                    this.CameraService.ChangeLens(CameraLens.Rear);
                    this.lensMode = CameraLens.Rear;
                }
            });

            this.CommandShot = new DelegateCommand(() =>
            {
                this.CameraService.TakePicture();
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
        /// Gets or sets the command change lens.
        /// </summary>
        /// <value>
        /// The command change lens.
        /// </value>
        public DelegateCommand CommandChangeLens { get; protected set; }

        /// <summary>
        /// Gets or sets the command shot.
        /// </summary>
        /// <value>
        /// The command shot.
        /// </value>
        public DelegateCommand CommandShot { get; protected set; }

        /// <summary>
        /// Gets or sets the saved path.
        /// </summary>
        /// <value>The saved path.</value>
        public string SavedPath { get; set; }

        /// <summary>
        /// Gets the camera service.
        /// </summary>
        /// <value>
        /// The camera service.
        /// </value>
        protected ICameraControl CameraService { get; private set; }

        /// <summary>
        /// Gets the device service.
        /// </summary>
        /// <value>
        /// The device service.
        /// </value>
        protected IDeviceService DeviceService { get; private set; }

        /// <summary>
        /// Gets the gallary service.
        /// </summary>
        /// <value>The gallary service.</value>
        protected IGallaryService GallaryService { get; private set; }

        /// <summary>
        /// On the resume.
        /// </summary>
        public override void OnResume()
        {
            base.OnResume();
            this.CameraService.OnResume();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public override void OnSleep()
        {
            this.CameraService.OnPause();
            base.OnSleep();
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public override void Destroy()
        {
            this.Logger.CalledMethod();
            this.CameraService.OnDestroy();
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public override void OnAppearing()
        {
            this.Logger.CalledMethod();
            base.OnAppearing();
            this.CameraService.CallabckOpened += this.EventHandelerCameraOpened;

            this.CameraService.CallbackSavedImage += this.EventHandlerSavedImage;
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public override void OnDisappearing()
        {
            this.Logger.CalledMethod();
            base.OnDisappearing();
            this.CameraService.CallabckOpened -= this.EventHandelerCameraOpened;
            this.CameraService.CallbackSavedImage -= this.EventHandlerSavedImage;
        }

        /// <summary>
        /// Events the handeler camera opened.
        /// </summary>
        protected void EventHandelerCameraOpened()
        {
            this.Logger.CalledMethod();

            var sizes = this.CameraService.GetSizeList();
            foreach (var size in sizes)
            {
                this.Logger.Debug($"Size width:{size.Width}, height:{size.Height}");
            }

            var ranges = this.CameraService.GetFpsRangeList();
            foreach (var range in ranges)
            {
                this.Logger.Debug($"Range lower:{range.Lower} upper:{range.Upper}");
            }
        }

        /// <summary>
        /// Event Handler Saved Image
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        protected virtual void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.Logger.CalledMethod();
            try
            {
                this.SavedPath = this.GallaryService.SaveImage(image, size, string.Empty, "photo");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message);
            }
        }
    }
}
