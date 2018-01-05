using PruSignBackEnd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.DTOs
{
    public class SignaturesDTO
    {
        public string User { get; set; }
        public string Imei { get; set; }
        public List<Signature> Signatures { get; set; }
    }
}