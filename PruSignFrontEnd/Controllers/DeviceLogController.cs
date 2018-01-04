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
        [Route("device/{imei}/logs", Name = "selectedDeviceLogs")]
        public async Task<ActionResult> Index(string searchText, string user)
        {
            var result = new List<DeviceLog>();
            var imei = this.RouteData.Values["imei"].ToString();
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/devicelog/", Method.GET);
                request.AddParameter("username", imei);
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
            ViewBag.SelectedUser = user;
            return View(result);
        }
    }
}