using System;
using SQLite;
using PruSign.Data.Entities;

namespace PruSign.Data.Entities
{
	public class Signature : IEntity
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string SignatureObject { get; set; }

        public string CustomerName { get; set; }

		public string DocumentId { get; set; }

		public string CustomerId { get; set; }

		public string ApplicationId { get; set; }

		public bool Sent { get; set; }

		public DateTime SentDate { get; set; }
    }
}
