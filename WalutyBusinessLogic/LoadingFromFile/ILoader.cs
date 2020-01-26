using System.Collections.Generic;

namespace WalutyBusinessLogic.LoadingFromFile
{
    public interface ILoader
    {
        /// <summary>
        /// This method should not be called manually.
        /// </summary>
        void Init();
        List<Currency> AllCurrencies { get; set; }
        Currency LoadCurrencyFromFile(string fileName);
        List<CurrencyInfo> LoadCurrencyInformation();
        List<Currency> GetListOfAllCurrencies();
        List<string> GetAvailableTxtFileNames();
    }
}
