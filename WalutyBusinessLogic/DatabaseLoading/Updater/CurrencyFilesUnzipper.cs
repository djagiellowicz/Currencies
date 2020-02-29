using System.IO;
using ICSharpCode.SharpZipLib.Zip;


namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public class CurrencyFilesUnzipper : ICurrencyFilesUnzipper
    {

        public bool UnzipFile(string fileName, string filePath)
        {
            string filePathName = Path.Combine(filePath + fileName);

            FastZip fastZip = new FastZip();
            string fileFilter = null;

            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(filePathName, filePath, fileFilter);

            return false;
        }


    }

    
}
