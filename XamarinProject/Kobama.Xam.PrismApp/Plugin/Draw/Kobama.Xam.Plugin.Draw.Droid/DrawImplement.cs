// -----------------------------------------------------------------------
// <copyright file="DrawImplement.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using Android.Graphics;
using Android.Runtime;
using Java.Nio;
using Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;
using System.Threading.Tasks;

namespace Kobama.Xam.Plugin.Draw.Droid
{
    /// <summary>
    /// Draw implement.
    /// </summary>
    public class DrawImplement : IDrawService
    {
        private Bitmap bitmap;

        /// <summary>
        /// Gets the bitmap byte.
        /// </summary>
        /// <returns>The bitmap byte.</returns>
        public byte[] GetBitmapByte()
        {
            return this.InternalGetBitmapByte(this.bitmap);
        }

        /// <summary>
        /// Internals the get bitmap byte.
        /// </summary>
        /// <returns>The get bitmap byte.</returns>
        /// <param name="bitmap">Bitmap.</param>
        public byte[] InternalGetBitmapByte(Bitmap bitmap)
        {
            ByteBuffer buffer = ByteBuffer.Allocate(bitmap.ByteCount);
            bitmap.CopyPixelsToBuffer(buffer);
            buffer.Rewind();

            IntPtr classHandle = JNIEnv.FindClass("java/nio/ByteBuffer");
            IntPtr methodId = JNIEnv.GetMethodID(classHandle, "array", "()[B");
            IntPtr resultHandle = JNIEnv.CallObjectMethod(buffer.Handle, methodId);
            byte[] result = JNIEnv.GetArray<byte>(resultHandle);
            JNIEnv.DeleteLocalRef(resultHandle);

            return result;
        }

        /// <summary>
        /// Loads the JPEG file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public void LoadJpegFile(string fileName)
        {
            var option = new BitmapFactory.Options
            {
                InMutable = true
            };

            if( System.IO.File.Exists($"Resources/drawable/{fileName}"))
            {
                System.Diagnostics.Debug.WriteLine($"Exists");
            }

            this.bitmap = BitmapFactory.DecodeFile(fileName, option);
        }

        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="source">Source.</param>
        public async Task<bool> LoadImageAsync(Xamarin.Forms.FileImageSource source)
        {
            try
            {
                this.bitmap = await this.internalLoadImage(source);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Internals the load image.
        /// </summary>
        /// <returns>The load image.</returns>
        /// <param name="source">Source.</param>
        private Task<Bitmap> internalLoadImage(Xamarin.Forms.FileImageSource source)
        {
            var handler = new FileImageSourceHandler();
            return handler.LoadImageAsync(source, CrossCurrentActivity.Current.Activity);
        }

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <returns>The bitmap.</returns>
        public BitmapInfo GetBitmap()
        {
            if (this.bitmap == null)
            {
                return null;
            }

            return new BitmapInfo
            {
                Size = new System.Drawing.Size(this.bitmap.Width, this.bitmap.Height),
                BitmapObject = (object)this.bitmap,
            };
        }
    }
}
