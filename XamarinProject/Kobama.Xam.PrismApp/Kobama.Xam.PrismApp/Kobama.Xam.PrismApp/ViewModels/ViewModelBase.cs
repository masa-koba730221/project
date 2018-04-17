// -----------------------------------------------------------------------
//  <copyright file="ViewModelBase.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using Prism.AppModel;
    using Prism.Mvvm;
    using Prism.Navigation;

    /// <summary>
    /// View model base.
    /// </summary>
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IApplicationLifecycleAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kobama.Xam.PrimsApp.ViewModels.ViewModelBase"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public ViewModelBase(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        /// <summary>
        /// The title.
        /// </summary>
        private string title;

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
    }
}
