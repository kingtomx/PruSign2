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
        public HttpResponseMessage Get(string username, string searchText)
        {
            try
            {
                var logs = serviceDeviceLog.GetAll();
                if (!String.IsNullOrEmpty(searchText))
                {
                    logs = logs.Where(d => d.User.Equals(username) &&
                                          (d.StackTrace.Contains(searchText) ||
                                           d.Message.Contains(searchText)))
                               .OrderByDescending(l => l.Created)
                               .Take(50);
                }
                else
                {
                    logs = logs.Where(log => log.User.Equals(username));
                }
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

        [Route("device/")]
        public HttpResponseMessage Get(string searchText)
        {
            try
            {
                var devices = serviceDeviceLog.GetAll()
                    .GroupBy(l => l.User)
                    .Select(group => group.FirstOrDefault());

                if (!String.IsNullOrEmpty(searchText))
                {
                    devices = devices.Where(d => d.User.Contains(searchText));
                }

                if (!devices.Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                List<DeviceViewModel> formattedDevices = new List<DeviceViewModel>();

                foreach (var item in devices)
                {
                    formattedDevices.Add(new DeviceViewModel()
                    {
                        Name = item.Device,
                        User = item.User
                    });
                }

                var resp = JsonConvert.SerializeObject(formattedDevices);
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
                    var exists = serviceDeviceLog.GetAll().Where(l => l.User.Equals(log.User)
                                                                   && l.FormattedDate.Equals(item.FormattedDate));
                    if (!exists.Any())
                    {
                        serviceDeviceLog.Add(new DeviceLog()
                        {
                            Device = log.Device,
                            User = log.User,
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