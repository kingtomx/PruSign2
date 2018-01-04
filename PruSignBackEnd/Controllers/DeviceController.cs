using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.Helpers;

namespace PruSignBackEnd.Controllers
{
    [RoutePrefix("api")]
    public class DeviceController : ApiController
    {
        private readonly Service<Device> _serviceDevice;
        private readonly PruSignContext _db = new PruSignContext();

        public DeviceController()
        {
            _serviceDevice = new Service<Device>(_db);
        }

        // GET: Device
        [Route("device/")]
        public async Task<HttpResponseMessage> Get(string searchText)
        {
            try
            {
                var devices = _serviceDevice.GetAll();
                if (!String.IsNullOrEmpty(searchText))
                {
                    devices = devices.Where(d => d.User.Contains(searchText));
                }

                if (!devices.Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                var deviceList = await devices.ToListAsync();
                var resp = JsonConvert.SerializeObject(deviceList);
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

        [Route("device/getbyimei")]
        public async Task<HttpResponseMessage> GetById(string imei)
        {
            try
            {
                var device = _serviceDevice.GetAll().FirstOrDefault(d => d.Imei.Equals(imei));

                if (device == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                var resp = JsonConvert.SerializeObject(device);
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

    }
}