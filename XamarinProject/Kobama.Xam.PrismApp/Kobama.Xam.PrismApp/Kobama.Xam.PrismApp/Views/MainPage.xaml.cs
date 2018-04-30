// -----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Views
{
    using System.Diagnostics;
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Main page.
    /// </summary>
    public partial class MainPage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
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
            Debug.WriteLine("MainPage: OnResume");
            this.VM?.OnResume();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public void OnSleep()
        {
            Debug.WriteLine("MainPage: OnSleep");
            this.VM?.OnSleep();
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            Debug.WriteLine("MainPage: Appearing");
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            Debug.WriteLine("MainPage: Disappearing");
        }
    }
}