// -----------------------------------------------------------------------
// <copyright file="MainApplication.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Droid
{
    using System;
    using Android.App;
    using Android.OS;
    using Android.Runtime;
    using global::Plugin.CurrentActivity;

    /// <summary>
    /// Main application.
    /// </summary>
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public partial class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainApplication"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        /// <param name="transer">Transer.</param>
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        /// <summary>
        /// Ons the create.
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            this.RegisterActivityLifecycleCallbacks(this);
        }

        /// <summary>
        /// Ons the terminate.
        /// </summary>
        public override void OnTerminate()
        {
            base.OnTerminate();
            this.UnregisterActivityLifecycleCallbacks(this);
        }

        /// <summary>
        /// Ons the activity created.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        /// <summary>
        /// Ons the activity destroyed.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public void OnActivityDestroyed(Activity activity)
        {
        }

        /// <summary>
        /// Ons the activity paused.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public void OnActivityPaused(Activity activity)
        {
        }

        /// <summary>
        /// Ons the activity resumed.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        /// <summary>
        /// Ons the state of the activity save instance.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="outState">Out state.</param>
        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        /// <summary>
        /// Ons the activity started.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        /// <summary>
        /// Ons the activity stopped.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public void OnActivityStopped(Activity activity)
        {
        }
    }
}
