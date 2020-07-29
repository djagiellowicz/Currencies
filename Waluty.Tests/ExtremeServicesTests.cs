using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Services;

namespace Waluty.Tests
{
    public class ExtremeServicesTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly string _thirdCurrencyName = "USD";

        public ExtremesServices CreateExtremeServices(ICurrencyRepository repository)
        {
            return new ExtremesServices(repository);
        }

        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateCurrency(1, 10, 2000, _firstCurrencyName, 2, 1);
            Currency secondCurrency = CreateCurrency(1, 10, 2000, _secondCurrencyName, 3, 0.5f);
            Currency thirdCurrency = CreateCurrency(1, 10, 2000, _thirdCurrencyName, 2, 1);

            mockRepository.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            mockRepository.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);
            mockRepository.Setup(x => x.GetCurrency(_thirdCurrencyName)).ReturnsAsync(thirdCurrency);

            return mockRepository.Object;
        }


        private Currency CreateCurrency(int startDay, int startMonth, int startYear,  string name,
                                        float startCloseValue, float incrementCloseValue)
        {
            Currency testCurrency = new Currency();

            // Creates 7 additional, concurent days to currency.
            for (int i = startDay; i <= startDay + 7; i++)
            {
                CurrencyRecord currencyRecord = new CurrencyRecord
                {
                    Date = new DateTime(startYear, startMonth, i)
                };

                testCurrency.ListOfRecords.Add(currencyRecord);
            }
            testCurrency.Name = name;

            return testCurrency;
        }


    }
}
