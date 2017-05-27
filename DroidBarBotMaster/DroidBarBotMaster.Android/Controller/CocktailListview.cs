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
using static Java.Util.ResourceBundle;

namespace DroidBarBotMaster.Droid
{
    [Activity(Label = "CocktailListview")]
    public class CocktailListview : Activity
    {

        List<string> localDrinkNames;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CocktailListview);

            FindViewById<Button>(Resource.Id.btnEdit).Click += CocktailListview_Click;

            FindViewById<Button>(Resource.Id.btnManualOrder).Click += btnManualOrder_Click;

            FindViewById<ListView>(Resource.Id.cocktailListView).ItemClick += listView_ItemClick;


            Tuple<List<DrinkMultiple>, List<string>> formatedDrinksTuple = FormatListUpdate();

            UpdateDrinkListView(formatedDrinksTuple.Item1, formatedDrinksTuple.Item2);
        }

        private void btnManualOrder_Click(object sender, EventArgs e)
        {
            var activity = new Intent(this, typeof(ManualOrderController));

            StartActivity(activity);
        }

        private Tuple<List<DrinkMultiple>, List<string>> FormatListUpdate()
        {
            List<String> bottleNames = TransporterClass.listContainer.Select(x => x.Name).ToList();

            // TODO: Dev, remove this
            //----------------------------


            //bottleNames = new List<string>() { "vodka", "lime", "tequila","whiskey","gin" };


            //----------------------------

            localDrinkNames = bottleNames;

            // Filter and update available drinks

            UpdateRepositoryData(bottleNames);


            List<DrinkMultiple> drinkMultiple = new List<DrinkMultiple>();


            for (int i = 0; i < bottleNames.Count; i++)
            {

                string key = bottleNames[i];
                if (TransporterClass.Repository[key] != null)
                {
                    drinkMultiple.Add(TransporterClass.Repository[key]);
                }
            }

            return Tuple.Create<List<DrinkMultiple>, List<string>>(drinkMultiple, bottleNames);
        }
        private void CocktailListview_Click(object sender, EventArgs e)
        {
            var newActivity = new Intent(this, typeof(EditPage));

            StartActivity(newActivity);
        }

        public override void OnVisibleBehindCanceled()
        {
            int x = 4;
            base.OnVisibleBehindCanceled();
        }

        protected override void OnResume()
        {

            List<String> bottleNames = TransporterClass.listContainer.Select(w => w.Name).ToList();



            //foreach (var bottleName in bottleNames)
            //{
            //    foreach (var localDrinkName in localDrinkNames)
            //    {
            //        if (!String.Equals(bottleName.ToLower(), localDrinkName.ToLower()))
            //        {
            //            Tuple<List<DrinkMultiple>, List<string>> formatedDrinksTuple = FormatListUpdate();

            //            UpdateDrinkListView(formatedDrinksTuple.Item1, formatedDrinksTuple.Item2);

            //            break;
            //        }
            //    }
            //}
            if (bottleNames.Count == localDrinkNames.Count)
            {
                for (int i = 0; i < bottleNames.Count; i++)
                {
                    if (!String.Equals(bottleNames[i].ToLower(), localDrinkNames[i].ToLower()))
                    {
                        Tuple<List<DrinkMultiple>, List<string>> formatedDrinksTuple = FormatListUpdate();

                        UpdateDrinkListView(formatedDrinksTuple.Item1, formatedDrinksTuple.Item2);

                        break;
                    }
                }
            }



            base.OnResume();
        }


        private void UpdateDrinkListView(List<DrinkMultiple> drinkMultiple, List<String> drinkNames)
        {


            if (drinkMultiple == null || drinkMultiple.Count < 1) return;

            int drinkOrder_MustContaianAtLeastIngridients = 3;

            List<DrinkMultiple> availableDrinksMixFiltered = CocktailDBService.MixableDrinksFiltered(drinkMultiple, drinkNames, drinkOrder_MustContaianAtLeastIngridients);

            ListView listView = FindViewById<ListView>(Resource.Id.cocktailListView);

            List<Drink> allDrinks = new List<Drink>();


            // Clean and Empty the list
            int childCount = listView.ChildCount;

            if (childCount > 0)
            {
                List<Drink> emptyList = new List<Drink>();

                listAdapterDrink adapterTemp = new listAdapterDrink(this, emptyList);


                listView.SetAdapter(adapterTemp);
            }


            // Add to list 

            foreach (var item in availableDrinksMixFiltered)
            {
                allDrinks.AddRange(item.Drinks);
            }

            listAdapterDrink adapter = new listAdapterDrink(this, allDrinks);

            listView.Adapter = adapter;



        }



        private SortedDictionary<string, DrinkMultiple> UpdateRepositoryData(List<string> drinkNames)
        {
            bool changed = false;

            // Repo is empty, get all new data
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
                // Check if exist in repo or get it from DB 

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

            //this.Finish();
        }
    }
}