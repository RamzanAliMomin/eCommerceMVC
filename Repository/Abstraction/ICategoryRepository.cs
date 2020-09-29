using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
using DomainModels.Models;

namespace BAL.Abstraction
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void SaveExcelData(string pathName, string sheetName, DataTable table);

    }
}
