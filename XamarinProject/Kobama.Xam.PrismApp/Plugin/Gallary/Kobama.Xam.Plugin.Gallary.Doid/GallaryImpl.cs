// -----------------------------------------------------------------------
// <copyright file="GallaryImpl.cs" company="Kobama">
// Copyright (c) Kobama. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Kobama.Xam.Plugin.Gallary.Doid
{
    using System.Drawing;
    using Android.Content;
    using Java.IO;
    using Java.Lang;
    using Java.Text;
    using Java.Util;
    using static Android.Provider.MediaStore;

    /// <summary>
    /// Gallary Impl
    /// </summary>
    /// <seealso cref="Kobama.Xam.Plugin.Gallary.IGallaryService" />
    public class GallaryImpl : IGallaryService
    {
        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <param name="path">The path.</param>
        /// <param name="fileName">Name of the file.</param>
        public void SaveImage(byte[] image, Size size, string path, string fileName)
        {
            var activity = global::Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            var file = new File(Android.OS.Environment.ExternalStorageDirectory.Path + path);
            try
            {
                if (!file.Exists())
                {
                    file.Mkdir();
                }
            }
            catch (SecurityException e)
            {
                e.PrintStackTrace();
                throw e;
            }

            Date mDate = new Date();
            SimpleDateFormat fileNameDate = new SimpleDateFormat("yyyyMMdd_HHmmss");
            fileName = fileName + fileNameDate.Format(mDate) + ".jpg";
            var attachName = file.AbsolutePath + "/" + fileName;
            FileOutputStream output = null;
            try
            {
                output = new FileOutputStream(attachName);
                output.Write(image);
                output.Flush();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
                throw e;
            }
            finally
            {
                output?.Close();
            }

            // save index
            var contentResolver = ((Context)activity).ContentResolver;
            Images.Media.InsertImage(contentResolver, attachName, fileName, string.Empty);
        }
    }
}
