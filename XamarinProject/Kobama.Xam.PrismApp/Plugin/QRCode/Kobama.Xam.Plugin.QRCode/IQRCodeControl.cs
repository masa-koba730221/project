// -----------------------------------------------------------------------
// <copyright file="IQRCodeControl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.QRCode
{
    using System.Drawing;

    /// <summary>
    /// QR Code Control Interface Class
    /// </summary>
    public interface IQRCodeControl
    {
        /// <summary>
        /// Decodes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <returns>Decoded String</returns>
        string Decode(byte[] image, Size size);
    }
}
