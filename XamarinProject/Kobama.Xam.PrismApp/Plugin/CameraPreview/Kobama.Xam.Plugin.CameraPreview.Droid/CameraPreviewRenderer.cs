// -----------------------------------------------------------------------
// <copyright file="CameraPreviewRenderer.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Android.Content;
using Kobama.Xam.Plugin.Camera.Droid;
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
        /// <see cref="CameraPreviewRenderer"/> class.
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
                this.cameraPreview = new CameraPreviewViewImpl(this.Context);
                this.cameraPreview.Camera.Lens = this.Element.Lens;
                this.cameraPreview.Camera.ImageMode = this.Element.ImageMode;
                this.SetNativeControl(this.cameraPreview);
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                this.Element.PropertyChanged -= this.OnElementPropertyChanged;
            }

            if (e.NewElement != null)
            {
                this.Element.PropertyChanged += this.OnElementPropertyChanged;
            }
        }

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            mLogger.CalledMethod();
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
            {
                return;
            }

            if (e.PropertyName == CameraPreviewView.LensProperty.PropertyName)
            {
                mLogger.Debug($"{e.PropertyName.ToString()}: {this.Element.Lens}");
                Camera2.Instance.Lens = this.Element.Lens;
            }
            else if (e.PropertyName == CameraPreviewView.ImageModeProperty.PropertyName)
            {
                mLogger.Debug($"{e.PropertyName.ToString()}: {this.Element.ImageMode}");
                Camera2.Instance.ImageMode = this.Element.ImageMode;
            }
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

        /// <summary>
        /// Ons the resume.
        /// </summary>
        private void OnResume()
        {
            mLogger.CalledMethod();
        }

        /// <summary>
        /// Ons the pause.
        /// </summary>
        private void OnPause()
        {
            mLogger.CalledMethod();
        }
   }
}