using PruSignBackEnd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.ViewModels
{
    public class DeviceLogViewModel
    {
        public string Device { get; set; }
        public List<DeviceLogEntriesViewModel> Entries { get; set; }
    }
}