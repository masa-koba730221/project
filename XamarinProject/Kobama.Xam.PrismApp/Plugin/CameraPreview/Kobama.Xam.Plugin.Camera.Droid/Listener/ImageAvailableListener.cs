// -----------------------------------------------------------------------
// <copyright file="ImageAvailableListener.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid.Listener
{
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
                    throw new System.ArgumentNullException("image");
                }

                this.owner = fragment ?? throw new System.ArgumentNullException("camera2");

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
                // this.logger.CallMethod();
                if (this.image == null)
                {
                    return;
                }

                ByteBuffer buffer = this.image.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Remaining()];
                buffer.Get(bytes);

                this.owner.NotifySavedIamage(bytes, new System.Drawing.Size(this.image.Width, this.image.Height));

                this.image.Close();

                isDecoding = false;

                // using (var output = new FileOutputStream(mFile))
                // {
                //    try
                //    {
                //        output.Write(bytes);
                //    }
                //    catch (IOException e)
                //    {
                //        e.PrintStackTrace();
                //    }
                //    finally
                //    {
                //        image.Close();
                //    }
                // }
            }
        }
    }
}