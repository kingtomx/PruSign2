using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.ViewModels
{
    public class DeviceLogViewModel
    {
        public string Device { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public DeviceLogDetailsViewModel[] Details{ get; set; }
    }
}