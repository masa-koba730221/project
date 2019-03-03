// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kobama.Xam.Plugin.Log;
    using Prism.AppModel;
    using Prism.Mvvm;
    using Prism.Navigation;

    /// <summary>
    /// View model base.
    /// </summary>
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IApplicationLifecycleAware
    {
        private const int PersonCount = 10000;
        private const int CallLimitPerSecond = 10;
        private static Queue<DateTime> timeStampQueue = new Queue<DateTime>(CallLimitPerSecond);

        private string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public ViewModelBase(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.Logger = new Logger(this.ToString());
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title</value>
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public Logger Logger { get; set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        protected INavigationService NavigationService { get; private set; }

        /// <summary>
        /// Wait Call Limit Per Seccond
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task WaitCallLimitPerSecondAsync()
        {
            Monitor.Enter(timeStampQueue);
            try
            {
                if (timeStampQueue.Count >= CallLimitPerSecond)
                {
                    TimeSpan timeInterval = DateTime.UtcNow - timeStampQueue.Peek();
                    if (timeInterval < TimeSpan.FromSeconds(1))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1) - timeInterval);
                    }

                    timeStampQueue.Dequeue();
                }

                timeStampQueue.Enqueue(DateTime.UtcNow);
            }
            finally
            {
                Monitor.Exit(timeStampQueue);
            }
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public virtual void Destroy()
        {
            this.Logger.CalledMethod();
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        public virtual void OnResume()
        {
            this.Logger.CalledMethod();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public virtual void OnSleep()
        {
            this.Logger.CalledMethod();
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public virtual void OnAppearing()
        {
            this.Logger.CalledMethod();
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public virtual void OnDisappearing()
        {
            this.Logger.CalledMethod();
        }

        /// <summary>
        /// Ons the navigated from.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// Ons the navigated to.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// Ons the navigating to.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }
    }
}
