﻿using Serilog;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesDownloader : ICurrencyFilesDownloader
    {
        // Could be put inside file, not hardcoded
        private readonly string _databaseZipFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.zip";
        private readonly string _databaseContentFileLink = @"https://info.bossa.pl/pub/waluty/omega/omeganbp.lst";
        private readonly string _pathToInternalDirectory = @"\Currencies\WalutyBusinessLogic\DatabaseLoading\Updater\Files\";

        public CurrencyFilesDownloader()
        {
          
        }

        public async Task<bool> DownloadFilesAsync()
        {
            bool result = true;
            WebClient webClient = new WebClient();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string fullPathToDirectory = projectDirectory + _pathToInternalDirectory + DateTime.Now.ToString("ddMMyyyy") + @"\";
;

            if (!Directory.Exists(fullPathToDirectory))
            {
                Directory.CreateDirectory(fullPathToDirectory);
            }

            try
            {
               await webClient.DownloadFileTaskAsync(new Uri(_databaseContentFileLink), fullPathToDirectory + "content.lst");
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
                await webClient.DownloadFileTaskAsync(new Uri(_databaseZipFileLink), fullPathToDirectory + "database.zip");
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
