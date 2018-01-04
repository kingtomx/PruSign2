using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public class Device : IEntity
    {
        [Key]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Imei { get; set; }
        public string User { get; set; }
    }
}