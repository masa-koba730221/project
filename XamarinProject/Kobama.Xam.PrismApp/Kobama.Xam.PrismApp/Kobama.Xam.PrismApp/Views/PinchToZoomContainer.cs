// -----------------------------------------------------------------------
// <copyright file="PinchToZoomContainer.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Views
{
    using System;
    using Kobama.Xam.Plugin.Log;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    /// <summary>
    /// Pinch to zoom container.
    /// </summary>
        public class PinchToZoomContainer : ContentView
    {
        private double currentScale = 1;
        private double startScale = 1;
        private double xOffset = 0;
        private double yOffset = 0;
        private Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinchToZoomContainer"/> class.
        /// </summary>
        public PinchToZoomContainer()
        {
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += this.OnPanUpdated;

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += this.OnPinchUpdated;
            this.GestureRecognizers.Add(pinchGesture);
            this.GestureRecognizers.Add(panGesture);

            this.logger = new Logger(this.ToString());
        }

        /// <summary>
        /// Ons the pan updated.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    // this.Content.TranslationX = Math.Max(Math.Min(0, this.xOffset + e.TotalX), -Math.Abs(Content.Width - App.ScreenWidth));
                    // this.Content.TranslationY = Math.Max(Math.Min(0, this.yOffset + e.TotalY), -Math.Abs(Content.Height - App.ScreenHeight));
                    this.Content.TranslationX = this.xOffset + e.TotalX;
                    this.Content.TranslationY = this.yOffset + e.TotalY;
                    break;
                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    this.xOffset = this.Content.TranslationX;
                    this.yOffset = this.Content.TranslationY;
                    break;
            }

            this.logger.Debug($"Content Translation:{this.Content.TranslationX}.{this.Content.TranslationY} Size:{this.Content.Width}:{this.Content.Height}");
            this.logger.Debug($"this    Translation:{this.TranslationX}.{this.TranslationY} Size:{this.Width}:{this.Height}");
        }

        /// <summary>
        /// Ons the pinch updated.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                this.startScale = this.Content.Scale;
                this.Content.AnchorX = 0;
                this.Content.AnchorY = 0;

                this.logger.Debug($"Started startScale:{this.startScale}");
            }

            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                this.currentScale += (e.Scale - 1) * this.startScale;
                this.currentScale = Math.Max(1, this.currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = this.Content.X + this.xOffset;
                double deltaX = renderedX / this.Width;
                double deltaWidth = this.Width / (this.Content.Width * this.startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = this.Content.Y + this.yOffset;
                double deltaY = renderedY / this.Height;
                double deltaHeight = this.Height / (this.Content.Height * this.startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = this.xOffset - ((originX * this.Content.Width) * (this.currentScale - this.startScale));
                double targetY = this.yOffset - ((originY * this.Content.Height) * (this.currentScale - this.startScale));

                // Apply translation based on the change in origin.
                // this.Content.TranslationX = targetX.Clamp(-this.Content.Width * (this.currentScale - 1), 0);
                // this.Content.TranslationY = targetY.Clamp(-this.Content.Height * (this.currentScale - 1), 0);
                this.Content.TranslationX = targetX;
                this.Content.TranslationY = targetY;

                // Apply scale factor
                this.Content.Scale = this.currentScale;

                this.logger.Debug($"Running currentScale:{this.currentScale}  origin:{originX}.{originY} delta:{deltaX}.{deltaY}");
                this.logger.Debug($"Content:{this.Content.X}.{this.Content.Y}");
                this.logger.Debug($"Content Translation:{this.Content.TranslationX}.{this.Content.TranslationY} Scale:{this.Content.Scale}");
                this.logger.Debug($"this    Translation:{this.TranslationX}.{this.TranslationY} Scale:{this.Scale}");
            }

            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                this.xOffset = this.Content.TranslationX;
                this.yOffset = this.Content.TranslationY;

                this.logger.Debug($"Completed offset:{this.xOffset}.{this.yOffset}");
            }
        }

        /// <summary>
        /// Ons the binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            /*
            if (this.Content != null)
            {
                var panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += this.OnPanUpdated;

                var pinchGesture = new PinchGestureRecognizer();
                pinchGesture.PinchUpdated += this.OnPinchUpdated;
                this.Content.GestureRecognizers.Add(pinchGesture);
                this.Content.GestureRecognizers.Add(panGesture);
            }
            */
        }
    }
}
