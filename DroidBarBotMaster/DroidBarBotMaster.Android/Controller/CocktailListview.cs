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

            foreach (var item in TransporterClass.listContainer)
            {
                DrinkMultiple tempDrinks = CocktailDBService.HttpGet(item.Name, HttpGetRequests.CocktailByIngridient);

                if (tempDrinks != null)
                {
                    available.Add(tempDrinks);
                }
            }

            Console.WriteLine(available);


        }

    }
}