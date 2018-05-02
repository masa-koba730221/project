// <copyright file="IGallaryService.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.Plugin.Gallary
{
    using System.Drawing;

    /// <summary>
    /// Gallary Service Interface Class
    /// </summary>
    public interface IGallaryService
    {
        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <param name="pathm">The pathm.</param>
        /// <param name="fileName">Name of the file.</param>
        void SaveImage(byte[] image, Size size, string pathm, string fileName);
    }
}
