using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Autofac;
using PruSign.Data.Interfaces;
using PruSign.Data.Services;
using PruSign.Droid.Binders;

namespace PruSign.Droid.Services
{
    [Service(Exported = true, Permission = "com.google.android.gms.permission.BIND_NETWORK_TASK_SERVICE")]
    [IntentFilter(new[] { "com.google.android.gms.gcm.ACTION_TASK_READY" })]
    public class BackgroundSyncService : GcmTaskService
    {

        private IDeviceLogService _deviceLogService { get; set; }

        public BackgroundSyncService()
        {
                
        }

        BackgroundSyncService(IDeviceLogService deviceLogService)
        {
            _deviceLogService = deviceLogService;
        }

        IBinder _binder;
        public override int OnRunTask(TaskParams @params)
        {
            var signatureService = App.Container.Resolve<SignatureService>();
            signatureService.SendSignatures();
            // TO-DO Check if the logs should be sent in background like the signatures.
            //await _deviceLogService.SendDeviceLogs();

            return GcmNetworkManager.ResultSuccess;
        }

        public override IBinder OnBind(Intent intent)
        {
            _binder = new BackgroundServiceBinder(this);
            return _binder;
        }
    }
}