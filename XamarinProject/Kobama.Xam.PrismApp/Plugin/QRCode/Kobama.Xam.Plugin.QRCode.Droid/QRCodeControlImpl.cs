// -----------------------------------------------------------------------
// <copyright file="QRCodeControlImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.QRCode.Droid
{
    using System.Drawing;
    using Android.Gms.Vision;
    using Android.Graphics;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// QR Code Control Impl
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.QRCode.IQRCodeControl" />
    public class QRCodeControlImpl : IQRCodeControl
    {
        private readonly Logger logger = new Logger(nameof(QRCodeControlImpl));

        /// <summary>
        /// Occurs when result QRC ode callback.
        /// </summary>
        public event ResultQRCodeDelegate ResultQRCodeCallback;

        /// <summary>
        /// Decode the specified image and size.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="size">Size.</param>
        public void Decode(byte[] image, Size size)
        {
            // var activity = global::Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            var activity = Android.App.Application.Context;
            var detector = new Android.Gms.Vision.Barcodes.BarcodeDetector.Builder(activity)
                                      .SetBarcodeFormats(Android.Gms.Vision.Barcodes.BarcodeFormat.QrCode)
                                      .Build();
            if (detector == null)
            {
                return;
            }

            BitmapFactory.Options bitmapFatoryOptions = new BitmapFactory.Options()
            {
                InPreferredConfig = Bitmap.Config.Argb8888,
                InMutable = false
            };
            Bitmap bmp = BitmapFactory.DecodeByteArray(image, 0, image.Length, bitmapFatoryOptions);
            this.logger.Debug($"bmp size: {bmp.Width} x {bmp.Height}");

            var frame = new Frame.Builder().SetBitmap(bmp).Build();
            var result = detector.Detect(frame);

            string text = string.Empty;
            if (result.Size() != 0)
            {
                var code = (Android.Gms.Vision.Barcodes.Barcode)result.ValueAt(0);
                text = code.DisplayValue;
                this.logger.Debug($"QR Code found {text}");
            }
            else
            {
                this.logger.Debug("QR Code not found");
            }

            detector.Release();
            detector.Dispose();

            this.ResultQRCodeCallback?.Invoke(text);
        }
    }
}
