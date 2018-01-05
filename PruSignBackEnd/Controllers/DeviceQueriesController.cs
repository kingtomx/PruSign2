using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;

namespace PruSignBackEnd.Controllers
{
    public class DeviceQueriesController : Controller
    {
        private readonly PruSignContext _db = new PruSignContext();
        private readonly Service<Answer> _serviceDeviceAnswers;

        public DeviceQueriesController()
        {
            _serviceDeviceAnswers = new Service<Answer>(_db);
        }


    }
}