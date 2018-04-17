// -----------------------------------------------------------------------
//  <copyright file="CameraPreviewViewImpl.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.CameraPreview.Droid
{
    using System;
    using System.Collections.Generic;
    using Android;
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.Graphics;
    using Android.Hardware.Camera2;
    using Android.Hardware.Camera2.Params;
    using Android.Media;
    using Android.OS;
    using Android.Util;
    using Android.Views;
    using Android.Widget;
    using Java.IO;
    using Java.Lang;
    using Java.Util;
    using Java.Util.Concurrent;
    using Kobama.Xam.Plugin.Camera.Droid;
    using Kobama.Xam.Plugin.CameraPreview.Droid;
    using Kobama.Xam.Plugin.Log;
    using Boolean = Java.Lang.Boolean;
    using Math = Java.Lang.Math;
    using Orientation = Android.Content.Res.Orientation;

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
        /// <see cref="T:Kobama.Xam.Plugin.CameraPreview.Droid.CameraPreviewViewImpl"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public CameraPreviewViewImpl(Context context)
            : base(context)
        {
            Log.CallMethod();

            this.mTextureView = new TextureView(context);
            var camera = Camera2.Instance;
            this.mTextureView.SurfaceTextureListener = new CameraPreviewSurfaceTextureListener(camera);
            camera.Context = context;
            camera.TextureView = this.mTextureView;

            this.AddView(this.mTextureView);
        }

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
            Log.CallMethod($"change:{changed} l:{l} t:{t} r:{r} b{b}");
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            Log.CallMethod($"msw:{msw} msh:{msh}");

            // mTextureView Size is detected .
            this.mTextureView.Measure(msw, msh);
            Log.CallMethod($"layout 0,0,{r - l},{b - t}");

            // mTextureView Position is detected.
            this.mTextureView.Layout(0, 0, r - l, b - t);
        }
    }
}