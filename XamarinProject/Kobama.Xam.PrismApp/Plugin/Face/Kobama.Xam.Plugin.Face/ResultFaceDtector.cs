// -----------------------------------------------------------------------
// <copyright file="ResultFaceDtector.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Face
{
    using System.Drawing;

    /// <summary>
    /// Result face dtector.
    /// </summary>
    public class ResultFaceDtector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultFaceDtector"/> class.
        /// </summary>
        public ResultFaceDtector()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultFaceDtector"/> class.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="image">The image.</param>
        /// <param name="imageSize">Size of the image.</param>
        public ResultFaceDtector(Rectangle[] boundingBox, byte[] image, Size imageSize)
        {
            this.BoundingBoxs = boundingBox;
            this.Image = image;
            this.ImageSize = imageSize;
        }

        /// <summary>
        /// Gets or sets the bounding boxs.
        /// </summary>
        /// <value>The bounding boxs.</value>
        public Rectangle[] BoundingBoxs { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public byte[] Image { get; set; }

        /// <summary>
        /// Gets or sets the size of the image.
        /// </summary>
        /// <value>The size of the image.</value>
        public Size ImageSize { get; set; }
    }
}
