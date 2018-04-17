// -----------------------------------------------------------------------
//  <copyright file="MainActivity.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Droid;
    using Kobama.Xam.Plugin.QRCode;
    using Kobama.Xam.Plugin.QRCode.Droid;
    using Kobama.Xam.PrismApp;
    using Prism;
    using Prism.Ioc;

    /// <summary>
    /// Main activity.
    /// </summary>
    [Activity(Label = "Kobama_Xam_PrimsApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="bundle">Bundle </param>
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            this.LoadApplication(new App(new AndroidInitializer()));
        }

        /// <summary>
        /// Android initializer.
        /// </summary>
        protected class AndroidInitializer : IPlatformInitializer
        {
            /// <summary>
            /// Registers the types.
            /// </summary>
            /// <param name="container">Container.</param>
            public void RegisterTypes(IContainerRegistry container)
            {
                container.RegisterInstance<ICameraControl>(Camera2.Instance);
                container.Register(typeof(IQRCodeControl), typeof(QRCodeControlImpl));
            }
        }
    }
}