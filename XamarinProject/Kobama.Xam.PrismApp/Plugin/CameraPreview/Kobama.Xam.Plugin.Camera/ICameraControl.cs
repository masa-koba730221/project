// -----------------------------------------------------------------------
// <copyright file="ICameraControl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Camera
{
    using System.Collections.Generic;
    using System.Drawing;
    using Kobama.Xam.Plugin.Camera.Options;

    /// <summary>
    /// Saved Image
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="size">The size.</param>
    public delegate void SavedImage(byte[] image, Size size);

    /// <summary>
    /// Opened.
    /// </summary>
    public delegate void Opened();

    /// <summary>
    /// Camera control
    /// </summary>
    public interface ICameraControl
    {
        /// <summary>
        /// Occurs when callback saved image.
        /// </summary>
        event SavedImage CallbackSavedImage;

        /// <summary>
        /// Occurs when callabck opened.
        /// </summary>
        event Opened CallabckOpened;

        /// <summary>
        /// Gets or sets the image mode.
        /// </summary>
        /// <value>
        /// The image mode.
        /// </value>
        ImageAvailableMode ImageMode { get; set; }

        /// <summary>
        /// Takes the picture.
        /// </summary>
        void TakePicture();

        /// <summary>
        /// Ons the pause.
        /// </summary>
        void OnPause();

        /// <summary>
        /// Ons the resume.
        /// </summary>
        void OnResume();

        /// <summary>
        /// Ons the destroy.
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// Changes the lens.
        /// </summary>
        /// <param name="lens">Lens.</param>
        void ChangeLens(Options.CameraLens lens);

        /// <summary>
        /// Gets the size list.
        /// </summary>
        /// <returns>The size list.</returns>
        List<Size> GetSizeList();

        /// <summary>
        /// Gets the fps range list.
        /// </summary>
        /// <returns>The fps range list.</returns>
        List<CameraFpsRange> GetFpsRangeList();

        /// <summary>
        /// Sets the option AE mode.
        /// </summary>
        /// <param name="range">The range.</param>
        void SetOptionAEMode(CameraFpsRange range);
    }
}
