using System;
using System.IO;
using PruSign.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace PruSign.Droid
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