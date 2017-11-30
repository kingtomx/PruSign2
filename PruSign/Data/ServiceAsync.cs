using PruSign.Data.Entities;
using PruSign.Data.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PruSign.Data
{
    public class ServiceAsync<T> where T : class, IEntity, new()
    {
        private SQLiteAsyncConnection _asyncConnection;
        private IDBService _db;

        public ServiceAsync(IDBService db)
        {
            _db = db;
            _asyncConnection = _db.ConnectionAsync;
        }

        public AsyncTableQuery<T> GetAll()
        {
            return _asyncConnection.Table<T>();
        }

        public Task<T> Get(int id)
        {
            return _asyncConnection.Table<T>().Where(t => t.ID == id).FirstOrDefaultAsync();
        }

        public async Task Delete(T entity)
        {
            await _asyncConnection.DeleteAsync(entity);
        }

        public async Task Add(T entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            await _asyncConnection.InsertAsync(entity);
        }

        public async Task Update(T entity)
        {
            entity.Updated = DateTime.Now;

            await _asyncConnection.UpdateAsync(entity);
        }

        public async Task DeleteAll(string table)
        {
            await _asyncConnection.ExecuteAsync("DELETE FROM " + table);
        }
    }
}
