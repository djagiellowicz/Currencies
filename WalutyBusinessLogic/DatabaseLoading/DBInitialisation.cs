using System.Linq;
using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public static class DBInitialisation
    {
        public static void InitialiseDB(WalutyDBContext context, ILoader loader)
        {
            if (!context.Currencies.Any() && !context.CurrencyInfos.Any())
            {
            context.AddRange(loader.GetListOfAllCurrencies());
            context.AddRange(loader.LoadCurrencyInformation());
            context.SaveChanges();
            }
        }
    }
}
