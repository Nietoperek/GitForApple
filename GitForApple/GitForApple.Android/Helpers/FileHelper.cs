using Xamarin.Forms;
using GitForApple.Droid.Helpers;
using GitForApple.Helpers;
using System.IO;
using System;

[assembly: Dependency(typeof(FileHelper))]
namespace GitForApple.Droid.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}