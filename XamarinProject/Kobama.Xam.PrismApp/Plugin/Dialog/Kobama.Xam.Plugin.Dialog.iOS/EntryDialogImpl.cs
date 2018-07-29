// -----------------------------------------------------------------------
// <copyright file="EntryDialogImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Dialog.iOS
{
    using System;
    using System.Threading.Tasks;
    using UIKit;

    /// <summary>
    /// Entry dialog impl.
    /// </summary>
    public class EntryDialogImpl : IEntryDialogService
    {
        /// <summary>
        /// Show
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="message">Message</param>
        /// <param name="accept">Accept Button Name</param>
        /// <param name="cancel">Cancel Button Name</param>
        /// <param name="isPassword">Whether if entering password</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<EntryResult> Show(string title, string message, string accept, string cancel, bool isPassword = false)
        {
            var tcs = new TaskCompletionSource<EntryResult>();

            UIKit.UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alert = new UIAlertView()
                {
                    Title = title,
                    Message = message,
                };

                alert.AddButton(accept);
                alert.AddButton(cancel);

                if (isPassword)
                {
                    alert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
                }
                else
                {
                    alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                }

                alert.Clicked += (sender, e) => tcs.SetResult(new EntryResult
                {
                    PressedButtonTitle = alert.ButtonTitle(e.ButtonIndex),
                    Text = alert.GetTextField(0).Text
                });
                alert.Show();
            });

            return tcs.Task;
        }
    }
}