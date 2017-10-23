using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Hardware;
using System.Threading;
using Flurl;
using Flurl.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace PruSign.Droid
{
    [Activity(Label = "PruSign.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static readonly object _syncLock = new object();
        private SensorManager sensorManager;
        private Sensor sensor;
        private ShakeDetector shakeDetector;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            sensor = sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            shakeDetector = new ShakeDetector();
            shakeDetector.Shaked += (sender, shakeCount) =>
                {
                    lock (_syncLock)
                    {
                        // Accion a realizar en el caso de que se detecte que el dispositivo ha sido agitado
                    }
                };


            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }



        protected override void OnStop()
        {
            base.OnStop();

            ThreadPool.QueueUserWorkItem(o => sendRestSignature());

        }

        private async void sendRestSignature()
        {
            try
            {
                FileHelper fh = new FileHelper();
                SignatureDatabase db = new SignatureDatabase(fh.GetLocalFilePath("PruSign.db"));
                System.Threading.Tasks.Task<System.Collections.Generic.List<SignatureItem>> items = db.GetItemsNotDoneAsync();
                foreach (var item in items.Result)
				{
					try
					{

                        			Signature requestItem = Signature.LoadFromJson(item.SignatureObject);
						var response = await "http://10.0.2.2:8080/api/SignatureApi".PostJsonAsync(requestItem);
						item.Sent = true;
						item.SentTimeStamp = System.DateTime.Now.Ticks;
						await db.SaveItemAsync(item);
					}
					catch (FlurlHttpTimeoutException timeOutException)
					{
						item.Miscelanea = timeOutException.Message;
						await db.SaveItemAsync(item);
					}
					catch (FlurlHttpException generalException)
					{
						item.Miscelanea = generalException.Message;
						await db.SaveItemAsync(item);
					}


                }
            }
            catch (Exception ex)
            {
                String error = ex.Message;
            }
        }


        protected override void OnResume()
        {
            base.OnResume();

            sensorManager.RegisterListener(shakeDetector, sensor, SensorDelay.Ui);
        }

        protected override void OnPause()
        {
            base.OnPause();

            sensorManager.UnregisterListener(shakeDetector);
        }


    }
}
