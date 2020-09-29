using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
using DomainModels.Models;
using BAL.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using DomainModels.Helper;

namespace BAL.Implementation
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public DatabaseContext context
        {
            get
            {
                return db as DatabaseContext;
            }
        }

        public CategoryRepository(DbContext db)
        {
            this.db = db;
        }

        public void SaveExcelData(string pathName, string sheetName, DataTable table)
        {
            HSSFWorkbook hssfwb = new HSSFWorkbook();
            var sheet = hssfwb.CreateSheet(sheetName);
            var columns = new List<string>();

            foreach (DataColumn col in table.Columns)
                columns.Add(col.ColumnName);

            var headerRow = sheet.CreateRow(0);
            var headerStyle = hssfwb.CreateCellStyle(); //Formatting
            var headerFont = hssfwb.CreateFont();
            headerFont.IsBold = true;
            headerStyle.SetFont(headerFont);

            for (int j = 0; j < table.Columns.Count; j++)
            {
                var cellValue = table.Columns[j].ColumnName;
                var cell = headerRow.CreateCell(j);
                cell.CellStyle = headerStyle;
                SetCellValue(cell, cellValue);
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = sheet.CreateRow(i + 1); //+1 header

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    var cellValue = table.Rows[i][j];
                    var cell = row.CreateCell(j);
                    SetCellValue(cell, cellValue);
                }
            }

            using (var fs = new FileStream(pathName, FileMode.Create))
            {
                hssfwb.Write(fs);
            }
        }

        private void SetCellValue(ICell cell, object cellValue)
        {
            if (cellValue == null)
                cell.SetCellValue((string)null);

            var numberTypes = new Type[] { typeof(double), typeof(int), typeof(float), typeof(decimal) };
            var dateTypes = new Type[] { typeof(DateTime), typeof(DateTimeOffset) };
            var boolTypes = new Type[] { typeof(bool) };

            var type = cellValue.GetType();

            if (numberTypes.Contains(type))
                cell.SetCellValue(cellValue.ChangeType<double>());
            else if (dateTypes.Contains(type))
                cell.SetCellValue(cellValue.ChangeType<DateTime>());
            else if (dateTypes.Contains(type))
                cell.SetCellValue(cellValue.ChangeType<bool>());
            else
                cell.SetCellValue(cellValue.ChangeType<string>());
        }
    }
}
