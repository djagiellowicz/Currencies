using System.Threading.Tasks;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.CurrenciesComparision
{
    public interface ICurrenciesComparator
    {
        Task<CurrenciesComparatorModel> CompareCurrencies(CurrenciesComparatorModel model);
    }
}