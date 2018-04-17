// -----------------------------------------------------------------------
//  <copyright file="CameraPreviewRenderer.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using Android.Content;
using Android.Hardware;
using Android.Views;
using Kobama.Xam.Plugin.CameraPreview;
using Kobama.Xam.Plugin.CameraPreview.Droid;
using Kobama.Xam.Plugin.Log;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CameraPreviewView), typeof(CameraPreviewRenderer))]

namespace Kobama.Xam.Plugin.CameraPreview.Droid
{
    /// <summary>
    /// Camera preview renderer.
    /// </summary>
    public class CameraPreviewRenderer : ViewRenderer<CameraPreviewView, CameraPreviewViewImpl>
    {
        /// <summary>
        /// The m logger.
        /// </summary>
        private static Logger mLogger = new Logger(nameof(CameraPreviewRenderer));

        /// <summary>
        /// The camera preview.
        /// </summary>
        private CameraPreviewViewImpl cameraPreview;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Kobama.Xam.Plugin.CameraPreview.Droid.CameraPreviewRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public CameraPreviewRenderer(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">Event Arguments</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreviewView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                this.cameraPreview = new CameraPreviewViewImpl(Context);
                this.SetNativeControl(this.cameraPreview);
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                this.Element.PropertyChanged -= OnElementPropertyChanged;
            }

            if (e.NewElement != null)
            {
                this.Element.PropertyChanged += OnElementPropertyChanged;
            }
        }

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            mLogger.CallMethod();
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        private void OnResume()
        {
            mLogger.CallMethod();
        }

        /// <summary>
        /// Ons the pause.
        /// </summary>
        private void OnPause()
        {
            mLogger.CallMethod();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
   }
}