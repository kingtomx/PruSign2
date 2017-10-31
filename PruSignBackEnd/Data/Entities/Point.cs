using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public partial class Point : IEntity
    {
        //Interface Implementation
        [Key]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public bool IsEmpty { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}