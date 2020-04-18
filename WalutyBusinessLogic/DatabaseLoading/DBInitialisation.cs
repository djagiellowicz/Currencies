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
            context.AddRange(loader.GetListOfAllCurrencies(loader.PathToDirectory));
            context.AddRange(loader.LoadCurrencyInformation(loader.PathToDirectory));
            context.SaveChanges();
            }
        }
    }
}
