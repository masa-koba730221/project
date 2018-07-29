// -----------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#pragma warning disable SA1300
namespace Kobama.Xam.Plugin.Camera.iOS
{
    using System.Collections.Generic;
    using CoreGraphics;
    using CoreImage;
    using Foundation;

    /// <summary>
    /// Dictionary Extensions
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// To the ns dictionary.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns>NSDictionary</returns>
        public static NSDictionary<NSString, CIVector> ToNSDictionary(this Dictionary<string, CGPoint> self)
        {
            var keys = new List<NSString>();
            var values = new List<CIVector>();

            // Process all keys in the dictionary
            foreach (string key in self.Keys)
            {
                keys.Add(new NSString(key));
                values.Add(new CIVector(self[key]));
            }

            // Return results
            return new NSDictionary<NSString, CIVector>(keys.ToArray(), values.ToArray());
        }

        /// <summary>
        /// To the ns dictionary.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns>NSDictionary</returns>
        public static NSDictionary<NSString, NSNumber> ToNSDictionary(this Dictionary<NSString, NSNumber> self)
        {
            var keys = new List<NSString>();
            var values = new List<NSNumber>();

            // Process all keys in the dictionary
            foreach (NSString key in self.Keys)
            {
                keys.Add(key);
                values.Add(self[key]);
            }

            // Return results
            return new NSDictionary<NSString, NSNumber>(keys.ToArray(), values.ToArray());
        }
    }
}
