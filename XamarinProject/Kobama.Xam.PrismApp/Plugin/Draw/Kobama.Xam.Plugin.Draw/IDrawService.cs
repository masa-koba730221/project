// -----------------------------------------------------------------------
// <copyright file="IDrawService.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Draw
{
    using System.Threading.Tasks;

        /// <summary>
    /// Draw service.
    /// </summary>
    public interface IDrawService
    {
        /// <summary>
        /// Gets the bitmap byte.
        /// </summary>
        /// <returns>The bitmap byte.</returns>
        byte[] GetBitmapByte();

        /// <summary>
        /// Loads the JPEG file.
        /// </summary>
        /// <param name="fileNam">File nam.</param>
        void LoadJpegFile(string fileNam);

        /// <summary>
        /// Loads the image async.
        /// </summary>
        /// <returns>The image async.</returns>
        /// <param name="source">Source.</param>
        Task<bool> LoadImageAsync(Xamarin.Forms.FileImageSource source);

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <returns>The bitmap.</returns>
        BitmapInfo GetBitmap();
    }
}
