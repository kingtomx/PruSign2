using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignFrontEnd.ViewModels
{
    public class SignatureObjectViewModel
    {
        public byte[] Image;
        public object Points;
        public string Datetime;
        public string CustomerName;
        public string CustomerId;
        public string DocumentId;
        public string ApplicationId;
        public string Hash;
        public string FormattedCreateDate { get; set; }
    }
}