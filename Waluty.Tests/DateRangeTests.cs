using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;

namespace Waluty.Tests
{
    class DateRangeTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly DateTime _firstCurrencyStartDate = new DateTime(2000,10,5);
        private readonly DateTime _secondCurrencyStartDate = new DateTime(2000,10,1);

        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateCurrency(_firstCurrencyName, _firstCurrencyStartDate, 10);
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
    }
}
