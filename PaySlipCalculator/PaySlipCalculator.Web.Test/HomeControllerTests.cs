using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using PaySlipCalculator.Core.Services;
using PaySlipCalculator.Web.Controllers;
using System.IO;
using System.Threading.Tasks;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using PaySlipCalculator.Core.Models;
using Xunit;

namespace PaySlipCalculator.Web.Test
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var salaryInfoService = new SalaryInfoService(new TaxingCalculator());
            _homeController = new HomeController(salaryInfoService);
        }

        [Fact]
        public async Task RawFileUploadTestWithNormalFile()
        {
            var file = GetFormFile();

            //Action
            var result = await _homeController.RawFileUpload(file);

            //Assert 
            Assert.IsType<JsonResult>(result);
        }


        [Fact]
        public async Task RawFileUploadTestWithEmptyFile()
        {
            var file = GetEmptyFormFile();

            //Action
            var result = await _homeController.RawFileUpload(file);

            //Assert 
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task GetPaySlipFileTestWithNormalFile()
        {
            await _homeController.RawFileUpload(GetFormFile());

            var result = _homeController.GetPaySlipFile("csv");
            Assert.IsType<FileContentResult>(result);

            result = _homeController.GetPaySlipFile("xls");
            Assert.IsType<FileContentResult>(result);

            result = _homeController.GetPaySlipFile("xlsx");
            Assert.IsType<FileContentResult>(result);

            result = _homeController.GetPaySlipFile("unknown");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetPaySlipFileTestWithEmptyFile()
        {
            await _homeController.RawFileUpload(GetEmptyFormFile());

            var result = _homeController.GetPaySlipFile("csv");
            Assert.IsType<FileContentResult>(result);

            result = _homeController.GetPaySlipFile("xls");
            Assert.IsType<FileContentResult>(result);

            result = _homeController.GetPaySlipFile("xlsx");
            Assert.IsType<FileContentResult>(result);

            result = _homeController.GetPaySlipFile("unknown");
            Assert.IsType<BadRequestObjectResult>(result);
        }


        private static IFormFile GetFormFile()
        {
            //Arrange
            var content = @"first name, last name, annual salary, super rate (%), payment start date
                            David,Rudd,60050,9%,01 March - 31 March
                            Ryan,Chen,120000,10%,01 March - 31 March
                            ";
            var fileName = "sample.csv";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            IFormFile file = new FormFile(ms, 0, ms.Length, "Data", fileName);
            return file;
        }

        private static IFormFile GetEmptyFormFile()
        {
            //Arrange
            var content = string.Empty;
            var fileName = "sample.csv";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            IFormFile file = new FormFile(ms, 0, ms.Length, "Data", fileName);
            return file;
        }
    }
}