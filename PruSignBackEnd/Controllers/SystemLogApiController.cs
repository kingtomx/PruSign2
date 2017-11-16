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
    [RoutePrefix("api")]
    public class SystemLogApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<SystemLog> serviceLog;

        public SystemLogApiController()
        {
            serviceLog = new Service<SystemLog>(db);
        }

        [Route("log/all")]
        public HttpResponseMessage Get()
        {
            try
            {
                var logs = serviceLog.GetAll().OrderByDescending(l => l.Created).Take(50);
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

        [Route("log/search")]
        public HttpResponseMessage Get(string searchText)
        {
            try
            {
                var logs = serviceLog.GetAll().Where(l => l.StackTrace.Contains(searchText)).OrderByDescending(l => l.Created)
                    .Take(50);
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
    }
}