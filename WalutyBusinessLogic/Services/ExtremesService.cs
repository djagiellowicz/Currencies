using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public class ExtremesServices : IExtremesServices
    {
        private readonly ICurrencyRepository _repository;

        public ExtremesServices(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<GlobalExtremeValueModel> GetGlobalExtremes(GlobalExtremeValueModel extremeValue)
        {
            List<CurrencyRecord> listOfRecords = await GetCurrencyList(extremeValue.NameCurrency);
            extremeValue.MaxValue = listOfRecords.Max(c => c.High);
            extremeValue.MinValue = listOfRecords.Min(c => c.Low);
            return extremeValue;
        }

        public async Task<LocalExtremeValueModel> GetLocalExtremes(LocalExtremeValueModel extremeValue)
        {
            List<CurrencyRecord> listOfRecords = await GetCurrencyList(extremeValue.NameCurrency);
            extremeValue.MaxValue = listOfRecords.Where
                (c => c.Date >= extremeValue.StartDate && c.Date <= extremeValue.EndDate)
                .Max(c => c.High);
            extremeValue.MinValue = listOfRecords.Where
                (c => c.Date >= extremeValue.StartDate && c.Date <= extremeValue.EndDate)
                .Min(c => c.Low);
            return extremeValue;
        }

        private async Task<List<CurrencyRecord>> GetCurrencyList(string codeCurrency)
        {
            Currency currency = await _repository.GetCurrency(codeCurrency); 
            List<CurrencyRecord> listOfRecords = currency.ListOfRecords;
            return listOfRecords;
        }
    }
}
