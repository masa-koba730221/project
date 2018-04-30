// -----------------------------------------------------------------------
// <copyright file="CameraFpsRange.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Camera
{
    /// <summary>
    /// Camera Fps Range
    /// </summary>
    public class CameraFpsRange
    {
        private int lower;
        private int upper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraFpsRange"/> class.
        /// </summary>
        public CameraFpsRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraFpsRange"/> class.
        /// </summary>
        /// <param name="lower">Lower.</param>
        /// <param name="upper">Upper.</param>
        public CameraFpsRange(int lower, int upper)
            : base()
        {
            this.lower = lower;
            this.upper = upper;
        }

        /// <summary>
        /// Gets or sets the Upper.
        /// </summary>
        /// <value>The Upper.</value>
        public int Upper
        {
            get { return this.upper; }
            set { this.upper = value; }
        }

        /// <summary>
        /// Gets or sets the Lower.
        /// </summary>
        /// <value>The Lower.</value>
        public int Lower
        {
            get { return this.lower; }
            set { this.lower = value; }
        }
    }
}