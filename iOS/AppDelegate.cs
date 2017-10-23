using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PruSign.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{


		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			/*
			MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", async message => {
				iOSLongRunningTask longRunningTask = new iOSLongRunningTask();
            	longRunningTask.Start();
        	});
			*/
			LoadApplication(new App());
			UIApplication.SharedApplication.ApplicationSupportsShakeToEdit = true;

			return base.FinishedLaunching(app, options);
		}

		public override void DidEnterBackground(UIApplication app)
		{
			sendRestSignature();
		}


		private void sendRestSignature()
		{
			nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() =>
			{
			});
			new Task(() =>
						{
							try
							{
								FileHelper fh = new FileHelper();
								SignatureDatabase db = new SignatureDatabase(fh.GetLocalFilePath("PruSign.db"));
								Task<List<SignatureItem>> items = db.GetItemsNotDoneAsync();

								foreach (var item in items.Result)
								{
									try
									{
										PruSign.HttpUtils.Post("https://api.prudentialseguros.com.ar:4043", "/Prusign/Api/SignatureApi", item.SignatureObject);
										item.Sent = true;
										item.SentTimeStamp = System.DateTime.Now.Ticks;
										item.SignatureObject = "";
										db.SaveItemAsync(item);
									}
									catch (Exception ex)
									{
										// POR AHORA HAGAMOS ESTO EN CASO DE ERROR
										item.Miscelanea = ex.Message;
										db.SaveItemAsync(item);
									}
								}

								UIApplication.SharedApplication.EndBackgroundTask(taskID);
							}
							catch (Exception ex)
							{
								String error = ex.Message;
							}
						}).Start();			
		}

		private NSHttpUrlResponse Post(string url, string jsonMessage)
		{
			NSUrlSession session = null;

			NSUrlSessionConfiguration configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration("com.SimpleBackgroundTransfer.BackgroundSession");
			session = NSUrlSession.FromConfiguration(configuration, (NSUrlSessionDelegate)new MySessionDelegate(), new NSOperationQueue());

			// URL
			NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(url));
			// METHOD
			request.HttpMethod = "POST";
			// HEADERS
			var keys = new object[] { "Content-Type" };
			var objects = new object[] { "application/json" };
			var dictionnary = NSDictionary.FromObjectsAndKeys(objects, keys);
			request.Headers = dictionnary;
			// BODY
			NSString postString = (NSString)jsonMessage;
			NSData postData = NSData.FromString(postString);
			request.Body = postData;

			NSUrlSessionUploadTask uploadTask = session.CreateUploadTask(request);
			uploadTask.Resume();
			return (Foundation.NSHttpUrlResponse)uploadTask.Response;

		}




	}


}
