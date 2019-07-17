using System.Threading.Tasks;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public interface ICurrencyConversionService
    {
        Task<CurrencyConversionModel> CalculateCurrencyConversionAmount(CurrencyConversionModel currencyConversionModel);
    }
}