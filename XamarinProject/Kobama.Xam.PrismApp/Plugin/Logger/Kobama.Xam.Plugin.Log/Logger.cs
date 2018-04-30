// -----------------------------------------------------------------------
// <copyright file="Logger.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Log
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Logger.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The name of the class.
        /// </summary>
        private string className = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="className">Class name.</param>
        public Logger(string className)
        {
            this.className = className;
        }

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="method">Method.</param>
        public void CalledMethod(string message = "", [CallerMemberName] string method = "")
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff ")} [Proc][Class]{this.className} [{method}()] {message}");
        }

        /// <summary>
        /// Error the specified message and method.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="method">Method.</param>
        public void Error(string message = "", [CallerMemberName] string method = "")
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff ")} [Err ][Class]{this.className} [{method}()] {message}");
        }

        /// <summary>
        /// Debug the specified message and method.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="method">Method.</param>
        public void Debug(string message = "", [CallerMemberName] string method = "")
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff ")} [DBG ][Class]{this.className} [{method}()] {message}");
        }
    }
}
