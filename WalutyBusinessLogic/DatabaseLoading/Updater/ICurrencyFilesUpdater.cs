using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public interface ICurrencyFilesUpdater
    {
        bool Process(WalutyDBContext context);
    }
}
