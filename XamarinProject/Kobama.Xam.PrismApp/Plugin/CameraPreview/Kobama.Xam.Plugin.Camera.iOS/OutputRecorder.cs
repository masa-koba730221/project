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
                this.GetImageFromSampleBuffer(sampleBuffer);

                sampleBuffer.Dispose();

                GC.Collect();  // "Received memory warning." 回避
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
                        var pixelData = new byte[pixelBuffer.Height * pixelBuffer.Width * 4];
                        var rawData = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)pixelBuffer.Height * (int)pixelBuffer.Width * 4);
                        try
                        {
                            // var contextNew = new CGBitmapContext(rawData, pixelBuffer.Width, pixelBuffer.Height, 8, 4 * pixelBuffer.Width, CGColorSpace.CreateDeviceRGB(), (CGImageAlphaInfo)flags);
                            // contextNew.DrawImage(new CGRect(0.0f, 0.0f, (float)pixelBuffer.Width, (float)pixelBuffer.Height), context.ToImage());
                            System.Runtime.InteropServices.Marshal.Copy(pixelBuffer.BaseAddress, pixelData, 0, pixelData.Length);
                            this.camera.NotifySavedIamage(pixelData, new Size((int)pixelBuffer.Width, (int)pixelBuffer.Height));
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.FreeHGlobal(rawData);
                        }

                        using (var image = context.ToImage())
                        {
                            pixelBuffer.Unlock(CVPixelBufferLock.None);
                            return UIImage.FromImage(image);
                        }
                    }
                }
            }
        }
    }
}
