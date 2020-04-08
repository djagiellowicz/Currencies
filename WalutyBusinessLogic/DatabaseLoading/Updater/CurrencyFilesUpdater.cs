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
        // Can be put inside file, not hardcoded. I've left it this way, but it should be changed.
        private readonly string _databaseZipFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.zip";
        private readonly string _databaseContentFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.lst";
        private readonly string _fileFolderName = "Files";
        private readonly string _contentFileName = "content.lst";
        private readonly string _databaseFileName = "database.zip";
        private readonly ICurrencyFilesDownloader _downloader;
        private readonly ICurrencyFilesUnzipper _unzipper;
        private readonly ILoader _loader;

        public CurrencyFilesUpdater(ICurrencyFilesDownloader downloader, ICurrencyFilesUnzipper unzipper, ILoader loader)
        {
            _downloader = downloader;
            _unzipper = unzipper;
            _loader = loader;

        }
        // Prevents method from running multiple times at once
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Process(WalutyDBContext context)
        {
            DateTime currentDate = DateTime.Now;
            string fullPathToDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName,
                                                      _fileFolderName,
                                                      currentDate.ToString("ddMMyyyy") + @"\");
            bool downloaderResult = false;
            bool unzipperResult = false;
            bool updateResult = false;
            IList<Currency> loadedCurrencies = new List<Currency>();

            downloaderResult = _downloader.DownloadFilesAsync(_databaseZipFileLink, _databaseContentFileLink, fullPathToDirectory, currentDate, _contentFileName, _databaseFileName).Result;
            unzipperResult = _unzipper.UnzipFile(_databaseFileName, fullPathToDirectory);
             
            if(downloaderResult && unzipperResult)
            {
                loadedCurrencies = _loader.GetListOfAllCurrencies(fullPathToDirectory);

                updateResult = UpdateCurrencies(loadedCurrencies, context);

                return true;
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
                    var currentCurrency = currenciesDbSet.Include(x => x.ListOfRecords).SingleOrDefault(x => x.Name.ToLower() == currency.Name.ToLower());
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
