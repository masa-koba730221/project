// -----------------------------------------------------------------------
// <copyright file="FaceDetectorImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Face.Droid
{
    /// <summary>
    /// Face Detector Impl
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.Face.IFaceDetectorService" />
    public class FaceDetectorImpl : IFaceDetectorService
    {
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
        }
    }
}
