using Newtonsoft.Json;
using PruSignFrontEnd.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PruSignFrontEnd.Controllers
{
    public class SystemLogController : Controller
    {
        // GET: SystemLog
        public async Task<ActionResult> Index(string searchText)
        {
            var result = new List<SystemLog>();
            try
            {
                var client = new RestClient(Constants.BACKEND_HOST_NAME);
                var request = new RestRequest("api/log/all", Method.GET);
                if (!String.IsNullOrEmpty(searchText))
                {
                    request = new RestRequest("api/log/search", Method.GET);
                    request.AddParameter("searchText", searchText);
                }
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<List<SystemLog>>(response.Content);
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
    }
}