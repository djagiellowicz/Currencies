namespace WalutyBusinessLogic.Services
{
    public interface ICurrencyNameChecker
    {
        bool AreDifferent(string firstCurrencyName, string secondCurrencyName);
    }
}