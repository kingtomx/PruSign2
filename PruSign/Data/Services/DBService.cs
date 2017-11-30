using SQLite;
using PruSign.Data.Entities;
using PruSign.Data.Interfaces;

namespace PruSign.Data.Services
{
	public class DBService : IDBService
    {
        public SQLiteAsyncConnection ConnectionAsync { get; set; }
        public SQLiteConnection Connection { get; set; }
        public ISQLite _sqlService { get; set; }

        public DBService(ISQLite sqlService)
        {
            _sqlService = sqlService;
            ConnectionAsync = _sqlService.GetConnectionAsync();
            ConnectionAsync.CreateTableAsync<Signature>();
            ConnectionAsync.CreateTableAsync<UserCredentials>();
            ConnectionAsync.CreateTableAsync<LogEntry>();

            Connection = _sqlService.GetConnection();
            Connection.CreateTable<Signature>();
            Connection.CreateTable<UserCredentials>();
            Connection.CreateTable<LogEntry>();
        }
    }
}
