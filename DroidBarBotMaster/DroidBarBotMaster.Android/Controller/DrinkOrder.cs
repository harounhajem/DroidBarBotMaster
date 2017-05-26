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

            LinearLayout drinkOrderLinearView = FindViewById<LinearLayout>(Resource.Id.linearLayoutDrinkOrder);


            if (!string.IsNullOrEmpty(drink.strIngredient1) && drink.strIngredient1.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient1, drink.strMeasure1)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient2) && drink.strIngredient2.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient2, drink.strMeasure2)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient3) && drink.strIngredient3.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient3, drink.strMeasure3)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient4) && drink.strIngredient4.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient4, drink.strMeasure4)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient5) && drink.strIngredient5.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient5, drink.strMeasure5)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient6) && drink.strIngredient6.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient6, drink.strMeasure6)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient7) && drink.strIngredient7.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient7, drink.strMeasure7)));
            }
            if (!string.IsNullOrEmpty(drink.strIngredient8) && drink.strIngredient8.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient8, drink.strMeasure8)));
            }
            if (!string.IsNullOrEmpty(drink.strIngredient9) && drink.strIngredient9.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient9, drink.strMeasure9)));
            }
            if (!string.IsNullOrEmpty(drink.strIngredient10) && drink.strIngredient10.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient10, drink.strMeasure10)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient11) && drink.strIngredient11.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient11, drink.strMeasure11)));
            }
            if (!string.IsNullOrEmpty(drink.strIngredient12) && drink.strIngredient12.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient12, drink.strMeasure12)));
            }
            if (!string.IsNullOrEmpty(drink.strIngredient13) && drink.strIngredient13.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient13, drink.strMeasure13)));
            }

            if (!string.IsNullOrEmpty(drink.strIngredient14) && drink.strIngredient14.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient14, drink.strMeasure14)));
            }
            if (!string.IsNullOrEmpty(drink.strIngredient15) && drink.strIngredient15.Length > 2)
            {
                RunOnUiThread(() => drinkOrderLinearView.AddView(Factory.ProduceDetailIngridientsForLinview(this, drink.strIngredient15, drink.strMeasure15)));
            }

            // TODO: Create a Factory for creating Textview with ingridients and populate the ListView


            FindViewById<TextView>(Resource.Id.strdescription).Text = drink.strInstructions;
            // ----- Remove this way of getting the data --------------------------------------------- //


        }


    }
}