namespace PaySlipCalculator.Core.Models
{
    public class TaxingRate
    { 
        public int TaxableIncomeBottom { get; set; }
        public int TaxableIncomeTop { get; set; } 
        public decimal TaxRatio { get; set; }
        public int TaxBase { get; set; }
    }
}