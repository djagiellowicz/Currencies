using System;
using System.Collections.Generic;
using System.Text;

namespace WalutyBusinessLogic.DatabaseLoading.Updater
{
    public interface ICurrencyFilesUnzipper
    {
        bool UnzipFile(string fileName, string filePath);
    }
}
