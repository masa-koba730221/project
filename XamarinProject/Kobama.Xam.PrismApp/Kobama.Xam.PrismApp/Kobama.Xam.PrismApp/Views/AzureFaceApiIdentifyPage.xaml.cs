// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiIdentifyPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Azure face API identify page.
    /// </summary>
    public partial class AzureFaceApiIdentifyPage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiIdentifyPage"/> class.
        /// </summary>
        public AzureFaceApiIdentifyPage()
        {
            this.InitializeComponent();
        }

        private AzureFaceApiIdentifyPageViewModel VM => this.BindingContext as AzureFaceApiIdentifyPageViewModel;

        /// <summary>
        /// On Resume.
        /// </summary>
        public void OnResume()
        {
            this.VM?.OnResume();
        }

        /// <summary>
        /// On Sleep.
        /// </summary>
        public void OnSleep()
        {
            this.VM?.OnSleep();
        }

        /// <summary>
        /// On Appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.VM?.OnAppearing();
        }

        /// <summary>
        /// On Disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            this.VM?.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
