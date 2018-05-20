// -----------------------------------------------------------------------
// <copyright file="IFaceDetectorService.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Drawing;

namespace Kobama.Xam.Plugin.Face
{
    /// <summary>
    /// Delegate for Result of Face Detector
    /// </summary>
    /// <param name="result">The result.</param>
    public delegate void ResultFaceDtectorDelegate(ResultFaceDtector result);

    /// <summary>
    /// Face Detector Service Interface
    /// </summary>
    public interface IFaceDetectorService
    {
        /// <summary>
        /// Occurs when resutl face detector callback.
        /// </summary>
        event ResultFaceDtectorDelegate ResutlFaceDetectorCallback;

        /// <summary>
        /// Detectors the specified byte image.
        /// </summary>
        /// <param name="byteImage">The byte image.</param>
        void Detector(byte[] byteImage);
    }
}
