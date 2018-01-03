using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
    public class Question : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}