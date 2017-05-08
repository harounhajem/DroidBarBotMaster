using Polenter.Serialization;
using System.Collections.Generic;


namespace LocalRepository
{
    public static class Repository
    {

        private static SortedDictionary<string, DrinkMultiple> repository = new SortedDictionary<string, DrinkMultiple>();

        private static string fileName = "RepositorySave.xml";

        public static void SaveData(DrinkMultiple newDrinkMultiple, string newSearchedIngridient)
        {
            var serilizer = new SharpSerializer();

            repository.Add(newSearchedIngridient, newDrinkMultiple);

            serilizer.Serialize(repository, fileName);

        }

        public static SortedDictionary<string, DrinkMultiple> GetSavedData()
        {
            var serilizer = new SharpSerializer();

            repository = (SortedDictionary<string, DrinkMultiple>)serilizer.Deserialize(fileName);

            return repository;
        }
        
    }
}
