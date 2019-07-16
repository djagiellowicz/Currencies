using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.Services
{
    public class DateRange : IDateRange
    {
        private readonly ICurrencyRepository _repository;

        public DateRange(ICurrencyRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> GetDateRangeCurrency(string currencyCode)
        {
            List<CurrencyRecord> listOfRecords = await GetCurrencyList(currencyCode);

            DateTime FirstDateCurrency = listOfRecords.FirstOrDefault().Date;
            DateTime LastDateCurrency= listOfRecords.LastOrDefault().Date;

            string dateRangeResult = $"{currencyCode} exist in this app from {FirstDateCurrency.ToShortDateString()} " +
                $"to {LastDateCurrency.ToShortDateString()}. Without weekends and holidays";

            return dateRangeResult;
        }

        public async Task<string> GetDateRangeTwoCurrencies(string firstCurrencyCode, string secondCurrencyCode)
        {
            List<CurrencyRecord> FirstListOfRecords = await GetCurrencyList(firstCurrencyCode);
            List<CurrencyRecord> SecondListOfRecords = await GetCurrencyList(secondCurrencyCode);

            DateTime FirstDateOfFirstCurrency = FirstListOfRecords.FirstOrDefault().Date;
            DateTime LastDateOfFirstCurrency = FirstListOfRecords.LastOrDefault().Date;
            
            DateTime FirstDateOfSecondCurrency = SecondListOfRecords.FirstOrDefault().Date;
            DateTime LastDateOfSecondCurrency = SecondListOfRecords.LastOrDefault().Date;

            DateTime FirstCommonDate = GetLaterDate(FirstDateOfSecondCurrency, FirstDateOfFirstCurrency);
            DateTime LastCommonDate = GetEarlierDate(LastDateOfFirstCurrency, LastDateOfSecondCurrency);

            string dateRangeResult = $"Date common for {firstCurrencyCode} and {secondCurrencyCode} " +
                                     $"exist in this app is from {FirstCommonDate.ToShortDateString()} to {LastCommonDate.ToShortDateString()}"
                                     + ". Without weekends and holidays";
            return dateRangeResult;
        }

        private DateTime GetLaterDate(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate > secondDate) return firstDate;
            else return secondDate;
        }

        private DateTime GetEarlierDate(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate > secondDate) return firstDate;
            else return secondDate;
        }

        private async Task<List<CurrencyRecord>> GetCurrencyList(string currencyCode)
        {
            Currency currency = await _repository.GetCurrency(currencyCode);
            List<CurrencyRecord> listOfRecords = currency.ListOfRecords;
            return listOfRecords;
        }
    }
}
