// -----------------------------------------------------------------------
// <copyright file="SettingsImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.PrismApp.Settings
{
    using Xamarin.Forms;

    /// <summary>
    /// Settings impl.
    /// </summary>
    public class SettingsImpl : ISettingsService
    {
        private const string KeyAzureFaceApiRoot = "AzureFaceApiRoot";
        private const string KeyAzureFaceApiKey = "AzureFaceApiKey";

        private const string DefaultAzureFaceApiKey = "";
        private const string DefaultAzureFaceApiRoot = "";

        /// <summary>
        /// Gets or sets the azure face API root.
        /// </summary>
        /// <value>The azure face API root.</value>
        public string AzureFaceApiRoot
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(KeyAzureFaceApiRoot))
                {
                    var value = (string)Application.Current.Properties[KeyAzureFaceApiRoot];
                    if (string.IsNullOrEmpty(value))
                    {
                        value = DefaultAzureFaceApiRoot;
                        Application.Current.SavePropertiesAsync();
                        Application.Current.Properties[KeyAzureFaceApiRoot] = value;
                    }

                    return value;
                }
                else
                {
                    var value = DefaultAzureFaceApiRoot;
                    Application.Current.Properties[KeyAzureFaceApiRoot] = value;
                    Application.Current.SavePropertiesAsync();
                    return value;
                }
            }

            set
            {
                Application.Current.Properties[KeyAzureFaceApiRoot] = value;
                Application.Current.SavePropertiesAsync();
            }
        }

        /// <summary>
        /// Gets or sets the azure face API key.
        /// </summary>
        /// <value>The azure face API key.</value>
        public string AzureFaceApiKey
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(KeyAzureFaceApiKey))
                {
                    var value = (string)Application.Current.Properties[KeyAzureFaceApiKey];
                    if (string.IsNullOrEmpty(value))
                    {
                        value = DefaultAzureFaceApiKey;
                        Application.Current.Properties[KeyAzureFaceApiKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }

                    return value;
                }
                else
                {
                    var value = DefaultAzureFaceApiKey;
                    Application.Current.Properties[KeyAzureFaceApiKey] = value;
                    Application.Current.SavePropertiesAsync();
                    return value;
                }
            }

            set
            {
                Application.Current.Properties[KeyAzureFaceApiKey] = value;
                Application.Current.SavePropertiesAsync();
            }
        }
    }
}
