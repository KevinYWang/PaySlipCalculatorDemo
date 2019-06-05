using PaySlipCalculator.Core.Models;
using System;
using System.Collections.Generic;

namespace PaySlipCalculator.Core.Data
{
    public static class DataSeeding
    {
        public static TaxingRule GeTaxingRule()
        {
            var newRule = new TaxingRule
            {
                ValidDate = new DateTime(2017, 7, 1),
                Description = "1 Jul 2017 Taxing Rule"
            };


            var taxingRates = new List<TaxingRate>
            {
                new TaxingRate
                {
                    TaxableIncomeBottom = 0,
                    TaxableIncomeTop = 18200,
                    TaxRatio = (decimal) 0,
                    TaxBase = 0
                },
                new TaxingRate
                {
                    TaxableIncomeBottom = 18201, TaxableIncomeTop = 37000, TaxRatio = (decimal) 0.19,
                    TaxBase = 0
                },
                new TaxingRate
                {
                    TaxableIncomeBottom = 37001, TaxableIncomeTop = 87000, TaxRatio = (decimal) 0.325,
                    TaxBase = 3572
                },
                new TaxingRate
                {
                    TaxableIncomeBottom = 87001, TaxableIncomeTop = 180000, TaxRatio = (decimal) 0.37,
                    TaxBase = 19822
                },
                new TaxingRate
                {
                    TaxableIncomeBottom = 180000, TaxableIncomeTop = int.MaxValue, TaxRatio = (decimal) 0.45,
                    TaxBase = 54232
                }
            };


            newRule.TaxingRates = taxingRates;
            return newRule;
        }
    }
}