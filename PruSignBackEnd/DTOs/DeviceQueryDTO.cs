using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.DTOs
{
    public class DeviceQueryDTO
    {
        public int QueryId { get; set; }
        public string QueryCode { get; set; }
        public string QueryDescription { get; set; }
    }
}