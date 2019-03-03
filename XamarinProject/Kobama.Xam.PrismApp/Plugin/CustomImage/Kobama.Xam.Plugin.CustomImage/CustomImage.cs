// -----------------------------------------------------------------------
// <copyright file="CustomImage.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.CustomImage
{
    using Kobama.Xam.Plugin.Draw;
    using Xamarin.Forms;

    /// <summary>
    /// Custom image.
    /// </summary>
    public class CustomImage : Image
    {
        /// <summary>
        /// Image Mode Property
        /// </summary>
        public static readonly BindableProperty BitmapInfoProperty =
            BindableProperty.Create(
                nameof(BitmapInfo),
                typeof(BitmapInfo),
                typeof(CustomImage),
                default(BitmapInfo));

        /// <summary>
        /// Gets or sets the bitmap info.
        /// </summary>
        /// <value>The bitmap info.</value>
        public BitmapInfo BitmapInfo
        {
            get { return (BitmapInfo)this.GetValue(BitmapInfoProperty); }
            set { this.SetValue(BitmapInfoProperty, value); }
        }
    }
}
