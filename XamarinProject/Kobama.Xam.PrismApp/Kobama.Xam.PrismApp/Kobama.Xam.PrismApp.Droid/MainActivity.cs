// -----------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Droid;
    using Kobama.Xam.Plugin.CustomImage.Droid;
    using Kobama.Xam.Plugin.Face;
    using Kobama.Xam.Plugin.Face.Droid;
    using Kobama.Xam.Plugin.QRCode;
    using Kobama.Xam.Plugin.QRCode.Droid;
    using Kobama.Xam.PrismApp;
    using Prism;
    using Prism.Ioc;

    /// <summary>
    /// Main activity.
    /// </summary>
    [Activity(Label = "Kobama_Xam_PrimsApp", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="bundle">Bundle </param>
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Kobama.Xam.PrimsApp.Droid.Resource.Layout.Tabbar;
            ToolbarResource = Kobama.Xam.PrimsApp.Droid.Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var width = this.Resources.DisplayMetrics.WidthPixels;
            var height = this.Resources.DisplayMetrics.HeightPixels;
            var density = this.Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;

            var ir = new CustomImageRenderer(this);

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
                container.Register(typeof(Plugin.Gallary.IGallaryService), typeof(Plugin.Gallary.Doid.GallaryImpl));
                container.Register(typeof(IFaceDetectorService), typeof(FaceDetectorImpl));
                container.Register(typeof(Plugin.Dialog.IEntryDialogService), typeof(Plugin.Dialog.Droid.EntryDialogImpl));
                container.Register(typeof(Plugin.Draw.IDrawService), typeof(Plugin.Draw.Droid.DrawImplement));
            }
        }
    }
}