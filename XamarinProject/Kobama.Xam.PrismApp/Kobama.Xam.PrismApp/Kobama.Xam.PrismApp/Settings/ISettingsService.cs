// -----------------------------------------------------------------------
// <copyright file="ISettingsService.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Settings
{
    /// <summary>
    /// Settings service.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the Azure Face API root.
        /// </summary>
        /// <value>The face API root.</value>
        string AzureFaceApiRoot { get; set; }

        /// <summary>
        /// Gets or sets the Azure Face API key.
        /// </summary>
        /// <value>The azure face API key.</value>
        string AzureFaceApiKey { get; set; }
    }
}
