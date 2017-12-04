using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Autofac;
using PruSign.Data.Services;
using PruSign.Droid.Binders;

namespace PruSign.Droid.Services
{
    [Service(Exported = true, Permission = "com.google.android.gms.permission.BIND_NETWORK_TASK_SERVICE")]
    [IntentFilter(new[] { "com.google.android.gms.gcm.ACTION_TASK_READY" })]
    public class BackgroundSyncService : GcmTaskService
    {
        IBinder _binder;
        public override int OnRunTask(TaskParams @params)
        {
            using (App.Container.BeginLifetimeScope())
            {
                var signatureService = App.Container.Resolve<SignatureService>();
                signatureService.SendSignatures();
            }
            return GcmNetworkManager.ResultSuccess;
        }

        public override IBinder OnBind(Intent intent)
        {
            _binder = new BackgroundServiceBinder(this);
            return _binder;
        }
    }
}