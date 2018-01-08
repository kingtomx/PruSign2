using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using PruSignBackEnd.Data.Entities;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.DTOs;
using PruSignBackEnd.Helpers;
using PruSignBackEnd.Services;

namespace PruSignBackEnd.Controllers.Api
{
    [RoutePrefix("api")]
    public class SignatureApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private readonly Service<Signature> _serviceSignature;
        private readonly DeviceQueryService _serviceDeviceQuery;
        private readonly DeviceService _serviceDevice;


        public SignatureApiController()
        {
            _serviceSignature = new Service<Signature>(db);
            _serviceDevice = new DeviceService();
            _serviceDeviceQuery = new DeviceQueryService();
        }

        [Route("signature/all")]
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                var signatures = _serviceSignature.GetAll().OrderByDescending(l => l.Created);
                var signaturesList = await signatures.ToListAsync();
                if (!signaturesList.Any())
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var resp = JsonConvert.SerializeObject(signaturesList);
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

        [Route("signature/")]
        public HttpResponseMessage Get(string customerid, string documentid, string applicationid)
        {
            var resp = "[]";

            try
            {
                var signatures = _serviceSignature.GetAll().Where(signature => signature.CustomerId.Equals(customerid) &&
                                                                  signature.DocumentId.Equals(documentid) &&
                                                                  signature.ApplicationId.Equals(applicationid))
                                                          .OrderByDescending(l => l.Created);

                if (signatures.Any())
                {
                    resp = JsonConvert.SerializeObject(signatures.FirstOrDefault());
                }


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

        [Route("signature/getbyid")]
        public HttpResponseMessage Get(int id)
        {
            var resp = String.Empty;
            try
            {
                var signature = _serviceSignature.GetAll().FirstOrDefault(s => s.ID.Equals(id));

                if (signature != null)
                {
                    resp = JsonConvert.SerializeObject(signature);
                }


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

        [Route("signature/search")]
        public HttpResponseMessage Get(string searchText)
        {
            try
            {
                var signatures = _serviceSignature.GetAll().Where(signature => signature.CustomerId.Contains(searchText) ||
                                                                  signature.DocumentId.Contains(searchText) ||
                                                                  signature.ApplicationId.Contains(searchText) ||
                                                                  signature.CustomerName.Contains(searchText))
                                                          .OrderByDescending(l => l.Created);

                if (!signatures.Any())
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var resp = JsonConvert.SerializeObject(signatures);
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
        [Route("signature/")]
        public async Task<HttpResponseMessage> Post(SignaturesDTO data)
        {
            try
            {
                var device = _serviceDevice.GetDevice(data.Imei);
                if (device == null)
                {
                    _serviceDevice.CreateDevice(new Device()
                    {
                        User = data.User,
                        Imei = data.Imei
                    });
                    device = _serviceDevice.GetDevice(data.Imei);
                }

                foreach (var s in data.Signatures)
                {
                    var newSignature = new Signature()
                    {
                        ApplicationId = s.ApplicationId,
                        CustomerId = s.CustomerId,
                        CustomerName = s.CustomerName,
                        DocumentId = s.DocumentId,
                        SignatureObject = s.SignatureObject,
                        DeviceID = device.ID
                    };

                    _serviceSignature.Add(newSignature);
                }

                db.SaveChanges();

                //Check for queries
                var pendingQueries = await _serviceDeviceQuery.GetPendingQueries(data.Imei);
                if (!pendingQueries.Any()) return new HttpResponseMessage(HttpStatusCode.OK);
                var resp = JsonConvert.SerializeObject(pendingQueries);

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
