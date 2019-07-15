using System.Threading.Tasks;

namespace WalutyBusinessLogic.Services
{
    public interface IDateRange
    {
        Task<string> GetDateRangeCurrency(string currencyCode);
        Task<string> GetDateRangeTwoCurrencies(string firstCurrencyCode, string secondCurrencyCode);
    }
}