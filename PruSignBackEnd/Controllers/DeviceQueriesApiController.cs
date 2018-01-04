using Newtonsoft.Json;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.Helpers;
using PruSignBackEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PruSignBackEnd.Controllers
{
    [RoutePrefix("api")]
    public class DeviceQueriesApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<Answer> serviceDeviceAnswers;

        public DeviceQueriesApiController()
        {
            serviceDeviceAnswers = new Service<Answer>(db);
        }

        [Route("devicequeries/")]
        public async Task<HttpResponseMessage> Get(string imei)
        {
            try
            {
                var answers = serviceDeviceAnswers.GetAll().Where(answer => answer.Device.Imei.Equals(imei));
                var answersList = await answers.ToListAsync();
                if (!answersList.Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                else
                {
                    var formattedAnswers = new List<DeviceQueryViewModel>();
                    foreach (var item in answersList)
                    {
                        formattedAnswers.Add(new DeviceQueryViewModel()
                        {
                            Created = item.Created,
                            QuestionCode = item.Question.Code,
                            QuestionDescription = item.Question.Description,
                            Status = item.Status == 0 ? "Waiting for Answer" : "Done",
                            Response = item.Data
                        });
                    }

                    var resp = JsonConvert.SerializeObject(formattedAnswers);
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(resp, Encoding.UTF8, "application/json")
                    };

                }
            }
            catch (Exception ex)
            {
                SystemLogHelper.LogNewError(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("devicequeries/")]
        public HttpResponseMessage Post(Answer answer)
        {
            try
            {
                serviceDeviceAnswers.Add(answer);
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