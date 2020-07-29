using Moq;
using System;
using System.Linq;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class ChartServiceTests
    {
        private readonly string _firstCurrencyName = "AUD";
        private readonly string _secondCurrencyName = "USD";
        private readonly string _thirdCurrencyName = "JPY";

        private ChartService CreateChartService(ICurrencyRepository currencyRepository)
        {
            return new ChartService(currencyRepository);
        }

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

            for (int i = 1; i <= random.Next(3,10); i++)
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

        [Fact]
        public async void ChartServiceTests_AUD_Model_Should_Be_Returned()
        {
            //Arange
            ICurrencyRepository repository = CreateICurrencyRepositoryMoq();
            ChartService chartService = CreateChartService(repository);
            Currency testedCurrency = await repository.GetCurrency(_firstCurrencyName);
            ChartModel expectedModel = new ChartModel(testedCurrency.Name, testedCurrency.ListOfRecords);
            bool testResult = false;

            //Act

            ChartModel resultModel = await chartService.CreateChartModel(_firstCurrencyName);

            if (resultModel.CurrencyCode.Equals(_firstCurrencyName) && resultModel.CurrencyRecords.SequenceEqual(expectedModel.CurrencyRecords))
            {
                testResult = true;
            }

            //Asert
            Assert.True(testResult);

        }

        [Fact]
        public async void ChartServiceTests_USD_Model_Should_Be_Returned()
        {
            //Arange
            ICurrencyRepository repository = CreateICurrencyRepositoryMoq();
            ChartService chartService = CreateChartService(repository);
            Currency testedCurrency = await repository.GetCurrency(_secondCurrencyName);
            ChartModel expectedModel = new ChartModel(testedCurrency.Name, testedCurrency.ListOfRecords);
            bool testResult = false;

            //Act

            ChartModel resultModel = await chartService.CreateChartModel(_secondCurrencyName);

            if (resultModel.CurrencyCode.Equals(_secondCurrencyName) && resultModel.CurrencyRecords.SequenceEqual(expectedModel.CurrencyRecords))
            {
                testResult = true;
            }

            //Asert
            Assert.True(testResult);
        }

        [Fact]
        public async  void ChartServiceTests_JPY_Model_Should_Be_Returned()
        {
            //Arange
            ICurrencyRepository repository = CreateICurrencyRepositoryMoq();
            ChartService chartService = CreateChartService(repository);
            Currency testedCurrency = await repository.GetCurrency(_thirdCurrencyName);
            ChartModel expectedModel = new ChartModel(testedCurrency.Name, testedCurrency.ListOfRecords);
            bool testResult = false;

            //Act

            ChartModel resultModel = await chartService.CreateChartModel(_thirdCurrencyName);

            if (resultModel.CurrencyCode.Equals(_thirdCurrencyName) && resultModel.CurrencyRecords.SequenceEqual(expectedModel.CurrencyRecords))
            {
                testResult = true;
            }

            //Asert
            Assert.True(testResult);
        }

    }
}
