using System;
using Xamarin.Forms;
using Foundation;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace PruSign.iOS
{
	public class iOSLongRunningTask
	{

		public void Start()
		{
			try {
				Post("https://reqres.in/api/users");
			} catch (OperationCanceledException) {
				Device.BeginInvokeOnMainThread(() => MessagingCenter.Send("Operation Cancelled", "CancelledMessage"));
			}
		}


		public void Post(string url)
		{
			NSUrlSession session = null;

			NSUrlSessionConfiguration configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration("com.SimpleBackgroundTransfer.BackgroundSession");
			session = NSUrlSession.FromConfiguration(configuration, (NSUrlSessionDelegate)new MySessionDelegate(), new NSOperationQueue());

			// URL
			NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(url));
			// METHOD
			request.HttpMethod = "POST";
			// HEADERS
			request.HttpMethod = "POST";
			var keys = new object[] { "Key1", "Key2" };
			var objects = new object[] { "Value1", "Value2" };
			var dictionnary = NSDictionary.FromObjectsAndKeys(objects, keys);
			request.Headers = dictionnary;
			// BODY
			NSString postString = (NSString)"\"{\\\"name\\\": \\\"tomas\\\",\\\"job\\\": \\\"supervisor\\\"}\"";
			NSData postData = NSData.FromString(postString);
			request.Body = postData;

			NSUrlSessionUploadTask uploadTask = session.CreateUploadTask(request);
			uploadTask.Resume();
		}
	}
}
