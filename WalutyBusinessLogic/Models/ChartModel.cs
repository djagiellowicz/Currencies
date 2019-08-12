
using System.Collections.Generic;
using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.Models
{
    public class ChartModel
    {
        public List<CurrencyRecord> CurrencyRecords { get; set; }
        public string CurrencyCode { get; set; }

        public ChartModel(string currencyCode, List<CurrencyRecord> currencyRecords)
        {
            CurrencyCode = currencyCode;
            CurrencyRecords = currencyRecords;
        }
    }
}
