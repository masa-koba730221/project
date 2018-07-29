// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Kobama.Xam.PrismApp.Settings;
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
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
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

            await this.NavigationService.NavigateAsync("MyMasterDetailPage/NavigationPage/MainPage");
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="containerRegistry">Container registry.</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(ISettingsService), typeof(SettingsImpl));
            containerRegistry.Register(typeof(IAzureFaceApiService), typeof(AzureFaceApiImpl));
            containerRegistry.RegisterForNavigation<MyMasterDetailPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<QRCodeTestPage>();
            containerRegistry.RegisterForNavigation<QRCodeReaderPage>();
            containerRegistry.RegisterForNavigation<CameraTestPage>();
            containerRegistry.RegisterForNavigation<CameraPage>();
            containerRegistry.RegisterForNavigation<AzureTestPage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiTopPage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiDetectPage>();
            containerRegistry.RegisterForNavigation<FaceDetectorPage>();
            containerRegistry.RegisterForNavigation<FaceDetectorTopPage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiAddFacePage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiPersonGroupListPage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiFaceListPage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiPersonListPage>();
            containerRegistry.RegisterForNavigation<AzureFaceApiIdentifyPage>();
        }
    }
}
