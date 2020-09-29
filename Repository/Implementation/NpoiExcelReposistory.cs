using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text;
using System.Threading.Tasks;
using BAL.Abstraction;
using DomainModels.Entities;
using DomainModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.VisualBasic;
using System.IO;
using DomainModels.Helper;

namespace BAL.Implementation
{
    public class NpoiExcelReposistory :INpoiExcelReposistory
    {

        public DataTable ConvertToDataTable<T>(List<T> items)
        {
            return items.ToDataTable();
        }

        public DataTable GetExcelData(string pathName, string sheetName)
        {
            var dt = new DataTable();

            using (var fs = new FileStream(pathName, FileMode.Open))
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(fs);
                var sheet = hssfwb.GetSheet(sheetName);
                sheet.DisplayRowColHeadings = true;

                var headerRow = sheet.GetRow(sheet.FirstRowNum).Cells.Select(getValue).Select(x => new DataColumn(x?.ToString())).ToArray();
                dt.Columns.AddRange(headerRow);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var values = new object[headerRow.Length];
                    foreach (var cell in row.Cells)
                    {
                        values[cell.ColumnIndex] = getValue(cell);
                    }
                    dt.Rows.Add(values);
                }
            }

            return dt;
        }

        private object getValue(ICell cell)
        {
            switch (cell.CellType)
            {
               
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    return cell.StringCellValue;
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    return cell.StringCellValue;
            }

            return null;
        }

        public Dictionary<string, List<string>> GetExcelWorksheets(string pathName)
        {
            var result = new Dictionary<string, List<string>>();

            using (var fs = new FileStream(pathName, FileMode.Open))
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(fs);

                for (int i = 0; i < hssfwb.NumberOfSheets; i++)
                {
                    var sheet = hssfwb.GetSheetAt(i);
                    sheet.DisplayRowColHeadings = true;
                    var headerRow = getHeaderRow(sheet).Select(x => x.ColumnName).ToList();
                    result.Add(sheet.SheetName, headerRow);
                }
            }

            return result;
        }

        private DataColumn[] getHeaderRow(ISheet sheet)
        {
            return sheet.GetRow(sheet.FirstRowNum).Cells.Select(getValue).Select(x => new DataColumn(x?.ToString())).ToArray();
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
