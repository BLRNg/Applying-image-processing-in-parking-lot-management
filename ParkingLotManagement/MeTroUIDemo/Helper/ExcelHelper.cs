using ClosedXML.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Helper
{
    public static class ExcelHelper
    {
        public static void ExportDataTableToExcel(DataTable dataTable, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cell(1, 1).InsertTable(dataTable);
                workbook.SaveAs(filePath);
            }

            // Open the file after saving
            Process.Start(filePath);
        }
    }
}
