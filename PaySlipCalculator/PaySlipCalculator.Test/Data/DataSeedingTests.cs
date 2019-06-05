using PaySlipCalculator.Core.Data;
using Xunit;

namespace PaySlipCalculator.Core.Test.Data
{
    public class DataSeedingTests
    {
        [Fact]
        public void TestDataSeeding()
        {
            var testingRule = DataSeeding.GeTaxingRule(); 

            Assert.True(testingRule.TaxingRates.Count==5); 
        }
    }
}