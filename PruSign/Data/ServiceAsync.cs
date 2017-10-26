using PruSign.Data.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PruSign.Data
{
    public class ServiceAsync<T> where T : class, IEntity, new()
    {
        private SQLiteAsyncConnection _connection;

        public ServiceAsync(PruSignDatabase db)
        {
            _connection = db.ConnectionAsync;
        }

        public AsyncTableQuery<T> GetAll()
        {
            return _connection.Table<T>();
        }

        public Task<T> Get(int id)
        {
            return _connection.Table<T>().Where(t => t.ID == id).FirstOrDefaultAsync();
        }

        public async Task Delete(T entity)
        {
            await _connection.DeleteAsync(entity);
        }

        public async Task Add(T entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            await _connection.InsertAsync(entity);
        }

        public async Task Update(T entity)
        {
            entity.Updated = DateTime.Now;

            await _connection.UpdateAsync(entity);
        }

        public async Task DeleteAll(string table)
        {
            await _connection.ExecuteAsync("DELETE FROM " + table);
        }
    }
}
