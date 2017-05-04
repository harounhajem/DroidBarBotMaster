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
using Android.Graphics.Drawables;
using System.IO;
using DroidBarBotMaster.Droid.Controller;

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

            ImageView drinkImage = row.FindViewById<ImageView>(Resource.Id.drinkImage);

            if (!String.IsNullOrEmpty(listDrink[position].strDrinkThumb))
            {
                Bitmap picture = GetImageBitmapFromUrl(listDrink[position].strDrinkThumb);
                Bitmap pictureRound = GetRoundedShape(picture);
                drinkImage.SetImageBitmap(pictureRound);

            }
            else
            {
                Stream picStream = context.Resources.OpenRawResource(Resource.Drawable.placeholder_white);
                var bitmapPicture = new BitmapDrawable(picStream);
                drinkImage.SetImageBitmap(GetRoundedShape(bitmapPicture.Bitmap));

            }

            

            return row;
        }

        public Bitmap GetRoundedShape(Bitmap scaleBitmapImage)
        {
            int targetWidth = 650;
            int targetHeight = 650;
            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Android.Graphics.Path path = new Android.Graphics.Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
                Android.Graphics.Path.Direction.Ccw);

            canvas.ClipPath(path);
            Bitmap sourceBitmap = scaleBitmapImage;
            canvas.DrawBitmap(sourceBitmap,
                new Rect(0, 0, sourceBitmap.Width,
                    sourceBitmap.Height),
                new Rect(0, 0, targetWidth, targetHeight), null);
            return targetBitmap;
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