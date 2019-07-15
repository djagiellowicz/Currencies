namespace WalutyBusinessLogic.Services
{
    public class CurrencyNameChecker : ICurrencyNameChecker
    {
        public bool CheckingIfCurrencyNamesAreDifferent(string firstCurrencyName, string secondCurrencyName)
        {
            if (firstCurrencyName != secondCurrencyName) return true;
            else return false;
        }
    }
}
