using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using PaySlipCalculator.Core.Data;
using PaySlipCalculator.Core.Models;

namespace PaySlipCalculator.Core.Services
{
    public interface ITaxingCalculator
    {
        Task<PaySlipDetail> GetPaySlipDetailFromSalaryInfo(SalaryInfo salaryInfo);
    }

    public class TaxingCalculator : ITaxingCalculator
    {
        public async Task<PaySlipDetail> GetPaySlipDetailFromSalaryInfo(SalaryInfo salaryInfo)
        {
            var taxingRule = DataSeeding.GeTaxingRule();
            var rates = taxingRule.TaxingRates;

            var suitableRate = rates.FirstOrDefault(x =>
                x.TaxableIncomeBottom <= salaryInfo.AnnualSalary &&
                x.TaxableIncomeTop >= salaryInfo.AnnualSalary );

            if (suitableRate == null)
                throw new Exception($"Error in finding suitable tax rate for {salaryInfo.AnnualSalary}");

            var baseTax = suitableRate.TaxBase;
            var extraTax = (int) ((salaryInfo.AnnualSalary - suitableRate.TaxableIncomeBottom) * suitableRate.TaxRatio);
            var incomeTax = (int) Math.Round((double) (baseTax + extraTax) / 12, MidpointRounding.ToEven);
            var grossIncome = (int) Math.Round((double) salaryInfo.AnnualSalary / 12, MidpointRounding.ToEven);
            var netIncome = grossIncome - incomeTax;
            var superAnnuation = (int) Math.Round(grossIncome * salaryInfo.SuperRate, MidpointRounding.ToEven);

            var result = new PaySlipDetail()
            {
                Name = $"{salaryInfo.FirstName} {salaryInfo.LastName}",
                PayPeriod = salaryInfo.PaymentDate,
                GrossIncome = grossIncome,
                IncomeTax = incomeTax,
                NetIncome = netIncome,
                SuperAnnunation = superAnnuation
            };

            return result;
        }
    }
}