using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PruSign.Data.Entities;
using PruSign.Data.Interfaces;
using RestSharp;

namespace PruSign.Data.Services
{
    public class DeviceLogService : IDeviceLogService
    {
        private IDBService _db;

        public DeviceLogService(IDBService db)
        {
            _db = db;
        }

        public async Task<HttpResponseMessage> SendDeviceLogs()
        {
            try
            {
                var serviceLogs = new ServiceAsync<LogEntry>(_db);
                var serviceUserCredentials = new ServiceAsync<UserCredentials>(_db);
                var user = await serviceUserCredentials.GetAll().FirstOrDefaultAsync();
                var logs = await serviceLogs.GetAll().Where(l => !l.Sent).ToListAsync();

                if (logs.Count > 0)
                {
                    var client = new RestClient(Constants.BACKEND_HOST_NAME);
                    var request = new RestRequest("api/devicelog", Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    var jsonBody = new
                    {
                        Device = "test",
                        User = user.Username,
                        Entries = logs
                    };
                    request.AddJsonBody(jsonBody);

                    var response = await client.ExecuteTaskAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in logs)
                        {
                            item.Sent = true;
                            item.SentDate = DateTime.Now;
                            await serviceLogs.Update(item);
                        }
                    }
                    else
                    {
                        // If something went wrong, we throw a new exception to return InternalServerError.
                        // In that way, the user will be notified about the problem and the error details will be handled
                        // By the log helper.
                        throw new Exception(response.ErrorMessage);
                    }

                }
                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                Log(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async void Log(Exception exception)
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

                var serviceLog = new ServiceAsync<LogEntry>(_db);
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

        public async void CleanOldLogs()
        {
            try
            {
                var serviceLogs = new ServiceAsync<LogEntry>(_db);
                var logs = await serviceLogs.GetAll().Where(l => l.Sent).ToListAsync();

                foreach (var l in logs)
                {
                    if ((DateTime.Now - l.SentDate).TotalDays >= 30)
                    {
                        await serviceLogs.Delete(l);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
    }
}
