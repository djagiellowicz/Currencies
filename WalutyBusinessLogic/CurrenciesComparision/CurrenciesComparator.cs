using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.CurrenciesComparision
{
    public class CurrenciesComparator : ICurrenciesComparator
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrenciesComparator(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<CurrenciesComparatorModel> CompareCurrencies(CurrenciesComparatorModel model)
        {
            Currency firstCurrency = await _currencyRepository.GetCurrency(model.FirstCurrencyCode);
            Currency secondCurrency = await _currencyRepository.GetCurrency(model.SecondCurrencyCode);

            CurrencyRecord firstCurrencyRecord =
                firstCurrency.ListOfRecords.Single(currency => currency.Date == model.Date);
            CurrencyRecord secondCurrencyRecord =
                secondCurrency.ListOfRecords.Single(currency => currency.Date == model.Date);

            float firstCloseValue = firstCurrencyRecord.Close;
            float secondCloseValue = secondCurrencyRecord.Close;

            float comparision = firstCloseValue / secondCloseValue;

            model.ComparatorResult = $"In day {model.Date.ToShortDateString()} {firstCurrency.Name} is worth {comparision} {secondCurrency.Name}";
            return model;
        }
    }
}
