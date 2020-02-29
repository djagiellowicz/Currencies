using System.IO;
using ICSharpCode.SharpZipLib.Zip;


namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesUnzipper : ICurrencyFilesUnzipper
    {

        public bool UnzipFile(string fileToExtractName, string filePath)
        {
            string filePathWithName = Path.Combine(filePath + fileToExtractName);

            FastZip fastZip = new FastZip();
            string fileFilter = null;

            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(filePathWithName, filePath, fileFilter);

            return false;
        }


    }

    
}
