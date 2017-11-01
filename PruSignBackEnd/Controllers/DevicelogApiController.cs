using Newtonsoft.Json;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.Helpers;
using PruSignBackEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PruSignBackEnd
{
    [RoutePrefix("api")]
    public class DeviceLogApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<DeviceLog> serviceDeviceLog;

        public DeviceLogApiController()
        {
            serviceDeviceLog = new Service<DeviceLog>(db);
        }

        [Route("devicelog/")]
        public HttpResponseMessage Get(string device)
        {
            try
            {
                var logs = serviceDeviceLog.GetAll().Where(log => log.Device.Equals(device));
                if (!logs.Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                // The original query returns the log.details field as a string. 
                // Below we deserialize the json string into a DeviceLogDetailsViewModel array and, finally, 
                // we create a DeviceLogViewModel object including the details inside. 
                // Now the details will be returned as json object arrays 
                List<object> formattedResults = new List<object>();

                foreach (var item in logs)
                {
                    DeviceLogDetailsViewModel[] result = JsonConvert.DeserializeObject<DeviceLogDetailsViewModel[]>(item.Details);
                    formattedResults.Add(new DeviceLogViewModel
                    {
                        Device = item.Device,
                        Created = item.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                        Updated = item.Updated.ToString("yyyy-MM-dd HH:mm:ss"),
                        Details = result
                    });
                    
                }
                var resp = JsonConvert.SerializeObject(formattedResults);
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

        [HttpPost]
        [Route("devicelog/")]
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