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

        private DateChecker CreateDateChecker(int firstCurrencyStartDay, int secondCurrencyStartDay, int startMonth, int startYear)
        {
            var moq = new Mock<ICurrencyRepository>();

            var firstCurrency = CreateTestCurrency(firstCurrencyStartDay, startYear, startMonth);
            var secondCurrency = CreateTestCurrency(secondCurrencyStartDay, startYear, startMonth);

            moq.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            moq.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);

            var firstCurrencyRecords = new List<CurrencyRecord>();

            return new DateChecker(moq.Object);
        }

        private Currency CreateTestCurrency(int startingPoint, int startYear, int startMonth)
        {
            Currency testCurrency = new Currency();

            // Creates 7 additional, concurent days to currency.
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
        public async void DateChecker_For_Two_Currencies_Must_Return_True_On_CommonDay()
        {
            // Arrange
            int startYear = 2000;
            int startMonth = 1;
            int firstCurrencyStartDay = 1;
            int secondCurrencyStartDay = 3;

            var unitUnderTest = CreateDateChecker(firstCurrencyStartDay, secondCurrencyStartDay, startMonth, startYear);
            var dateTime = new DateTime(startYear, startMonth, 4);

            // Act

            var result = await unitUnderTest.CheckIfDateExistsForTwoCurrencies(
                dateTime,
                _firstCurrencyName,
                _secondCurrencyName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void DateChecker_For_Two_Currencies_Must_Return_False_On_Non_Common_Day()
        {
            // Arrange
            int startYear = 2000;
            int startMonth = 1;
            int firstCurrencyStartDay = 1;
            int secondCurrencyStartDay = 3;

            var unitUnderTest = this.CreateDateChecker(firstCurrencyStartDay, secondCurrencyStartDay, startMonth, startYear);
            DateTime dateCurrency = new DateTime(startYear, startMonth, 30);

            // Act
            var result = await unitUnderTest.CheckIfDateExistsForTwoCurrencies(
                dateCurrency,
                _firstCurrencyName,
                _secondCurrencyName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void DateChecker_For_Two_Currencies_Must_Return_False_On_Day_Existing_Only_In_One_Currency()
        {
            // Arrange
            int startYear = 2000;
            int startMonth = 1;
            int firstCurrencyStartDay = 1;
            int secondCurrencyStartDay = 3;

            var unitUnderTest = this.CreateDateChecker(firstCurrencyStartDay, secondCurrencyStartDay, startMonth, startYear);
            DateTime dateCurrency = new DateTime(startYear, startMonth, 2);

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
