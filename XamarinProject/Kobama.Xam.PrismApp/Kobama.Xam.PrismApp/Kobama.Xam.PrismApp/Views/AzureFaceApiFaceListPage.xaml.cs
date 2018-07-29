// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiFaceListPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Azure face API registration person page.
    /// </summary>
    public partial class AzureFaceApiFaceListPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiFaceListPage"/> class.
        /// </summary>
        public AzureFaceApiFaceListPage()
        {
            this.InitializeComponent();
        }

        private AzureFaceApiFaceListPageViewModel VM => this.BindingContext as AzureFaceApiFaceListPageViewModel;

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.VM.OnAppearing();
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            this.VM.OnDisappearing();

            base.OnDisappearing();
        }
    }
}
