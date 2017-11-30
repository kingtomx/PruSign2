using System;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using UIKit;
using PruSign.Background;
using PruSign.Data.Services;

namespace PruSign.iOS
{
    public class iOSLongRunningTask
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunningTask", OnExpiration);
            try
            {
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    var signatureService = App.Container.Resolve<SignatureService>();
                    await signatureService.SendSignatures();
                }
            }
            catch (OperationCanceledException ex)
            {
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    var deviceLogService = App.Container.Resolve<DeviceLogService>();
                    deviceLogService.Log(ex);
                }
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var stopMessage = new StopDataSync();
                    MessagingCenter.Send(stopMessage, "StopDataSyncMessage");
                });
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void OnExpiration()
        {
            _cts.Cancel();
        }


    }
}
