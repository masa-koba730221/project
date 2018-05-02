// <copyright file="CameraTestPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;

    /// <summary>
    /// Camera Test Page View Model
    /// </summary>
    /// <seealso cref="Kobama.Xam.PrismApp.ViewModels.ViewModelBase" />
    public class CameraTestPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraTestPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public CameraTestPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            this.CommandStart = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("CameraPage");
            });
        }

        /// <summary>
        /// Gets the command start.
        /// </summary>
        /// <value>
        /// The command start.
        /// </value>
        public DelegateCommand CommandStart { get; }
    }
}
