using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Hardware;
using System.Threading;
using PruSign.Data;
using PruSign.Helpers;
using System.Collections.Generic;
using RestSharp;
using System.Net;
using Android.Gms.Gcm;
using PruSign.Droid.Services;

namespace PruSign.Droid
{
    [Activity(Label = "PruSign.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static readonly object _syncLock = new object();
        private SensorManager sensorManager;
        private Sensor sensor;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            sensor = sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // Used to remove old logs and sent signatures
            StartupCleanUp();

            // Used to start synch job
            StartBackgroundSync();

            LoadApplication(new App());
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

        private void StartupCleanUp()
        {
            CleanUpHelper.CleanOldLogs();
            CleanUpHelper.CleanSentSignatures();
        }

        protected override void OnStop()
        {
            ThreadPool.QueueUserWorkItem(o => SendHelper.SendSignatures());
            base.OnStop();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }


    }
}
