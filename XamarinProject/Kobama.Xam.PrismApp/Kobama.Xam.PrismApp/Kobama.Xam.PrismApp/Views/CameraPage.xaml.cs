// <copyright file="CameraPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Camera Page
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    public partial class CameraPage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPage"/> class.
        /// </summary>
        public CameraPage()
        {
            this.InitializeComponent();
        }

        private CameraPageViewModel VM => this.BindingContext as CameraPageViewModel;

        /// <summary>
        /// Ons the resume.
        /// </summary>
        public virtual void OnResume()
        {
            this.VM?.OnResume();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public virtual void OnSleep()
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
