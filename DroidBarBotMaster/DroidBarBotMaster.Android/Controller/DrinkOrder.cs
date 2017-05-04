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

namespace DroidBarBotMaster.Droid.Controller
{
    [Activity(Label = "DrinkOrder")]
    public class DrinkOrder : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DrinkOrder);


            PopulateData();
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

                FindViewById<ImageView>(Resource.Id.drinkImage).SetImageBitmap(pictureRound);
            }
            else
            {
                Stream picStream = this.Resources.OpenRawResource(Resource.Drawable.placeholder_white);
                var bitmapPicture = new BitmapDrawable(picStream);
                
                FindViewById<ImageView>(Resource.Id.drinkImage).SetImageBitmap(listAdapter.GetRoundedShape(bitmapPicture.Bitmap));
            }



            // Set text 

            FindViewById<TextView>(Resource.Id.drinkName).Text = drink.strDrink;

            FindViewById<TextView>(Resource.Id.drinkIngridient1).Text = drink.strIngredient1;
            FindViewById<TextView>(Resource.Id.drinkIngridient2).Text = drink.strIngredient2;

        }


    }
}