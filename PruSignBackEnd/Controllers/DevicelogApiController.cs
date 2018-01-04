using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.Helpers;
using PruSignBackEnd.ViewModels;

namespace PruSignBackEnd.Controllers
{
    [RoutePrefix("api")]
    public class DeviceLogApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private readonly Service<DeviceLog> _serviceDeviceLog;
        private readonly Service<Device> _serviceDevice;

        public DeviceLogApiController()
        {
            _serviceDeviceLog = new Service<DeviceLog>(db);
            _serviceDevice = new Service<Device>(db);
        }

        public Device GetDevice(string imei)
        {
            return _serviceDevice.GetAll().FirstOrDefault(d => d.Imei.Equals(imei));
        }

        public void CreateDevice(Device device)
        {
            _serviceDevice.Add(new Device()
            {
                Imei = device.Imei,
                User = device.User
            });

            db.SaveChanges();
        }

        [Route("devicelog/")]
        public HttpResponseMessage Get(string username, string searchText)
        {
            try
            {
                var device = GetDevice(username);
                if (device == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
                var logs = _serviceDeviceLog.GetAll();
                if (!String.IsNullOrEmpty(searchText))
                {
                    logs = logs.Where(d => d.DeviceID == device.ID &&
                                          (d.StackTrace.Contains(searchText) ||
                                           d.Message.Contains(searchText)))
                               .OrderByDescending(l => l.Created)
                               .Take(50);
                }
                else
                {
                    logs = logs.Where(log => log.DeviceID == device.ID);
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
                        StackTrace = item.StackTrace,
                        DeviceImei = device.Imei,
                        DeviceUser = device.User
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
                var device = GetDevice(log.Device.Imei);
                if (device == null)
                {
                    CreateDevice(log.Device);
                    device = GetDevice(log.Device.Imei);
                }

                foreach (var item in log.Entries)
                {
                    // This is a control to prevent duplicated logs
                    var exists = _serviceDeviceLog.GetAll().Where(l => l.Device.Imei.Equals(log.Device.Imei)
                                                                   && l.FormattedDate.Equals(item.FormattedDate));
                    if (exists.Any()) continue;

                    _serviceDeviceLog.Add(new DeviceLog()
                    {
                        DeviceID = device.ID,
                        LogDate = item.Created,
                        Message = item.Message,
                        StackTrace = item.StackTrace,
                        ErrorLocation = item.ErrorLocation
                    });
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