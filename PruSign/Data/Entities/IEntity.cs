using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.Entities
{
    public interface IEntity
    {
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
