using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using PruSign.Data.Entities;
using SQLite;
using Xamarin.Forms;
using PruSign.iOS.Data;
using System.IO;

[assembly: Dependency(typeof(SQLiteIOS))]
namespace PruSign.iOS.Data
{
    class SQLiteIOS : ISQLite
    {
        public SQLiteAsyncConnection GetConnectionAsync()
        {
            var fileName = "PruSign.db";

            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            var path = Path.Combine(libFolder, fileName);
            var connection = new SQLiteAsyncConnection(path);

            return connection;
        }

        public SQLiteConnection GetConnection()
        {
            var fileName = "PruSign.db";

            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            var path = Path.Combine(libFolder, fileName);
            var connection = new SQLiteConnection(path);

            return connection;
        }
    }
}