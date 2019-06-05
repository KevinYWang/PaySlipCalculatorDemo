using AutoFixture;
using PaySlipCalculator.Core.Models;
using System.Threading.Tasks;
using Xunit;

namespace PaySlipCalculator.Core.Test
{
    public class PaySlipCalculatorTests
    {
        [Fact]
        public async Task GetPaySlipDetailFromSalaryInfoTestWithNormalData()
        {
            var fixture = new Fixture();
            var salaryInfo = fixture.Build<SalaryInfo>().With(x=>x.AnnualSalary,120000).Create();
            var calculator = new PaySlipCalculator.Core.Services.TaxingCalculator();


            var returnedDetail = await calculator.GetPaySlipDetailFromSalaryInfo(salaryInfo);

            Assert.IsType<PaySlipDetail>(returnedDetail);
            Assert.True(returnedDetail.GrossIncome ==10000);
        }

        [Fact]
        public async Task GetPaySlipDetailFromSalaryInfoTestWithEmptyData()
        {
            
            var salaryInfo = new SalaryInfo();
            var calculator = new PaySlipCalculator.Core.Services.TaxingCalculator();


            var returnedDetail = await calculator.GetPaySlipDetailFromSalaryInfo(salaryInfo);

            Assert.IsType<PaySlipDetail>(returnedDetail);
            Assert.True(returnedDetail.GrossIncome == 0);
        }
    }
}