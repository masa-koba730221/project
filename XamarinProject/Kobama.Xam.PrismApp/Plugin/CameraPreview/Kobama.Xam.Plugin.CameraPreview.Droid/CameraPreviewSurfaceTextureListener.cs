// -----------------------------------------------------------------------
//  <copyright file="CameraPreviewSurfaceTextureListener.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.CameraPreview.Droid
{
    using Android.Views;
    using Kobama.Xam.Plugin.Camera.Droid;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Camera2 preview surface texture listener.
    /// </summary>
    public class CameraPreviewSurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger = new Logger(nameof(CameraPreviewSurfaceTextureListener));

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Kobama.Xam.Plugin.CameraPreview.Droid.CameraPreviewSurfaceTextureListener"/> class.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public CameraPreviewSurfaceTextureListener(Camera2 owner)
        {
            if (owner == null)
            {
                throw new System.ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// Ons the surface texture available.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            this.logger.CallMethod($"width:{width} height:{height}");
            owner.StartBackgroundThread();
            owner.OpenCamera(width, height);
        }

        /// <summary>
        /// Ons the surface texture destroyed.
        /// </summary>
        /// <returns><c>true</c>, if surface texture destroyed was oned, <c>false</c> otherwise.</returns>
        /// <param name="surface">Surface.</param>
        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            this.logger.CallMethod();
            return true;
        }

        /// <summary>
        /// Ons the surface texture size changed.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            this.logger.CallMethod($"width:{width}, height:{height}");
            owner.ConfigureTransform(width, height);
        }

        /// <summary>
        /// Ons the surface texture updated.
        /// </summary>
        /// <param name="surface">Surface.</param>
        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {
        }
     }
}
