using System;
using System.IO;
using System.Runtime.CompilerServices;


namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesUpdater : ICurrencyFilesUpdater
    {
        // Could be put inside file, not hardcoded
        private readonly string _databaseZipFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.zip";
        private readonly string _databaseContentFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.lst";
        private readonly string _pathToInternalDirectory = @"\Currencies\WalutyBusinessLogic\DatabaseLoading\Updater\Files\";
        private readonly string _contentFileName = "content.lst";
        private readonly string _databaseFileName = "database.zip";

        public CurrencyFilesUpdater()
        {

        }
        // Prevents method from running multiple times at once
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Process(ICurrencyFilesDownloader downloader, ICurrencyFilesUnzipper unzipper)
        {
            DateTime currentDate = DateTime.Now;
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string fullPathToDirectory = projectDirectory + _pathToInternalDirectory + currentDate.ToString("ddMMyyyy") + @"\";
            bool downloaderResult = false;
            bool unzipperResult = false;

            downloaderResult = downloader.DownloadFilesAsync(_databaseZipFileLink, _databaseContentFileLink, fullPathToDirectory, currentDate, _contentFileName, _databaseFileName).Result;
          


            return false;
        }
    }
}
