using DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity model);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetCountry();
        List<State> GetState(int cid);
        List<City> GetCity(int sid);
        TEntity GetById(object Id);
        void Modify(TEntity model);
        void Delete(TEntity model);
        void DeleteById(object Id);
    }
}
