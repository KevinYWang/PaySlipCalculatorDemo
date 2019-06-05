namespace PaySlipCalculator.Core.Models
{
    public class SalaryInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AnnualSalary { get; set; }
        public decimal SuperRate { get; set; }
        public string PaymentDate { get; set; }
    }
}