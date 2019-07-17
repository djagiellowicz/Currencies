using System.Threading.Tasks;

namespace WalutyBusinessLogic.Services
{
    public interface IDateRange
    {
        Task<string> GetCurrencyDateRange(string currencyCode);
        Task<string> GetCommonDateRangeForTwoCurrencies(string firstCurrencyCode, string secondCurrencyCode);
    }
}