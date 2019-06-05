using GemBox.Spreadsheet;
using PaySlipCalculator.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PaySlipCalculator.Core.ExcelHelper
{
    public static class ExcelFileHelper
    {
        public static byte[] GetBytes(this ExcelFile workbook, SpreadSheetFileFormat fileFormat)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                switch (fileFormat)
                {
                    case SpreadSheetFileFormat.csv:
                        workbook.Save(ms, SaveOptions.CsvDefault);
                        break;
                    case SpreadSheetFileFormat.xls:
                        workbook.Save(ms, SaveOptions.XlsDefault);
                        break;
                    case SpreadSheetFileFormat.xlsx:
                        workbook.Save(ms, SaveOptions.XlsxDefault);
                        break;
                }

                fileBytes = ms.ToArray();
            }

            return fileBytes;
        }

        public static async Task<List<SalaryDataRawRow>> ExtractDataFromSpreadsheetAsync(IFormFile file)
        {
            var dataRawRows = new List<SalaryDataRawRow>();
            // read in data 
            ExcelFile salaryFile = null;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var fileType = Path.GetExtension(file.FileName).ToLower();

                ms.Position = 0;
                switch (fileType)
                {
                    case ".csv":
                        salaryFile = ExcelFile.Load(ms, LoadOptions.CsvDefault);
                        break;
                    case ".xls":
                        salaryFile = ExcelFile.Load(ms, LoadOptions.XlsDefault);
                        break;
                    case ".xlsx":
                        salaryFile = ExcelFile.Load(ms, LoadOptions.XlsxDefault);
                        break;
                }
            }

            if (salaryFile == null)
                return dataRawRows;

            var dataSheet = salaryFile.Worksheets[0];

            //skip the header row
            foreach (var row in dataSheet.Rows.Skip(1))
            {
                if (row.Cells[0].Value == null || row.Cells[1].Value == null)
                    continue;
                dataRawRows.Add(new SalaryDataRawRow()
                {
                    FirstName = row.Cells[0].Value.ToString(),
                    LastName = row.Cells[1].Value.ToString(),
                    AnnualSalary = row.Cells[2].Value.ToString(),
                    SuperRate = row.Cells[3].Value.ToString(),
                    PaymentDate = row.Cells[4].Value.ToString()
                });
            }


            return dataRawRows;
        }


        public static ExcelFile GenerateExcelFileFromPaySlipDetails(List<PaySlipDetail> paySlipDetails)
        {
            var workbook = new ExcelFile();
            var paySlipWorksheet = workbook.Worksheets.Add("Pay Slip");
            var dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Pay Period", typeof(string));
            dataTable.Columns.Add("Gross Income", typeof(int));
            dataTable.Columns.Add("Income Tax", typeof(int));
            dataTable.Columns.Add("Net Income", typeof(int));
            dataTable.Columns.Add("Super", typeof(int));
            foreach (var row in paySlipDetails)
                dataTable.Rows.Add(new object[]
                    {row.Name, row.PayPeriod, row.GrossIncome, row.IncomeTax, row.NetIncome, row.SuperAnnunation});

            // Insert DataTable to an Excel worksheet.
            paySlipWorksheet.InsertDataTable(dataTable,
                new InsertDataTableOptions
                {
                    ColumnHeaders = true,
                    StartRow = 0
                });

            paySlipWorksheet.Columns["C"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            paySlipWorksheet.Columns["D"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            paySlipWorksheet.Columns["E"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            paySlipWorksheet.Columns["F"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

            return workbook;
        }
    }
}