﻿using System;
using SQLite;
using PruSign.Data.Entities;

namespace PruSign.Data
{
	public class SignatureItem : IEntity
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string SignatureObject { get; set; }

        public string CustomerName { get; set; }

		public string DocumentId { get; set; }

		public string DNI { get; set; }

		public string AppId { get; set; }

		public bool Sent { get; set; }

		public string SentFormattedDate { get; set; }

		public string Miscelanea { get; set; }
    }
}
