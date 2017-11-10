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

                List<DeviceLogEntriesViewModel> formattedLogs = new List<DeviceLogEntriesViewModel>();

                foreach (var item in logs)
                {
                    formattedLogs.Add(new DeviceLogEntriesViewModel()
                    {
                        Created = item.Created,
                        Message = item.Message,
                        ErrorLocation = item.ErrorLocation,
                        StackTrace = item.StackTrace
                    });
                }
                
                var resp = JsonConvert.SerializeObject(formattedLogs);
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
        public HttpResponseMessage Post(DeviceLogViewModel log)
        {
            try
            {
                foreach (var item in log.Entries)
                {
                    var exists = serviceDeviceLog.GetAll().Where(l => l.Device.Equals(log.Device)
                                                                   && l.FormattedDate.Equals(item.FormattedDate));
                    if (!exists.Any())
                    {
                        serviceDeviceLog.Add(new DeviceLog()
                        {
                            Device = log.Device,
                            LogDate = item.Created,
                            Message = item.Message,
                            StackTrace = item.StackTrace,
                            ErrorLocation = item.ErrorLocation
                        });
                    }
                }

                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                SystemLogHelper.LogNewError(ex);

                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json")
                };
            }
        }
    }
}