// -----------------------------------------------------------------------
//  <copyright file="CompareSizesByArea.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.CameraPreview.Droid
{
    using Android.Util;
    using Java.Lang;
    using Java.Util;

    /// <summary>
    /// Compare sizes by area.
    /// </summary>
    public class CompareSizesByArea : Java.Lang.Object, IComparator
    {
        /// <summary>
        /// Compare the specified lhs and rhs.
        /// </summary>
        /// <returns>The compare.</returns>
        /// <param name="lhs">Lhs.</param>
        /// <param name="rhs">Rhs.</param>
        public int Compare(Object lhs, Object rhs)
        {
            var lhsSize = (Size)lhs;
            var rhsSize = (Size)rhs;

            // We cast here to ensure the multiplications won't overflow
            return Long.Signum((long)lhsSize.Width * lhsSize.Height - (long)rhsSize.Width * rhsSize.Height);
        }
    }
}