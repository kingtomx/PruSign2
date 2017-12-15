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

namespace PruSign.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            KeyboardOverlapRenderer.Init();
            LoadApplication(new App(new IModule[] { new PlatformSpecificModule() }, new SignatureViewModel()));
            UIApplication.SharedApplication.ApplicationSupportsShakeToEdit = true;
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
            WireUpBackgroundDataSync();
            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication app)
        {
            var taskId = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(async () =>
            {
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    var signatureService = App.Container.Resolve<SignatureService>();
                    await signatureService.SendSignatures();
                    UIApplication.SharedApplication.EndBackgroundTask(taskId);
                }
            }).Start();

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

    }
}
