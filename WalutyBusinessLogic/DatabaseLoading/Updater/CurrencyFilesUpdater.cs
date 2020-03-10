using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public bool Process(ICurrencyFilesDownloader downloader, ICurrencyFilesUnzipper unzipper, ILoader loader, WalutyDBContext context)
        {
            DateTime currentDate = DateTime.Now;
            string fullPathToDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + _pathToInternalDirectory + currentDate.ToString("ddMMyyyy") + @"\";
            bool downloaderResult = false;
            bool unzipperResult = false;
            bool updateResult = false;
            IList<Currency> loadedCurrencies = new List<Currency>();

            downloaderResult = downloader.DownloadFilesAsync(_databaseZipFileLink, _databaseContentFileLink, fullPathToDirectory, currentDate, _contentFileName, _databaseFileName).Result;
            unzipperResult = unzipper.UnzipFile(_databaseFileName, fullPathToDirectory);
             
            if(downloaderResult && unzipperResult)
            {
                loadedCurrencies = loader.GetListOfAllCurrencies(fullPathToDirectory);

                updateResult = UpdateCurrencies(loadedCurrencies, context);

                return updateResult;
            }

            return false;
        }

        private bool UpdateCurrencies(IList<Currency> loadedCurrencies, WalutyDBContext context)
        {
            DbSet<Currency> currenciesDbSet = context.Currencies;

            try
            {
                foreach (Currency currency in loadedCurrencies)
                {
                    var currentCurrency = currenciesDbSet.SingleOrDefault(x => x.Name.ToLower() == currency.Name.ToLower());
                    var latestCurrencyDate = currentCurrency.ListOfRecords.Max(x => x.Date);

                    var currencyRecordsToUpdate = currency.ListOfRecords.Where(x => x.Date > latestCurrencyDate).ToList();

                    currentCurrency.ListOfRecords.AddRange(currencyRecordsToUpdate);

                }
                context.SaveChanges();
                return true;
            }
            catch (DbUpdateException e)
            {
                Log.Logger.Error("Couldn't update the database");
                Log.Logger.Error(e.Message);
            }

            return false;
        }
    }
}
