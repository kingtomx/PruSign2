using Newtonsoft.Json;
using PruSignBackEnd.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using PruSignBackEnd.Data.Entities;

namespace PruSignFrontEnd.Controllers
{
    public class DeviceQueriesController : Controller
    {
        // GET: DeviceQueries
        [Route("device/{imei}/queries", Name = "selectedDeviceQueries")]
        public async Task<ActionResult> Index(string user)
        {
            var result = new List<DeviceQueryViewModel>();
            var imei = RouteData.Values["imei"].ToString();
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/devicequeries/", Method.GET);
                request.AddParameter("imei", imei);
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
            ViewBag.SelectedUser = user;
            return View(result);
        }

        [Route("device/{imei}/queries/create", Name = "createDeviceQuery")]
        public async Task<ActionResult> Create(string user)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/questions/all", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("There was an error trying to get the questions");
                }

                ViewBag.QuestionList = JsonConvert.DeserializeObject<List<Question>>(response.Content);
                var imei = RouteData.Values["imei"].ToString();


                client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                request = new RestRequest("api/device/getbyimei", Method.GET);
                request.AddParameter("imei", imei);
                request.AddHeader("Content-Type", "application/json");
                response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("There was an error trying to get the device by imei");
                }

                var selectedDevice = JsonConvert.DeserializeObject<Device>(response.Content);
                ViewBag.DeviceId = selectedDevice.ID;
                ViewBag.SelectedUser = user;
                ViewBag.Imei = imei;
                return View();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Index", "Signature");
            }
        }


        public async Task<RedirectToRouteResult> CreateAnswer(Answer answer, string user, string imei)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/devicequeries/", Method.POST);
                request.AddJsonBody(answer);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToRoute("selectedDeviceQueries", new RouteValueDictionary(new { Imei = imei, User = user }));
        }
    }
}