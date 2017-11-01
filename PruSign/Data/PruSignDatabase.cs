using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using PruSign.Data.Entities;

namespace PruSign.Data
{
	public class PruSignDatabase
	{
        public SQLiteAsyncConnection ConnectionAsync;
        public SQLiteConnection Connection;

        public PruSignDatabase()
		{
            ConnectionAsync = DependencyService.Get<ISQLite>().GetConnectionAsync();
            ConnectionAsync.CreateTableAsync<SignatureItem>();
            ConnectionAsync.CreateTableAsync<UserCredentials>();
            ConnectionAsync.CreateTableAsync<LogEntry>();

            Connection = DependencyService.Get<ISQLite>().GetConnection();
            Connection.CreateTable<SignatureItem>();
            Connection.CreateTable<UserCredentials>();
            Connection.CreateTable<LogEntry>();
        }
	}
}
