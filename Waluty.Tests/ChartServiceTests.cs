using Moq;
using System;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;

namespace Waluty.Tests
{
    class ChartServiceTests
    {
        private readonly string _firstCurrencyName = "AUD";
        private readonly string _secondCurrencyName = "USD";
        private readonly string _thirdCurrencyName = "JPY";


        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {

            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateTestCurrency(_firstCurrencyName);
            Currency secondCurrency = CreateTestCurrency(_secondCurrencyName);
            Currency thirdCurrency = CreateTestCurrency(_thirdCurrencyName);

            mockRepository.Setup(x => x.GetCurrency(_firstCurrencyName)).ReturnsAsync(firstCurrency);
            mockRepository.Setup(x => x.GetCurrency(_secondCurrencyName)).ReturnsAsync(secondCurrency);
            mockRepository.Setup(x => x.GetCurrency(_thirdCurrencyName)).ReturnsAsync(thirdCurrency);

            return mockRepository.Object;
        }

        private Currency CreateTestCurrency(string name)
        {
            Currency testCurrency = new Currency();
            Random random = new Random();

            // Creates 7 additional, concurent days to currency.
            for (int i = 0; i <= random.Next(2,10); i++)
            {
                CurrencyRecord currencyRecord = new CurrencyRecord
                {
                    Date = new DateTime(2000, 10, i)
                };

                testCurrency.ListOfRecords.Add(currencyRecord);
            }

            testCurrency.Name = name;

            return testCurrency;
        }
    }
}
