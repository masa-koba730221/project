// -----------------------------------------------------------------------
//  <copyright file="CameraLens.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;

namespace Kobama.Xam.Plugin.Camera.Options
{
    /// <summary>
    /// Camera lens.
    /// </summary>
    public enum CameraLens
    {
        /// <summary>
        /// The front.
        /// </summary>
        Front,

        /// <summary>
        /// The rear.
        /// </summary>
        Rear,
    }

    public enum ImageAvailableMode
    {
        Auto,
        EachFrame
    }
}
