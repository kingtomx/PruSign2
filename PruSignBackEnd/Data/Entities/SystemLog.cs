using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public class SystemLog : IEntity
    {
        //Interface Implementation
        [Key]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string StackTrace { get; set; }
        public string ErrorLocation { get; set; }
    }
}