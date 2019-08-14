using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public class ChartService :IChartService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public ChartService(ICurrencyRepository currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }
        public async Task<ChartModel> CreateChartModel(string currencyCode)
        {
            var currency = await _currencyRepository.GetCurrency(currencyCode);

            return new ChartModel(currency.Name, currency.ListOfRecords);
        }
    }
}
