// -----------------------------------------------------------------------
// <copyright file="EntryDialogImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Dialog.Droid
{
    using System;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Widget;

    /// <summary>
    /// Entry dialog impl.
    /// </summary>
    public class EntryDialogImpl : IEntryDialogService
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
        public Task<EntryResult> Show(string title, string message, string accepte, string cancel, bool isPassword = false)
        {
            var tcs = new TaskCompletionSource<EntryResult>();

            var editText = new EditText(Android.App.Application.Context);
            if (isPassword)
            {
                editText.InputType = global::Android.Text.InputTypes.TextVariationPassword
                | global::Android.Text.InputTypes.ClassText;
            }

            new AlertDialog.Builder(Android.App.Application.Context)
                .SetTitle(title)
                .SetMessage(message)
                .SetView(editText)
                .SetNegativeButton(cancel, (o, e) => tcs.SetResult(new EntryResult
                {
                    PressedButtonTitle = cancel,
                    Text = editText.Text
                }))
                .SetPositiveButton(accepte, (o, e) => tcs.SetResult(new EntryResult
                {
                    PressedButtonTitle = accepte,
                    Text = editText.Text
                }))
                .Show();

            return tcs.Task;
        }
    }
}
