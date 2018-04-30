// -----------------------------------------------------------------------
// <copyright file="Camera2CaptureSessionCallback.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using Android.Hardware.Camera2;
    using Android.Util;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Camera2 capture session callback.
    /// </summary>
    public class Camera2CaptureSessionCallback : CameraCaptureSession.StateCallback
    {
        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        /// <summary>
        /// The logger.
        /// </summary>
        private Logger logger = new Logger(nameof(Camera2CaptureSessionCallback));

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Camera2CaptureSessionCallback"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public Camera2CaptureSessionCallback(Camera2 owner)
        {
            this.logger.CalledMethod();
            if (owner == null)
            {
                throw new System.ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// Ons the configure failed.
        /// </summary>
        /// <param name="session">Session.</param>
        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            this.logger.CalledMethod();
        }

        /// <summary>
        /// Ons the configured.
        /// </summary>
        /// <param name="session">Session.</param>
        public override void OnConfigured(CameraCaptureSession session)
        {
            this.logger.CalledMethod();

            // The camera is already closed
            if (this.owner.mCameraDevice == null)
            {
                return;
            }

            // When the session is ready, we start displaying the preview.
            this.owner.mCaptureSession = session;
            try
            {
                // Auto focus should be continuous for camera preview.
                this.owner.mPreviewRequestBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);

                // var list = this.owner.GetFpsRangeList();
                this.owner.mPreviewRequestBuilder.Set(CaptureRequest.ControlAeTargetFpsRange, new Range(15, 15));

                // Flash is automatically enabled when necessary.
                this.owner.SetAutoFlash(this.owner.mPreviewRequestBuilder);

                // Finally, we start displaying the camera preview.
                this.owner.mCaptureSession.SetRepeatingRequest(this.owner.mPreviewRequestBuilder.Build(), this.owner.mCaptureCallback, this.owner.mBackgroundHandler);
                this.owner.mCaptureSession.Capture(this.owner.mPreviewRequestBuilder.Build(), this.owner.mCaptureCallback, this.owner.mBackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }
    }
}