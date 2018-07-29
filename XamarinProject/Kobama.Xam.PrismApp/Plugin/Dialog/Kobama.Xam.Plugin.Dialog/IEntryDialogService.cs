// -----------------------------------------------------------------------
// <copyright file="IEntryDialogService.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kobama.Xam.Plugin.Dialog
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Entry dialog service.
    /// </summary>
    public interface IEntryDialogService
    {
        /// <summary>
        /// Show the specified title, message, accepte, cancel and isPassword.
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="accepte">Accepte.</param>
        /// <param name="cancel">Cancel.</param>
        /// <param name="isPassword">If set to <c>true</c> is password.</param>
        Task<EntryResult> Show(string title, string message, string accepte, string cancel, bool isPassword = false);
    }

    /// <summary>
    /// Entry result.
    /// </summary>
    public class EntryResult
    {
        /// <summary>
        /// Gets or sets the pressed button title.
        /// </summary>
        /// <value>The pressed button title.</value>
        public string PressedButtonTitle { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
    }
}
