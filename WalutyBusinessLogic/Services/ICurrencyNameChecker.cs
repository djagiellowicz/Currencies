namespace WalutyBusinessLogic.Services
{
    public interface ICurrencyNameChecker
    {
        bool CheckingIfCurrencyNamesAreDifferent(string firstCurrencyName, string secondCurrencyName);
    }
}