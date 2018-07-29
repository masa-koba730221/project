// -----------------------------------------------------------------------
// <copyright file="QRCodeControlImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.QRCode.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using CoreGraphics;
    using CoreImage;
    using CoreVideo;
    using Foundation;
    using Kobama.Xam.Plugin.Log;
    using UIKit;
    using Vision;

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
            var uiImage = this.ByteArrayToUiimage(image);
            var ciImage = new CIImage(uiImage);
            if (ciImage == null)
            {
                this.logger.CalledMethod("Unable to create required CIImage from UIImage.");

                // ShowAlert("Processing Error", "Unable to create required CIImage from UIImage.");
                return;
            }

            this.ReadQRCode(ciImage);
        }

        private void ReadQRCode(CIImage image)
        {
            if (image == null)
            {
                return;
            }

            var faceRectangleRequest = new VNDetectBarcodesRequest(this.HandleBarcode);
            var handler = new VNSequenceRequestHandler();
            handler.Perform(new VNRequest[] { faceRectangleRequest }, image, out NSError error);
        }

        private void HandleBarcode(VNRequest request, NSError error)
        {
            if (error != null)
            {
                this.logger.Error(error.ToString());
                return;
            }

            var observations = request.GetResults<VNBarcodeObservation>();
            if (observations == null)
            {
                this.logger.CalledMethod("Unexpected result type from VNDetectBarcodesRequest.");
                return;
            }

            if (observations.Length < 1)
            {
                // ClassificationLabel.Text = "No faces detected.";
                this.logger.CalledMethod("No QR Code");
                return;
            }

            this.ResultQRCodeCallback?.Invoke(observations[0].PayloadStringValue);
        }

        /// <summary>
        /// Bytes the array to uiimage.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>UIImage</returns>
        private UIImage ByteArrayToUiimage(byte[] bytes)
        {
            UIImage image = null;
            try
            {
                image = new UIImage(NSData.FromArray(bytes));
            }
            catch (Exception)
            {
                image = null;
            }

            return image;
        }
    }
}
