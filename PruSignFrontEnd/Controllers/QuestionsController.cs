using Newtonsoft.Json;
using PruSignBackEnd.Data.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace PruSignFrontEnd.Controllers
{
    public class QuestionsController : Controller
    {
        // GET: Questions
        [Route("questions/", Name = "questionList")]
        public async Task<ActionResult> Index()
        {
            try
            {
                var result = new List<Question>();
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/questions/all", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ErrorMessage);
                }

                result = JsonConvert.DeserializeObject<List<Question>>(response.Content);
                return View(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.ErrorMessage = "There was an error trying to retrieve questions";
                return View("~/Views/Signature/Index.cshtml");
            }
        }

        [Route("questions/create", Name = "newQuestion")]
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> CreateQuestion(Question question)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/questions/", Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(question);
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return View("Index");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.ErrorMessage = "There was an error trying to create a new question";
                return View("~/Views/Questions/Create.cshtml");
            }
        }

        [Route("questions/edit/{id}", Name = "editQuestion")]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var result = new Question();
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/questions/getById", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("id", id);
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ErrorMessage);
                }

                result = JsonConvert.DeserializeObject<Question>(response.Content);
                return View(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.ErrorMessage = "There was an error trying to edit a question";
                return View("~/Views/Signature/Index.cshtml");
            }
            return View();
        }

        public async Task<ActionResult> Update(Question updatedQuestion)
        {
            try
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["BackendHostName"]);
                var request = new RestRequest("api/questions/", Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(updatedQuestion);
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return View("Index");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.ErrorMessage = "There was an error trying to edit a question";
                return View("~/Views/Questions/Edit.cshtml");
            }
        }

    }
}