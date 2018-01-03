using Newtonsoft.Json;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.ViewModels;
using PruSignFrontEnd.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace PruSignFrontEnd.Controllers
{
    public class DeviceQueriesController : Controller
    {
        // GET: DeviceQueries
        [Route("device/{deviceId}/queries", Name = "selectedDeviceQueries")]
        public async Task<ActionResult> Index()
        {

            var result = new List<DeviceQueryViewModel>();
            var deviceId = this.RouteData.Values["deviceId"].ToString();
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/devicequeries/", Method.GET);
                request.AddParameter("deviceId", deviceId);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<List<DeviceQueryViewModel>>(response.Content);
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
            ViewBag.DeviceUser = deviceId;
            return View(result);
        }
    }
}