// <copyright file="FaceDetectorPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Face;
    using Kobama.Xam.Plugin.Gallary;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Face Detector View Model
    /// </summary>
    /// <seealso cref="Kobama.Xam.PrismApp.ViewModels.CameraPageViewModel" />
    public class FaceDetectorPageViewModel : CameraPageViewModel
    {
        private bool isSaved = false;
        private IFaceDetectorService faceDetector;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceDetectorPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="camera">The camera.</param>
        /// <param name="gallary">The Gallary Service</param>
        /// <param name="device">The device.</param>
        /// <param name="faceDetector">Face Detector</param>
        public FaceDetectorPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            IGallaryService gallary,
            IDeviceService device,
            IFaceDetectorService faceDetector)
            : base(navigationService, camera, gallary, device)
        {
            this.isSaved = false;
            this.faceDetector = faceDetector;
            this.faceDetector.ResutlFaceDetectorCallback += this.FaceDetector_ResutlFaceDetectorCallback;
        }

        /// <summary>
        /// Event Handler Save Image
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        protected override void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.Logger.CalledMethod();
            this.faceDetector.Detector(image);
        }

        private void FaceDetector_ResutlFaceDetectorCallback(Plugin.Face.ResultFaceDtector result)
        {
            if (result.BoundingBoxs.Length > 0)
            {
                this.Logger.Debug("Face found");
            }
            else
            {
                this.Logger.Debug("Face not found");
            }

            if (this.isSaved)
            {
                return;
            }

            this.isSaved = true;
            this.DeviceService.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    this.Logger.CalledMethod("Saved");
                    this.SavedPath = this.GallaryService.SaveImage(result.Image, result.ImageSize, string.Empty, "photo");
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }
    }
}
