// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiAddFacePage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Views
{
    using Kobama.Xam.PrismApp.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Azure face API registration camera page.
    /// </summary>
    public partial class AzureFaceApiAddFacePage : ContentPage, Prism.AppModel.IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiAddFacePage"/> class.
        /// </summary>
        public AzureFaceApiAddFacePage()
        {
            this.InitializeComponent();
        }

        private AzureFaceApiAddFacePageViewModel VM => this.BindingContext as AzureFaceApiAddFacePageViewModel;

        /// <summary>
        /// On Resume
        /// </summary>
        public void OnResume()
        {
            this.VM?.OnResume();
        }

        /// <summary>
        /// On Sleep
        /// </summary>
        public void OnSleep()
        {
            this.VM?.OnSleep();
        }

        /// <summary>
        /// On Appearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.VM?.OnAppearing();
        }

        /// <summary>
        /// On Disappearing
        /// </summary>
        protected override void OnDisappearing()
        {
            this.VM?.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
