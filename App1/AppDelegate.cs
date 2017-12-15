using System;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;
using Autofac.Core;
using PruSign.Background;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using PruSign.Data.Services;
using PruSign.Data.ViewModels;
using PruSign;

namespace App1
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public string IncomingData { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            KeyboardOverlapRenderer.Init();

            LoadApplication(new App(new IModule[] { new PlatformSpecificModule() }));
            ((PruSign.App)App.Current).Properties["isOpen"] = true;
            UIApplication.SharedApplication.ApplicationSupportsShakeToEdit = true;
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
            MessagingCenter.Subscribe<App>(this, "startIOSBackgroundSync", message =>
            {
                this.WireUpBackgroundDataSync();
            });
            return base.FinishedLaunching(app, options);
        }



        public override void DidEnterBackground(UIApplication app)
        {
            //var taskId = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            //new Task(async () =>
            //{
            //    using (var scope = App.Container.BeginLifetimeScope())
            //    {
            //        var signatureService = App.Container.Resolve<SignatureService>();
            //        await signatureService.SendSignatures();
            //        UIApplication.SharedApplication.EndBackgroundTask(taskId);
            //    }
            //}).Start();

        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                var signatureService = App.Container.Resolve<SignatureService>();
                await signatureService.SendSignatures();
                completionHandler(UIBackgroundFetchResult.NewData);
            }
        }

        public void WireUpBackgroundDataSync()
        {
            MessagingCenter.Subscribe<StartDataSync>(this, "StartDataSyncMessage", async message =>
            {
                var asyncTask = new iOSLongRunningTask();
                await asyncTask.Start();
            });

            MessagingCenter.Subscribe<StopDataSync>(this, "StopDataSyncMessage", async message =>
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                var startMessage = new StartDataSync();
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send(startMessage, "StartDataSyncMessage");
                });
            });
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            ((PruSign.App)App.Current).ParseIncomingData(url.AbsoluteString);
            // Check if the application is already open when the parameters are sent
            if (((PruSign.App)App.Current).Properties.ContainsKey("isOpen"))
            {
                // TO-DO call Update
                ((PruSign.App)App.Current).UpdateIncomingData();
            }

            return true;
        }

    }
}
