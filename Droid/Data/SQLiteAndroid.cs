using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using PruSign.Data.Entities;
using Xamarin.Forms;
using PruSign.Droid.Data;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace PruSign.Droid.Data
{
    public class SQLiteAndroid : ISQLite
    {
        public SQLiteAsyncConnection GetConnectionAsync()
        {
            var fileName = "PruSign.db";

            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);

            var connection = new SQLiteAsyncConnection(path);

            return connection;
        }
    }
}