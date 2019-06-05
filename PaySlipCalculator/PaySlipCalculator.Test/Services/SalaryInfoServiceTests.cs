using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using PaySlipCalculator.Core.Models;
using PaySlipCalculator.Core.Services;
using Xunit;

namespace PaySlipCalculator.Test
{
    public class SalaryInfoServiceTests
    {
        private SalaryInfoService salaryInfoService;

        public SalaryInfoServiceTests()
        {
            var calculator = new TaxingCalculator();
            salaryInfoService = new SalaryInfoService(calculator);
        }

        [Fact]
        public async Task ImportSalaryInfoTestWithNormalData()
        {
            var dataRows = GetSalaryDataRawRows();

            var results = await salaryInfoService.ImportSalaryInfo(dataRows);

            Assert.True(results.Count == 5);
        }

        [Fact]
        public async Task ImportSalaryInfoTestWithEmptyData()
        {
            var dataRows = new List<SalaryDataRawRow>();

            var results = await salaryInfoService.ImportSalaryInfo(dataRows);

            Assert.True(results.Count == 0);
        }


        [Fact]
        public async Task GetPaySlipDetailsTestWithNormalData()
        {
            var dataRows = GetSalaryDataRawRows();
            await salaryInfoService.ImportSalaryInfo(dataRows);
            var results = salaryInfoService.GetPaySlipDetails();

            Assert.True(results.Count == 5);
        }

        [Fact]
        public async Task GetPaySlipDetailsTestWithEmptyData()
        {
            var dataRows = new List<SalaryDataRawRow>();
            await salaryInfoService.ImportSalaryInfo(dataRows);
            var results = salaryInfoService.GetPaySlipDetails();

            Assert.True(results.Count == 0);
        }

        private List<SalaryDataRawRow> GetSalaryDataRawRows()
        {
            var fixture = new Fixture();
            return new List<SalaryDataRawRow>()
            {
                fixture.Build<SalaryDataRawRow>()
                    .With(x => x.AnnualSalary, "120000")
                    .With(x => x.SuperRate, "0.05")
                    .Create(),
                fixture.Build<SalaryDataRawRow>()
                    .With(x => x.AnnualSalary, "120000")
                    .With(x => x.SuperRate, "0.05")
                    .Create(),
                fixture.Build<SalaryDataRawRow>()
                    .With(x => x.AnnualSalary, "120000")
                    .With(x => x.SuperRate, "0.05")
                    .Create(),
                fixture.Build<SalaryDataRawRow>()
                    .With(x => x.AnnualSalary, "120000")
                    .With(x => x.SuperRate, "0.05")
                    .Create(),
                fixture.Build<SalaryDataRawRow>()
                    .With(x => x.AnnualSalary, "120000")
                    .With(x => x.SuperRate, "0.05")
                    .Create(),
            };
        }
    }
}