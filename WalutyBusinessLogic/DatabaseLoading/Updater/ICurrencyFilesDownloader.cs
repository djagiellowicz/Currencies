using System.Threading.Tasks;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public interface ICurrencyFilesDownloader
    {
        Task<bool> DownloadFilesAsync();
    }
}
