// -----------------------------------------------------------------------
// <copyright file="ImageUtils.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using System;
    using System.IO;
    using Android.Graphics;
    using Android.Media;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Image Utilis
    /// </summary>
    public class ImageUtils
    {
        private static Logger logger = new Logger(nameof(ImageUtils));

        /// <summary>
        /// Image to Byte Array
        /// </summary>
        /// <param name="image">image</param>
        /// <param name="orientation">orientation</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ImageToByteArray(Image image, int orientation = 0)
        {
            byte[] data = null;
            if (image.Format == ImageFormatType.Jpeg)
            {
                Image.Plane[] planes = image.GetPlanes();
                var buffer = planes[0].Buffer;
                data = new byte[buffer.Capacity()];
                buffer.Get(data);
            }
            else if (image.Format == ImageFormatType.Yuv420888)
            {
                data = NV21toJPEG(YUV_420_888toNV21(image), image.Width, image.Height);
            }

            BitmapFactory.Options bitmapFatoryOptions = new BitmapFactory.Options()
            {
                InPreferredConfig = Bitmap.Config.Rgb565,
                InMutable = true
            };
            Bitmap bmp = BitmapFactory.DecodeByteArray(data, 0, data.Length, bitmapFatoryOptions);

            if (orientation == 0 || image.Format == ImageFormatType.Jpeg)
            {
                return bmp;
            }

            var matrix = new Matrix();
            matrix.PostRotate(orientation);
            bmp = Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true);
            return bmp;
        }

        private static byte[] YUV_420_888toNV21(Image image)
        {
            byte[] nv21;
            var yBuffer = image.GetPlanes()[0].Buffer;
            var uBuffer = image.GetPlanes()[1].Buffer;
            var vBuffer = image.GetPlanes()[2].Buffer;

            int ySize = yBuffer.Remaining();
            int uSize = uBuffer.Remaining();
            int vSize = vBuffer.Remaining();

            nv21 = new byte[ySize + uSize + vSize];

            // U and V are swapped
            yBuffer.Get(nv21, 0, ySize);
            vBuffer.Get(nv21, ySize, vSize);
            uBuffer.Get(nv21, ySize + vSize, uSize);

            return nv21;
        }

        private static byte[] NV21toJPEG(byte[] nv21, int width, int height)
        {
            using (var stream = new MemoryStream())
            {
                YuvImage yuv = new YuvImage(nv21, ImageFormatType.Nv21, width, height, null);
                yuv.CompressToJpeg(new Rect(0, 0, width, height), 100, stream);
                return stream.ToArray();
            }
        }
    }
}
