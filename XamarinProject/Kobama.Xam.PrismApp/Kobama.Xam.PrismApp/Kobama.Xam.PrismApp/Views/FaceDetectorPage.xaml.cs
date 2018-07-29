// <copyright file="FaceDetectorPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Face Detector
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    public partial class FaceDetectorPage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceDetectorPage"/> class.
        /// </summary>
        public FaceDetectorPage()
        {
            this.InitializeComponent();
        }

        private FaceDetectorPageViewModel VM => this.BindingContext as FaceDetectorPageViewModel;

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
