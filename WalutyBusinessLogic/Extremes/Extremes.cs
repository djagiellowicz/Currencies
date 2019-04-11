﻿using System.Collections.Generic;
using System.Linq;
using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.Extremes
{
    public class Extremes
    {
        public ExtremeValue GetGlobalExtremes(string nameCurrency)
        {
            ExtremeValue extremeValue = new ExtremeValue();
            Currency currency = Loader.GetLoaderInstance().LoadCurrencyFromFile(nameCurrency);
            List<CurrencyRecord> listOfRecords = currency.ListOfRecords;
            extremeValue.MaxValue = listOfRecords.Max(c=> c.High);
            extremeValue.MinValue = listOfRecords.Min(c => c.Low);
            return extremeValue;
        }
   
        public ExtremeValue GetLocalExtremes(string nameCurrency, int startDate, int endDate)
        {
            ExtremeValue extremeValue = new ExtremeValue();
            Currency currency = Loader.GetLoaderInstance().LoadCurrencyFromFile(nameCurrency);
            List<CurrencyRecord> listOfRecords = currency.ListOfRecords;
            extremeValue.MaxValue = listOfRecords.Where(c => c.Date >= startDate && c.Date <= endDate)
                .Max(c => c.High);
            extremeValue.MinValue = listOfRecords.Where(c => c.Date >= startDate && c.Date <= endDate)
                .Min(c => c.Low);
            return extremeValue;
        }
    }
}
