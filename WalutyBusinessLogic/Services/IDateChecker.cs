using System;
using System.Threading.Tasks;

namespace WalutyBusinessLogic.Services
{
    public interface IDateChecker
    {
        Task<bool> CheckingIfDateExistsForTwoCurrencies(DateTime dateCurrency, string firstNameCurrency,
            string secondNameCurrency);

        Task<bool> CheckingIfDateExistInRange(DateTime firstDate, DateTime secondDate, string currencyName);
    }
}