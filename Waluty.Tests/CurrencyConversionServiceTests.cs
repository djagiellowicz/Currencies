using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.LoadingFromFile.DatabaseLoading;
using WalutyBusinessLogic.Services;

namespace Waluty.Tests
{
    class CurrencyConversionServiceTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly DateTime _commonDate = DateTime.Now;

        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {
            int firstCurrencyCloseValue = 5;
            int secondCurrencyCloseValue = 10;
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateCurrency(firstCurrencyCloseValue, _firstCurrencyName, _commonDate);
            Currency secondCurrency = CreateCurrency(secondCurrencyCloseValue, _secondCurrencyName, _commonDate);
          
            mockRepository.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            mockRepository.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);

            return mockRepository.Object;
        }

        private Currency CreateCurrency(int CloseValue, string CurrencyName, DateTime commonDate)
        {
            Currency currency = new Currency();
            List<CurrencyRecord> records = new List<CurrencyRecord>();
            CurrencyRecord firstRecord = new CurrencyRecord()
            {
                Date = commonDate,
                Close = CloseValue
            };
            CurrencyRecord secondRecord = new CurrencyRecord()
            {
                Date = new DateTime(1000, 1, 1),
                Close = 15
            };
            
            records.Add(firstRecord);
            records.Add(secondRecord);

            currency.Name = _firstCurrencyName;
            currency.ListOfRecords = records;

            return currency;
        }


    }
}
