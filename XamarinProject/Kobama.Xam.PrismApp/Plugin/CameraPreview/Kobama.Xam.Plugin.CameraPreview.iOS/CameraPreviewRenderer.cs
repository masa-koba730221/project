// -----------------------------------------------------------------------
// <copyright file="CameraPreviewRenderer.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
using Kobama.Xam.Plugin.CameraPreview;
using Kobama.Xam.Plugin.CameraPreview.iOS;
using Kobama.Xam.Plugin.Log;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraPreviewView), typeof(CameraPreviewRenderer))]
namespace Kobama.Xam.Plugin.CameraPreview.iOS
{
    /// <summary>
    /// Camera Preview Renderer
    /// </summary>
    public class CameraPreviewRenderer : ViewRenderer<CameraPreviewView, CameraPreviewViewImpl>
    {
        private static readonly Logger Log = new Logger(nameof(CameraPreviewRenderer));
        private CameraPreviewViewImpl uiCameraPreview;

        /// <summary>
        /// Initalizes this instance.
        /// </summary>
        public static void Initalize()
        {
        }

        /// <summary>
        /// Raises the <see cref="E:ElementChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ElementChangedEventArgs{CameraPreviewView}"/> instance containing the event data.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreviewView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                Log.CalledMethod();
                this.uiCameraPreview = new CameraPreviewViewImpl(e.NewElement);
                this.SetNativeControl(this.uiCameraPreview);
            }

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
            }
        }

        /// <summary>
        /// Called when [element property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
            {
                return;
            }
        }
    }
}