// <copyright file="AzureFaceApiDetectPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Azure Face API Detect Page
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    public partial class AzureFaceApiDetectPage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiDetectPage"/> class.
        /// </summary>
        public AzureFaceApiDetectPage()
        {
            this.InitializeComponent();
        }

        private AzureFaceApiDetectPageViewModel VM => this.BindingContext as AzureFaceApiDetectPageViewModel;

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
