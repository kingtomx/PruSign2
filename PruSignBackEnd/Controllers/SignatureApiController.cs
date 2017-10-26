using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Collections;

namespace PruSignBackEnd
{
	public class SignatureApiController : ApiController
	{

        public PruSignBackEnd.PruSign.Object.Signature[] get( string customerid, 
                                                              string documentid,
                                                              string applicationid) {
                PruSignBackEnd.PruSign.Object.Signature[] response = { };
                using (var context = new DB.PruSignEntities())
                {
                    string sqlSearch = "select * from Signature where customerid=@customerid and documentid=@documentid and applicationid=@applicationid";
                    object[] parameters = {
                                            new SqlParameter("@customerid", customerid),
                                            new SqlParameter("@documentid", documentid),
                                            new SqlParameter("@applicationid", applicationid),
                                          };
                    var results = context.Signature.SqlQuery(sqlSearch, parameters);

                    foreach (var s in results) {
                        PruSignBackEnd.PruSign.Object.Signature aux = new PruSign.Object.Signature();
                        aux.applicationId = s.applicationid;
                        aux.customerId = s.customerid;
                        aux.customerName = s.customername;
                        aux.datetime = s.datetime.ToString();
                        aux.documentId = s.documentid;
                        aux.hash = s.hash;
                        aux.image = s.image;
                        Array.Resize(ref response, response.Length+1);
                        response[response.Length - 1] = aux;
                    }


                }
                return response;

        }

        public string get(int id)
		{
			return "value";
		}

		public HttpResponseMessage Post(PruSignBackEnd.PruSign.Object.Signature signature)
		{

			try
			{

                var context = new DB.PruSignEntities();
                context.Signature.Add(new DB.Signature() {
                    applicationid = signature.applicationId,
                    customerid = signature.customerId,
                    customername = signature.customerName,
                    datetime = System.DateTime.Parse(signature.datetime),
                    documentid = signature.documentId,
                    hash = signature.hash,
                    image = signature.image
                });
                context.SaveChanges();

				return new HttpResponseMessage(HttpStatusCode.OK);

			}
			catch (Exception)
			{
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}


		}



	}
}
