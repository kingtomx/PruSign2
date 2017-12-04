using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Threading;
using Android.Gms.Gcm;
using Autofac;
using Autofac.Core;
using PruSign.Data.Interfaces;
using PruSign.Droid.Services;

namespace PruSign.Droid
{
    [Activity(Label = "PruSign.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new IModule[] { new PlatformSpecificModule() }));

            // Used to remove old logs and sent signatures
            StartupCleanUp();

            // Used to start synch job
            StartBackgroundSync();

        }

        private void StartBackgroundSync()
        {
            var periodicTask = new PeriodicTask.Builder()
                .SetPeriod(Constants.BACKGROUND_SEND_INTERVAL)
                .SetService(Java.Lang.Class.FromType(typeof(BackgroundSyncService)))
                .SetRequiredNetwork(0)
                .SetTag("com.prudential.prusign")
                .Build();

            GcmNetworkManager.GetInstance(this).Schedule(periodicTask);
        }

        private static void StartupCleanUp()
        {
            using (App.Container.BeginLifetimeScope())
            {
                var signatureService = App.Container.Resolve<ISignatureService>();
                var deviceLogService = App.Container.Resolve<IDeviceLogService>();

                deviceLogService.CleanOldLogs();
                signatureService.CleanSentSignatures();
            }
        }

        protected override void OnStop()
        {
            using (App.Container.BeginLifetimeScope())
            {
                var signatureService = App.Container.Resolve<ISignatureService>();
                ThreadPool.QueueUserWorkItem(async o => await signatureService.SendSignatures());
                base.OnStop();
            }
        }
    }
}
