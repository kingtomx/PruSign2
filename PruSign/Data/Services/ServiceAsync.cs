using PruSign.Data.Entities;
using PruSign.Data.Interfaces;
using SQLite;
using System;
using System.Threading.Tasks;

namespace PruSign.Data.Services
{
    public class ServiceAsync<T> : IServiceAsync<T> where T : class, IEntity, new()
    {

        private SQLiteAsyncConnection _asyncConnection { get; }
        private IDBService _db { get; }

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
