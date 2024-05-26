using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Helper
{
    public static class ExcelHelper
    {

        public static DataTable GetDataTimeReport(string start, string end)
        {
            // Fetch data from your data source (e.g., database, collection)
            DataTable data = new DataTable();
            // Populate your DataTable with data
            // For demonstration, I'll just create a dummy DataTable
            data.Columns.Add("start", typeof(string));
            data.Columns.Add("end", typeof(string));
            data.Rows.Add(start, end);

            return data;
        }
        public static void ExportToExcel(string templatePath, string outputPath, DataTable data, DataTable TimeReport, int stRow, int stColumn)
        {
            // Load the template Excel file
            FileInfo templateFile = new FileInfo(templatePath);
            using (ExcelPackage package = new ExcelPackage(templateFile))
            {
                // Get the first worksheet in the template
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];


                // Insert your data into the worksheet
                int startRow = stRow; // Assuming your data starts from the second row
                int startColumn = stColumn; // Assuming your data starts from the first column

                for (int row = 0; row < data.Rows.Count; row++)
                {
                    for (int col = 0; col < data.Columns.Count; col++)
                    {
                        worksheet.Cells[startRow + row, startColumn + col].Value = data.Rows[row][col];


                        // Set horizontal and vertical alignment to Center
                        worksheet.Cells[startRow + row, startColumn + col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[startRow + row, startColumn + col].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        // Set border for all sides
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        // Set border color
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Top.Color.SetColor(Color.Black);
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Bottom.Color.SetColor(Color.Black);
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Left.Color.SetColor(Color.Black);
                        worksheet.Cells[startRow + row, startColumn + col].Style.Border.Right.Color.SetColor(Color.Black);


                    }
                    
                }

                startRow = 1;
                startColumn = 1;

                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];

                for (int row = 0; row < TimeReport.Rows.Count; row++)
                {
                    for (int col = 0; col < TimeReport.Columns.Count; col++)
                    {
                        worksheet1.Cells[startRow + row, startColumn + col].Value = TimeReport.Rows[row][col];
                    }
                }

                // Save the modified Excel file
                FileInfo outputFile = new FileInfo(outputPath);
                package.SaveAs(outputFile);

                Process.Start(outputPath);
            }
        }
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
