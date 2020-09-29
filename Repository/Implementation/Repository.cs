using BAL.Abstraction;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
namespace BAL.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext db { get; set; }

        public void Add(TEntity model)
        {
            db.Set<TEntity>().Add(model);
        }

        public void Delete(TEntity model)
        {
            db.Set<TEntity>().Remove(model);
        }

        public void DeleteById(object Id)
        {
            TEntity entity = db.Set<TEntity>().Find(Id);
            this.Delete(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return db.Set<TEntity>().ToList();
        }

        public TEntity GetById(object Id)
        {
            return db.Set<TEntity>().Find(Id);
        }

        public IEnumerable<TEntity> GetCity(int sid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetCountry()
        {
            return db.Set<TEntity>().ToList();
        }

        List<State> IRepository<TEntity>.GetState(int cid)
        {
            return db.Set<State>().Where(x => x.CountryId == cid).ToList();
        }

        public void Modify(TEntity model)
        {
            db.Entry<TEntity>(model).State = EntityState.Modified;
        }

        List<City> IRepository<TEntity>.GetCity(int sid)
        {
            return db.Set<City>().Where(x => x.StateId == sid).ToList();
        }
    }
}
