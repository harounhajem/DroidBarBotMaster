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
using DroidBarBotMaster.Droid.Class.Helper;
using DroidBarBotMaster.Droid.Class.Service;
using DroidBarBotMaster.Droid.Class.Model;
using Android.Views.InputMethods;

namespace DroidBarBotMaster.Droid.Controller
{
    [Activity(Label = "BarBot")]
    public class ManualOrderController : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ManualOrder);

            FindViewById<Button>(Resource.Id.drinkOrder).Click += btnDrinkOrder_click;

            AnimateButton(FindViewById<Button>(Resource.Id.drinkOrder));
        }

        private void AnimateButton(Button btn)
        {
            Button btnConnect = btn;
            btnConnect.Alpha = 0f;
            btnConnect.Animate()
                      .Alpha(1f)
                      .SetDuration(1200);
            btnConnect.Animate().Start();

        }

        private void btnDrinkOrder_click(object sender, EventArgs e)
        {


            // Get values for a drink
            Drink drink = new Drink();

            drink.strIngredient1 = TransporterClass.listContainer[0].Name;
            drink.strMeasure1 = FindViewById<EditText>(Resource.Id.txtbAmount1).Text + "cl";

            drink.strIngredient2 = TransporterClass.listContainer[1].Name;
            drink.strMeasure2 = FindViewById<EditText>(Resource.Id.txtbAmount2).Text + "cl";

            drink.strIngredient3 = TransporterClass.listContainer[2].Name;
            drink.strMeasure3 = FindViewById<EditText>(Resource.Id.txtbAmount3).Text + "cl";

            drink.strIngredient4 = TransporterClass.listContainer[3].Name;
            drink.strMeasure4 = FindViewById<EditText>(Resource.Id.txtbAmount4).Text + "cl";

            drink.strIngredient5 = TransporterClass.listContainer[4].Name;
            drink.strMeasure5 = FindViewById<EditText>(Resource.Id.txtbAmount5).Text + "cl";

            drink.strIngredient6 = TransporterClass.listContainer[5].Name;
            drink.strMeasure6 = FindViewById<EditText>(Resource.Id.txtbAmount6).Text + "cl";

            // Send values
            BarBot barbot = new BarBot(TransporterClass.bluetoothService);

            barbot.SendCocktailOrder(drink);

        }

        protected override void OnStart()
        {
            // Set bottles container values in view
            if (TransporterClass.listContainer != null)
            {
                FindViewById<TextView>(Resource.Id.txtingridient1).Text = FirstCharToUpper(TransporterClass.listContainer[0].Name);
                FindViewById<TextView>(Resource.Id.txtingridient2).Text = FirstCharToUpper(TransporterClass.listContainer[1].Name);
                FindViewById<TextView>(Resource.Id.txtingridient3).Text = FirstCharToUpper(TransporterClass.listContainer[2].Name);
                FindViewById<TextView>(Resource.Id.txtingridient4).Text = FirstCharToUpper(TransporterClass.listContainer[3].Name);
                FindViewById<TextView>(Resource.Id.txtingridient5).Text = FirstCharToUpper(TransporterClass.listContainer[4].Name);
                FindViewById<TextView>(Resource.Id.txtingridient6).Text = FirstCharToUpper(TransporterClass.listContainer[5].Name);
            }
            else
            {
                FindViewById<TextView>(Resource.Id.txtingridient1).Text = "Empty";
                FindViewById<TextView>(Resource.Id.txtingridient2).Text = "";
                FindViewById<TextView>(Resource.Id.txtingridient3).Text = "";
                FindViewById<TextView>(Resource.Id.txtingridient4).Text = "";
                FindViewById<TextView>(Resource.Id.txtingridient5).Text = "";
                FindViewById<TextView>(Resource.Id.txtingridient6).Text = "";
            }



            base.OnStart();
        }

        private string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
   
}