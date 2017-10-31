using PruSignBackEnd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.Data.DB
{
    public class Service<T> where T : class, IEntity, new()
    {
        private PruSignContext db = new PruSignContext();

        public Service(PruSignContext db)
        {
            this.db = db;
        }

        public IQueryable<T> GetAll()
        {
            return db.Set<T>();
        }

        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            db.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            var existingEntity = this.Get(entity.ID);
            if (existingEntity != null)
            {
                entity.Updated = DateTime.Now;
                db.Entry(existingEntity).CurrentValues.SetValues(entity);
                db.SaveChanges();
            }
        }

        public void Detele(T entity)
        {
            db.Set<T>().Remove(entity);
            db.SaveChanges();
        }


    }
}