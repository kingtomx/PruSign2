using System;
using System.Collections.Generic;
using System.Text;
using PruSign.Data.Entities;
using SQLite;

namespace PruSign.Data.Interfaces
{
    public interface IDBService
    {
        SQLiteAsyncConnection ConnectionAsync { get; set; }
        SQLiteConnection Connection { get; set; }
        ISQLite _sqlService { get; set; }
    }
}
