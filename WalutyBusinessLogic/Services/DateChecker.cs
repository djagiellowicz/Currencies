using System;
using System.Collections.Generic;
using WalutyBusinessLogic.LoadingFromFile;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;

namespace WalutyBusinessLogic.Services
{
    public class DateChecker : IDateChecker
    {
        private readonly ICurrencyRepository _repository;

        public DateChecker(ICurrencyRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> CheckingIfDateExistsForTwoCurrencies(DateTime dateCurrency, string firstNameCurrency,
            string secondNameCurrency)
        {
            List<CurrencyRecord> FirstCurrencyRecordList = await GetRecordDateList(firstNameCurrency);
            List<CurrencyRecord> SecondCurrencyRecordList = await GetRecordDateList(secondNameCurrency);
            if ((FirstCurrencyRecordList.Any(c => c.Date == dateCurrency))
            && (SecondCurrencyRecordList.Any(c => c.Date == dateCurrency)))
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> CheckingIfDateExistInRange(DateTime firstDate, DateTime secondDate, string currencyName)
        {
            List<CurrencyRecord> CurrencyRecordList = await GetRecordDateList(currencyName);
            if (CurrencyRecordList.Exists(c => c.Date >= firstDate) &&
                CurrencyRecordList.Exists(c => c.Date <= secondDate))
            {
                return true;
            }
            else return false;
        }

        private async Task<List<CurrencyRecord>> GetRecordDateList(string nameCurrency)
        {
            Currency currency = await _repository.GetCurrency(nameCurrency);
            List<CurrencyRecord> CurrencyDateList = currency.ListOfRecords;
            return CurrencyDateList;
        }
    }
}

