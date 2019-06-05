using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaySlipCalculator.Core.ExcelHelper;
using PaySlipCalculator.Core.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PaySlipCalculator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISalaryInfoService _salaryInfoService;


        public HomeController(ISalaryInfoService salaryInfoService)
        {
            _salaryInfoService = salaryInfoService;
        }

        [HttpPost]
        public async Task<IActionResult> RawFileUpload(IFormFile file)
        {
            if (file == null)
                return BadRequest("No file data!");

            if (!IsRightFileType(file.FileName))
                return BadRequest("Wrong file type!");
            try
            {
                var dataRawRows = await ExcelFileHelper.ExtractDataFromSpreadsheetAsync(file);
                if (dataRawRows.Count < 1)
                    return BadRequest("Erro in reading out data, please check data file format and content.");

                var paySlipDetails = await _salaryInfoService.ImportSalaryInfo(dataRawRows);
                return Json(new {data = paySlipDetails});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }


        [HttpGet]
        public IActionResult GetPaySlipFile(string fileFormat)
        {
            var paySlipDetails = _salaryInfoService.GetPaySlipDetails();
            var workBook = ExcelFileHelper.GenerateExcelFileFromPaySlipDetails(paySlipDetails);

            fileFormat = fileFormat.ToLower();

            switch (fileFormat)
            {
                case "csv":
                    return File(workBook.GetBytes(SpreadSheetFileFormat.csv), "text/csv", "PaySlip.csv");
                case "xls":
                    return File(workBook.GetBytes(SpreadSheetFileFormat.xls), "application/vnd.ms-excel",
                        "PaySlip.xls");
                case "xlsx":
                    return File(workBook.GetBytes(SpreadSheetFileFormat.xlsx),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PaySlip.xlsx");
            }

            return BadRequest("Error in fetching the pay slip file.");
        }

        private bool IsRightFileType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                return false;
            extension = extension.ToLower();
            var acceptableFiles = new string[] {".csv", ".xls", ".xlsx"};
            if (!acceptableFiles.Contains(extension))
                return false;
            return true;
        }
    }
}