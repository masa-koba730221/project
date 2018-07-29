// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiIdentifyPageViewModel.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using Kobama.Xam.Plugin.Camera;
    using Kobama.Xam.Plugin.Camera.Options;
    using Kobama.Xam.Plugin.Face;
    using Kobama.Xam.Plugin.Gallary;
    using Kobama.Xam.PrismApp.Settings;
    using Microsoft.ProjectOxford.Face;
    using Prism.Navigation;
    using Prism.Services;

    /// <summary>
    /// Azure face API verify page view model.
    /// </summary>
    public class AzureFaceApiIdentifyPageViewModel : AzureFaceApiCameraViewModelBase
    {
        private IFaceDetectorService faceDetector;
        private bool isDetecting = false;
        private byte[] imageBuffer;
        private List<PersonItem> personList = new List<PersonItem>();

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureFaceApiIdentifyPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="gallary">Gallary.</param>
        /// <param name="device">Device.</param>
        /// <param name="dialog">Dialog.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="faceDetector">Face detector.</param>
        /// <param name="faceApiService">Face Api Service</param>
        public AzureFaceApiIdentifyPageViewModel(
            INavigationService navigationService,
            ICameraControl camera,
            IGallaryService gallary,
            IDeviceService device,
            IPageDialogService dialog,
            ISettingsService settings,
            IFaceDetectorService faceDetector,
            IAzureFaceApiService faceApiService)
            : base(navigationService, camera, gallary, device, dialog, settings, faceApiService)
        {
            this.isDetecting = false;
            this.faceDetector = faceDetector;
            this.faceDetector.ResutlFaceDetectorCallback += this.EventHandlerFaceDetector;
            this.GetPersonList();
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public override void Destroy()
        {
            this.Logger.CalledMethod();
            base.Destroy();
        }

        /// <summary>
        /// Event Handler Saved Image
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">Size</param>
        protected override void EventHandlerSavedImage(byte[] image, Size size)
        {
            this.Logger.CalledMethod();
            if (!this.isDetecting)
            {
                this.isDetecting = true;
                this.imageBuffer = new byte[image.Length];
                image.CopyTo(this.imageBuffer, 0);
                this.faceDetector.Detector(this.imageBuffer);
            }
        }

        private void EventHandlerFaceDetector(Plugin.Face.ResultFaceDtector result)
        {
            this.Logger.CalledMethod();
            if (result.BoundingBoxs.Length == 0)
            {
                this.Logger.CalledMethod("Face not found");
                this.DeviceService.BeginInvokeOnMainThread(async () =>
                {
                    await this.Dialog.DisplayAlertAsync("Error", $"The face can not be found in the image.", "OK");
                });

                this.isDetecting = false;
                return;
            }

            this.VerifyFace(result.Image);
        }

        private void VerifyFace(byte[] image)
        {
            this.Logger.CalledMethod();
            this.DeviceService.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var resultVerify = await this.FaceApi.IdentifyFace(this.PersonGroupId, image);

                    if (resultVerify.Length == 0 && resultVerify[0].Candidates.Length == 0)
                    {
                        await this.Dialog.DisplayAlertAsync("Error", $"The face identify is failed", "OK");
                    }
                    {
                        foreach (var person in this.personList)
                        {
                            if (resultVerify[0].Candidates[0].PersonId.CompareTo(Guid.Parse(person.Id)) == 0)
                            {
                                await this.Dialog.DisplayAlertAsync("Found", $"The face is {person.PersonName}", "OK");
                                break;
                            }
                        }
                    }

                    await this.NavigationService.GoBackAsync();
                }
                catch (Exception ex)
                {
                    await this.Dialog.DisplayAlertAsync("Error", ex.Message, "OK");
                }
                finally
                {
                    this.isDetecting = false;
                }
            });
        }

        private void GetPersonList()
        {
            this.Logger.CalledMethod();

            this.DeviceService.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var list = await this.FaceApi.GetPersonList(this.PersonGroupId);
                    this.personList.Clear();
                    foreach (var person in list)
                    {
                        this.Logger.CalledMethod($"Person List: Name:{person.Name} Id:{person.PersonId}");
                        foreach (var faceid in person.PersistedFaceIds)
                        {
                            this.Logger.CalledMethod($"   FaceId:{faceid.ToString()}");
                        }
                        this.personList.Add(new PersonItem { PersonName = person.Name, Id = person.PersonId.ToString() });
                    }
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
        }
    }
}
