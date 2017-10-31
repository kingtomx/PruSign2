using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Helpers
{
    public static class SystemLogHelper
    {

        public static void LogNewError(Exception ex) {

            var db = new PruSignContext();
            var serviceSystemLog = new Service<SystemLog>(db);

            var systemLog = new SystemLog()
            {
                StackTrace = ex.Message
            };
            serviceSystemLog.Add(systemLog);
            db.SaveChanges();
        }
    }
}