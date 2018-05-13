// -----------------------------------------------------------------------
// <copyright file="FaceDetectorImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Face.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using CoreFoundation;
    using CoreGraphics;
    using CoreImage;
    using Foundation;
    using Kobama.Xam.Plugin.Log;
    using UIKit;
    using Vision;

    /// <summary>
    /// Vision Face Detector
    /// </summary>
    /// <seealso cref="UIKit.UIViewController" />
    public class FaceDetectorImpl : IFaceDetectorService
    {
        private readonly Logger logger = new Logger(nameof(FaceDetectorImpl));
        private CIImage inputImage;
        private UIImage rawImage;
        private VNDetectFaceRectanglesRequest faceRectangleRequest;

        /// <summary>
        /// Occurs when resutl face detector callback.
        /// </summary>
        public event ResultFaceDtectorDelegate ResutlFaceDetectorCallback;

        /// <summary>
        /// Overlays the rectangles.
        /// </summary>
        /// <param name="uiImage">The UI image.</param>
        /// <param name="imageSize">Size of the image.</param>
        /// <param name="observations">The observations.</param>
        /// <returns>UIImage</returns>
        public static UIKit.UIImage OverlayRectangles(UIImage uiImage, CGSize imageSize, VNFaceObservation[] observations)
        {
            nfloat fWidth = uiImage.Size.Width;
            nfloat fHeight = uiImage.Size.Height;

            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();

            using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)fWidth, (nint)fHeight, 8, 4 * (nint)fWidth, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.PremultipliedFirst))
            {
                Console.WriteLine("Orientation:" + uiImage.Orientation);
                CGSize newSize = imageSize;
                if (uiImage.Orientation == UIImageOrientation.Up)
                {
                    // correct orientation
                    ctx.DrawImage(new CGRect(0, 0, (double)fWidth, (double)fHeight), uiImage.CGImage);
                }
                else if (uiImage.Orientation == UIImageOrientation.Down)
                {
                    ctx.DrawImage(new CGRect((double)fWidth, (double)fHeight, 0, 0), uiImage.CGImage);
                }
                else
                {
                    // need to rotate image so that rectangle overlays match
                    UIGraphics.BeginImageContextWithOptions(uiImage.Size, false, 0);
                    uiImage.Draw(new CGRect(0, 0, (double)fWidth, (double)fHeight));
                    var img = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    ctx.DrawImage(new CGRect(0, 0, (double)fWidth, (double)fHeight), img.CGImage);
                    newSize = new CGSize(imageSize.Height, imageSize.Width);
                }

                var count = 0;
                foreach (var o in observations)
                {
                    // Draw rectangle
                    count++;
                    var text = "Face: " + count.ToString();
                    Console.WriteLine(o.BoundingBox + " " + o.Confidence);

                    // Fudge/switch coordinates to match UIImage
                    var topLeft = new CGPoint(o.BoundingBox.Left, o.BoundingBox.Top).Scaled(newSize);
                    var topRight = new CGPoint(o.BoundingBox.Left, o.BoundingBox.Bottom).Scaled(newSize);
                    var bottomLeft = new CGPoint(o.BoundingBox.Right, o.BoundingBox.Top).Scaled(newSize);
                    var bottomRight = new CGPoint(o.BoundingBox.Right, o.BoundingBox.Bottom).Scaled(newSize);

                    // set up drawing attributes
                    ctx.SetStrokeColor(UIColor.Red.CGColor);
                    ctx.SetLineWidth(10);

                    // create geometry
                    var path = new CGPath();

                    path.AddLines(new CGPoint[]
                    {
                        topLeft, topRight, bottomRight, bottomLeft
                    });

                    path.CloseSubpath();

                    // add geometry to graphics context and draw it
                    ctx.AddPath(path);
                    ctx.DrawPath(CGPathDrawingMode.Stroke);

                    // Draw text
                    ctx.SelectFont("Helvetica", 60, CGTextEncoding.MacRoman);

                    // Measure the text's width - This involves drawing an invisible string to calculate the X position difference
                    float start, end, textWidth;

                    // Get the texts current position
                    start = (float)ctx.TextPosition.X;

                    // Set the drawing mode to invisible
                    ctx.SetTextDrawingMode(CGTextDrawingMode.Invisible);

                    // Draw the text at the current position
                    ctx.ShowText(text);

                    // Get the end position
                    end = (float)ctx.TextPosition.X;

                    // Subtract start from end to get the text's width
                    textWidth = end - start;
                    ctx.SetFillColor(UIColor.Red.CGColor);

                    // Set the drawing mode back to something that will actually draw Fill for example
                    ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

                    // Draw the text at given coords.
                    ctx.ShowTextAtPoint(topLeft.X, topLeft.Y - 60, text);
                }

                return UIImage.FromImage(ctx.ToImage());
            }
        }

        /// <summary>
        /// Detectors the specified byte image.
        /// </summary>
        /// <param name="byteImage">The byte image.</param>
        public void Detector(byte[] byteImage)
        {
            // Setup Vision Faces Request
            this.faceRectangleRequest = new VNDetectFaceRectanglesRequest(this.HandleRectangles);

            var uiImage = this.ByteArrayToUiimage(byteImage);
            this.rawImage = uiImage;
            var ciImage = new CIImage(uiImage);
            if (ciImage == null)
            {
                this.logger.CalledMethod("Unable to create required CIImage from UIImage.");

                // ShowAlert("Processing Error", "Unable to create required CIImage from UIImage.");
                return;
            }

            this.inputImage = ciImage;
            this.logger.CalledMethod($"inputImage orientation: {uiImage.Orientation.ToCIImageOrientation().ToString()}");

            var handler = new VNImageRequestHandler(this.inputImage, uiImage.Orientation.ToCGImagePropertyOrientation(), new VNImageOptions());
            DispatchQueue.DefaultGlobalQueue.DispatchAsync(() =>
            {
                handler.Perform(new VNRequest[] { this.faceRectangleRequest }, out NSError error);
            });
        }

        private void HandleRectangles(VNRequest request, NSError error)
        {
            var observations = request.GetResults<VNFaceObservation>();
            if (observations == null)
            {
                // ShowAlert("Processing Error", "Unexpected result type from VNDetectFaceRectanglesRequest.");
                this.logger.CalledMethod("Unexpected result type from VNDetectFaceRectanglesRequest.");
                return;
            }

            if (observations.Length < 1)
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    // ClassificationLabel.Text = "No faces detected.";
                    this.logger.CalledMethod("No faces detected.");
                });
                return;
            }

            var summary = string.Empty;
            var imageSize = this.inputImage.Extent.Size;
            bool atLeastOneValid = false;
            this.logger.CalledMethod("Faces:");
            summary += "Faces:" + Environment.NewLine;
            var rectList = new List<Rectangle>();
            foreach (var o in observations)
            {
                // Verify detected face rectangle is valid.
                var boundingBox = o.BoundingBox.Scaled(imageSize);
                if (!this.inputImage.Extent.Contains(boundingBox))
                {
                    this.logger.CalledMethod(" --- Faces out of bounds: " + boundingBox);
                    summary += " --- Faces out of bounds:" + boundingBox + Environment.NewLine;
                }
                else
                {
                    rectList.Add(this.CGRectToRectangle(boundingBox));
                    this.logger.CalledMethod(o.BoundingBox.ToString());
                    summary += o.BoundingBox + Environment.NewLine;
                    atLeastOneValid = true;
                }
            }

            if (!atLeastOneValid)
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    // ClassificationLabel.Text = "No _valid_ faces detected." + Environment.NewLine + summary;
                    this.logger.CalledMethod("No _valid_ faces detected." + Environment.NewLine + summary);
                });
                return;
            }

            // Show the pre-processed image
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                // ClassificationLabel.Text = summary;
                // ClassificationLabel.Lines = 0;
                var overlariedImage = OverlayRectangles(this.rawImage, imageSize, observations);
                using (var data = overlariedImage.AsJPEG())
                {
                    var result = new ResultFaceDtector(
                        rectList.ToArray(),
                        data.ToArray(),
                        new Size((int)overlariedImage.Size.Width, (int)overlariedImage.Size.Height));
                    this.ResutlFaceDetectorCallback?.Invoke(result);
                }
            });
        }

        /// <summary>
        /// Bytes the array to uiimage.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>UIImage</returns>
        private UIImage ByteArrayToUiimage(byte[] bytes)
        {
            UIImage image = null;
            try
            {
                image = new UIImage(NSData.FromArray(bytes));
            }
            catch (Exception)
            {
                image = null;
            }

            return image;
        }

        private Rectangle CGRectToRectangle(CGRect cgRect)
        {
            return new Rectangle((int)cgRect.X, (int)cgRect.Y, (int)cgRect.Width, (int)cgRect.Height);
        }
    }
}
