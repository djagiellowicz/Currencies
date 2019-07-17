using System;
using System.Threading.Tasks;

namespace WalutyBusinessLogic.Services
{
    public interface IDateChecker
    {
        Task<bool> CheckIfDateExistsForTwoCurrencies(DateTime dateCurrency, string firstNameCurrency,
            string secondNameCurrency);

        Task<bool> CheckIfDateExistInRange(DateTime firstDate, DateTime secondDate, string currencyName);
    }
}