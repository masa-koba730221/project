// -----------------------------------------------------------------------
// <copyright file="QRCodeControlImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.QRCode.Droid
{
    using System.Drawing;

    /// <summary>
    /// QR Code Control Impl
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.QRCode.IQRCodeControl" />
    public class QRCodeControlImpl : IQRCodeControl
    {
        /// <summary>
        /// Decodes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// Decoded String
        /// </returns>
        public string Decode(byte[] image, Size size)
        {
            var source = new ZXing.RGBLuminanceSource(image, size.Width, size.Height, ZXing.RGBLuminanceSource.BitmapFormat.BGRA32);

            var reader = new ZXing.BarcodeReader();

            var result = reader.Decode(source);
            if (result != null)
            {
                return result.Text;
            }
            else
            {
                return null;
            }
        }
    }
}
