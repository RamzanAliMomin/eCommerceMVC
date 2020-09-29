using BAL.Abstraction;
using ApplicationCore;
using DomainModels.Entities;
using DomainModels.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Implementation
{
    public class AuthenticateRepository : Repository<User>, IAuthenticateRepository
    {
        private DatabaseContext context
        {
            get
            {
                return this.db as DatabaseContext;
            }
        }
        public AuthenticateRepository(DbContext _db)
        {
            this.db = _db;
        }
    }
}
