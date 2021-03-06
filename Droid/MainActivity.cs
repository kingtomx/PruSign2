using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Threading;
using Android.Content;
using Android.Gms.Gcm;
using Autofac;
using Autofac.Core;
using Java.Lang;
using PruSign.Data.Interfaces;
using PruSign.Droid.Services;
using Xamarin.Forms;
using System;
using PruSign.Data.ViewModels;

namespace PruSign.Droid
{
    [IntentFilter(new[] { Intent.ActionSend },
        Categories = new[] { Intent.CategoryDefault },
        Icon = "@drawable/icon",
        DataMimeType = "text/plain"
        )]
    [Activity(Label = "PruSign", Name = "com.prudential.prusign.activity", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App(new IModule[] { new PlatformSpecificModule() }));

            var customerName = Intent.GetStringExtra("customerName");
            var customerId = Intent.GetStringExtra("customerId");
            if (customerName != null && customerId != null)
            {
                var signatureData = new SignatureViewModel()
                {
                    customerName = customerName,
                    customerId = customerId
                };

                ((PruSign.App)App.Current).HandleIncomingDataForAndroid(signatureData);
            }

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
            try
            {
                using (App.Container.BeginLifetimeScope())
                {
                    var signatureService = App.Container.Resolve<ISignatureService>();
                    var deviceLogService = App.Container.Resolve<IDeviceLogService>();

                    deviceLogService.CleanOldLogs();
                    signatureService.CleanSentSignatures();
                }
            }
            catch(System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
