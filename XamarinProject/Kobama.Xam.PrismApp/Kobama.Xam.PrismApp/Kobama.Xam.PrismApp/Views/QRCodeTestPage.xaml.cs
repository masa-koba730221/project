// -----------------------------------------------------------------------
// <copyright file="QRCodeTestPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// QRC ode test page.
    /// </summary>
    public partial class QRCodeTestPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeTestPage"/> class.
        /// </summary>
        public QRCodeTestPage()
        {
            this.InitializeComponent();
        }

        private QRCodeTestPageViewModel VM => this.BindingContext as QRCodeTestPageViewModel;

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.VM?.OnAppearing();
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            this.VM?.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
