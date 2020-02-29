using Serilog;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesDownloader : ICurrencyFilesDownloader
    {

        public CurrencyFilesDownloader()
        {
          
        }

        public async Task<bool> DownloadFilesAsync(string databaseZipFileLink, string databaseContentFileLink, string fullPathToDirectory,
                                                   DateTime date, string contentFileName, string databaseFileName)
        {
            bool result = true;
            WebClient webClient = new WebClient();

            if (!Directory.Exists(fullPathToDirectory))
            {
                Directory.CreateDirectory(fullPathToDirectory);
            }

            try
            {
               await webClient.DownloadFileTaskAsync(new Uri(databaseContentFileLink), fullPathToDirectory + contentFileName);
               Log.Logger.Information($"Downloaded files to: {fullPathToDirectory}");
            }
            catch (WebException e)
            {
                Log.Logger.Error("Couldn't download database content file");
                Log.Logger.Error(e.Message);
                Log.Logger.Error(e.InnerException.Message);
                result = false;
            }

            try
            {
                await webClient.DownloadFileTaskAsync(new Uri(databaseZipFileLink), fullPathToDirectory + databaseFileName);
                Log.Logger.Information($"Downloaded files to: {fullPathToDirectory}");
            }
            catch (WebException e)
            {
                Log.Logger.Error("Couldn't download database zip file");
                Log.Logger.Error(e.Message);
                Log.Logger.Error(e.InnerException.Message);
                result = false;
            }

            return result;
        }




    }
}
