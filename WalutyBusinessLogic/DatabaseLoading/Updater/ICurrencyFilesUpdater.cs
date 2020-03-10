using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public interface ICurrencyFilesUpdater
    {
        bool Process(ICurrencyFilesDownloader downloader, ICurrencyFilesUnzipper unzipper, ILoader loader, ICurrencyRepository repository);
    }
}
