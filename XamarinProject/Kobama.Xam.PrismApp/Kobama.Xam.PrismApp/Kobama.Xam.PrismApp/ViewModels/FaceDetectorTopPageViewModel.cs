// <copyright file="FaceDetectorTopPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Navigation;

    /// <summary>
    /// Face Detector Top Page View Model
    /// </summary>
    /// <seealso cref="Kobama.Xam.PrismApp.ViewModels.ViewModelBase" />
    public class FaceDetectorTopPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceDetectorTopPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public FaceDetectorTopPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            this.Logger.CalledMethod();

            this.CommandStart = new DelegateCommand(async () =>
            {
                this.Logger.Debug("Navigate to FaceDetectorPage");
                await this.NavigationService.NavigateAsync("FaceDetectorPage");
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
