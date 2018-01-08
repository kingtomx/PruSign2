using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.Entities
{
    public class Query : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int QueryId { get; set; }
        public string QueryCode { get; set; }
        public string QueryDescription { get; set; }
        public string Response { get; set; }
        public bool Sent { get; set; }
    }
}
