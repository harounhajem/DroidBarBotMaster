using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DroidBarBotMaster.Droid.Class.Service;
using DroidBarBotMaster.Droid.Class.Helper;
using DroidBarBotMaster.Droid.Class.Model;
using DroidBarBotMaster.Droid.Controller;

namespace DroidBarBotMaster.Droid
{
    [Activity(Label = "CocktailListview")]
    public class CocktailListview : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CocktailListview);

        }



        protected override void OnStart()
        {
            base.OnStart();

            
            // TODO: Change MockData to real

            List<String> drinkNames = new List<string>() { "tequila", "vodka", "lime" };


            // Filter and update available drinks

            UpdateRepositoryData(drinkNames);


            var drinkMultiple = (from drinkName in drinkNames
                                 where TransporterClass.Repository.ContainsKey(drinkName)
                                 select TransporterClass.Repository[drinkName]).ToList<DrinkMultiple>();

            List<DrinkMultiple> availableDrinksMixFiltered = CocktailDBService.MixableDrinksFiltered(drinkMultiple, drinkNames, 2);






            ListView listView = FindViewById<ListView>(Resource.Id.cocktailListView);

            List<Drink> allDrinks = new List<Drink>();

            foreach (var item in availableDrinksMixFiltered)
            {
                allDrinks.AddRange(item.Drinks);
            }

            listAdapterDrink adapter = new listAdapterDrink(this, allDrinks);
            listView.Adapter = adapter;

            listView.ItemClick += listView_ItemClick;

            #region How to Connect

            // TODO: Dev deactivate

            //foreach (var item in TransporterClass.listContainer)
            //{
            //    DrinkMultiple tempDrinks = CocktailDBService.HttpGet(item.Name, HttpGetRequests.CocktailByIngridient);

            //    if (tempDrinks != null)
            //    {
            //        available.Add(tempDrinks);
            //    }
            //}

            //Console.WriteLine(available); 
            #endregion


        }

        private SortedDictionary<string, DrinkMultiple> UpdateRepositoryData(List<string> drinkNames)
        {
            bool changed = false;

            // Repository does not exist
            if (TransporterClass.Repository == null)
            {
                foreach (var item in drinkNames)
                {
                    DrinkMultiple availableDrinks = CocktailDBService.getAllDrinks(item);

                    Repository.AddDrinkMultiple(availableDrinks, item);

                    changed = true;
                }

            }
            else
            {
                // Repository exists but checks for new data

                foreach (string drinkItem in drinkNames)
                {
                    if (!TransporterClass.Repository.ContainsKey(drinkItem))
                    {

                        DrinkMultiple missingDrinkMultiple = CocktailDBService.getAllDrinks(drinkItem);

                        Repository.AddDrinkMultiple(missingDrinkMultiple, drinkItem);

                        changed = true;

                    }
                }
            }

            if (changed)
            {
                Repository.SaveData();

                TransporterClass.Repository = Repository.repositoryDictionery;
            }


            return TransporterClass.Repository;
        }

        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            ListView listView = sender as Android.Widget.ListView;

            if (listView == null) return;

            listAdapterDrink listAdapterDrink = listView.Adapter as listAdapterDrink;

            if (listAdapterDrink == null) return;

            Drink drink = listAdapterDrink.listDrink[e.Position];

            // TODO: DEV ONLY :  Remove this? Or you can have this left?
            Drink detailDrink = CocktailDBService.HttpGet(drink.idDrink.ToString(), HttpGetRequests.CocktailByID).Drinks[0];

            ChangePage(detailDrink);
        }

        private void ChangePage(Drink selectedDrink)
        {
            var activity = new Intent(this, typeof(DrinkOrder));

            TransporterClass.SelectedDrink = selectedDrink;

            StartActivity(activity);
        }
    }
}