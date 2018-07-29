// -----------------------------------------------------------------------
// <copyright file="ImageData.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Camera
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Image data.
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageData"/> class.
        /// </summary>
        public ImageData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageData"/> class.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="size">Size.</param>
        public ImageData(byte[] image, Size size)
        {
            this.Image = image;
            this.Size = size;
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public byte[] Image { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size Size { get; set; }
    }
}
