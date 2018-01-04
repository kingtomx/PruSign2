using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.ViewModels
{
    public class DeviceLogEntriesViewModel
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public DateTime Created { get; set; }

        public string FormattedDate
        {
            get => Created.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string ErrorLocation { get; set; }

        public string DeviceUser { get; set; }

        public string DeviceImei { get; set; }
    }
}