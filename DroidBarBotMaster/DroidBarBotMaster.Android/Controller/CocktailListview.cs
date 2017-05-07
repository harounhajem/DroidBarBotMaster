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

            List<DrinkMultiple> available = new List<DrinkMultiple>();

            List<String> drinkNames = new List<string>() {"tequila", "vodka", "lime" };


            // TODO: Activate filter & Get real ingridients

            //List<DrinkMultiple> availableDrinks = CocktailDBService.getAllDrinks(drinkNames);
            List<DrinkMultiple> availableDrinks = CocktailDBService.getAllDrinksShallow(drinkNames);

            //List<DrinkMultiple> availableDrinksMixFiltered = CocktailDBService.MixableDrinksFiltered(availableDrinks, drinkNames, drinkNames.Count);

            //FindViewById<Button>(Resource.Id.cocktailListView).AddChildrenForAccessibility(drinkNames);

            ListView listView = FindViewById<ListView>(Resource.Id.cocktailListView);


            //ArrayAdapter<Drink> adapter = new ArrayAdapter<Drink>(this, Resource.Layout.XMLFile1, availableDrinks[0].Drinks);

            List<Drink> allDrinks = new List<Drink>();

            foreach (var item in availableDrinks)
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