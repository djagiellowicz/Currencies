using Moq;
using System;
using System.Collections.Generic;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class DateRangeTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly DateTime _firstCurrencyStartDate = new DateTime(2000,10,5);
        private readonly DateTime _secondCurrencyStartDate = new DateTime(2000,10,1);

        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateCurrency(_firstCurrencyName, _firstCurrencyStartDate, 5);
            Currency secondCurrency = CreateCurrency(_secondCurrencyName, _secondCurrencyStartDate, 10);

            mockRepository.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            mockRepository.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);

            return mockRepository.Object;
        }

        private Currency CreateCurrency(string CurrencyName, DateTime startDate, int numberOfDays)
        {
            Currency currency = new Currency();
            List<CurrencyRecord> records = new List<CurrencyRecord>();

            for (int i = 0; i<numberOfDays; i++)
            {
                CurrencyRecord newRecord = new CurrencyRecord()
                {
                    Date = startDate.AddDays(i),
                };
                records.Add(newRecord);
            }

            currency.Name = _firstCurrencyName;
            currency.ListOfRecords = records;

            return currency;
        }

        [Fact]
        public async void DateRangeTests_Are_Proper_Common_Days_Returned()
        {
            //Arrange
            IDateRange dateRange = new DateRange(CreateICurrencyRepositoryMoq());
            string expectedResult = "Date common for GBP and EUR exist in this app is from 05.10.2000 to 10.10.2000. Without weekends and holidays";

            //Act
            var result = await dateRange.GetCommonDateRangeForTwoCurrencies(_firstCurrencyName, _secondCurrencyName);

            //Asert
            Assert.Equal(expectedResult, result);
        }
    }
}
