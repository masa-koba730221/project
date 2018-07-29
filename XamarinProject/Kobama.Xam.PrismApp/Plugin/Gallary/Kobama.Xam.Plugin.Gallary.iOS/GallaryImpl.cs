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
            try
            {
                var uiImage = new UIImage(NSData.FromArray(image));

                // フォトアルバムに保存する
                uiImage.SaveToPhotosAlbum((i, e) =>
                {
                    if (e != null)
                    {
                        var o = i as UIImage;
                        Console.WriteLine("Imae Save Error:" + e?.LocalizedFailureReason + System.Environment.NewLine +
                                                 e?.LocalizedDescription);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image Save Error: {ex.Message}");
            }

            return string.Empty;
        }
    }
}
