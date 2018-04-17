﻿// -----------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1300
namespace Kobama_Xam_PrismApp.iOS
{
    using Foundation;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.iOS;
    using Kobama.Xam.Plugin.CameraPreview.iOS;
    using Kobama.Xam.Plugin.QRCode;
    using Kobama.Xam.Plugin.QRCode.Droid;
    using Kobama.Xam.PrismApp;
    using Prism;
    using Prism.Ioc;
    using UIKit;

    /// <summary>
    /// App delegate.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        /// <summary>
        /// Finisheds the launching.
        /// </summary>
        /// <returns><c>true</c>, if launching was finisheded, <c>false</c> otherwise.</returns>
        /// <param name="app">App.</param>
        /// <param name="options">Options.</param>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            CameraPreviewRenderer.Initalize();

            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// iOS Initializer.
        /// </summary>
        protected class iOSInitializer : IPlatformInitializer
        {
            /// <summary>
            /// Registers the types.
            /// </summary>
            /// <param name="container">Container.</param>
            public void RegisterTypes(IContainerRegistry container)
            {
                container.RegisterInstance(typeof(ICameraControl), Camera.Instance);
                container.Register(typeof(IQRCodeControl), typeof(QRCodeControlImpl));
            }
        }
    }
}