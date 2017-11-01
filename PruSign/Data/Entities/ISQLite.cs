using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.Entities
{
    public interface ISQLite
    {
        SQLite.SQLiteAsyncConnection GetConnectionAsync();
        SQLite.SQLiteConnection GetConnection();
    }
}
