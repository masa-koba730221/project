// <copyright file="AzureFaceApiTopPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;

    /// <summary>
    /// Azure Face API Top Page View Model
    /// </summary>
    /// <seealso cref="Kobama.Xam.PrismApp.ViewModels.ViewModelBase" />
    public class AzureFaceApiTopPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiTopPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public AzureFaceApiTopPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.CommandDetect = new DelegateCommand(() =>
            {
                this.NavigationService.NavigateAsync("AzureFaceApiDetectPage");
            });

            this.CommandRegistrationTop = new DelegateCommand(() =>
            {
                this.NavigationService.NavigateAsync("AzureFaceApiPersonGroupListPage");
            });

            this.CommandVerify = new DelegateCommand(() =>
            {
                this.NavigationService.NavigateAsync("AzureFaceApiIdentifyPage");
            });
        }

        /// <summary>
        /// Gets the command detect.
        /// </summary>
        /// <value>
        /// The command detect.
        /// </value>
        public DelegateCommand CommandDetect { get; }

        /// <summary>
        /// Gets the command registration top.
        /// </summary>
        /// <value>The command registration top.</value>
        public DelegateCommand CommandRegistrationTop { get; }

        /// <summary>
        /// Gets the command verify.
        /// </summary>
        /// <value>The command verify.</value>
        public DelegateCommand CommandVerify { get; }
    }
}
