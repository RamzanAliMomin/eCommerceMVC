﻿using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
using DomainModels.Models;
using BAL.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace BAL.Implementation
{
    public class CityRepository : Repository<City> ,ICityRepository
    {
        private DatabaseContext context
        {
            get
            {
                return db as DatabaseContext;
            }
        }

        public CityRepository(DbContext db)
        {
            this.db = db;
        }
       
    }
}
