// <copyright file="FaceDetectorPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera;
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
        private readonly IFaceDetectorService faceDetector;

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
            this.faceDetector = faceDetector;
            this.faceDetector.ResutlFaceDetectorCallback += this.EventHandlerFaceDetector;
        }

        /// <summary>
        /// Events the handler saved image.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        protected override void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.faceDetector.Detector(image);
        }

        private void EventHandlerFaceDetector(ResultFaceDtector result)
        {
            if (result.BoundingBoxs.Length > 0)
            {
                foreach (var b in result.BoundingBoxs)
                {
                    this.Logger.Debug($"BoundingBox : {b.X},{b.Y},{b.Width},{b.Height}");
                }

                if (result.Image != null)
                {
                    try
                    {
                        this.SavedPath = this.GallaryService.SaveImage(result.Image, result.ImageSize, string.Empty, "photo");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message);
                    }
                }
            }
        }
    }
}
