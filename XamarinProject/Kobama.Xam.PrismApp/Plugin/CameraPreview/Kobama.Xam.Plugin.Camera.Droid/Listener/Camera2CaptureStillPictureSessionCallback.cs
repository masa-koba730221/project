// -----------------------------------------------------------------------
// <copyright file="Camera2CaptureStillPictureSessionCallback.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using Android.Hardware.Camera2;
    using Android.Util;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Camera2 capture still picture session callback.
    /// </summary>
    public class Camera2CaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger = new Logger(nameof(Camera2CaptureStillPictureSessionCallback));

        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Camera2CaptureStillPictureSessionCallback"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public Camera2CaptureStillPictureSessionCallback(Camera2 owner)
        {
            if (owner == null)
            {
                throw new System.ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// Ons the capture completed.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="result">Result.</param>
        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            // If something goes wrong with the save (or the handler isn't even
            // registered, this code will toast a success message regardless...)
            //            owner.ShowToast("Saved: " + owner.mFile);
            this.logger.CalledMethod(this.owner.mFile.ToString());
            this.owner.UnlockFocus();
        }
    }
}