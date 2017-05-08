using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using DroidBarBotMaster.Droid.Class.Model;

namespace DroidBarBotMaster.Droid.Class.Service
{
    public static class CocktailDBService
    {
        public static DrinkMultiple HttpGet(string stringGet, HttpGetRequests enumRequest)
        {

            string html = string.Empty;

            string requestApi = "empty";

            switch (enumRequest)
            {
                case HttpGetRequests.CocktailByIngridient:
                    requestApi = "filter";
                    break;
                case HttpGetRequests.CocktailByID:
                    requestApi = "lookup";
                    break;
                case HttpGetRequests.IngridientInfo:
                    requestApi = "search";
                    break;
                default:
                    break;
            }

            string url = $"http://www.thecocktaildb.com/api/json/v1/1/{requestApi}.php?i={stringGet}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Accept = "application/json";

            DrinkMultiple drinks = new DrinkMultiple();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
                drinks = JsonConvert.DeserializeObject<DrinkMultiple>(html);
            }

            return drinks;
        }

        public static List<DrinkMultiple> getAllDrinks(List<String> drinkNames)
        {

            List<DrinkMultiple> availableDrinks = new List<DrinkMultiple>();

            foreach (var stringDrinkName in drinkNames)
            {
                // Get all drinks accosieated with the ingridient
                // ! Only shallow information ! 
                DrinkMultiple tempDrinks = HttpGet(stringDrinkName, HttpGetRequests.CocktailByIngridient);

                if (tempDrinks == null) continue;



                // Get all drinks details based on id

                for (int i = 0; i < tempDrinks.Drinks.Count; i++)
                {

                    string drinkID = tempDrinks.Drinks[i].idDrink.ToString();

                    
                    tempDrinks.Drinks[i] = HttpGet(drinkID, HttpGetRequests.CocktailByID).Drinks[0];

                }

                availableDrinks.Add(tempDrinks);

            }

            return availableDrinks;
        }


        public static DrinkMultiple getAllDrinks(String drinkNames)
        {

                // Get all drinks accosieated with the ingridient
                // ! Only shallow information ! 
                DrinkMultiple tempDrinks = HttpGet(drinkNames, HttpGetRequests.CocktailByIngridient);

                if (tempDrinks == null) return null;

                // Get all drinks details based on id

                for (int i = 0; i < tempDrinks.Drinks.Count; i++)
                {

                    string drinkID = tempDrinks.Drinks[i].idDrink.ToString();


                    tempDrinks.Drinks[i] = HttpGet(drinkID, HttpGetRequests.CocktailByID).Drinks[0];

                }

            return tempDrinks;
        }





        public static List<DrinkMultiple> getAllDrinksShallow(List<String> drinkNames)
        {

            List<DrinkMultiple> availableDrinks = new List<DrinkMultiple>();

            foreach (var stringDrinkName in drinkNames)
            {
                // Get all drinks accosieated with the ingridient
                // ! Only shallow information ! 
                DrinkMultiple tempDrinks = HttpGet(stringDrinkName, HttpGetRequests.CocktailByIngridient);

                if (tempDrinks == null) continue;

                availableDrinks.Add(tempDrinks);

            }

            return availableDrinks;
        }

        public static bool checkIfCanBeMixed(Drink drink, int amountToBeMixed, List<string> bottleNameList)
        {
            int counterCanBeMixed = 0;

            foreach (string bottleName in bottleNameList)
            {

                foreach (string strIngridients in drink.GetStrIngredientsList())
                {

                    if (strIngridients != null && strIngridients.ToLower().Contains(bottleName.ToLower()))
                    {
                        counterCanBeMixed++;
                        break;
                    }

                }
            }

            if (counterCanBeMixed >= amountToBeMixed)
            {
                return true;
            }

            return false;

        }

        public static List<DrinkMultiple> MixableDrinksFiltered(List<DrinkMultiple> availableDrinks, List<String> drinkNames, int amountToBeMixed)
        {
            List<DrinkMultiple> filteredDrinkList = new List<DrinkMultiple>();

            DrinkMultiple tempDrinkMultiple = new DrinkMultiple();

            foreach (DrinkMultiple drinksMultiple in availableDrinks)
            {

                List<Drink> tempDrinks = new List<Drink>();

                tempDrinkMultiple.Drinks = tempDrinks;

                foreach (Drink drink in drinksMultiple.Drinks)
                {

                    bool canBeMixed = checkIfCanBeMixed(drink, amountToBeMixed, drinkNames);

                    if (canBeMixed)
                    {

                        tempDrinkMultiple.Drinks.Add(drink);

                    }
                }
            }

            filteredDrinkList.Add(tempDrinkMultiple);

            return filteredDrinkList;
        }


    }

    public enum HttpGetRequests
    {
        CocktailByIngridient,
        CocktailByID,
        IngridientInfo

    }
}