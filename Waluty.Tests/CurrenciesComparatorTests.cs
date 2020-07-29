using Moq;
using System;
using System.Collections.Generic;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Services;
using Xunit;
using WalutyBusinessLogic.Models;

namespace Waluty.Tests
{
    public class CurrenciesComparatorTests : IDisposable
    {
        private readonly DateTime _date = new DateTime(2001,1,6); 
        private readonly string _firstCurrencyName = "USD";
        private readonly string _secondCurrencyName = "AUD";
        private readonly MockRepository _mockRepository;

        public CurrenciesComparatorTests()
        {
            this._mockRepository = new MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            this._mockRepository.VerifyAll();
        }

        private CurrenciesComparator CreateCurrencyNameChecker(int firstStartValue,int secondStartValue )
        {
            var moq = new Mock<ICurrencyRepository>();

            var firstCurrency = CreateTestCurrency(firstStartValue);
            firstCurrency.Name = _firstCurrencyName;
            var secondCurrency = CreateTestCurrency(secondStartValue);
            secondCurrency.Name = _secondCurrencyName;

            moq.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            moq.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);

            var firstCurrencyRecords = new List<CurrencyRecord>();

            return new CurrenciesComparator(moq.Object);
        }

        private Currency CreateTestCurrency(int startValue)
        {
            Currency testCurrency = new Currency();

            for (int i = 0; i <= 7; i++)
            {
                CurrencyRecord currencyRecord = new CurrencyRecord
                {
                    Date = new DateTime(_date.Year, _date.Month, _date.Day + i),
                    Close = startValue + i
                };

                testCurrency.ListOfRecords.Add(currencyRecord);
            }
            return testCurrency;
        }

        [Fact]
        public async void CurrencyNameChecker_1_USD_Should_Be_Worth_Half_Of_AUD()
        {
            //Arrange
            int usdStartValue = 1;
            int audStartValue = 3;
            //bool result;

            CurrenciesComparator currenciesComparator = CreateCurrencyNameChecker(usdStartValue, audStartValue);
            CurrenciesComparatorModel currenciesComparatorModel = new CurrenciesComparatorModel();
            currenciesComparatorModel.FirstCurrencyCode = _firstCurrencyName;
            currenciesComparatorModel.SecondCurrencyCode = _secondCurrencyName;
            currenciesComparatorModel.Date = new DateTime(_date.Year, _date.Month, _date.Day + 1);

            //Act
            var result = await currenciesComparator.CompareCurrencies(currenciesComparatorModel);


            //Asert
            Assert.Equal(($"On the day of {currenciesComparatorModel.Date.ToShortDateString()} {_firstCurrencyName} is worth 0,5 {_secondCurrencyName}").ToLower()
                         , result.ComparatorResult.ToLower());
        }
    }
}
