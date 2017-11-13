using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Helpers
{
    public static class SystemLogHelper
    {

        public static void LogNewError(Exception ex) {

            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            // Get the file name from the stack frame
            var filename = frame.GetFileName();

            var errorLocation = $"File: {filename}\tLine: {line}";

            var db = new PruSignContext();
            var serviceSystemLog = new Service<SystemLog>(db);

            var systemLog = new SystemLog()
            {
                StackTrace = ex.ToString(),
                ErrorLocation = errorLocation
            };
            serviceSystemLog.Add(systemLog);
            db.SaveChanges();
        }
    }
}