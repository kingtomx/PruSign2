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

namespace PruSign.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            UIApplication.SharedApplication.ApplicationSupportsShakeToEdit = true;

            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication app)
        {
            SendRestSignature();
        }


        private void SendRestSignature()
        {
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(async () =>
            {
                try
                {
                    PruSignDatabase db = new PruSignDatabase();
                    ServiceAsync<Signature> serviceSignature = new ServiceAsync<Signature>(db);
                    List<Signature> signaturesToSend = await serviceSignature.GetAll().Where(i => !i.Sent).ToListAsync();

                    var client = new RestClient(Constants.BackendHostName);
                    var request = new RestRequest("api/signature", Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddJsonBody(signaturesToSend);
                    var response = await client.ExecuteTaskAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in signaturesToSend)
                        {
                            item.Sent = true;
                            item.SentDate = DateTime.Now;
                            await serviceSignature.Update(item);
                        }
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }

            });

            UIApplication.SharedApplication.EndBackgroundTask(taskID);
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
