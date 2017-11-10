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
using Android.Gms.Gcm;
using PruSign.Droid.Binders;
using PruSign.Helpers;

namespace PruSign.Droid.Services
{
    [Service(Exported = true, Permission = "com.google.android.gms.permission.BIND_NETWORK_TASK_SERVICE")]
    [IntentFilter(new[] { "com.google.android.gms.gcm.ACTION_TASK_READY" })]
    public class BackgroundSyncService : GcmTaskService
    {
        IBinder binder;
        public override int OnRunTask(TaskParams @params)
        {
            SendHelper.SendSignatures();
            // TO-DO Check if the logs should be sent in background like the signatures.
            //await SendHelper.SendDeviceLogs();

            return GcmNetworkManager.ResultSuccess;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new BackgroundServiceBinder(this);
            return binder;
        }
    }
}