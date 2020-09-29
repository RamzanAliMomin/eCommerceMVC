using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
using System.Data;

namespace BAL.Abstraction
{
    public interface INpoiExcelReposistory 
    {
        Dictionary<string, List<string>> GetExcelWorksheets(string pathName);
        DataTable GetExcelData(string pathName, string sheetName);
        void SaveExcelData(string pathName, string sheetName, DataTable table);
        DataTable ConvertToDataTable<T>(List<T> items);
    }
}
