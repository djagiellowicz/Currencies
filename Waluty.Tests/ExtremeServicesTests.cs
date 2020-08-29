using Moq;
using System;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class ExtremeServicesTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly string _thirdCurrencyName = "USD";
        private readonly int _startDay = 1;
        private readonly int _startMonth = 10;
        private readonly int _startYear = 2000;


        public ExtremesServices CreateExtremeServices(ICurrencyRepository repository)
        {
            return new ExtremesServices(repository);
        }

        private ICurrencyRepository CreateICurrencyRepositoryMoq()
        {
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();
            Currency firstCurrency = CreateCurrency(_startDay, _startMonth, _startYear, _firstCurrencyName, 2, 1);
            Currency secondCurrency = CreateCurrency(_startDay, _startMonth, _startYear, _secondCurrencyName, 3, 0.5f);
            Currency thirdCurrency = CreateCurrency(_startDay, _startMonth, _startYear, _thirdCurrencyName, 4, 2);

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
                    Date = new DateTime(startYear, startMonth, i),
                    Close = startCloseValue + i * incrementCloseValue,
                    High = startCloseValue + i * incrementCloseValue + incrementCloseValue,
                    Low = startCloseValue + i * incrementCloseValue - incrementCloseValue
                };

                testCurrency.ListOfRecords.Add(currencyRecord);
            }
            testCurrency.Name = name;

            return testCurrency;
        }

        [Fact]
        public async void ExtremeServicesTests_Get_Global_Extreme_Should_Return_MinValue_2_MaxValue_11_GBP()
        {
            //Arrange
            bool result = false;
            ICurrencyRepository repository = CreateICurrencyRepositoryMoq();
            ExtremesServices extremesServices = CreateExtremeServices(repository);
            GlobalExtremeValueModel resultModel = new GlobalExtremeValueModel { NameCurrency = _firstCurrencyName };
            GlobalExtremeValueModel expectedModel = new GlobalExtremeValueModel
            {
                NameCurrency = _firstCurrencyName,
                MaxValue = 11,
                MinValue = 2
            };

            //Act
            resultModel = await extremesServices.GetGlobalExtreme(resultModel);

            if(resultModel.NameCurrency == expectedModel.NameCurrency && resultModel.MinValue == expectedModel.MinValue 
                && resultModel.MaxValue == expectedModel.MaxValue)
            {
                result = true;
            }

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async void ExtremeServicesTests_Get_Local_Extreme_Should_Return_MinValue_3_MaxValue_6_GBP()
        {
            //Arrange
            bool result = false;
            ICurrencyRepository repository = CreateICurrencyRepositoryMoq();
            ExtremesServices extremesServices = CreateExtremeServices(repository);
            LocalExtremeValueModel resultModel = new LocalExtremeValueModel {
                NameCurrency = _firstCurrencyName,
                StartDate = new DateTime(_startYear, _startMonth, _startDay + 1),
                EndDate = new DateTime(_startYear, _startMonth, _startDay + 2)
            };
            LocalExtremeValueModel expectedModel = new LocalExtremeValueModel
            {
                NameCurrency = _firstCurrencyName,
                MinValue = 3,
                MaxValue = 6
            };

            //Act
            resultModel = await extremesServices.GetLocalExtremes(resultModel);

            if (resultModel.NameCurrency == expectedModel.NameCurrency && resultModel.MinValue == expectedModel.MinValue
                && resultModel.MaxValue == expectedModel.MaxValue)
            {
                result = true;
            }

            //Assert
            Assert.True(result);
        }
    }
}
