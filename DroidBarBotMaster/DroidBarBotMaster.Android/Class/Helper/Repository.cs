using DroidBarBotMaster.Droid.Class.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    public static class Repository
    {

        public static SortedDictionary<string, DrinkMultiple> repositoryDictionery = new SortedDictionary<string, DrinkMultiple>();

        private const string fileNameString = "RepositorySave.txt";

        public static void SaveData()
        {

            string serilized = JsonConvert.SerializeObject(repositoryDictionery);

            string path = Android.OS.Environment.ExternalStorageDirectory.ToString() ;

            string filename = Path.Combine(path, fileNameString);

            File.WriteAllText(filename, serilized);

        }

        public static void AddDrinkMultiple(DrinkMultiple newDrinkMultiple, string newSearchedIngridient)
        {
            repositoryDictionery.Add(newSearchedIngridient, newDrinkMultiple);
        }

        public static SortedDictionary<string, DrinkMultiple> GetSavedData()
        {

            string path = Android.OS.Environment.ExternalStorageDirectory.ToString();

            string filename = Path.Combine(path, fileNameString);

            string content;

            if (!File.Exists(filename)) return null;

            using (var streamReader = new StreamReader(filename))
            {
                content = streamReader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(content);
            }

            if (string.IsNullOrEmpty(content)) return null;

            repositoryDictionery = JsonConvert.DeserializeObject<SortedDictionary<string, DrinkMultiple>>(content);

            return repositoryDictionery;
        }
        
    }
}
