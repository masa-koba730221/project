// -----------------------------------------------------------------------
// <copyright file="QRCodeReaderPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// QRC ode reader page.
    /// </summary>
    public partial class QRCodeReaderPage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeReaderPage"/> class.
        /// </summary>
        public QRCodeReaderPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the vm.
        /// </summary>
        /// <value>The vm.</value>
        private ViewModelBase VM => this.BindingContext as ViewModelBase;

        /// <summary>
        /// Ons the resume.
        /// </summary>
        public void OnResume()
        {
            this.VM?.OnResume();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public void OnSleep()
        {
            this.VM?.OnSleep();
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            this.VM?.OnAppearing();
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            this.VM?.OnDisappearing();
        }
    }
}
