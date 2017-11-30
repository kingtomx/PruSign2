using PruSign.Data.Entities;
using SQLite;
using System;
using PruSign.Data.Interfaces;

namespace PruSign.Data
{
    public class Service<T> where T : class, IEntity, new()
    {
        private SQLiteConnection _connection;
        private readonly IDBService _db;

        public Service(IDBService db)
        {
            _db = db;
            _connection = _db.Connection;
        }

        public TableQuery<T> GetAll()
        {

            return (from t in _connection.Table<T>()
                    select t);
        }

        public T Get(int id)
        {
            return _connection.Table<T>().FirstOrDefault(t => t.ID == id);
        }

        public void Delete(int id)
        {
            _connection.Delete<T>(id);
        }

        public void Add(T entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            _connection.Insert(entity);
        }

        public void Update(T entity)
        {
            entity.Updated = DateTime.Now;

            _connection.Update(entity);
        }
    }
}
