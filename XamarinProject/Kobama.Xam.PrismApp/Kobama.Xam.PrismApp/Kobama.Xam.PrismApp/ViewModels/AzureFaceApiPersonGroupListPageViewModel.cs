// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiPersonGroupListPageViewModel.cs" company="Kobama">
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
    /// Person Group List Page
    /// </summary>
    public class AzureFaceApiPersonGroupListPageViewModel : AzureFaceApiViewModelBase
    {
        private ObservableCollection<PersonGroupItem> personList = new ObservableCollection<PersonGroupItem>();
        private PersonGroupItem selectedItem;
        private IEntryDialogService entryDialogService;
        private DelegateCommand deleteCommand;
        private string trainStatus = string.Empty;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiPersonGroupListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="entryDialog">Entry dialog.</param>
        /// <param name="faceApiService">Face Api Service</param>
        public AzureFaceApiPersonGroupListPageViewModel(
            INavigationService navigationService,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IEntryDialogService entryDialog,
            IAzureFaceApiService faceApiService)
            : base(navigationService, device, dialog, settings, faceApiService)
        {
            this.Logger.CalledMethod();

            this.PersonGroupList = new ObservableCollection<PersonGroupItem>();
            this.entryDialogService = entryDialog;
            this.GetGroupList();

            this.CommandAddNePersonGroup = new DelegateCommand(async () =>
            {
                var result = await this.entryDialogService.Show("New Create Group", "Enter Group Name", "New", "Cancel");
                if (result.PressedButtonTitle == "New")
                {
                    this.CreateGroup(result.Text);
                }
            });

            this.deleteCommand = new DelegateCommand(async () =>
            {
                var result = await this.Dialog.DisplayAlertAsync("Delete Group", "Are you sure you want to person group?", "Yes", "No");
                if (result)
                {
                    this.DeleteGroup();
                }
            });
        }

        /// <summary>
        /// Gets the command add ne person group.
        /// </summary>
        /// <value>The command add ne person group.</value>
        public DelegateCommand CommandAddNePersonGroup { get; }

        /// <summary>
        /// Gets or sets the train status.
        /// </summary>
        /// <value>The train status.</value>
        public string TrainStatus
        {
            get
            {
                return this.trainStatus;
            }

            set
            {
                this.SetProperty(ref this.trainStatus, value);
            }
        }

        /// <summary>
        /// Gets or sets Personal Group List
        /// </summary>
        public ObservableCollection<PersonGroupItem> PersonGroupList { get; set; }

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:Kobama.Xam.PrismApp.ViewModels.AzureFaceApiPersonGroupListPageViewModel"/> is addable.
        /// </summary>
        /// <value><c>true</c> if is addable; otherwise, <c>false</c>.</value>
        public bool IsAddable
        {
            get
            {
                return this.PersonGroupList.Count == 0;
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public PersonGroupItem SelectedItem
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
                    AzureFaceApiPersonListPageViewModel.PersonGroupName = value.PersonGroupName;
                    this.NavigationService.NavigateAsync("AzureFaceApiPersonListPage");
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

        private void CreateGroup(string name)
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await this.FaceApi.CreateGroup(this.PersonGroupId, name);
                    this.GetGroupList();
                }
                catch (Exception ex)
                {
                    await this.Dialog.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            });
        }

        private void GetGroupList()
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var list = await this.FaceApi.GetGroupList(this.PersonGroupId);
                    this.PersonGroupList.Clear();
                    this.Logger.CalledMethod($"PersonGroup Name{list.Name} Id:{list.PersonGroupId}");
                    this.PersonGroupList.Add(new PersonGroupItem { PersonGroupName = list.Name, CommandDeletePersonGroup = this.deleteCommand });
                    this.RaisePropertyChanged(nameof(this.IsAddable));

                    this.GetTrainStatus();
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }

        private void GetTrainStatus()
        {
            this.Logger.CalledMethod();

            if (this.PersonGroupList.Count == 0)
            {
                this.Logger.CalledMethod("Person Group not found");
            }

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var result = await this.FaceApi.GetTrainStatus(this.PersonGroupId);
                    if (result != null)
                    {
                        var str = $"{result.Status} Create:{result.CreatedDateTime.ToString()}";
                        this.Logger.CalledMethod("Train Status: " + str);
                        this.TrainStatus = str;
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }

        private void DeleteGroup()
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await this.FaceApi.DeleteGroup(this.PersonGroupId);
                    this.GetGroupList();
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }

        /// <summary>
        /// Person group list.
        /// </summary>
        public class PersonGroupItem
        {
            /// <summary>
            /// Gets or sets the name of the person group.
            /// </summary>
            /// <value>The name of the person group.</value>
            public string PersonGroupName { get; set; }

            /// <summary>
            /// Gets or sets the command delete person group.
            /// </summary>
            /// <value>The command delete person group.</value>
            public DelegateCommand CommandDeletePersonGroup { get; set; }
        }
    }
}
