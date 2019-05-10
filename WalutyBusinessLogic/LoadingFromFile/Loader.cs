﻿using System;
using System.Collections.Generic;
using System.IO;


namespace WalutyBusinessLogic.LoadingFromFile
{
    public class Loader : ILoader
    {
        public List<Currency> AllCurrencies { get; set; }
        private string PathToDirectory = @"WalutyBusinessLogic\LoadingFromFile\FilesToLoad\omeganbp";
        private string Separator = ",";

        public void Init()
        {
            if (AllCurrencies != null && AllCurrencies.Count > 0)
            {
                // Loader was already initialized.
                return;
            }

            this.AllCurrencies = GetListOfAllCurrencies();
        }

        public Currency LoadCurrencyFromFile(string fileName)
        {
            List<string> linesFromFile = LoadLinesFromFile(fileName);
            return GetCurrency(linesFromFile);
        }

        public List<Currency> GetListOfAllCurrencies()
        {
            List<Currency> currencies = new List<Currency>();

            foreach (string currencyFileName in GetAvailableTxtFileNames())
            {
                currencies.Add(LoadCurrencyFromFile(currencyFileName));
            }
            return currencies;
        }

        public List<string> GetAvailableTxtFileNames()
        {
            string pathToDirectory = GetCurrenciesFolderPath();
            string[] filePaths = Directory.GetFiles(pathToDirectory, "*.txt", SearchOption.TopDirectoryOnly);
            List<string> listOfFileNames = new List<string>();

            foreach (string line in filePaths)
            {
                listOfFileNames.Add(Path.GetFileName(line));
            }

            return listOfFileNames;
        }

        private string GetCurrenciesFolderPath()
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, $"{PathToDirectory}");
        }

        private List<string> LoadLinesFromFile(string fileName)
        {
            StreamReader streamReader;
            List<string> listOfLines = new List<string>();

            var pathToFile = Path.Combine(GetCurrenciesFolderPath(), fileName);

            if (File.Exists(pathToFile))
            {
                streamReader = File.OpenText(pathToFile);
            }
            else
            {
                throw new FileLoadException();
            }

            //Ignore first line from currenty data
            if (!streamReader.EndOfStream)
            {
                streamReader.ReadLine();
            }

            while (!streamReader.EndOfStream)
            {
                listOfLines.Add(streamReader.ReadLine());
            }

            return listOfLines;

        }

        private Currency GetCurrency(List<string> listOfLines)
        {
            Currency currency = new Currency();

            for (int i = 0; i < listOfLines.Count; i++)
            {
                CurrencyRecord currencyRecord = new CurrencyRecord();
                var splittedLine = listOfLines[i].Split(Separator);

                if (i == 0)
                {
                    currency.Name = splittedLine[0];
                }
                try
                {
                    currencyRecord.Date = DateTime.ParseExact(splittedLine[1], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    currencyRecord.Open = float.Parse(splittedLine[2].Replace(".", ","));
                    currencyRecord.High = float.Parse(splittedLine[3].Replace(".", ","));
                    currencyRecord.Low = float.Parse(splittedLine[4].Replace(".", ","));
                    currencyRecord.Close = float.Parse(splittedLine[5].Replace(".", ","));
                    currencyRecord.Volume = float.Parse(splittedLine[6].Replace(".", ","));
                }
                catch (FormatException e)
                {
                    Console.WriteLine("error loading file at line: " + i);
                    Console.WriteLine(e.Message);
                }
                currency.ListOfRecords.Add(currencyRecord);
            }
            return currency;
        }
    }

}
