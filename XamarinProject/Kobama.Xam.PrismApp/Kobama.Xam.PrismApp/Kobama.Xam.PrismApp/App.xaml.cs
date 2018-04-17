// -----------------------------------------------------------------------
//  <copyright file="App.xaml.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Kobama.Xam.PrismApp.ViewModels;
using Kobama.Xam.PrismApp.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Kobama.Xam.PrismApp
{
    /// <summary>
    /// App.
    /// </summary>
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.PrimsApp.App"/> class.
        /// </summary>
        public App()
            : this(null) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.PrimsApp.App"/> class.
        /// </summary>
        /// <param name="initializer">Initializer.</param>
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        /// <summary>
        /// Ons the initialized.
        /// </summary>
        protected override async void OnInitialized()
        {
            this.InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="containerRegistry">Container registry.</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
        }
    }
}
