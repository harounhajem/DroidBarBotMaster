using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalRepository
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedDictionary<string, DrinkMultiple> repository = new SortedDictionary<string, DrinkMultiple>();



            var drinkMulti = new DrinkMultiple();


            var drink = new Drink();

            drink.idDrink = 12345;

            drink.strDrink = "Hello World22";

            drink.strIngredient1 = "Lemon";



            drinkMulti.Drinks = new List<Drink>() { drink };


            Repository.SaveData(drinkMulti, "Lemon");


            repository = Repository.GetSavedData();
        }


    }
}
