using Moq;
using System;
using System.Collections.Generic;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests.Services
{
    public class DateCheckerTests : IDisposable
    {
        private MockRepository _mockRepository;
        private readonly string _firstCurrencyName = "USD";
        private readonly string _secondCurrencyName = "AUD";

        public DateCheckerTests()
        {
            this._mockRepository = new MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            this._mockRepository.VerifyAll();
        }

        private DateChecker CreateDateChecker()
        {
            var moq = new Mock<ICurrencyRepository>();

            var firstCurrency = CreateTestCurrency(0);
            var secondCurrency = CreateTestCurrency(2);

            moq.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            moq.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);

            var firstCurrencyRecords = new List<CurrencyRecord>();

            return new DateChecker(moq.Object);
        }

        private Currency CreateTestCurrency(int startingPoint)
        {
            Currency testCurrency = new Currency();

            for (int i = startingPoint; i <= startingPoint + 7; i++)
            {
                CurrencyRecord currencyRecord = new CurrencyRecord();
                currencyRecord.Date.AddDays(i);

                testCurrency.ListOfRecords.Add(currencyRecord);
            }

            return testCurrency;
        }

        [Fact]
        public void DateChecker_For_Two_Currencies_Must_Return_True_On_WorkingDay()
        {
            // Arrange
            var unitUnderTest = this.CreateDateChecker();
            DateTime dateCurrency = new DateTime(2001, 06, 11);
            string firstNameCurrency = "USD";
            string secondNameCurrency = "AUD";

            // Act
            var result = unitUnderTest.CheckIfDateExistsForTwoCurrencies(
                dateCurrency,
                firstNameCurrency,
                secondNameCurrency);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DateChecker_For_Two_Currencies_Must_Return_False_On_Holiday()
        {
            // Arrange
            var unitUnderTest = this.CreateDateChecker();
            DateTime dateCurrency = new DateTime(2001, 06, 10);
            string firstNameCurrency = "USD";
            string secondNameCurrency = "AUD";

            // Act
            var result = unitUnderTest.CheckIfDateExistsForTwoCurrencies(
                dateCurrency,
                firstNameCurrency,
                secondNameCurrency);

            // Assert
            Assert.False(result);
        }
    }
}
