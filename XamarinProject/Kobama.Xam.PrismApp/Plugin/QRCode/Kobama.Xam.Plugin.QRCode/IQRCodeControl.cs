// -----------------------------------------------------------------------
// <copyright file="IQRCodeControl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.QRCode
{
    using System.Drawing;

    /// <summary>
    /// Result QRC ode delegate.
    /// </summary>
    public delegate void ResultQRCodeDelegate(string result);

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
        void Decode(byte[] image, Size size);

        /// <summary>
        /// Occurs when result of QRC ode callback.
        /// </summary>
        event ResultQRCodeDelegate ResultQRCodeCallback;
    }
}
