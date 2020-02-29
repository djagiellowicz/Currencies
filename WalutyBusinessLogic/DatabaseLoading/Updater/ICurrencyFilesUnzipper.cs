
namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public interface ICurrencyFilesUnzipper
    {
        bool UnzipFile(string fileName, string filePath);
    }
}
