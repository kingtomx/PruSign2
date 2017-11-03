using PruSign.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PruSign.Helpers
{
    public static class LogHelper
    {
        public static async void Log(Exception exception)
        {
            try
            {
                var st = new StackTrace(exception, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                // Get the file name from the stack frame
                var filename = frame.GetFileName();

                var errorLocation = $"File: {filename}\tLine: {line}";

                PruSignDatabase db = new PruSignDatabase();
                var serviceLog = new ServiceAsync<LogEntry>(db);
                await serviceLog.Add(new LogEntry()
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    ErrorLocation = errorLocation
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
