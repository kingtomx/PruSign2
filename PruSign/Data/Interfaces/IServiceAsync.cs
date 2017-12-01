using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PruSign.Data.Entities;
using SQLite;

namespace PruSign.Data.Interfaces
{
    public interface IServiceAsync<T> where T: class, IEntity, new()
    {
        AsyncTableQuery<T> GetAll();
        Task<T> Get(int id);
        Task Delete(T entity);
        Task Add(T entity);
        Task Update(T entity);
        Task DeleteAll(string table);
    }
}
