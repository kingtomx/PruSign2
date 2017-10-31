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
    public class SignatureApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<Signature> serviceSignature;


        public SignatureApiController()
        {
            serviceSignature = new Service<Signature>(db);
        }

        [Route("api/signature/")]
        public HttpResponseMessage get(string customerid, string documentid, string applicationid)
        {
            try
            {
                var signatures = serviceSignature.GetAll().Where(signature => signature.CustomerId.Equals(customerid) &&
                                                                  signature.DocumentId.Equals(documentid) &&
                                                                  signature.ApplicationId.Equals(applicationid));

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

        [Route("api/signature/")]
        public HttpResponseMessage Post(Signature signature)
        {
            try
            {
                var newSignature = new Signature()
                {
                    ApplicationId = signature.ApplicationId,
                    CustomerId = signature.CustomerId,
                    CustomerName = signature.CustomerName,
                    DocumentId = signature.DocumentId,
                    Hash = signature.Hash,
                    Image = signature.Image
                };

                serviceSignature.Add(newSignature);
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
