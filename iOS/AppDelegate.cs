using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Forms;
using PruSign.Data;
using PruSign.Helpers;
using RestSharp;
using System.Net;
using PruSign.Data.Entities;
using PruSign.Background;
using KeyboardOverlap.Forms.Plugin.iOSUnified;

namespace PruSign.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            KeyboardOverlapRenderer.Init();
            LoadApplication(new App());
            UIApplication.SharedApplication.ApplicationSupportsShakeToEdit = true;
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
            WireUpBackgroundDataSync();
            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication app)
        {
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(async () =>
            {
                await SendHelper.SendSignatures();
                UIApplication.SharedApplication.EndBackgroundTask(taskID);
            }).Start();

        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            await SendHelper.SendSignatures();
            completionHandler(UIBackgroundFetchResult.NewData);

        }

        public void WireUpBackgroundDataSync()
        {
            MessagingCenter.Subscribe<StartDataSync>(this, "StartDataSyncMessage", async message => {
                var asyncTask = new iOSLongRunningTask();
                await asyncTask.Start();
            });

            MessagingCenter.Subscribe<StopDataSync>(this, "StopDataSyncMessage", async message =>
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                var startMessage = new StartDataSync();
                Device.BeginInvokeOnMainThread(() => {
                    MessagingCenter.Send(startMessage, "StartDataSyncMessage");
                });
            });
        }

    }
}
