// -----------------------------------------------------------------------
// <copyright file="AzureFaceApiImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Kobama.Xam.Plugin.Log;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;

    /// <summary>
    /// Azure face API impl.
    /// </summary>
    public class AzureFaceApiImpl : IAzureFaceApiService
    {
        private FaceServiceClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFaceApiImpl"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public AzureFaceApiImpl(
            ISettingsService settings)
        {
            this.FaceApiRoot = settings.AzureFaceApiRoot;
            this.FaceApiKey = settings.AzureFaceApiKey;
            this.client = new FaceServiceClient(this.FaceApiKey, this.FaceApiRoot);

            this.Logger = new Logger(this.ToString());
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public Logger Logger { get; set; }

        /// <summary>
        /// Gets the face API root.
        /// </summary>
        /// <value>The face API root.</value>
        protected string FaceApiRoot { get; private set; }

        /// <summary>
        /// Gets the face API key.
        /// </summary>
        /// <value>The face API key.</value>
        protected string FaceApiKey { get; private set; }

        /// <summary>
        /// Detect the specified image, returnFaceId, returnFaceLandMark and returnFaceAttributes.
        /// </summary>
        /// <returns>The detect.</returns>
        /// <param name="image">Image.</param>
        /// <param name="returnFaceId">If set to <c>true</c> return face identifier.</param>
        /// <param name="returnFaceLandMark">If set to <c>true</c> return face land mark.</param>
        /// <param name="returnFaceAttributes">Return face attributes.</param>
        public async Task<Face[]> Detect(byte[] image, bool returnFaceId = true, bool returnFaceLandMark = true, IEnumerable<FaceAttributeType> returnFaceAttributes = null)
        {
            using (var strm = new MemoryStream(image))
            {
                this.Logger.CalledMethod($"Steam Size:{strm.Length}");
                return await this.client.DetectAsync(strm, returnFaceId, returnFaceLandMark, returnFaceAttributes);
            }
        }

        /// <summary>
        /// Identifies the face.
        /// </summary>
        /// <returns>The face.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="image">Image.</param>
        public async Task<IdentifyResult[]> IdentifyFace(string personGroupId, byte[] image)
        {
            this.Logger.CalledMethod();

            var resultDetect = await this.Detect(image);
            if (resultDetect.Length == 0)
            {
                throw new Exception($"The face can not be found in the image.");
            }

            var list = new List<Guid>();
            list.Add(resultDetect[0].FaceId);

            return await this.client.IdentifyAsync(personGroupId, list.ToArray());
        }

        /// <summary>
        /// Adds the face.
        /// </summary>
        /// <returns>The face.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        /// <param name="image">Image.</param>
        public async Task<AddPersistedFaceResult> AddFace(string personGroupId, Guid personId, byte[] image)
        {
            using (var strm = new MemoryStream(image))
            {
                this.Logger.CalledMethod($"Steam Size:{strm.Length}");
                return await this.client.AddPersonFaceAsync(personGroupId, personId, strm);
            }
        }

        /// <summary>
        /// Deletes the face.
        /// </summary>
        /// <returns>The face.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        /// <param name="faceId">Face identifier.</param>
        public async Task DeleteFace(string personGroupId, Guid personId, Guid faceId)
        {
            this.Logger.CalledMethod();

            var client = new FaceServiceClient(this.FaceApiKey, this.FaceApiRoot);
            await client.DeletePersonFaceAsync(personGroupId, personId, faceId);
        }

        /// <summary>
        /// Gets the face list.
        /// </summary>
        /// <returns>The face list.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        public async Task<Person> GetFaceList(string personGroupId, Guid personId)
        {
            this.Logger.CalledMethod();

            var client = new FaceServiceClient(this.FaceApiKey, this.FaceApiRoot);
            return await client.GetPersonAsync(personGroupId, personId);
        }

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <returns>The group.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="name">Name.</param>
        public async Task CreateGroup(string personGroupId, string name)
        {
            this.Logger.CalledMethod();
            await this.client.CreatePersonGroupAsync(personGroupId, name);
        }

        /// <summary>
        /// Gets the group list.
        /// </summary>
        /// <returns>The group list.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        public async Task<PersonGroup> GetGroupList(string personGroupId)
        {
            this.Logger.CalledMethod();
            return await this.client.GetPersonGroupAsync(personGroupId);
        }

        /// <summary>
        /// Training the specified persongGroupId.
        /// </summary>
        /// <returns>The training.</returns>
        /// <param name="persongGroupId">Persong group identifier.</param>
        public async Task StartTraining(string persongGroupId)
        {
            await this.client.TrainPersonGroupAsync(persongGroupId);
        }

        /// <summary>
        /// Gets the train status.
        /// </summary>
        /// <returns>The train status.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        public async Task<TrainingStatus> GetTrainStatus(string personGroupId)
        {
            this.Logger.CalledMethod();
            return await this.client.GetPersonGroupTrainingStatusAsync(personGroupId);
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <returns>The group.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        public async Task DeleteGroup(string personGroupId)
        {
            this.Logger.CalledMethod();
            await this.client.DeletePersonGroupAsync(personGroupId);
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="name">Name.</param>
        public async Task<CreatePersonResult> CreatePerson(string personGroupId, string name)
        {
            this.Logger.CalledMethod();
            return await this.client.CreatePersonAsync(personGroupId, name);
        }

        /// <summary>
        /// Gets the person list.
        /// </summary>
        /// <returns>The person list.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        public async Task<Person[]> GetPersonList(string personGroupId)
        {
            this.Logger.CalledMethod();
            return await this.client.ListPersonsAsync(personGroupId);
        }

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        public async Task DeletePerson(string personGroupId, Guid personId)
        {
            this.Logger.CalledMethod();
            await this.client.DeletePersonAsync(personGroupId, personId);
        }
    }
}
