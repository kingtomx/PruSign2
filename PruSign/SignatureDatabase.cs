using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace PruSign
{
	public class SignatureDatabase
	{
		readonly SQLiteAsyncConnection database;

		public SignatureDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
			database.CreateTableAsync<SignatureItem>().Wait();
		}

		public Task<List<SignatureItem>> GetItemsAsync()
		{
			return database.Table<SignatureItem>().ToListAsync();
		}

		public Task<List<SignatureItem>> GetItemsNotDoneAsync()
		{
			return database.QueryAsync<SignatureItem>("SELECT * FROM [SignatureItem] WHERE [Sent] = 0");
		}

		public Task<SignatureItem> GetItemAsync(int id)
		{
			return database.Table<SignatureItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(SignatureItem item)
		{
			if (item.ID != 0)
			{
				return database.UpdateAsync(item);
			}
			else
			{
				return database.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(SignatureItem item)
		{
			return database.DeleteAsync(item);
		}
	}
}
