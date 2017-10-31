using Newtonsoft.Json;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PruSignBackEnd.Controllers
{
    public class DeviceLogApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<DeviceLog> serviceDeviceLog;

        public DeviceLogApiController()
        {
            serviceDeviceLog = new Service<DeviceLog>(db);
        }

        [Route("api/devicelog/")]
        public HttpResponseMessage Get(string device)
        {
            try
            {
                var logs = serviceDeviceLog.GetAll().Where(log => log.Device.Equals(device));
                if (!logs.Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                var resp = JsonConvert.SerializeObject(logs);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(resp, Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                SystemLogHelper.LogNewError(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [Route("api/devicelog/")]
        public HttpResponseMessage Post(DeviceLog log)
        {
            try
            {
                var newLog = new DeviceLog()
                {
                    Device = log.Device,
                    Details = log.Details
                };
                serviceDeviceLog.Add(newLog);

                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                SystemLogHelper.LogNewError(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}