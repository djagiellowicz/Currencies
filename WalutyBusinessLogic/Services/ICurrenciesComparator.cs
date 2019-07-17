using System.Threading.Tasks;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public interface ICurrenciesComparator
    {
        Task<CurrenciesComparatorModel> CompareCurrencies(CurrenciesComparatorModel model);
    }
}