using System;
using SQLite;

namespace PruSign
{
	public class SignatureItem
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string SignatureObject { get; set; }

        public string CustomerName { get; set; }

		public string DocumentId { get; set; }

		public string DNI { get; set; }

		public string AppId { get; set; }

		public long CreationTimeStamp { get; set; }

		public bool Sent { get; set; }

		public long SentTimeStamp { get; set; }

		public string Miscelanea { get; set; }

	}
}
