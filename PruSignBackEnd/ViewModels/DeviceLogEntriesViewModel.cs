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

        public string FormattedDate { get; set; }

    }
}