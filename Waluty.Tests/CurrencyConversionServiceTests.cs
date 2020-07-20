using Moq;
using System;
using System.Collections.Generic;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class CurrencyConversionServiceTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly DateTime _commonDate = DateTime.Now;
        private readonly int _firstCurrencyCloseValue = 5;
        private readonly int _secondCurrencyCloseValue = 10;

        private CurrencyConversionService CreateCurrencyConversionService()
        {
            ICurrencyRepository currencyRepository = CreateICurrencyRepositoryMoq();

            return new CurrencyConversionService(currencyRepository);
        }

        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {
            
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateCurrency(_firstCurrencyCloseValue, _firstCurrencyName, _commonDate);
            Currency secondCurrency = CreateCurrency(_secondCurrencyCloseValue, _secondCurrencyName, _commonDate);
          
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

        [Fact]
        public async void CurrencyConversionService_is_conversion_correct()
        {
            //Arange
            CurrencyConversionService testService = CreateCurrencyConversionService();
            CurrencyConversionModel currencyConversionModel = new CurrencyConversionModel();
            currencyConversionModel.FirstCurrency = _firstCurrencyName;
            currencyConversionModel.SecondCurrency = _secondCurrencyName;
            currencyConversionModel.AmountFirstCurrency = 10;
            currencyConversionModel.Date = _commonDate;
            float expectedResult = currencyConversionModel.AmountFirstCurrency * _firstCurrencyCloseValue / _secondCurrencyCloseValue;
            bool resultFlag = false;

            //Act
            currencyConversionModel = await testService.CalculateCurrencyConversionAmount(currencyConversionModel);

            if(currencyConversionModel.AmountSecondCurrency == expectedResult)
            {
                resultFlag = true;
            }

            //Asert
            Assert.True(resultFlag);
        }

    }
}
