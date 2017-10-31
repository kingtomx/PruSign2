using PruSign.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Helpers
{
    public static class LogHelper
    {
        public static async void Log(Exception ex)
        {
            PruSignDatabase db = new PruSignDatabase();
            var serviceLog = new ServiceAsync<LogEntry>(db);
            await serviceLog.Add(new LogEntry()
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
}
