// -----------------------------------------------------------------------
//  <copyright file="CameraPreviewView.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Linq;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using Kobama.Xam.Plugin.Camera.iOS;
using Kobama.Xam.Plugin.Log;
using UIKit;

namespace Kobama.Xam.Plugin.CameraPreview.iOS
{
    public class CameraPreviewViewImpl : UIView
    {
        private static readonly Logger Log = new Logger(nameof(CameraPreviewViewImpl));
        AVCaptureVideoPreviewLayer previewLayer;

        public event EventHandler<EventArgs> Tapped;

        private UIPinchGestureRecognizer Pinch;

        private CameraPreviewView mCameraPreview;

        private Camera.iOS.Camera mCamera;
        public Kobama.Xam.Plugin.Camera.iOS.Camera CameraControl { get { return this.mCamera; } }

        public CameraPreviewViewImpl(CameraPreviewView camera)
        {
            Log.CallMethod();
            this.mCamera = Camera.iOS.Camera.Instance;
            this.mCamera.CameraView = this;
            this.mCamera.OpenCamera();
            this.previewLayer = this.mCamera.PreviewLayer;
            Layer.AddSublayer(this.previewLayer);

            this.SetPinchGesture();
        }

        public override void Draw(CGRect rect)
        {
            AuthorizeCameraUse();
            Log.CallMethod($"Rect Width:{rect.Width} Height:{rect.Height}");
            base.Draw(rect);
            this.previewLayer.Frame = rect;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            Log.CallMethod();
            base.TouchesBegan(touches, evt);
            OnTapped();
        }

        protected virtual void OnTapped()
        {
            Log.CallMethod();
            var eventHandler = Tapped;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        public void SetPinchGesture()
        {
            nfloat lastscale = 1.0f;
            Pinch = new UIPinchGestureRecognizer((e) => {
                if (e.State == UIGestureRecognizerState.Changed)
                {
                    lastscale = this.mCamera.GesturePinch(e.Scale, lastscale);
                }
                else if (e.State == UIGestureRecognizerState.Ended)
                {
                    lastscale = 1.0f;
                }
            });
            this.AddGestureRecognizer(Pinch);
        }

		protected override void Dispose(bool disposing)
		{
            if (disposing){
                this.RemoveGestureRecognizer(Pinch);
                this.mCamera.Dispose();
            }
            base.Dispose(disposing);
		}

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
