using PruSign.Data.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data
{
    public class LogEntry : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string ErrorLocation { get; set; }

        public string FormattedDate
        {
            get => Created.ToString("yyyy-MM-dd HH:mm:ss");
            set { }
        }

        public bool Sent { get; set; }
    }
}
