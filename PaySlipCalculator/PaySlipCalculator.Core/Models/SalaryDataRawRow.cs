namespace PaySlipCalculator.Core.Models
{
    public class SalaryDataRawRow
    {
        //first name, last name, annual salary, super rate (%), payment start date
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AnnualSalary { get; set; }
        public string SuperRate { get; set; }
        public string PaymentDate { get; set; }
    }
}