// -----------------------------------------------------------------------
// <copyright file="QRCodeTestPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;

    /// <summary>
    /// QRC ode test page view model.
    /// </summary>
    public class QRCodeTestPageViewModel : ViewModelBase
    {
        private string labelResultScan;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeTestPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public QRCodeTestPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            this.CommandStartScan = new DelegateCommand(async () =>
            {
                await this.NavigationService.NavigateAsync("QRCodeReaderPage");
            });
        }

        /// <summary>
        /// Gets or sets the label result scan.
        /// </summary>
        /// <value>The label result scan.</value>
        public string LabelResultScan
        {
            get { return this.labelResultScan; }
            set { this.SetProperty(ref this.labelResultScan, value); }
        }

        /// <summary>
        /// Gets the command start scan.
        /// </summary>
        /// <value>The command start scan.</value>
        public DelegateCommand CommandStartScan { get; }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public override void OnAppearing()
        {
            base.OnAppearing();
            this.LabelResultScan = QRCodeReaderPageViewModel.Result;
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
