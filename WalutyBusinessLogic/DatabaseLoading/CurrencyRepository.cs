using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using X.PagedList;

namespace WalutyBusinessLogic.LoadingFromFile.DatabaseLoading
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly WalutyDBContext _walutyDBContext;

        public CurrencyRepository(WalutyDBContext walutyDBContext)
        {
            _walutyDBContext = walutyDBContext;
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            return await _walutyDBContext.Currencies.ToListAsync();
        }

        public async Task<IPagedList<Currency>> GetAllCurrencies(int pageSize, int pageNumber)
        {
            return await _walutyDBContext.Currencies.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<Currency>> GetAllCurrencies(int pageSize, int pageNumber, string filter)
        {
            return await _walutyDBContext.Currencies
                                         .Where(x => x.Name.ToLower().Contains(filter.ToLower()))
                                         .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<List<CurrencyInfo>> GetAllCurrencyInfo()
        {
            return await _walutyDBContext.CurrencyInfos.ToListAsync();
        }

        public async Task<IPagedList<CurrencyInfo>> GetAllCurrencyInfo(int pageSize, int pageNumber)
        {
            return await _walutyDBContext.CurrencyInfos.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<CurrencyInfo>> GetAllCurrencyInfo(int pageSize, int pageNumber, string filter)
        {
            return await _walutyDBContext.CurrencyInfos.Where(x => x.Code.Contains(filter.ToUpper())).ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Currency> GetCurrency(string currencyCode)
        {
            return await _walutyDBContext.Currencies.Include(lor => lor.ListOfRecords)
                                                    .SingleAsync(x => x.Name.ToLower() == currencyCode.ToLower()); ;
        }

        public async Task<Currency> GetCurrency(int currencyId)
        {
            return await _walutyDBContext.Currencies.Include(lor => lor.ListOfRecords)
                                                    .SingleAsync(x => x.Id == currencyId); ;
        }

        public async Task<List<CurrencyRecord>> GetCurrencyRecordsBtwnDates(string currencyCode, DateTime begDate, DateTime endDate)
        {
            return await _walutyDBContext.Currencies.Single(c => c.Name.ToLower() == currencyCode.ToLower())
                    .ListOfRecords.Where(cr => cr.Date >= begDate && cr.Date <= endDate).ToListAsync();
        }
    }
}
