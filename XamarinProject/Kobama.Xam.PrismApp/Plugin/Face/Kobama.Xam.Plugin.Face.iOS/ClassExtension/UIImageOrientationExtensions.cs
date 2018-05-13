// -----------------------------------------------------------------------
// <copyright file="UIImageOrientationExtensions.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Face.iOS
{
    using CoreImage;
    using ImageIO;
    using UIKit;

    /// <summary>
    /// UIImage Orientation Extensions
    /// </summary>
    public static class UIImageOrientationExtensions
    {
        /// <summary>
        /// To the cg image property orientation.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns>CGImagePropertyOrientation</returns>
        public static CGImagePropertyOrientation ToCGImagePropertyOrientation(this UIImageOrientation self)
        {
            // Take action based on value
            switch (self)
            {
                case UIImageOrientation.Up:
                    return CGImagePropertyOrientation.Up;
                case UIImageOrientation.UpMirrored:
                    return CGImagePropertyOrientation.UpMirrored;
                case UIImageOrientation.Down:
                    return CGImagePropertyOrientation.Down;
                case UIImageOrientation.DownMirrored:
                    return CGImagePropertyOrientation.DownMirrored;
                case UIImageOrientation.Left:
                    return CGImagePropertyOrientation.Left;
                case UIImageOrientation.LeftMirrored:
                    return CGImagePropertyOrientation.LeftMirrored;
                case UIImageOrientation.Right:
                    return CGImagePropertyOrientation.Right;
                case UIImageOrientation.RightMirrored:
                    return CGImagePropertyOrientation.RightMirrored;
            }

            // Default to up
            return CGImagePropertyOrientation.Up;
        }

        /// <summary>
        /// To the ci image orientation.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns>CIImageOrientation</returns>
        public static CIImageOrientation ToCIImageOrientation(this UIImageOrientation self)
        {
            // Take action based on value
            switch (self)
            {
                case UIImageOrientation.Up:
                    return CIImageOrientation.TopLeft;
                case UIImageOrientation.UpMirrored:
                    return CIImageOrientation.TopRight;
                case UIImageOrientation.Down:
                    return CIImageOrientation.BottomLeft;
                case UIImageOrientation.DownMirrored:
                    return CIImageOrientation.BottomRight;
                case UIImageOrientation.Left:
                    return CIImageOrientation.LeftTop;
                case UIImageOrientation.LeftMirrored:
                    return CIImageOrientation.LeftBottom;
                case UIImageOrientation.Right:
                    return CIImageOrientation.RightTop;
                case UIImageOrientation.RightMirrored:
                    return CIImageOrientation.RightBottom;
            }

            // Default to up
            return CIImageOrientation.TopLeft;
        }
    }
}
