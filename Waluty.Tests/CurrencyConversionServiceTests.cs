using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile.DatabaseLoading;
using WalutyBusinessLogic.Services;

namespace Waluty.Tests
{
    class CurrencyConversionServiceTests
    {
        private readonly string _firstCurrencyName = "GBP";
        private readonly string _secondCurrencyName = "EUR";
        private readonly DateTime _date = DateTime.Now;

        private ICurrencyConversionService CreateCurrencySonversionService()
        {
            Mock<ICurrencyRepository> mockRepository = new Mock<ICurrencyRepository>();

            return null;
        }


    }
}
