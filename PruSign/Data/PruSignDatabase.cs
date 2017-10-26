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

        public PruSignDatabase()
		{
            ConnectionAsync = DependencyService.Get<ISQLite>().GetConnectionAsync();
            ConnectionAsync.CreateTableAsync<SignatureItem>();
            ConnectionAsync.CreateTableAsync<UserCredentials>();
        }
	}
}
