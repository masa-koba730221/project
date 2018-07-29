// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiPersonListPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Kobama.Xam.Plugin.Dialog;
    using Kobama.Xam.PrismApp.Settings;
    using Microsoft.ProjectOxford.Face;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure face API person list page view model.
    /// </summary>
    public class AzureFaceApiPersonListPageViewModel : AzureFaceApiViewModelBase
    {
        private PersonItem selectedItem;
        private DelegateCommand<PersonItem> deleteCommand;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiPersonListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="entryDialog">Entry Dialog</param>
        /// <param name="faceApiService">Face Api</param>
        public AzureFaceApiPersonListPageViewModel(
            INavigationService navigationService,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IEntryDialogService entryDialog,
            IAzureFaceApiService faceApiService)
            : base(navigationService, device, dialog, settings, faceApiService)
        {
            this.PersonList = new ObservableCollection<PersonItem>();

            this.deleteCommand = new DelegateCommand<PersonItem>(async p =>
            {
                var result = await dialog.DisplayAlertAsync("Delete Person", "Are you sure you want to delete this person?", "OK", "Cancel");
                if (result)
                {
                    this.DeletePerson(System.Guid.Parse(p.Id));
                }
            });

            this.CommandAddNewPerson = new DelegateCommand(async () =>
            {
                var result = await entryDialog.Show("Create New Person", "Enter Person Name", "New", "Cancel");
                if (result.PressedButtonTitle == "New")
                {
                    this.CreatePerson(result.Text);
                }
            });

            this.GetPersonList();
        }

        /// <summary>
        /// Gets or sets the name of the person group.
        /// </summary>
        /// <value>The name of the person group.</value>
        public static string PersonGroupName { get; set; }

        /// <summary>
        /// Gets the command add new person.
        /// </summary>
        /// <value>The command add new person.</value>
        public DelegateCommand CommandAddNewPerson { get; }

        /// <summary>
        /// Gets or sets Personal Group List
        /// </summary>
        public ObservableCollection<PersonItem> PersonList { get; set; }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public PersonItem SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.SetProperty(ref this.selectedItem, value);
                if (value != null)
                {
                    AzureFaceApiFaceListPageViewModel.PersonGroupName = PersonGroupName;
                    AzureFaceApiFaceListPageViewModel.PersonName = value.PersonName;
                    AzureFaceApiFaceListPageViewModel.PersonId = value.Id.ToString();
                    this.NavigationService.NavigateAsync("AzureFaceApiFaceListPage");
                }
            }
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public override void OnAppearing()
        {
            this.Logger.CalledMethod();
            base.OnAppearing();
        }

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public override void OnDisappearing()
        {
            this.Logger.CalledMethod();
            base.OnDisappearing();

            this.SelectedItem = null;
        }

        private void CreatePerson(string name)
        {
            this.Logger.CalledMethod();

            var id = name.ToLower().Replace(" ", string.Empty);

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var result = await this.FaceApi.CreatePerson(this.PersonGroupId, name);
                    if (result != null)
                    {
                        this.Logger.CalledMethod($"PersonId: {result.PersonId}");
                    }
                    this.GetPersonList();
                }
                catch (Exception ex)
                {
                    await this.Dialog.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            });
        }

        private void GetPersonList()
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var list = await this.FaceApi.GetPersonList(this.PersonGroupId);
                    this.PersonList.Clear();
                    foreach (var person in list)
                    {
                        this.Logger.CalledMethod($"Person List: Name:{person.Name} Id:{person.PersonId}");
                        foreach (var faceid in person.PersistedFaceIds)
                        {
                            this.Logger.CalledMethod($"   FaceId:{faceid.ToString()}");
                        }
                        this.PersonList.Add(new PersonItem { PersonName = person.Name, Id = person.PersonId.ToString(), CommandDeletePerson = this.deleteCommand });
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }

        private void DeletePerson(System.Guid personId)
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await this.FaceApi.DeletePerson(this.PersonGroupId, personId);
                    this.GetPersonList();
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }

        /// <summary>
        /// Person Item
        /// </summary>
        public class PersonItem
        {
            /// <summary>
            /// Gets or sets the name of the person group.
            /// </summary>
            /// <value>The name of the person group.</value>
            public string PersonName { get; set; }

            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the command delete person group.
            /// </summary>
            /// <value>The command delete person group.</value>
            public DelegateCommand<PersonItem> CommandDeletePerson { get; set; }
        }
    }
}
