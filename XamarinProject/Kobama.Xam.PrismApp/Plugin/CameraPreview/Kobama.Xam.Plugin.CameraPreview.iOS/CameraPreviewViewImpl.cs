// -----------------------------------------------------------------------
// <copyright file="CameraPreviewViewImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.CameraPreview.iOS
{
    using System;
    using AVFoundation;
    using CoreGraphics;
    using Foundation;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Log;
    using UIKit;

    /// <summary>
    /// Camera Preview View Impl
    /// </summary>
    /// <seealso cref="UIKit.UIView" />
    public class CameraPreviewViewImpl : UIView
    {
        private static readonly Logger Log = new Logger(nameof(CameraPreviewViewImpl));
        private readonly AVCaptureVideoPreviewLayer previewLayer;
        private readonly Camera.iOS.Camera mCamera;
        private UIPinchGestureRecognizer pinch;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPreviewViewImpl"/> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        public CameraPreviewViewImpl(CameraPreviewView camera)
        {
            Log.CalledMethod();
            this.mCamera = Camera.iOS.Camera.Instance;
            this.mCamera.Lens = camera.Lens;
            this.mCamera.ImageMode = camera.ImageMode;
            this.mCamera.CameraView = this;
            this.mCamera.OpenCamera();
            this.previewLayer = this.mCamera.PreviewLayer;
            this.Layer.AddSublayer(this.previewLayer);

            this.SetPinchGesture();
        }

        /// <summary>
        /// Occurs when [tapped].
        /// </summary>
        public event EventHandler<EventArgs> Tapped;

        /// <summary>
        /// Gets the camera control.
        /// </summary>
        /// <value>
        /// The camera control.
        /// </value>
        public Kobama.Xam.Plugin.Camera.iOS.Camera CameraControl
        {
            get { return this.mCamera; }
        }

        /// <summary>
        /// Sets the pinch gesture.
        /// </summary>
        public void SetPinchGesture()
        {
            nfloat lastscale = 1.0f;
            this.pinch = new UIPinchGestureRecognizer((e) =>
            {
                if (e.State == UIGestureRecognizerState.Changed)
                {
                    lastscale = this.mCamera.GesturePinch(e.Scale, lastscale);
                }
                else if (e.State == UIGestureRecognizerState.Ended)
                {
                    lastscale = 1.0f;
                }
            });
            this.AddGestureRecognizer(this.pinch);
        }

        /// <inheritdoc/>
        public override void Draw(CGRect rect)
        {
            this.AuthorizeCameraUse();
            Log.CalledMethod($"Rect Width:{rect.Width} Height:{rect.Height}");
            base.Draw(rect);
            this.previewLayer.Frame = rect;
        }

        /// <inheritdoc/>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            Log.CalledMethod();
            base.TouchesBegan(touches, evt);
            this.OnTapped();
        }

        /// <inheritdoc/>
        public override void LayoutSubviews()
        {
            Log.CalledMethod();
            base.LayoutSubviews();
            this.mCamera.UpdateOrientation();
        }

        /// <summary>
        /// Called when [tapped].
        /// </summary>
        protected virtual void OnTapped()
        {
            if (this.mCamera.ImageMode == ImageMode.Photo)
            {
                Log.CalledMethod();
                this.CameraControl.TakePicture();
                this.Tapped?.Invoke(this, new EventArgs());
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveGestureRecognizer(this.pinch);
                this.mCamera.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Authorizes the camera use.
        /// </summary>
        protected void AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, (accessGranted) => System.Diagnostics.Debug.WriteLine(accessGranted));
            }
        }
    }
}
