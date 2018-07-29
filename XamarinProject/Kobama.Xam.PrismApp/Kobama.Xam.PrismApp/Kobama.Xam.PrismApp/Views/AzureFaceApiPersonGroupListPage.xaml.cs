// <copyright file="AzureFaceApiPersonGroupListPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Person Group List Page
    /// </summary>
    public partial class AzureFaceApiPersonGroupListPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiPersonGroupListPage"/> class.
        /// </summary>
        public AzureFaceApiPersonGroupListPage()
        {
            this.InitializeComponent();
        }

        private AzureFaceApiPersonGroupListPageViewModel VM => this.BindingContext as AzureFaceApiPersonGroupListPageViewModel;

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
