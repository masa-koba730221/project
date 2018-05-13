// -----------------------------------------------------------------------
// <copyright file="UIImageExtensions.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Camera.iOS
{
    using System;
    using System.Drawing;
    using CoreGraphics;
    using UIKit;

    /// <summary>
    /// UII mage extensions.
    /// </summary>
    public static class UIImageExtensions
    {
        /// <summary>
        /// Crop image to specitic size and at specific coordinates
        /// </summary>
        /// <param name="sourceImage">Source Image</param>
        /// <param name="crop_x">Crop X</param>
        /// <param name="crop_y">Crop Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>UIImage</returns>
        public static UIImage CropImage(this UIImage sourceImage, int crop_x, int crop_y, int width, int height)
        {
            var imgSize = sourceImage.Size;
            UIGraphics.BeginImageContext(new SizeF(width, height));
            var context = UIGraphics.GetCurrentContext();
            var clippedRect = new RectangleF(0, 0, width, height);
            context.ClipToRect(clippedRect);
            var drawRect = new CGRect(-crop_x, -crop_y, imgSize.Width, imgSize.Height);
            sourceImage.Draw(drawRect);
            var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return modifiedImage;
        }

        /// <summary>
        /// Rotates the image.
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="sourceImage">Source image.</param>
        /// <param name="rotationAngle">Rotation angle.</param>
        /// <returns>UIImage</returns>
        public static UIImage RotateImage(this UIImage sourceImage, double rotationAngle)
        {
            CGImage imgRef = sourceImage.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();
            RectangleF bounds = new RectangleF(0, 0, width, height);

            float angle = Convert.ToSingle((rotationAngle / 180f) * Math.PI);
            transform = CGAffineTransform.MakeRotation(angle);

            UIGraphics.BeginImageContext(bounds.Size);

            CGContext context = UIGraphics.GetCurrentContext();

            context.TranslateCTM(width / 2, height / 2);
            context.SaveState();
            context.ConcatCTM(transform);
            context.SaveState();
            context.ConcatCTM(CGAffineTransform.MakeScale(1.0f, -1.0f));

            context.DrawImage(new RectangleF(-width / 2, -height / 2, width, height), imgRef);
            context.RestoreState();

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }
    }
}
