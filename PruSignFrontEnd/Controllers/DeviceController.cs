﻿using Newtonsoft.Json;
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
using PruSignBackEnd.Data.Entities;

namespace PruSignFrontEnd.Controllers
{
    public class DeviceController : Controller
    {
        // GET: Device
        public async Task<ActionResult> Index(string searchText)
        {
            var result = new List<Device>();
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/device/", Method.GET);
                request.AddParameter("searchText", searchText);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<List<Device>>(response.Content);
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

        [Route("device/{imei}", Name = "selectedDevice")]
        public ActionResult SelectedDevice(string imei, string user)
        {
            ViewBag.SelectedUser = user;
            return View((object)imei);
        }

    }
}