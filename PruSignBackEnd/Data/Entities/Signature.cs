using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.Entities
{
	public class Signature : IEntity
	{
        //Interface Implementation
        [Key]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

		public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string DocumentId { get; set; }
        public string ApplicationId { get; set; }
        public string SignatureObject { get; set; }
    }
}
