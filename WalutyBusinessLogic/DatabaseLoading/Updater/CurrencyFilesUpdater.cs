using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesUpdater : ICurrencyFilesUpdater
    {
        // Can be put inside file, not hardcoded
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
        public bool Process(ICurrencyFilesDownloader downloader, ICurrencyFilesUnzipper unzipper, ILoader loader, ICurrencyRepository repository)
        {
            DateTime currentDate = DateTime.Now;
            string fullPathToDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + _pathToInternalDirectory + currentDate.ToString("ddMMyyyy") + @"\";
            bool downloaderResult = false;
            bool unzipperResult = false;
            IList<Currency> loadedCurrencies = new List<Currency>();

            downloaderResult = downloader.DownloadFilesAsync(_databaseZipFileLink, _databaseContentFileLink, fullPathToDirectory, currentDate, _contentFileName, _databaseFileName).Result;
            unzipperResult = unzipper.UnzipFile(_databaseFileName, fullPathToDirectory);
             
            if(downloaderResult && unzipperResult)
            {
                loadedCurrencies = loader.GetListOfAllCurrencies(fullPathToDirectory);
                UpdateCurrencies(loadedCurrencies, repository);
                return true;
            }

            return false;
        }

        private bool UpdateCurrencies(IList<Currency> loadedCurrencies, ICurrencyRepository repository)
        {
            foreach(Currency currency in loadedCurrencies)
            {
                
            }

            return false;
        }
    }
}
