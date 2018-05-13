// -----------------------------------------------------------------------
// <copyright file="Camera2StateListener.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using Android.App;
    using Android.Hardware.Camera2;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Camera2 state listener.
    /// </summary>
    public class Camera2StateListener : CameraDevice.StateCallback
    {
        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger = new Logger(nameof(Camera2StateListener));

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Camera2StateListener"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public Camera2StateListener(Camera2 owner)
        {
            this.logger.CalledMethod();
            if (owner == null)
            {
                throw new System.ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// Ons the opened.
        /// </summary>
        /// <param name="camera">Camera device.</param>
        public override void OnOpened(CameraDevice camera)
        {
            this.logger.CalledMethod();

            // This method is called when the camera is opened.  We start camera preview here.
            this.owner.CameraOpenCloseLock.Release();
            this.owner.CameraDevice = camera;
            this.owner.CreateCameraPreviewSession();
        }

        /// <summary>
        /// Ons the disconnected.
        /// </summary>
        /// <param name="camera">Camera device.</param>
        public override void OnDisconnected(CameraDevice camera)
        {
            this.logger.CalledMethod();
            this.owner.CameraOpenCloseLock.Release();
            camera.Close();
            this.owner.CameraDevice = null;
        }

        /// <summary>
        /// Ons the error.
        /// </summary>
        /// <param name="camera">Camera device.</param>
        /// <param name="error">Error.</param>
        public override void OnError(CameraDevice camera, CameraError error)
        {
            this.logger.CalledMethod();
            this.owner.CameraOpenCloseLock.Release();
            camera.Close();
            this.owner.CameraDevice = null;
            if (this.owner == null)
            {
                return;
            }

            Activity activity = (Activity)this.owner.Context;
            if (activity != null)
            {
                activity.Finish();
            }
        }
    }
}