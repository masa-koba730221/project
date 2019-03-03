// -----------------------------------------------------------------------
// <copyright file="CustomImageRenderer.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Kobama.Xam.Plugin.CustomImage;
using Kobama.Xam.Plugin.CustomImage.Droid;
using Kobama.Xam.Plugin.Draw;
using Kobama.Xam.Plugin.Log;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]

namespace Kobama.Xam.Plugin.CustomImage.Droid
{
    /// <summary>
    /// Custom image renderer.
    /// </summary>
    public class CustomImageRenderer: ImageRenderer
    {
        private static Logger mLogger = new Logger(nameof(CustomImageRenderer));
        private Context context;
        private Bitmap rotatedBitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.Plugin.CustomImage.Droid.CustomImageRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public CustomImageRenderer(Context context)
            : base(context)
        {
            this.context = context;
        }

        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">Event Argument</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            var element = (CustomImage)this.Element;

            this.UpdateImage(element.BitmapInfo);

        }

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Argument</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomImage.BitmapInfoProperty.PropertyName)
            {
                this.UpdateImage(((CustomImage)this.Element).BitmapInfo);
            }
        }

        private void UpdateImage(BitmapInfo bi)
        {
            mLogger.CalledMethod();

            if (this.Control == null || bi == null)
            {
                return;
            }

            Handler handler = new Handler(Looper.MainLooper);
            handler.Post(()=>
            {
                var bitmap = (Bitmap)bi.BitmapObject;

                Matrix mat = new Matrix();
                mat.PostRotate(90);
                this.rotatedBitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mat, true);

                this.Control.SetImageBitmap(this.rotatedBitmap);
            });
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(this.rotatedBitmap != null)
            {
                this.rotatedBitmap.Recycle();
            }
        }
    }
}
