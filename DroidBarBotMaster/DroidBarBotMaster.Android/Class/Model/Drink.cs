using DroidBarBotMaster.Droid.Class.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidBarBotMaster.Droid.Class.Model
{
    public class Drink
    {
        public int idDrink { get; set; }
        public string strDrink { get; set; }
        public string strCategory { get; set; }
        public string strAlcoholic { get; set; }
        public string strGlass { get; set; }
        public string strInstructions { get; set; }
        public string strDrinkThumb { get; set; }
        public string strIngredient1 { get; set; }
        public string strIngredient2 { get; set; }
        public string strIngredient3 { get; set; }
        public string strIngredient4 { get; set; }
        public string strIngredient5 { get; set; }
        public string strIngredient6 { get; set; }
        public string strIngredient7 { get; set; }
        public string strIngredient8 { get; set; }
        public string strIngredient9 { get; set; }
        public string strIngredient10 { get; set; }
        public string strIngredient11 { get; set; }
        public string strIngredient12 { get; set; }
        public string strIngredient13 { get; set; }
        public string strIngredient14 { get; set; }
        public string strIngredient15 { get; set; }
        public string strMeasure1 { get; set; }
        public string strMeasure2 { get; set; }
        public string strMeasure3 { get; set; }
        public string strMeasure4 { get; set; }
        public string strMeasure5 { get; set; }
        public string strMeasure6 { get; set; }
        public string strMeasure7 { get; set; }
        public string strMeasure8 { get; set; }
        public string strMeasure9 { get; set; }
        public string strMeasure10 { get; set; }
        public string strMeasure11 { get; set; }
        public string strMeasure12 { get; set; }
        public string strMeasure13 { get; set; }
        public string strMeasure14 { get; set; }
        public string strMeasure15 { get; set; }
        public string dateModified { get; set; }

        public List<string> strIngridientsList { get; private set; }

        public List<string> GetStrIngredientsList()
        {

            strIngridientsList = new List<string>()
            {
                strIngredient1,
                strIngredient2,
                strIngredient3,
                strIngredient4,
                strIngredient5,
                strIngredient6,
                strIngredient7,
                strIngredient8,
                strIngredient9,
                strIngredient10,
                strIngredient11,
                strIngredient12,
                strIngredient13,
                strIngredient14,
                strIngredient15
            };

            return strIngridientsList;
        }

        public SortedDictionary<string, int> GetStrIngridientsMeasurementDictionary()
        {
            SortedDictionary<string, int> drinkIngridientsSorted = new SortedDictionary<string, int>();

            
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient1, strMeasure1);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient2, strMeasure2);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient3, strMeasure3);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient4, strMeasure4);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient5, strMeasure5);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient6, strMeasure6);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient7, strMeasure7);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient8, strMeasure8);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient9, strMeasure9);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient10, strMeasure10);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient11, strMeasure11);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient12, strMeasure12);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient13, strMeasure13);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient14, strMeasure14);
            CheckForDublicatedKeyAndAdd(drinkIngridientsSorted, strIngredient15, strMeasure15);


            return drinkIngridientsSorted;

        }

        static void CheckForDublicatedKeyAndAdd(SortedDictionary<string, int> drinkIngridientsSorted, string ingridient, string measure)
        {
            if (String.IsNullOrEmpty(ingridient)) return;

            measure.Replace(System.Environment.NewLine, string.Empty);
            ingridient.Replace(System.Environment.NewLine, string.Empty);

            FractionalConverter fractConv = new FractionalConverter(measure);

            int amountParsed = Convert.ToInt32(fractConv.ResultCl);

            if (amountParsed <= 0) return;

            if (drinkIngridientsSorted.ContainsKey(ingridient))
            {
                drinkIngridientsSorted[ingridient] += amountParsed;
            }
            else
            {
                drinkIngridientsSorted.Add(ingridient, amountParsed);
            }
        }
    }
}
