using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Serilog;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesUnzipper : ICurrencyFilesUnzipper
    {

        public bool UnzipFile(string fileToExtractName, string filePath)
        {
            string zipFileName = Path.Combine(filePath, fileToExtractName);

            FastZip fastZip = new FastZip();
            string fileFilter = null;

            // Will always overwrite if target filenames already exist
            try
            {
                fastZip.ExtractZip(zipFileName, filePath, fileFilter);
                Log.Logger.Information($"Extracted database content file to {filePath}");
                return true;
  
            }
            catch (IOException e)
            {
                Log.Logger.Error("Couldn't extract database content file");
                Log.Logger.Error(e.Message);
                if(e.InnerException != null)
                {
                    Log.Logger.Error(e.InnerException.Message);
                }
    
            }

            return false;
        }


    }

    
}
