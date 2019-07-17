using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyConversionService(ICurrencyRepository repository)
        { 
            _repository = repository;
        }

        public async Task<CurrencyConversionModel> CalculateCurrencyConversionAmount(CurrencyConversionModel currencyConversionModel)
        {
            CurrencyRecord firstDesiredCurrency = await GetDesiredCurrency(currencyConversionModel.FirstCurrency, currencyConversionModel.Date);
            CurrencyRecord secondDesiredCurrency = await GetDesiredCurrency(currencyConversionModel.SecondCurrency, currencyConversionModel.Date);
            currencyConversionModel.AmountSecondCurrency = currencyConversionModel.AmountFirstCurrency * firstDesiredCurrency.Close / secondDesiredCurrency.Close;

            return currencyConversionModel;
        }

        private async Task<CurrencyRecord> GetDesiredCurrency(string nameCurrency, DateTime date)
        {
            Currency currency =  await _repository.GetCurrency(nameCurrency);
            List<CurrencyRecord> listOfRecords = currency.ListOfRecords;
            CurrencyRecord desiredRecord = listOfRecords.SingleOrDefault(record => record.Date == date);

            return desiredRecord;
        }

    }
}
