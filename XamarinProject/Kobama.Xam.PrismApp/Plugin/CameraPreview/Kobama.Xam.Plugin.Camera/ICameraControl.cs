// -----------------------------------------------------------------------
//  <copyright file="ICameraControl.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using Kobama.Xam.Plugin.Camera.Options;

namespace Kobama.Xam.Plugin.Camera
{
    /// <summary>
    /// delegate Saved Image
    /// </summary>
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

        void SetOptionAEMode(CameraFpsRange range);
    }
}
