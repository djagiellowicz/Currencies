﻿using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class ServicesTests
    {
        [Fact]
        public void CurrencyNameChecker_Check_If_Two_Different_Names_Are_Equal()
        {
            //Arrange
            CurrencyNameChecker nameChecker = new CurrencyNameChecker();
            string firstCurrencyName = "USD";
            string secondCurrencyName = "PLN";
            bool result;

            //Act

            result = nameChecker.CheckingIfCurrenciesIsDifferent(firstCurrencyName, secondCurrencyName);

            //Asert
            Assert.True(result);

        }

        [Fact]
        public void CurrencyNameChecker_Check_If_Two_Same_Names_Are_Equal()
        {
            //Arrange
            CurrencyNameChecker nameChecker = new CurrencyNameChecker();
            string firstCurrencyName = "USD";
            string secondCurrencyName = "USD";
            bool result;

            //Act

            result = nameChecker.CheckingIfCurrenciesIsDifferent(firstCurrencyName, secondCurrencyName);

            //Asert
            Assert.False(result);

        }
    }
}
