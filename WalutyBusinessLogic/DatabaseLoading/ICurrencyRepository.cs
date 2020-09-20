using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalutyBusinessLogic.LoadingFromFile;
using X.PagedList;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllCurrencies();
        Task<IPagedList<Currency>> GetAllCurrencies(int pageSize, int pageNumber);
        Task<IPagedList<Currency>> GetAllCurrencies(int pageSize, int pageNumber, string filter);
        Task<List<CurrencyInfo>> GetAllCurrencyInfo();
        Task<IPagedList<CurrencyInfo>> GetAllCurrencyInfo(int pageSize, int pageNumber);
        Task<IPagedList<CurrencyInfo>> GetAllCurrencyInfo(int pageSize, int pageNumber, string filter);
        Task<Currency> GetCurrency(string currencyCode);
        Task<List<CurrencyRecord>> GetCurrencyRecordsBtwnDates(string currencyCode, DateTime begDate, DateTime endDate);
        Task<Currency> GetCurrency(int currencyId);
    }
}