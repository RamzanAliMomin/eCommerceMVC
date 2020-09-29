using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Abstraction;
using Microsoft.EntityFrameworkCore;
using ApplicationCore;
using BAL.Implementation;

namespace BAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext db;
        public UnitOfWork(DatabaseContext _db)
        {
            db = _db;
        }
        private IAuthenticateRepository _AuthenticateRepo;
        public IAuthenticateRepository AuthenticateRepo
        {
            get
            {
                if (_AuthenticateRepo == null)
                    _AuthenticateRepo = new AuthenticateRepository(db);

                return _AuthenticateRepo;
            }
        }

        private ICategoryRepository _CategoryRepo;
        public ICategoryRepository CategoryRepo
        {
            get
            {
                if (_CategoryRepo == null)
                    _CategoryRepo = new CategoryRepository(db);

                return _CategoryRepo;
            }
        }
        private IProductRepository _ProductRepo;
        public IProductRepository ProductRepo
        {
            get
            {
                if (_ProductRepo == null)
                    _ProductRepo = new ProductRepository(db);

                return _ProductRepo;
            }
        }
        private IOrderRepository _OrderRepo;
        public IOrderRepository OrderRepo
        {
            get
            {
                if (_OrderRepo == null)
                    _OrderRepo = new OrderRepository(db);

                return _OrderRepo;
            }
        }

        private ICountryRepository _CountryRepo;
        public ICountryRepository CountryRepo
        {
            get
            {
                if (_CountryRepo == null)
                    _CountryRepo = new CountryRepository(db);

                return _CountryRepo;
            }
        }
        private IStateRepository _StateRepo;
        public IStateRepository StateRepo
        {
            get
            {
                if (_StateRepo == null)
                    _StateRepo = new StateRepository(db);

                return _StateRepo;
            }
        }

        private ICityRepository _CityRepo;
        public ICityRepository CityRepo
        {
            get
            {
                if (_CityRepo == null)
                    _CityRepo = new CityRepository(db);

                return _CityRepo;
            }
        }
        private ICompanyRepository _CompanyRepo;
        public ICompanyRepository CompanyRepo
        {
            get
            {
                if (_CompanyRepo == null)
                    _CompanyRepo = new CompanyRepository(db);

                return _CompanyRepo;
            }
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}
