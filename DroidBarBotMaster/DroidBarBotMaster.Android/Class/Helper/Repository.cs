using DroidBarBotMaster.Droid.Class.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    public static class Repository
    {

        private static SortedDictionary<string, DrinkMultiple> repository = new SortedDictionary<string, DrinkMultiple>();

        private const string fileNameString = "RepositorySave.txt";

        public static void SaveData(DrinkMultiple newDrinkMultiple, string newSearchedIngridient)
        {

            repository.Add(newSearchedIngridient, newDrinkMultiple);

            string serilized = JsonConvert.SerializeObject(repository);

            string path = Android.OS.Environment.ExternalStorageDirectory.ToString() ;

            string filename = Path.Combine(path, fileNameString);

            File.WriteAllText(filename, serilized);

        }

        public static SortedDictionary<string, DrinkMultiple> GetSavedData()
        {

            string path = Android.OS.Environment.ExternalStorageDirectory.ToString();

            string filename = Path.Combine(path, fileNameString);

            string content;

            using (var streamReader = new StreamReader(filename))
            {
                content = streamReader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(content);
            }

            if (string.IsNullOrEmpty(content)) return null;

            repository = JsonConvert.DeserializeObject<SortedDictionary<string, DrinkMultiple>>(content);

            return repository;
        }
        
    }
}
