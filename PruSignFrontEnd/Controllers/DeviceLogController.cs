using Newtonsoft.Json;
using PruSignFrontEnd.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace PruSignFrontEnd.Controllers
{
    public class DeviceLogController : Controller
    {
        // GET: DeviceLog
        [Route("device/{deviceId}/logs", Name = "selectedDeviceLogs")]
        public async Task<ActionResult> Index(string searchText)
        {
            var result = new List<DeviceLog>();
            var username = this.RouteData.Values["deviceId"].ToString();
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/devicelog/", Method.GET);
                request.AddParameter("username", username);
                request.AddParameter("searchText", searchText);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<List<DeviceLog>>(response.Content);
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
            ViewBag.DeviceUser = username;
            return View(result);
        }
    }
}