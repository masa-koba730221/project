// -----------------------------------------------------------------------
// <copyright file="CameraPreviewView.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.CameraPreview
{
    using Kobama.Xam.Plugin.Camera.Options;
    using Xamarin.Forms;

    /// <summary>
    /// Camera preview view.
    /// </summary>
    public class CameraPreviewView : View
    {
        /// <summary>
        /// Image Mode Property
        /// </summary>
        public static readonly BindableProperty ImageModeProperty =
            BindableProperty.Create(
                nameof(ImageMode),
                typeof(ImageMode),
                typeof(CameraPreviewView),
                default(ImageMode));

        /// <summary>
        /// Lens Property
        /// </summary>
        public static readonly BindableProperty LensProperty =
            BindableProperty.Create(
                nameof(ImageMode),
                typeof(CameraLens),
                typeof(CameraPreviewView),
                default(CameraLens),
                BindingMode.TwoWay);

        /// <summary>
        /// Gets or sets image Mode
        /// </summary>
        public ImageMode ImageMode
        {
            get { return (ImageMode)this.GetValue(ImageModeProperty); }
            set { this.SetValue(ImageModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets Lens
        /// </summary>
        public CameraLens Lens
        {
            get { return (CameraLens)this.GetValue(LensProperty); }
            set { this.SetValue(LensProperty, value); }
        }
    }
}