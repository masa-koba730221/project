// -----------------------------------------------------------------------
// <copyright file="ZoomTestPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.Navigation;

    /// <summary>
    /// Zoom test page view model.
    /// </summary>
    public class ZoomTestPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomTestPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public ZoomTestPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
