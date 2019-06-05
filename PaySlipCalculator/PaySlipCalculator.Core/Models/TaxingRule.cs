using System;
using System.Collections.Generic;

namespace PaySlipCalculator.Core.Models
{
    public class TaxingRule
    {  
        public DateTime ValidDate { get; set; } 
        public string Description { get; set; } 
        public ICollection<TaxingRate> TaxingRates { get; set; }
    }
}