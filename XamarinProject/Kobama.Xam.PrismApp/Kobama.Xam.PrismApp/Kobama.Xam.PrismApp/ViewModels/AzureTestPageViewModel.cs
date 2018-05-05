// <copyright file="AzureTestPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;

    /// <summary>
    /// Azure Test Page View Model
    /// </summary>
    /// <seealso cref="Kobama.Xam.PrismApp.ViewModels.ViewModelBase" />
    public class AzureTestPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTestPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public AzureTestPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.CommandFaceApi = new DelegateCommand(() =>
            {
                this.NavigationService.NavigateAsync("AzureFaceApiTopPage");
            });
        }

        /// <summary>
        /// Gets the command face API.
        /// </summary>
        /// <value>
        /// The command face API.
        /// </value>
        public DelegateCommand CommandFaceApi { get; }
    }
}
