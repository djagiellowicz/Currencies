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
        private readonly int _startYear = 2001;
        private readonly int _startMonth = 1;

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

            int _startYear = 2001;
            int firstCurrencyStartDay = 1;
            int secondCurrencyStartDay = 3;

            var firstCurrency = CreateTestCurrency(firstCurrencyStartDay, _startYear, _startMonth);
            var secondCurrency = CreateTestCurrency(secondCurrencyStartDay, _startYear, _startMonth);

            moq.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            moq.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);

            var firstCurrencyRecords = new List<CurrencyRecord>();

            return new DateChecker(moq.Object);
        }

        private Currency CreateTestCurrency(int startingPoint, int startYear, int startMonth)
        {
            Currency testCurrency = new Currency();

            for (int i = startingPoint; i <= startingPoint + 7; i++)
            {
                CurrencyRecord currencyRecord = new CurrencyRecord
                {
                    Date = new DateTime(startYear, startMonth, i)
                };

                testCurrency.ListOfRecords.Add(currencyRecord);
            }

            return testCurrency;
        }

        [Fact]
        public async void DateChecker_For_Two_Currencies_Must_Return_True_On_WorkingDay()
        {
            // Arrange
            var unitUnderTest = CreateDateChecker();
            var dateTime = new DateTime(_startYear, _startMonth, 4);

            // Act

            var result = await unitUnderTest.CheckIfDateExistsForTwoCurrencies(
                dateTime,
                _firstCurrencyName,
                _secondCurrencyName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void DateChecker_For_Two_Currencies_Must_Return_False_On_Non_Existent_Day()
        {
            // Arrange
            var unitUnderTest = this.CreateDateChecker();
            DateTime dateCurrency = new DateTime(_startYear, _startMonth, 30);

            // Act
            var result = await unitUnderTest.CheckIfDateExistsForTwoCurrencies(
                dateCurrency,
                _firstCurrencyName,
                _secondCurrencyName);

            // Assert
            Assert.False(result);
        }
    }
}
