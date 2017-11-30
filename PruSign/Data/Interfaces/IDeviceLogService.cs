using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PruSign.Data.Interfaces
{
    public interface IDeviceLogService
    {
        Task<HttpResponseMessage> SendDeviceLogs();
        void Log(Exception exception);
        void CleanOldLogs();
    }
}
