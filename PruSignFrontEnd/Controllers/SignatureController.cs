using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PruSignFrontEnd.Models;
using PruSignFrontEnd.ViewModels;
using System.IO;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace PruSignFrontEnd.Controllers
{
    public class SignatureController : Controller
    {
        // GET: Signature
        public async Task<ActionResult> Index(string searchText)
        {
            var result = new List<Signature>();
            try
            {
                var client = new RestClient(Constants.BACKEND_HOST_NAME);
                var request = new RestRequest("api/signature/all", Method.GET);
                if (!String.IsNullOrEmpty(searchText))
                {
                    request = new RestRequest("api/signature/search", Method.GET);
                    request.AddParameter("searchText", searchText);
                }
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<List<Signature>>(response.Content);
                }
                else
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View(result);
        }

        public async Task<ActionResult> Details(string customerId, string documentId, string applicationId)
        {
            var result = new SignatureViewModel();
            try
            {
                var client = new RestClient(Constants.BACKEND_HOST_NAME);
                var request = new RestRequest("api/signature", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("customerid", customerId);
                request.AddParameter("documentid", documentId);
                request.AddParameter("applicationid", applicationId);

                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<SignatureViewModel>(response.Content);
                }
                else
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View(result);
        }

        public ActionResult Imagen(string imageByteArray)
        {

            return File(imageByteArray, "image/jpeg");
        }
    }
}