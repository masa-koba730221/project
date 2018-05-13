// -----------------------------------------------------------------------
// <copyright file="Camera2CaptureStillPictureSessionCallback.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using Android.Hardware.Camera2;
    using Android.Util;
    using Android.Views;
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
            this.owner = owner ?? throw new System.ArgumentNullException("owner");
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
            this.logger.CalledMethod();
            this.owner.UnlockFocus();
        }

        /// <summary>
        /// Ons the capture started.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="frameNumber">Frame number.</param>
        public override void OnCaptureStarted(CameraCaptureSession session, CaptureRequest request, long timestamp, long frameNumber)
        {
            this.logger.CalledMethod();
            base.OnCaptureStarted(session, request, timestamp, frameNumber);
        }

        /// <summary>
        /// Ons the capture progressed.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="partialResult">Partial result.</param>
        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            this.logger.CalledMethod();
            base.OnCaptureProgressed(session, request, partialResult);
        }

        /// <summary>
        /// Ons the capture failed.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="failure">Failure.</param>
        public override void OnCaptureFailed(CameraCaptureSession session, CaptureRequest request, CaptureFailure failure)
        {
            this.logger.CalledMethod();
            base.OnCaptureFailed(session, request, failure);
        }

        /// <summary>
        /// Ons the capture sequence aborted.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="sequenceId">Sequence identifier.</param>
        public override void OnCaptureSequenceAborted(CameraCaptureSession session, int sequenceId)
        {
            this.logger.CalledMethod();
            base.OnCaptureSequenceAborted(session, sequenceId);
        }

        /// <summary>
        /// Ons the capture sequence completed.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="sequenceId">Sequence identifier.</param>
        /// <param name="frameNumber">Frame number.</param>
        public override void OnCaptureSequenceCompleted(CameraCaptureSession session, int sequenceId, long frameNumber)
        {
            this.logger.CalledMethod();
            base.OnCaptureSequenceCompleted(session, sequenceId, frameNumber);
        }

        /// <summary>
        /// Ons the capture buffer lost.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="target">Target.</param>
        /// <param name="frameNumber">Frame number.</param>
        public override void OnCaptureBufferLost(CameraCaptureSession session, CaptureRequest request, Surface target, long frameNumber)
        {
            this.logger.CalledMethod();
            base.OnCaptureBufferLost(session, request, target, frameNumber);
        }
    }
}