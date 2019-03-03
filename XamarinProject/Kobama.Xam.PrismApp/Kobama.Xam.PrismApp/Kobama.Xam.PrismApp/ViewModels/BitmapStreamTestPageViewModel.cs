// -----------------------------------------------------------------------
// <copyright file="BitmapStreamTestPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Kobama.Xam.Plugin.Draw;
    using Prism.Navigation;
    using Xamarin.Forms;

        /// <summary>
    /// Bitmap stream test page view model.
    /// </summary>
    public class BitmapStreamTestPageViewModel : ViewModelBase
    {
        private BitmapInfo bitmapInfo;
        private IDrawService drawService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapStreamTestPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="drawService">Draw Service</param>
        public BitmapStreamTestPageViewModel(
            INavigationService navigationService,
            IDrawService drawService)
            : base(navigationService)
        {
            this.drawService = drawService;

            this.Title = "Bitmap Stream Test Page";

            this.InitScreen().ConfigureAwait(false);

            this.Logger.CalledMethod("End");
        }

        /// <summary>
        /// Gets or sets the bitmap info.
        /// </summary>
        /// <value>The bitmap info.</value>
        public BitmapInfo BitmapInfo
        {
            get
            {
                return this.bitmapInfo;
            }

            set
            {
                this.SetProperty(ref this.bitmapInfo, value);
            }
        }

        private async Task InitScreen()
        {
            try
            {
                this.Logger.CalledMethod("Start load");
                await this.drawService.LoadImageAsync(new FileImageSource { File = "MonoMonkey.jpg" }).ConfigureAwait(false);
                this.Logger.CalledMethod("Start GetBitmap");
                this.BitmapInfo = this.drawService.GetBitmap();
                this.Logger.CalledMethod("End");
            }
            catch (Exception ex)
            {
                this.Logger.CalledMethod(ex.ToString());
            }
        }
    }
}
