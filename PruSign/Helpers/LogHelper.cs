using PruSign.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Helpers
{
    public static class LogHelper
    {
        public static async void Log(Exception exception)
        {
            try
            {
                PruSignDatabase db = new PruSignDatabase();
                var serviceLog = new ServiceAsync<LogEntry>(db);
                await serviceLog.Add(new LogEntry()
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
