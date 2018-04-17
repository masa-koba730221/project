// -----------------------------------------------------------------------
//  <copyright file="CameraPreviewRenderer.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Linq;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using Kobama.Xam.Plugin.CameraPreview;
using Kobama.Xam.Plugin.CameraPreview.iOS;
using Kobama.Xam.Plugin.Log;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraPreviewView), typeof(CameraPreviewRenderer))]
namespace Kobama.Xam.Plugin.CameraPreview.iOS
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreviewView, CameraPreviewViewImpl>
    {
        private static readonly Logger Log = new Logger(nameof(CameraPreviewRenderer));
        CameraPreviewViewImpl uiCameraPreview;

        public static void Initalize(){
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreviewView> e)
        {
            base.OnElementChanged(e);


            if (Control == null)
            {
                Log.CallMethod();
                uiCameraPreview = new CameraPreviewViewImpl(e.NewElement);
                SetNativeControl(uiCameraPreview);
            }
            if (e.OldElement != null)
            {
            }
            if (e.NewElement != null)
            {
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;
        }
    }
}