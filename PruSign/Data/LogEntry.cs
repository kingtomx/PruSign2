﻿using PruSign.Data.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data
{
    class LogEntry : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
