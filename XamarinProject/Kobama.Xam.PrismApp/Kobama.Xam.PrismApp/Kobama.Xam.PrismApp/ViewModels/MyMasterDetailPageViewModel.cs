// -----------------------------------------------------------------------
// <copyright file="MyMasterDetailPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.ViewModels
{
    using System.Collections.ObjectModel;
    using Prism.Navigation;

    /// <summary>
    /// My master detail page view model.
    /// </summary>
    public class MyMasterDetailPageViewModel : ViewModelBase
    {
        private MenuItem selectedItem = null;
        private bool isPresented;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyMasterDetailPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public MyMasterDetailPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            this.Logger.CalledMethod();
            this.Title = "MasterDetailPage";
            this.IsPresented = false;

            this.Menus = new ObservableCollection<MenuItem>
            {
                new MenuItem { Title = "MainPage", Icon = string.Empty, TargetPage = "MainPage" },
                new MenuItem { Title = "QR Code",  Icon = "qr14.png", TargetPage = "QRCodeTestPage" }
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Kobama.Xam.PrismApp.ViewModels.MyMasterDetailPageViewModel"/> is presented.
        /// </summary>
        /// <value><c>true</c> if is presented; otherwise, <c>false</c>.</value>
        public bool IsPresented
        {
            get { return this.isPresented; }
            set { this.SetProperty(ref this.isPresented, value); }
        }

        /// <summary>
        /// Gets the menus.
        /// </summary>
        /// <value>The menus.</value>
        public ObservableCollection<MenuItem> Menus { get; }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public MenuItem SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.SetProperty(ref this.selectedItem, value);

                if (value == null)
                {
                    return;
                }

                this.NavigationService.NavigateAsync($"NavigationPage/{value.TargetPage}");
                this.IsPresented = false;
            }
        }

        /// <summary>
        /// Menu item.
        /// </summary>
        public class MenuItem
        {
            /// <summary>
            /// Gets or sets the title.
            /// </summary>
            /// <value>The title.</value>
            public string Title { get; set; }

            /// <summary>
            /// Gets or sets the icon.
            /// </summary>
            /// <value>The icon.</value>
            public string Icon { get; set; }

            /// <summary>
            /// Gets or sets the target page.
            /// </summary>
            /// <value>The target page.</value>
            public string TargetPage { get; set; }
        }
    }
}
