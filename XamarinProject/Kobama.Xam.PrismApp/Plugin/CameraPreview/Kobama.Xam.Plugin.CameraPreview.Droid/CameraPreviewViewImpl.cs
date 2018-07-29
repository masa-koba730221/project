// -----------------------------------------------------------------------
// <copyright file="CameraPreviewViewImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.CameraPreview.Droid
{
    using Android.Content;
    using Android.Views;
    using Kobama.Xam.Plugin.Camera.Droid;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Camera2 preview.
    /// </summary>
    public class CameraPreviewViewImpl : ViewGroup
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly Logger Log = new Logger(nameof(CameraPreviewViewImpl));

        /// <summary>
        /// An AutoFitTextureView for camera preview
        /// </summary>
        private TextureView mTextureView;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CameraPreviewViewImpl"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public CameraPreviewViewImpl(Context context)
            : base(context)
        {
            Log.CalledMethod();

            this.mTextureView = new TextureView(context);
            this.Camera = Camera2.Instance;
            this.mTextureView.SurfaceTextureListener = new CameraPreviewSurfaceTextureListener(this.Camera);
            this.Camera.Context = context;
            this.Camera.TextureView = this.mTextureView;

            this.AddView(this.mTextureView);
        }

        /// <summary>
        /// Gets or sets the camera.
        /// </summary>
        /// <value>The camera.</value>
        public Camera2 Camera { get; set; }

       /// <summary>
        /// Ons the layout.
        /// </summary>
        /// <param name="changed">If set to <c>true</c> changed.</param>
        /// <param name="l">L.</param>
        /// <param name="t">T.</param>
        /// <param name="r">The red component.</param>
        /// <param name="b">The blue component.</param>
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Log.CalledMethod($"change:{changed} l:{l} t:{t} r:{r} b{b}");
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            Log.CalledMethod($"msw:{msw} msh:{msh}");

            // mTextureView Size is detected .
            this.mTextureView.Measure(msw, msh);
            Log.CalledMethod($"layout 0,0,{r - l},{b - t}");

            // mTextureView Position is detected.
            this.mTextureView.Layout(0, 0, r - l, b - t);
        }
    }
}