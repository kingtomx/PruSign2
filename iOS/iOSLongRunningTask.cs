using System;
using Xamarin.Forms;
using Foundation;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using PruSign.Helpers;
using PruSign.Background;

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
                await SendHelper.SendSignatures();
            }
            catch (OperationCanceledException ex) {
                LogHelper.Log(ex);
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
