// -----------------------------------------------------------------------
//  <copyright file="Camera2CaptureListener.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
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
        /// <see cref="T:Kobama.Xam.Plugin.Camera.Droid.Listener.Camera2CaptureListener"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public Camera2CaptureListener(Camera2 owner)
        {
            this.logger.CallMethod();
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
            Process(result);
        }

        /// <summary>
        /// Ons the capture progressed.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="request">Request.</param>
        /// <param name="partialResult">Partial result.</param>
        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            Process(partialResult);
        }

        /// <summary>
        /// Process the specified result.
        /// </summary>
        /// <param name="result">Result.</param>
        private void Process(CaptureResult result)
        {
            switch (owner.State)
            {
                case CameraState.STATE_WAITING_LOCK:
                    {
                        Integer afState = (Integer)result.Get(CaptureResult.ControlAfState);
                        if (afState == null)
                        {
                            owner.CaptureStillPicture();
                        }
                        else if ((((int)ControlAFState.FocusedLocked) == afState.IntValue()) ||
                                   (((int)ControlAFState.NotFocusedLocked) == afState.IntValue()))
                        {
                            // ControlAeState can be null on some devices
                            Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                            if (aeState == null ||
                                    aeState.IntValue() == ((int)ControlAEState.Converged))
                            {
                                owner.State = CameraState.STATE_PICTURE_TAKEN;
                                owner.CaptureStillPicture();
                            }
                            else
                            {
                                owner.RunPrecaptureSequence();
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
                            owner.State = CameraState.STATE_WAITING_NON_PRECAPTURE;
                        }

                        break;
                    }

                case CameraState.STATE_WAITING_NON_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null || aeState.IntValue() != ((int)ControlAEState.Precapture))
                        {
                            owner.State = CameraState.STATE_PICTURE_TAKEN;
                            owner.CaptureStillPicture();
                        }

                        break;
                    }
            }
        }
    }
}