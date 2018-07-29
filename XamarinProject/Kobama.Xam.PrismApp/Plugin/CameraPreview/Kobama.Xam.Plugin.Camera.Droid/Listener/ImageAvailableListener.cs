// -----------------------------------------------------------------------
// <copyright file="ImageAvailableListener.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
    using System.IO;
    using System.Threading.Tasks;
    using Android.Graphics;
    using Android.Media;
    using Java.IO;
    using Java.Lang;
    using Java.Nio;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Image available listener.
    /// </summary>
    public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger = new Logger(nameof(ImageAvailableListener));

        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ImageAvailableListener"/> class.
        /// </summary>
        /// <param name="fragment">Fragment.</param>
        public ImageAvailableListener(Camera2 fragment)
        {
            this.owner = fragment ?? throw new System.ArgumentNullException("fragment");
        }

        // public File File { get; private set; }
        // public Camera2BasicFragment Owner { get; private set; }

        /// <summary>
        /// Ons the image available.
        /// </summary>
        /// <param name="reader">Reader.</param>
        public void OnImageAvailable(ImageReader reader)
        {
            // this.logger.CallMethod()
            this.owner.BackgroundHandler.Post(new ImageSaver(this.owner, reader.AcquireNextImage()));
        }

        /// <summary>
        /// Saves a JPEG {@link Image} into the specified {@link File}.
        /// </summary>
        private class ImageSaver : Java.Lang.Object, IRunnable
        {
            private static bool isDecoding = false;
            private readonly Logger logger = new Logger(nameof(ImageSaver));
            private readonly Image image;
            private readonly Camera2 owner;

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="ImageSaver"/> class.
            /// </summary>
            /// <param name="fragment">Frqgment</param>
            /// <param name="image">Image.</param>
            public ImageSaver(Camera2 fragment, Image image)
            {
                // this.logger.CallMethod();
                if (image == null)
                {
                    throw new System.ArgumentNullException(nameof(Image));
                }

                this.owner = fragment ?? throw new System.ArgumentNullException(nameof(Camera2));

                if (isDecoding)
                {
                    image.Close();
                    return;
                }

                isDecoding = true;

                this.image = image;
            }

            /// <summary>
            /// Run this instance.
            /// </summary>
            public void Run()
            {
                // this.logger.CalledMethod();
                if (this.image == null)
                {
                    return;
                }

                Task.Run(() =>
                {
                    byte[] byteImage;
                    System.Drawing.Size size;
                    if (this.image.Format == ImageFormatType.Jpeg)
                    {
                        Image.Plane[] planes = this.image.GetPlanes();
                        var buffer = planes[0].Buffer;
                        byteImage = new byte[buffer.Capacity()];
                        buffer.Get(byteImage);
                        size = new System.Drawing.Size(this.image.Width, this.image.Height);
                    }
                    else
                    {
                        var bmp = ImageUtils.ImageToByteArray(this.image, this.owner.GetOrientation());
                        using (var stream = new MemoryStream())
                        {
                            bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            byteImage = stream.ToArray();
                        }
                        size = new System.Drawing.Size(bmp.Width, bmp.Height);
                    }

                    this.owner.NotifySavedIamage(byteImage, size);

                    this.image.Close();
                    isDecoding = false;
                });
            }
        }
    }
}