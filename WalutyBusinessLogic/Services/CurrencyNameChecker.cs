namespace WalutyBusinessLogic.Services
{
    public class CurrencyNameChecker : ICurrencyNameChecker
    {
        public bool AreDifferent(string firstCurrencyName, string secondCurrencyName)
        {
            if (firstCurrencyName != secondCurrencyName) return true;
            else return false;
        }
    }
}
