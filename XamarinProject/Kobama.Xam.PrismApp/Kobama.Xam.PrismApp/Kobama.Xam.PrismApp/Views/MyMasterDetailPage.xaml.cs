// -----------------------------------------------------------------------
// <copyright file="MyMasterDetailPage.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Views
{
    using Prism.Navigation;
    using Xamarin.Forms;

    /// <summary>
    /// My master detail page.
    /// </summary>
    public partial class MyMasterDetailPage : MasterDetailPage, IMasterDetailPageOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyMasterDetailPage"/> class.
        /// </summary>
        public MyMasterDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Kobama.Xam.PrismApp.Views.MyMasterDetailPage"/> is
        /// presented after navigation.
        /// </summary>
        /// <value><c>true</c> if is presented after navigation; otherwise, <c>false</c>.</value>
        public bool IsPresentedAfterNavigation
        {
            get { return Device.Idiom != TargetIdiom.Phone; }
        }
    }
}
