using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DroidBarBotMaster.Droid.Class.Helper;
using DroidBarBotMaster.Droid.Class.Model;
using System.IO;
using Android.Graphics.Drawables;
using DroidBarBotMaster.Droid.Class.Service;

namespace DroidBarBotMaster.Droid.Controller
{
    [Activity(Label = "DrinkOrder")]
    public class DrinkOrder : Activity
    {
        private Drink drink;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DrinkOrder);

            drink = TransporterClass.SelectedDrink;

            FindViewById<Button>(Resource.Id.drinkOrder).Click += DrinkOrder_Click; 

            PopulateData();
        }

        private void DrinkOrder_Click(object sender, EventArgs e)
        {
            BarBot barbot = new BarBot(TransporterClass.bluetoothService);

            barbot.SendCocktailOrder(drink);
        }

        private void PopulateData()
        {
            Drink drink = TransporterClass.SelectedDrink;

            // Set picture

            listAdapterDrink listAdapter = new listAdapterDrink(null, null);

            if (drink.strDrinkThumb != null)
            {

                Bitmap picture = listAdapter.GetImageBitmapFromUrl(drink.strDrinkThumb);

                Bitmap pictureRound = listAdapter.GetRoundedShape(picture);

                FindViewById<ImageView>(Resource.Id.imageView4).SetImageBitmap(pictureRound);
            }
            else
            {
                Stream picStream = this.Resources.OpenRawResource(Resource.Drawable.placeholder_white);
                var bitmapPicture = new BitmapDrawable(picStream);
                FindViewById<ImageView>(Resource.Id.imageView4).SetImageBitmap(listAdapter.GetRoundedShape(bitmapPicture.Bitmap));
            }



            // Set text 

            FindViewById<TextView>(Resource.Id.strDrink).Text = drink.strDrink.ToUpper();

            LinearLayout temp = FindViewById<LinearLayout>(Resource.Id.linearLayoutDrinkOrder);
             
            //temp.AddView(2)


            // TODO: Create a Factory for creating Textview with ingridients and populate the ListView
            FindViewById<TextView>(Resource.Id.ingridients1).Text = drink.strIngredient1;
            FindViewById<TextView>(Resource.Id.cl1).Text = drink.strMeasure1;

            FindViewById<TextView>(Resource.Id.ingridients2).Text = drink.strIngredient2;
            FindViewById<TextView>(Resource.Id.cl2).Text = drink.strMeasure2;

            FindViewById<TextView>(Resource.Id.ingridients3).Text = drink.strIngredient3;
            FindViewById<TextView>(Resource.Id.cl3).Text = drink.strMeasure3;

            FindViewById<TextView>(Resource.Id.ingridients4).Text = drink.strIngredient4;
            FindViewById<TextView>(Resource.Id.cl4).Text = drink.strMeasure4;

            FindViewById<TextView>(Resource.Id.strdescription).Text = drink.strInstructions;
            // ----- Remove this way of getting the data --------------------------------------------- //


        }


    }
}