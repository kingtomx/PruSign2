﻿using System;
using System.IO;
using PruSign.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace PruSign.iOS
{

    public class FileHelper : IFileHelper
	{
		public string GetLocalFilePath(string filename)
		{
			string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

			if (!Directory.Exists(libFolder))
			{
				Directory.CreateDirectory(libFolder);
			}

			return Path.Combine(libFolder, filename);
		}
	}
}