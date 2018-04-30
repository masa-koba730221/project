// -----------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.Navigation;

    /// <summary>
    /// Main page view model.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.Title = "Main Page";
        }
    }
}
