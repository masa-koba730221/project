// -----------------------------------------------------------------------
// <copyright file="IAzureFaceApiService.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.PrismApp.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;

    /// <summary>
    /// Azure face API service.
    /// </summary>
    public interface IAzureFaceApiService
    {
        /// <summary>
        /// Identifies the face.
        /// </summary>
        /// <returns>The face.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="image">Image.</param>
        Task<IdentifyResult[]> IdentifyFace(string personGroupId, byte[] image);

        /// <summary>
        /// Deletes the face.
        /// </summary>
        /// <returns>The face.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        /// <param name="faceId">Face identifier.</param>
        Task DeleteFace(string personGroupId, Guid personId, Guid faceId);

        /// <summary>
        /// Gets the face list.
        /// </summary>
        /// <returns>The face list.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        Task<Person> GetFaceList(string personGroupId, Guid personId);

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <returns>The group.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="name">Name.</param>
        Task CreateGroup(string personGroupId, string name);

        /// <summary>
        /// Gets the group list.
        /// </summary>
        /// <returns>The group list.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        Task<PersonGroup> GetGroupList(string personGroupId);

        /// <summary>
        /// Starts the training.
        /// </summary>
        /// <returns>The training.</returns>
        /// <param name="persongGroupId">Persong group identifier.</param>
        Task StartTraining(string persongGroupId);

        /// <summary>
        /// Gets the train status.
        /// </summary>
        /// <returns>The train status.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        Task<TrainingStatus> GetTrainStatus(string personGroupId);

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <returns>The group.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        Task DeleteGroup(string personGroupId);

        /// <summary>
        /// Detect the specified image, returnFaceId, returnFaceLandMark and returnFaceAttributes.
        /// </summary>
        /// <returns>The detect.</returns>
        /// <param name="image">Image.</param>
        /// <param name="returnFaceId">If set to <c>true</c> return face identifier.</param>
        /// <param name="returnFaceLandMark">If set to <c>true</c> return face land mark.</param>
        /// <param name="returnFaceAttributes">Return face attributes.</param>
        Task<Face[]> Detect(byte[] image, bool returnFaceId = true, bool returnFaceLandMark = true, IEnumerable<FaceAttributeType> returnFaceAttributes = null);

        /// <summary>
        /// Adds the face.
        /// </summary>
        /// <returns>The face.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        /// <param name="image">Image.</param>
        Task<AddPersistedFaceResult> AddFace(string personGroupId, Guid personId, byte[] image);

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="name">Name.</param>
        Task<CreatePersonResult> CreatePerson(string personGroupId, string name);

        /// <summary>
        /// Gets the person list.
        /// </summary>
        /// <returns>The person list.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        Task<Person[]> GetPersonList(string personGroupId);

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="personGroupId">Person group identifier.</param>
        /// <param name="personId">Person identifier.</param>
        Task DeletePerson(string personGroupId, Guid personId);
    }
}
