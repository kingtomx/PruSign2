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
        private readonly IServiceAsync<LogEntry> _serviceLogEntry;
        private readonly IServiceAsync<UserCredentials> _serviceUserCredentials;

        public DeviceLogService(IServiceAsync<LogEntry> serviceLogEntry, IServiceAsync<UserCredentials> serviceUserCredentials)
        {
            _serviceLogEntry = serviceLogEntry;
            _serviceUserCredentials = serviceUserCredentials;
        }

        public async Task<HttpResponseMessage> SendDeviceLogs()
        {
            try
            {
                var user = await _serviceUserCredentials.GetAll().FirstOrDefaultAsync();
                var logs = await _serviceLogEntry.GetAll().Where(l => !l.Sent).ToListAsync();

                if (logs.Count > 0)
                {
                    var client = new RestClient(Constants.BACKEND_HOST_NAME);
                    var request = new RestRequest("api/devicelog", Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    var jsonBody = new
                    {
                        Device = App.GetImei(),
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
                            await _serviceLogEntry.Update(item);
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

                await _serviceLogEntry.Add(new LogEntry()
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
                var logs = await _serviceLogEntry.GetAll().Where(l => l.Sent).ToListAsync();

                foreach (var l in logs)
                {
                    if ((DateTime.Now - l.SentDate).TotalDays >= 30)
                    {
                        await _serviceLogEntry.Delete(l);
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
