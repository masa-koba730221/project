// -----------------------------------------------------------------------
//  <copyright file="ImageAvailableListener.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
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
        /// Initializes a new instance of the
        /// <see cref="T:Kobama.Xam.Plugin.Camera.Droid.Listener.ImageAvailableListener"/> class.
        /// </summary>
        /// <param name="fragment">Fragment.</param>
        /// <param name="file">File.</param>
        public ImageAvailableListener(Camera2 fragment, File file)
        {
 //           this.logger.CallMethod($"file:{file.ToString()}");

            if (fragment == null)
            {
                throw new System.ArgumentNullException("fragment");
            }

            if (file == null)
            {
                throw new System.ArgumentNullException("file");
            }

            owner = fragment;
            this.file = file;
        }

        /// <summary>
        /// The file.
        /// </summary>
        private readonly File file;

        /// <summary>
        /// The owner.
        /// </summary>
        private readonly Camera2 owner;

        // public File File { get; private set; }
        // public Camera2BasicFragment Owner { get; private set; }

        /// <summary>
        /// Ons the image available.
        /// </summary>
        /// <param name="reader">Reader.</param>
        public void OnImageAvailable(ImageReader reader)
        {
            // this.logger.CallMethod()
            owner.mBackgroundHandler.Post(new ImageSaver(owner, reader.AcquireNextImage(), file));

            //var image = reader.AcquireNextImage();
            //ByteBuffer buffer = image.GetPlanes()[0].Buffer;
            //byte[] bytes = new byte[buffer.Remaining()];
            //buffer.Get(bytes);
            //image.Close();
        }

        /// <summary>
        /// Saves a JPEG {@link Image} into the specified {@link File}.
        /// </summary>
        private class ImageSaver : Java.Lang.Object, IRunnable
        {
            /// <summary>
            /// The logger.
            /// </summary>
            private readonly Logger logger = new Logger(nameof(ImageSaver));

            /// <summary>
            /// The JPEG image
            /// </summary>
            private Image mImage;

            /// <summary>
            /// The file we save the image into.
            /// </summary>
            private File mFile;

            private static bool IsDecoding = false;

            private Camera2 mOwner;

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:Kobama.Xam.Plugin.Camera.Droid.Listener.ImageAvailableListener.ImageSaver"/> class.
            /// </summary>
            /// <param name="image">Image.</param>
            /// <param name="file">File.</param>
            public ImageSaver(Camera2 fragment, Image image, File file)
            {
                
//                this.logger.CallMethod();
                if (image == null)
                {
                    throw new System.ArgumentNullException("image");
                }

                if (file == null)
                {
                    throw new System.ArgumentNullException("file");
                }

                if (fragment == null){
                    throw new System.ArgumentNullException("camera2");
                }

                if (IsDecoding)
                {
                    image.Close();
                    return;
                }

                IsDecoding = true;

                mImage = image;
                mFile = file;
                mOwner = fragment;

            }

            /// <summary>
            /// Run this instance.
            /// </summary>
            public void Run()
            {
//                this.logger.CallMethod();

                if (mImage==null){
                    return;
                }

                ByteBuffer buffer = mImage.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Remaining()];
                buffer.Get(bytes);

                //                var source = new ZXing.PlanarYUVLuminanceSource(bytes, this.mImage.Width, this.mImage.Height, 0, 0, this.mImage.Width, this.mImage.Height, false);

                this.mOwner.NotifySavedIamage(bytes, new System.Drawing.Size(this.mImage.Width, this.mImage.Height));

                mImage.Close();
                //var reader = new ZXing.BarcodeReader();

                //var result = reader.Decode(source);
                //if (result != null){
                //    this.mOwner.NotifyQRCode(true, result.Text);
                //}
                //else
                //{
                //    this.mOwner.NotifyQRCode(false, string.Empty);    
                //}

                IsDecoding = false;

                //using (var output = new FileOutputStream(mFile))
                //{
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
                //        mImage.Close();
                //    }
                //}
            }
        }
    }
}