// -----------------------------------------------------------------------
// <copyright file="OutputRecorder.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Camera.iOS
{
    using System;
    using System.Drawing;
    using AVFoundation;
    using CoreGraphics;
    using CoreMedia;
    using CoreVideo;
    using UIKit;

    /// <summary>
    /// Output Recorder
    /// </summary>
    /// <seealso cref="AVFoundation.AVCaptureVideoDataOutputSampleBufferDelegate" />
    public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        private Camera camera;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputRecorder"/> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        public OutputRecorder(Camera camera)
        {
            this.camera = camera;
        }

        /// <summary>
        /// Method invoked when a sample buffer has been written on the configured dispatch queue;  You must dispose the sampleBuffer before returning.
        /// </summary>
        /// <param name="captureOutput">The capture output on which the frame was captured.</param>
        /// <param name="sampleBuffer">The video frame data, part of a small finite pool of buffers.</param>
        /// <param name="connection">The connection on which the video frame was received.</param>
        /// <remarks>
        /// Unless you need to keep the buffer for longer, you must call
        /// Dispose() on the sampleBuffer before returning.  The system
        /// has a limited pool of video frames, and once it runs out of
        /// those buffers, the system will stop calling this method
        /// until the buffers are released.
        /// </remarks>
        public override void DidOutputSampleBuffer(
            AVCaptureOutput captureOutput,
            CMSampleBuffer sampleBuffer,
            AVCaptureConnection connection)
        {
            try
            {
                var uiImage = this.GetImageFromSampleBuffer(sampleBuffer);

                Console.WriteLine("Orientation: " + uiImage.Orientation);
                using (var data = uiImage.AsJPEG())
                {
                    this.camera.RaisedImage(data.ToArray(), new Size((int)uiImage.Size.Width, (int)uiImage.Size.Height));
                }

                sampleBuffer.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sampling buffer: {0}", e.Message);
            }
        }

        private UIImage GetImageFromSampleBuffer(CMSampleBuffer sampleBuffer)
        {
            // Get a pixel buffer from the sample buffer
            using (var pixelBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
            {
                // Lock the base address
                pixelBuffer.Lock(CVPixelBufferLock.None);

                // Prepare to decode buffer
                var flags = CGBitmapFlags.PremultipliedFirst | CGBitmapFlags.ByteOrder32Little;

                // Decode buffer - Create a new colorspace
                using (var cs = CGColorSpace.CreateDeviceRGB())
                {
                    // Create new context from buffer
                    using (var context = new CGBitmapContext(
                        pixelBuffer.BaseAddress,
                        pixelBuffer.Width,
                        pixelBuffer.Height,
                        8,
                        pixelBuffer.BytesPerRow,
                        cs,
                        (CGImageAlphaInfo)flags))
                    {
                        // Get the image from the context
                        using (var cgImage = context.ToImage())
                        {
                            // Unlock and return image
                            pixelBuffer.Unlock(CVPixelBufferLock.None);
                            return UIImage.FromImage(cgImage, 100, this.camera.GetPhotoOrientation());
                        }
                    }
                }
            }
        }
    }
}
