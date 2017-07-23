using System;
using System.IO;
using Xamarin.Forms;
using GitForApple.Droid;
using GitForApple.Helpers;

[assembly: Dependency(typeof(FileHelper))]
namespace GitForApple.Droid
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