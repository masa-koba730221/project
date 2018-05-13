// -----------------------------------------------------------------------
// <copyright file="Camera2CaptureListener.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using Android.Hardware.Camera2;
    using Java.IO;
    using Java.Lang;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Camera2 capture listener.
    /// </summary>
    public class Camera2CaptureListener : CameraCaptureSession.CaptureCallback
    {
        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger = new Logger(nameof(Camera2CaptureListener));

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Camera2CaptureListener"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public Camera2CaptureListener(Camera2 owner)
        {
            this.logger.CalledMethod();
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
            this.Process(result);
        }

        /// <summary>
        /// Ons the capture progressed.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="partialResult">Partial result.</param>
        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            this.Process(partialResult);
        }

        /// <summary>
        /// Process the specified result.
        /// </summary>
        /// <param name="result">Result.</param>
        private void Process(CaptureResult result)
        {
            switch (this.owner.State)
            {
                case CameraState.STATE_WAITING_LOCK:
                    {
                        Integer afState = (Integer)result.Get(CaptureResult.ControlAfState);
                        if (afState == null)
                        {
                            this.logger.CalledMethod("STATE_WAITING_LOCK: afState is null");
                            this.owner.State = CameraState.STATE_PICTURE_TAKEN;
                            this.owner.CaptureStillPicture();
                        }
                        else if ((afState.IntValue() == ((int)ControlAFState.FocusedLocked)) ||
                                   (afState.IntValue() == ((int)ControlAFState.NotFocusedLocked)))
                        {
                            // ControlAeState can be null on some devices
                            Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                            if (aeState == null ||
                                    aeState.IntValue() == ((int)ControlAEState.Converged))
                            {
                                this.logger.CalledMethod("STATE_WAITING_LOCK: aeState");
                                this.owner.State = CameraState.STATE_PICTURE_TAKEN;
                                this.owner.CaptureStillPicture();
                            }
                            else
                            {
                                this.owner.RunPrecaptureSequence();
                            }
                        }
                        else if (!this.owner.IsAFRun())
                        {
                            if (this.owner.HitTimeoutLocked())
                            {
                                this.logger.CalledMethod("STATE_WAITING_LOCK: !IsAFRun()");
                                this.owner.State = CameraState.STATE_PICTURE_TAKEN;
                                this.owner.CaptureStillPicture();
                            }
                        }

                        break;
                    }

                case CameraState.STATE_WAITING_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null ||
                                aeState.IntValue() == ((int)ControlAEState.Precapture) ||
                                aeState.IntValue() == ((int)ControlAEState.FlashRequired))
                        {
                            this.owner.State = CameraState.STATE_WAITING_NON_PRECAPTURE;
                        }

                        break;
                    }

                case CameraState.STATE_WAITING_NON_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null || aeState.IntValue() != ((int)ControlAEState.Precapture))
                        {
                            this.owner.State = CameraState.STATE_PICTURE_TAKEN;
                            this.owner.CaptureStillPicture();
                        }

                        break;
                    }
            }
        }
    }
}