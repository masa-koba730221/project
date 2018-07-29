// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiPersonListPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Azure face API person list page.
    /// </summary>
    public partial class AzureFaceApiPersonListPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiPersonListPage"/> class.
        /// </summary>
        public AzureFaceApiPersonListPage()
        {
            this.InitializeComponent();
        }

        private AzureFaceApiPersonListPageViewModel VM => this.BindingContext as AzureFaceApiPersonListPageViewModel;

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
