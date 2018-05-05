// -----------------------------------------------------------------------
// <copyright file="GallaryImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Gallary.iOS
{
    using System;
    using System.Drawing;
    using Foundation;
    using UIKit;

    /// <summary>
    /// Gallary Impl
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.Gallary.IGallaryService" />
    public class GallaryImpl : IGallaryService
    {
        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <param name="path">The path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Saved Path</returns>
        public string SaveImage(byte[] image, Size size, string path, string fileName)
        {
            var uiImage = new UIImage(NSData.FromArray(image));

            // フォトアルバムに保存する
            uiImage.SaveToPhotosAlbum((i, e) =>
            {
                var o = i as UIImage;
                Console.WriteLine("error:" + e?.LocalizedFailureReason + System.Environment.NewLine +
                                             e?.LocalizedDescription);
            });

            return string.Empty;
        }
    }
}
