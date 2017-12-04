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
    public class BackgroundServiceBinder : Binder
    {
        private readonly BackgroundSyncService _service;

        public BackgroundServiceBinder(BackgroundSyncService service)
        {
            this._service = service;
        }

        public BackgroundSyncService GetBackgroundService()
        {
            return _service;
        }

    }
}