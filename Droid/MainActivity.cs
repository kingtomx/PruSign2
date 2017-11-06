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

            StartBackgroundCleanUp();
            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }


        private void StartBackgroundCleanUp()
        {
            var periodicTask = new PeriodicTask.Builder()
                .SetPeriod(30)
                .SetService(Java.Lang.Class.FromType(typeof(BackgroundService)))
                .SetRequiredNetwork(0)
                .SetTag("com.prudential.prusign")
                .Build();

            GcmNetworkManager.GetInstance(this).Schedule(periodicTask);
        }

        protected override void OnStop()
        {
            base.OnStop();

            ThreadPool.QueueUserWorkItem(o => SendHelper.SendSignatures());

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
