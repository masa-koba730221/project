// <copyright file="AzureFaceApiRegistrationTopPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Kobama.Xam.Plugin.Dialog;
    using Kobama.Xam.PrismApp.Settings;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure Face Api Registration Top Page View Model
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase" />
    public class AzureFaceApiRegistrationTopPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService pageDialog;
        private readonly ISettingsService settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiRegistrationTopPageViewModel"/> class.
        /// </summary>
        /// <param name="pageDialogService">Page Dialog Service</param>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="entryDialogService">Entry Dialog Service</param>
        /// <param name="settingsService">Settings Service</param>
        public AzureFaceApiRegistrationTopPageViewModel(
            IPageDialogService pageDialogService,
            INavigationService navigationService,
            IEntryDialogService entryDialogService,
            ISettingsService settingsService)
            : base(navigationService)
        {
            this.pageDialog = pageDialogService;
            this.settings = settingsService;

            this.CommandPersonGroupList = new DelegateCommand(async () =>
            {
                await this.NavigationService.NavigateAsync("AzureFaceApiRegistrationPersonGroupListPage");
            });
        }

        /// <summary>
        /// Gets the command to Person Group List Page
        /// </summary>
        /// <value>
        /// The command add new group.
        /// </value>
        public DelegateCommand CommandPersonGroupList { get; }
    }
}
