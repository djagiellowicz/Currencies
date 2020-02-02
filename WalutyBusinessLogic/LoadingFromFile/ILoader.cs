using System.Collections.Generic;

namespace WalutyBusinessLogic.LoadingFromFile
{
    public interface ILoader
    {
        /// <summary>
        /// This method should not be called manually.
        /// </summary>
        void Init();
        string PathToDirectory { get; }
        List<Currency> AllCurrencies { get; set; }
        Currency LoadCurrencyFromFile(string fileName, string receivedPathToDirectory);
        List<CurrencyInfo> LoadCurrencyInformation(string receivedPathToDirectory);
        List<Currency> GetListOfAllCurrencies(string receivedPathToDirectory);
        List<string> GetAvailableTxtFileNames(string receivedPathToDirectory);
    }
}
