using AutoFixture;
using GemBox.Spreadsheet;
using PaySlipCalculator.Core.ExcelHelper;
using PaySlipCalculator.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Xunit;

namespace PaySlipCalculator.Core.Test.ExcelHelper
{
    public class ExcelHelperTests
    {
        private ExcelFile _testingFile;
        private ExcelWorksheet _testingSWorksheet;

        public ExcelHelperTests()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            _testingFile = new ExcelFile();
            _testingSWorksheet = _testingFile.Worksheets.Add("Testing Sheet");
        }

        [Fact]
        public void GetBytesTestWithNormalFile()
        {
            var testingFile = ExcelFileHelper.GenerateExcelFileFromPaySlipDetails(GetTestingPaySlipDetails());

            var dataRows = testingFile.Worksheets[0].Rows;
            var fileBytes = testingFile.GetBytes(SpreadSheetFileFormat.csv);

            Assert.True(dataRows.Count == 11);
            Assert.True(fileBytes.Length > 0);
        }

        [Fact]
        public void GetBytesTestWithEmptyFile()
        {
            var fileBytes = _testingFile.GetBytes(SpreadSheetFileFormat.csv);
            var dataRows = _testingFile.Worksheets[0].Rows;

            Assert.True(dataRows.Count == 0);
            Assert.True(fileBytes.Length > 0);
        }
        
        [Fact]
        public async Task ExtractDataFromSpreadsheetAsyncTestWithNormalFile()
        {
            //Arrange
            var content = @"
first name, last name, annual salary, super rate (%), payment start date
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

            //Action
            var dataRows = await ExcelFileHelper.ExtractDataFromSpreadsheetAsync(file);

            //Assert
            Assert.IsType<List<SalaryDataRawRow>>(dataRows);
            Assert.True(dataRows.Count == 3);
        }

        [Fact]
        public async Task ExtractDataFromSpreadsheetAsyncTestWithEmptyFile()
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

            //Action
            var dataRows = await ExcelFileHelper.ExtractDataFromSpreadsheetAsync(file);

            //Assert
            Assert.IsType<List<SalaryDataRawRow>>(dataRows);
            Assert.True(dataRows.Count == 0);
        }

        [Fact]
        public void GenerateExcelFileFromPaySlipDetailstWithNormalData()
        {
            var testingFile = ExcelFileHelper.GenerateExcelFileFromPaySlipDetails(GetTestingPaySlipDetails());
            var dataRows = testingFile.Worksheets[0].Rows;

            Assert.True(testingFile.Worksheets.Count == 1);
            Assert.True(dataRows.Count == 11);
        }

        [Fact]
        public void GenerateExcelFileFromPaySlipDetailstWithEmptyData()
        {
            var testingFile = ExcelFileHelper.GenerateExcelFileFromPaySlipDetails(new List<PaySlipDetail>());
            var dataRows = testingFile.Worksheets[0].Rows;

            Assert.True(testingFile.Worksheets.Count == 1);
            Assert.True(dataRows.Count == 1);
        }


        private List<PaySlipDetail> GetTestingPaySlipDetails()
        {
            var fixture = new Fixture();

            return new List<PaySlipDetail>
            {
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>(),
                fixture.Create<PaySlipDetail>()
            };
        }
    }
}