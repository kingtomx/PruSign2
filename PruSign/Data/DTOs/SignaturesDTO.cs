using PruSign.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.DTOs
{
    public class SignaturesDTO
    {
        public string User { get; set; }
        public string Imei { get; set; }
        public List<Signature> Signatures { get; set; }
    }
}
