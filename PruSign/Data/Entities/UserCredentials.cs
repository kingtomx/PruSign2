using PruSign.Data.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.Entities
{
    public class UserCredentials : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
