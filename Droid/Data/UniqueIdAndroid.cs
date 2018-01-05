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
using PruSign.Data.Interfaces;
using PruSign.Droid.Data;
using Xamarin.Forms;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(UniqueIdAndroid))]
namespace PruSign.Droid.Data
{
    public class UniqueIdAndroid : IUniqueID
    {
        public string GetIdentifier()
        {
            return Secure.GetString(MainActivity.Instance.ContentResolver, Secure.AndroidId);
        }
    }
}