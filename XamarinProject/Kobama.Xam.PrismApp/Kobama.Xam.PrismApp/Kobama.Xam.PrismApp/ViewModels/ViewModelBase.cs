// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Kobama.Xam.Plugin.Log;
    using Prism.AppModel;
    using Prism.Mvvm;
    using Prism.Navigation;

    /// <summary>
    /// View model base.
    /// </summary>
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IApplicationLifecycleAware
    {
        /// <summary>
        /// The title.
        /// </summary>
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
        /// Ons the navigated from.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        /// <summary>
        /// Ons the navigated to.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        /// <summary>
        /// Ons the navigating to.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public virtual void Destroy()
        {
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        public virtual void OnResume()
        {
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        public virtual void OnSleep()
        {
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public virtual void OnAppearing()
        {
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public virtual void OnDisappearing()
        {
        }
    }
}
