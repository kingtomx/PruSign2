using Newtonsoft.Json;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
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
    public class QuestionsApiController : ApiController
    {
        private PruSignContext db = new PruSignContext();
        private Service<Question> serviceQuestions;

        public QuestionsApiController()
        {
            serviceQuestions = new Service<Question>(db);
        }

        [HttpGet]
        [Route("questions/all")]
        public HttpResponseMessage Get()
        {
            var questions = serviceQuestions.GetAll();
            if (!questions.Any())
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var resp = JsonConvert.SerializeObject(questions);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(resp, Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [Route("questions/getbyid")]
        public HttpResponseMessage GetById(int id)
        {
            var questions = serviceQuestions.GetAll().FirstOrDefault(question => question.ID == id);
            if (questions == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var resp = JsonConvert.SerializeObject(questions);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(resp, Encoding.UTF8, "application/json")
            };
        }


        [HttpPost]
        [Route("questions/")]
        public HttpResponseMessage Post(Question question)
        {
            try
            {
                serviceQuestions.Add(new Question()
                {
                    Code = question.Code,
                    Description = question.Description
                });
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Helpers.SystemLogHelper.LogNewError(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("questions/")]
        public HttpResponseMessage Put(Question question)
        {
            try
            {
                serviceQuestions.Update(question);
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Helpers.SystemLogHelper.LogNewError(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}