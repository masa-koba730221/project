// -----------------------------------------------------------------------
//  <copyright file="CameraFpsRange.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
namespace Kobama.Xam.Plugin.Camera
{
    public class CameraFpsRange
    {
        private int lower;
        private int upper;

        public CameraFpsRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.Plugin.Camera.CameraFpsRange"/> class.
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