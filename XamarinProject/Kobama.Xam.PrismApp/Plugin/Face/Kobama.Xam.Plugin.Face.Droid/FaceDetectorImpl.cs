// -----------------------------------------------------------------------
// <copyright file="FaceDetectorImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Face.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using Android.Gms.Vision;
    using Android.Gms.Vision.Faces;
    using Android.Graphics;
    using Java.Nio;
    using Kobama.Xam.Plugin.Log;

    /// <summary>
    /// Face Detector Impl
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.Face.IFaceDetectorService" />
    public class FaceDetectorImpl : IFaceDetectorService
    {
        private const float IdTextSize = 40.0f;
        private const float IdYOffset = 10.0f;
        private const float IdXOffset = -50.0f;
        private const float BoxStrokeWidth = 5.0f;

        private static int colorIndex = 0;

        private readonly Android.Graphics.Color[] colorChoices =
        {
            Android.Graphics.Color.Blue,
            Android.Graphics.Color.Cyan,
            Android.Graphics.Color.Green,
            Android.Graphics.Color.Magenta,
            Android.Graphics.Color.Red,
            Android.Graphics.Color.White,
            Android.Graphics.Color.Yellow
        };

        private Logger logger = new Logger(nameof(FaceDetectorImpl));

        private Android.Graphics.Paint facePositionPaint;
        private Android.Graphics.Paint idPaint;
        private Android.Graphics.Paint boxPaint;

        /// <summary>
        /// Occurs when resutl face detector callback.
        /// </summary>
        public event ResultFaceDtectorDelegate ResutlFaceDetectorCallback;

        /// <summary>
        /// Detectors the specified byte image.
        /// </summary>
        /// <param name="byteImage">The byte image.</param>
        public void Detector(byte[] byteImage)
        {
            this.logger.CalledMethod();
            var activity = global::Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            var detector = new FaceDetector.Builder(activity)
                                           .SetTrackingEnabled(false)
                                           .SetLandmarkType(LandmarkDetectionType.All)
                                           .SetMode(FaceDetectionMode.Accurate)
                                           .Build();
            var bitmapOptions = new BitmapFactory.Options();
            bitmapOptions.InMutable = true;
            var bitmap = BitmapFactory.DecodeByteArray(byteImage, 0, byteImage.Length, bitmapOptions);
            this.logger.CalledMethod($"Bitmap: {bitmap.Width}, {bitmap.Height}");
            var frame = new Frame.Builder().SetBitmap(bitmap).Build();
            var result = detector.Detect(frame);

            var list = new List<Android.Gms.Vision.Faces.Face>();

            if (result.Size() != 0)
            {
                for (int i = 0; i < result.Size(); i++)
                {
                    var face = (Android.Gms.Vision.Faces.Face)result.ValueAt(i);
                    list.Add(face);
                    Console.WriteLine($"Face {face.Position.X},{face.Position.Y}");
                }

                this.OverlayRectangle(bitmap, list.ToArray());
            }
            else
            {
                Console.WriteLine($"Face is not found");
            }
        }

/// <summary>
        /// Overlaies the rectangle.
        /// </summary>
        /// <param name="bitmap">Bitmap.</param>
        /// <param name="faces">Faces.</param>
        public void OverlayRectangle(Bitmap bitmap, Android.Gms.Vision.Faces.Face[] faces)
        {
            this.logger.CalledMethod();

            colorIndex = (colorIndex + 1) % this.colorChoices.Length;
            var selectedColor = this.colorChoices[colorIndex];

            this.idPaint = new Paint();
            this.idPaint.Color = selectedColor;
            this.idPaint.TextSize = IdTextSize;

            this.boxPaint = new Paint();
            this.boxPaint.Color = selectedColor;
            this.boxPaint.SetStyle(Paint.Style.Stroke);
            this.boxPaint.StrokeWidth = BoxStrokeWidth;

            var canvas = new Canvas(bitmap);

            var rectList = new List<Rectangle>();
            int id = 0;
            foreach (var face in faces)
            {
                id++;
                rectList.Add(new Rectangle((int)face.Position.X, (int)face.Position.Y, (int)face.Width, (int)face.Height));
                float x = face.Width / 2;
                float y = face.Height / 2;

                float xOffset = face.Width / 2.0f;
                float yOffset = face.Height / 2.0f;
                float left = x - xOffset;
                float top = y - yOffset;
                float right = x + xOffset;
                float bottom = y + yOffset;

                canvas.DrawRect(face.Position.X + left, face.Position.Y + top, face.Position.X + right, face.Position.Y + bottom, this.boxPaint);
                canvas.DrawText($"id:{id}", face.Position.X + left, face.Position.Y + bottom + IdYOffset + IdTextSize, this.idPaint);
            }

            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                byteArray = stream.ToArray();
            }

            var result = new ResultFaceDtector(
                rectList.ToArray(),
                byteArray,
                new Size(bitmap.Width, bitmap.Height));
            this.ResutlFaceDetectorCallback?.Invoke(result);
        }
    }
}
