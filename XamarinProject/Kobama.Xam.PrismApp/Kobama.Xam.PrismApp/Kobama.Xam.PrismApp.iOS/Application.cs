// -----------------------------------------------------------------------
//  <copyright file="Application.cs" company="mkoba">
//      Copyright (c) mkoba. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1300
namespace Kobama_Xam_PrismApp.iOS
{
    using UIKit;

    /// <summary>
    /// Application.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
