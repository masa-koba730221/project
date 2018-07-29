// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiFaceListPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Kobama.Xam.PrismApp.Settings;
    using Microsoft.ProjectOxford.Face;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure face API registration person page view model.
    /// </summary>
    public class AzureFaceApiFaceListPageViewModel : AzureFaceApiViewModelBase
    {
        private DelegateCommand<FaceItem> deleteCommand;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiFaceListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="faceApiService">Face Api Service</param>
        public AzureFaceApiFaceListPageViewModel(
            INavigationService navigationService,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IAzureFaceApiService faceApiService)
            : base(navigationService, device, dialog, settings, faceApiService)
        {
            this.Title = PersonName;
            this.FaceList = new ObservableCollection<FaceItem>();

            this.CommandAddNewFace = new DelegateCommand(async () =>
            {
                AzureFaceApiAddFacePageViewModel.PersonGroupName = PersonGroupName;
                AzureFaceApiAddFacePageViewModel.PersonId = PersonId.ToString();
                AzureFaceApiAddFacePageViewModel.PersonName = PersonName;
                await this.NavigationService.NavigateAsync("AzureFaceApiAddFacePage");
            });

            this.deleteCommand = new DelegateCommand<FaceItem>(async item =>
            {
                var result = await dialog.DisplayAlertAsync("Delete Face", "Are you sure you want this face?", "Ok", "Cancel");
                if (result)
                {
                    this.DeleteFace(item.PersistedFaceIds);
                }
            });
        }

        /// <summary>
        /// Gets or sets the person group name.
        /// </summary>
        /// <value>The person group name</value>
        public static string PersonGroupName { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        /// <value>The name of the person.</value>
        public static string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>The person identifier.</value>
        public static string PersonId { get; set; }

        /// <summary>
        /// Gets the command add new face.
        /// </summary>
        /// <value>The command add new face.</value>
        public DelegateCommand CommandAddNewFace { get; }

        /// <summary>
        /// Gets or sets Personal Group List
        /// </summary>
        public ObservableCollection<FaceItem> FaceList { get; set; }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public override void OnAppearing()
        {
            this.Logger.CalledMethod();

            base.OnAppearing();

            this.GetFaceList();
        }

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="faceId">Person identifier.</param>
        private void DeleteFace(Guid faceId)
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await this.FaceApi.DeleteFace(this.PersonGroupId, System.Guid.Parse(PersonId), faceId);
                    this.GetFaceList();
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            });
        }

        private void GetFaceList()
        {
            this.Logger.CalledMethod();

            this.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var person = await this.FaceApi.GetFaceList(this.PersonGroupId, System.Guid.Parse(PersonId));

                    this.FaceList.Clear();
                    foreach (var id in person.PersistedFaceIds)
                    {
                        this.FaceList.Add(new FaceItem()
                        {
                            PersistedFaceIds = id,
                            PersistedFaceIdText = id.ToString(),
                            CommandDeleteFace = this.deleteCommand
                        });

                        this.Logger.CalledMethod($"Face ID:{id}");
                    }
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
        public class FaceItem
        {
            /// <summary>
            /// Gets or sets the persisted face identifier text.
            /// </summary>
            /// <value>The persisted face identifier text.</value>
            public string PersistedFaceIdText { get; set; }

            /// <summary>
            /// Gets or sets the persisted face identifiers.
            /// </summary>
            /// <value>The persisted face identifiers.</value>
            public System.Guid PersistedFaceIds { get; set; }

            /// <summary>
            /// Gets or sets the command delete face.
            /// </summary>
            /// <value>The command delete face.</value>
            public DelegateCommand<FaceItem> CommandDeleteFace { get; set; }
        }
    }
}
