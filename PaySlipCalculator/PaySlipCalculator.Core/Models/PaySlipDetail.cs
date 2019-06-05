using System;
using PaySlipCalculator.Core.Models;

namespace PaySlipCalculator.Core.Models
{
    public class PaySlipDetail
    { 
        public string Name { get; set; }
        public string PayPeriod { get; set; }
        public int GrossIncome { get; set; }
        public int IncomeTax { get; set; }
        public int NetIncome { get; set; }
        public int SuperAnnunation { get; set; }
    }
}