using System;
using System.Threading.Tasks;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public interface ICurrencyFilesDownloader
    {
        Task<bool> DownloadFilesAsync(string databaseZipFileLink, string databaseContentFileLink, string fullPathToDirectory, DateTime date, string contentFileName, string databaseFileName);
    }
}
