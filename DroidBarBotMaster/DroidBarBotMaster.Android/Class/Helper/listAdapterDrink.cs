using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;
using DroidBarBotMaster.Droid.Class.Model;
using Android.Graphics;
using Java.IO;
using Android.OS;
using System.Net;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    class listAdapterDrink : BaseAdapter<Drink>
    {
        Context context;
        List<Drink> listDrink;

        public listAdapterDrink(Context context, List<Drink> listDrink)
        {
            this.context = context;

            this.listDrink = listDrink;
        }



        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.listItemDrink, null, false);
            }

            TextView drinkName = row.FindViewById<TextView>(Resource.Id.drinkName);
            drinkName.Text = listDrink[position].strDrink;

            if (!String.IsNullOrEmpty(listDrink[position].strDrinkThumb))
            {
                ImageView drinkImage = row.FindViewById<ImageView>(Resource.Id.drinkImage);
                Bitmap picture = GetImageBitmapFromUrl(listDrink[position].strDrinkThumb);
                drinkImage.SetImageBitmap(picture);
            }
            return row;
        }


        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }




        public override int Count
        {
            get
            {
                return listDrink.Count;
            }
        }

        public override Drink this[int position]
        {
            get
            {
                return listDrink[position];
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

    }

}