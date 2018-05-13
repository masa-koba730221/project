// -----------------------------------------------------------------------
// <copyright file="CGPointExtensions.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Face.iOS
{
    using System;
    using CoreGraphics;

    /// <summary>
    /// CGPoint Extensions
    /// </summary>
    public static class CGPointExtensions
    {
        /// <summary>
        /// Scaleds the specified size.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="size">The size.</param>
        /// <returns>Scaled Point</returns>
        public static CGPoint Scaled(this CGPoint self, CGSize size)
        {
            Console.WriteLine($"in x:{self.X} y:{self.Y} width:{size.Width} height:{size.Height}");
            Console.WriteLine($"out x:{self.X * size.Width} y:{self.Y * size.Height}");
            return new CGPoint(self.X * size.Width, self.Y * size.Height);
        }
    }
}