using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PaySlipCalculator.Core.Data;
using PaySlipCalculator.Core.Models;

namespace PaySlipCalculator.Core.Services
{
    public interface ISalaryInfoService
    {
        Task<List<PaySlipDetail>> ImportSalaryInfo(List<SalaryDataRawRow> dataRows);
        List<PaySlipDetail> GetPaySlipDetails();
    }

    public class SalaryInfoService : ISalaryInfoService
    {
        private readonly ITaxingCalculator paySlipCalculator;
        private static List<PaySlipDetail> _paySlipDetails;


        public SalaryInfoService(ITaxingCalculator paySlipCalculator)
        {
            this.paySlipCalculator = paySlipCalculator;
        }

        public async Task<List<PaySlipDetail>> ImportSalaryInfo(List<SalaryDataRawRow> dataRows)
        {
            var paySlipDetailList = new List<PaySlipDetail>();

            foreach (var row in dataRows)
            {
                var tempSalaryInfo = ParseRawDataRow(row);

                var tempPaySlipDetail =
                    await paySlipCalculator.GetPaySlipDetailFromSalaryInfo(tempSalaryInfo);

                paySlipDetailList.Add(tempPaySlipDetail);
            }

            _paySlipDetails = paySlipDetailList;
            return paySlipDetailList;
        }

        public List<PaySlipDetail> GetPaySlipDetails()
        {
            if (_paySlipDetails == null)
            {
                return new List<PaySlipDetail>();
            }

            return _paySlipDetails;
        }

        private SalaryInfo ParseRawDataRow(SalaryDataRawRow row)
        {
            int tempAnnualSalary;
            if (!int.TryParse(row.AnnualSalary, out tempAnnualSalary))
                throw new Exception($"Error in reading annual salary of {row.FirstName} {row.LastName}");

            var tempSuperRate = ConvertPercentageToDecimal(row.SuperRate);
            if (!tempSuperRate.HasValue)
                throw new Exception(
                    $"Error in reading super rate {row.SuperRate} of {row.FirstName} {row.LastName}");

            return new SalaryInfo
            {
                FirstName = row.FirstName,
                LastName = row.LastName,
                PaymentDate = row.PaymentDate,
                AnnualSalary = tempAnnualSalary,
                SuperRate = tempSuperRate.Value
            };
        }

        private decimal? ConvertPercentageToDecimal(string percentageStr)
        {
            decimal result;
            if (percentageStr.Contains("%"))
            {
                var pieces = percentageStr.Split('%');
                if (pieces.Length > 2 || !string.IsNullOrEmpty(pieces[1])) return null;


                if (!decimal.TryParse(pieces[0], out result))
                    return null;
                result = result / 100M;
            }
            else
            {
                if (!decimal.TryParse(percentageStr, out result))
                    return null;
            }

            return result;
        }
    }
}