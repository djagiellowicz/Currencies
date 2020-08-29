using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class CurrencyNameCheckerTests
    {
        [Fact]
        public void CurrencyNameChecker_Check_If_Two_Different_Currencies_Names_Are_Not_Equal()
        {
            //Arrange
            CurrencyNameChecker nameChecker = new CurrencyNameChecker();
            string firstCurrencyName = "USD";
            string secondCurrencyName = "PLN";
            bool result;

            //Act
            result = nameChecker.AreDifferent(firstCurrencyName, secondCurrencyName);

            //Asert
            Assert.True(result);
        }

        [Fact]
        public void CurrencyNameChecker_Check_If_Two_Same_Currencies_Names_Are_Equal()
        {
            //Arrange
            CurrencyNameChecker nameChecker = new CurrencyNameChecker();
            string firstCurrencyName = "USD";
            string secondCurrencyName = "USD";
            bool result;

            //Act
            result = nameChecker.AreDifferent(firstCurrencyName, secondCurrencyName);

            //Asert
            Assert.False(result);
        }

        [Fact]
        public void CurrencyNameChecker_Check_If_Two_Same_Currencies_Names_Are_Equal_Different_()
        {
            //Arrange
            CurrencyNameChecker nameChecker = new CurrencyNameChecker();
            string firstCurrencyName = "USD";
            string secondCurrencyName = "USD";
            bool result;

            //Act
            result = nameChecker.AreDifferent(firstCurrencyName, secondCurrencyName);

            //Asert
            Assert.False(result);
        }
    }
}
