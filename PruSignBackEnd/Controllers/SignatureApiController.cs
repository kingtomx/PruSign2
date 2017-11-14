using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Collections;
using PruSignBackEnd.Data.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Helpers;

namespace PruSignBackEnd
{
    [RoutePrefix("api")]
    public class SignatureApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<Signature> serviceSignature;


        public SignatureApiController()
        {
            serviceSignature = new Service<Signature>(db);
        }

        [Route("signature/all")]
        public HttpResponseMessage Get()
        {
            try
            {
                var signatures = serviceSignature.GetAll().OrderByDescending(l => l.Created);
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

        [Route("signature/")]
        public HttpResponseMessage Get(string customerid, string documentid, string applicationid)
        {
            var resp = "[]";

            try
            {
                var signatures = serviceSignature.GetAll().Where(signature => signature.CustomerId.Equals(customerid) &&
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
                var signature = serviceSignature.GetAll().Where(s => s.ID.Equals(id)).FirstOrDefault();

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
                var signatures = serviceSignature.GetAll().Where(signature => signature.CustomerId.Contains(searchText) ||
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
        public HttpResponseMessage Post(List<Signature> signatures)
        {
            try
            {
                foreach (var s in signatures)
                {
                    var newSignature = new Signature()
                    {
                        ApplicationId = s.ApplicationId,
                        CustomerId = s.CustomerId,
                        CustomerName = s.CustomerName,
                        DocumentId = s.DocumentId,
                        SignatureObject = s.SignatureObject,
                    };

                    serviceSignature.Add(newSignature);
                }

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
