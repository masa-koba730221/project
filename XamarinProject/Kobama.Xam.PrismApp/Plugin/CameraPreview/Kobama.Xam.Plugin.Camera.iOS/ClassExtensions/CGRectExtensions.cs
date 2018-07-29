// -----------------------------------------------------------------------
// <copyright file="CGRectExtensions.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Camera.iOS
{
    using CoreGraphics;

    /// <summary>
    /// CGRect Extensions
    /// </summary>
    public static class CGRectExtensions
    {
        /// <summary>
        /// Scaleds the specified size.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="size">The size.</param>
        /// <returns>Scaled Rect</returns>
        public static CGRect Scaled(this CGRect self, CGSize size)
        {
            return new CGRect(
                self.X * size.Width,
                self.Y * size.Height,
                self.Size.Width * size.Width,
                self.Size.Height * size.Height);
        }
    }
}