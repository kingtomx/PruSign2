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
using PruSign.Droid.Services;

namespace PruSign.Droid.Binders
{
    class BackgroundServiceBinder : Binder
    {
        BackgroundService service;

        public BackgroundServiceBinder(BackgroundService service)
        {
            this.service = service;
        }

        public BackgroundService GetBackgroundService()
        {
            return service;
        }

    }
}