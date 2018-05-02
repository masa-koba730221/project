// -----------------------------------------------------------------------
// <copyright file="CameraState.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Camera.Droid
{
    /// <summary>
    /// Camera state.
    /// </summary>
    public enum CameraState
    {
        /// <summary>
        /// Camera state: Showing camera preview.
        /// </summary>
        STATE_PREVIEW,

        /// <summary>
        /// Camera state: Waiting for the focus to be locked.
        /// </summary>
        STATE_WAITING_LOCK,

        /// <summary>
        /// Camera state: Waiting for the exposure to be precapture state.
        /// </summary>
        STATE_WAITING_PRECAPTURE,

        /// <summary>
        /// Camera state: Waiting for the exposure state to be something other than precapture.
        /// </summary>
        STATE_WAITING_NON_PRECAPTURE,

        /// <summary>
        /// Camera state: Picture was taken.
        /// </summary>
        STATE_PICTURE_TAKEN,
    }
}
