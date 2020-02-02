using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesDownloader : ICurrencyFilesDownloader
    {
        // Could be put inside file, not hardcoded
        private readonly string _databaseZipFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.zip";
        private readonly string _databaseContentFileLink = @"https://info.bossa.pl/pub/fundinwest/omega/omegafun.lst";
        // Change Path
        private readonly string _pathToDirectory = @"c:\test\";

        public CurrencyFilesDownloader()
        {
          
        }

        public async Task<bool> DownloadFilesAsync()
        {
            bool result = true;
            WebClient webClient = new WebClient();

            try
            {
               await webClient.DownloadFileTaskAsync(new Uri(_databaseContentFileLink), _pathToDirectory + "content.lst");
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
                await webClient.DownloadFileTaskAsync(new Uri(_databaseZipFileLink), _pathToDirectory + "database.zip");
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
