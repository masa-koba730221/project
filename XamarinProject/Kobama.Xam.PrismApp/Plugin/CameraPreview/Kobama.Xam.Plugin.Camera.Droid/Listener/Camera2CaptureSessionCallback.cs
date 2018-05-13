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
        private readonly Logger logger = new Logger(nameof(Camera2CaptureSessionCallback));

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Camera2CaptureSessionCallback"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public Camera2CaptureSessionCallback(Camera2 owner)
        {
            this.logger.CalledMethod();
            this.owner = owner ?? throw new System.ArgumentNullException("owner");
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
            if (this.owner.CameraDevice == null)
            {
                return;
            }

            // When the session is ready, we start displaying the preview.
            this.owner.CaptureSession = session;
            try
            {
                // Auto focus should be continuous for camera preview.
                this.owner.Setup3AControlLock(this.owner.PreviewRequestBuilder);

                this.owner.PreviewRequest = this.owner.PreviewRequestBuilder.Build();

                // Finally, we start displaying the camera preview.
                this.owner.CaptureSession.SetRepeatingRequest(this.owner.PreviewRequest, this.owner.CaptureCallback, this.owner.BackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }
   }
}