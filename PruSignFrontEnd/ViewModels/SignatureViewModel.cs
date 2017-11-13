using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PruSignFrontEnd.ViewModels
{
    public class SignatureViewModel
    {
        public string Created { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string DocumentId { get; set; }
        public string ApplicationId { get; set; }
        public string SignatureObject { get; set; }
        public string Image { get; set; }
    }
}