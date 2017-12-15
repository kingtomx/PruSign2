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
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            var customerName = Intent.GetStringExtra("customerName");
            var customerId = Intent.GetStringExtra("customerId");
            if (customerName != null && customerId != null)
            {
                ((PruSign.App)App.Current).SetAppProperty("customerName",customerName);
                ((PruSign.App)App.Current).SetAppProperty("customerId", customerId);
                if (((PruSign.App)App.Current).Properties.ContainsKey("isOpen"))
                {
                    // TO-DO call Update
                    ((PruSign.App)App.Current).UpdateIncomingData();
                }
            }

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
