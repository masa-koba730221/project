// -----------------------------------------------------------------------
//  <copyright file="OutputRecoder.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using AVFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using UIKit;

namespace Kobama.Xam.Plugin.Camera.iOS
{

    public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        private Camera camera;

        public OutputRecorder(Camera camera){
            this.camera = camera;
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
                    using (var context = new CGBitmapContext(pixelBuffer.BaseAddress,
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
//                            var contextNew = new CGBitmapContext(rawData, pixelBuffer.Width, pixelBuffer.Height, 8, 4 * pixelBuffer.Width, CGColorSpace.CreateDeviceRGB(), (CGImageAlphaInfo)flags);
//                            contextNew.DrawImage(new CGRect(0.0f, 0.0f, (float)pixelBuffer.Width, (float)pixelBuffer.Height), context.ToImage());
                            System.Runtime.InteropServices.Marshal.Copy(pixelBuffer.BaseAddress, pixelData, 0, pixelData.Length);
                            this.camera.NotifySavedIamage(pixelData, new Size((int)pixelBuffer.Width, (int)pixelBuffer.Height));
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.FreeHGlobal(rawData);
                        }

                        using(var image = context.ToImage()){
                            pixelBuffer.Unlock(CVPixelBufferLock.None);
                            return UIImage.FromImage(image);
                        }
                    }
                }
            }
        }

        public override void DidOutputSampleBuffer(
            AVCaptureOutput captureOutput,
            CMSampleBuffer sampleBuffer,
            AVCaptureConnection connection)
        {

            try
            {
                GetImageFromSampleBuffer(sampleBuffer);

                //これがないと"Received memory warning." で落ちたり、画面の更新が止まったりする
                GC.Collect();  //  "Received memory warning." 回避

            }
            catch (Exception e)
            {
                Console.WriteLine("Error sampling buffer: {0}", e.Message);
            }
        }
    }
}
