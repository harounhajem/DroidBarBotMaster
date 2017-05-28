using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using DroidBarBotMaster.Droid.Class.Model;
using Android.Graphics;
using System.Net;
using Android.Graphics.Drawables;
using System.IO;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    class listAdapterDrink : BaseAdapter<Drink>
    {
        Context context;
        
        public List<Drink> listDrink { get; private set; }

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
            drinkName.Text = listDrink[position].strDrink.Replace(" ","\n");

            TextView txtvDrinkCategori= row.FindViewById<TextView>(Resource.Id.drinkCategori);
            txtvDrinkCategori.Text =  String.IsNullOrEmpty(listDrink[position].strCategory) ? "" : listDrink[position].strCategory;

            ImageView drinkImage = row.FindViewById<ImageView>(Resource.Id.drinkImage);

            if (!String.IsNullOrEmpty(listDrink[position].strDrinkThumb))
            {
                Bitmap picture = GetImageBitmapFromUrl(listDrink[position].strDrinkThumb);
                Bitmap pictureRound = GetRoundedShape(picture,400,400);
                drinkImage.SetImageBitmap(pictureRound);

            }
            else
            {
                Stream picStream = context.Resources.OpenRawResource(Resource.Drawable.placeholder2);
                var bitmapPicture = new BitmapDrawable(picStream);
                drinkImage.SetImageBitmap(GetRoundedShape(bitmapPicture.Bitmap, 400, 400));

            }

            

            return row;
        }

        public Bitmap GetRoundedShape(Bitmap scaleBitmapImage, int width, int height)
        {
            int targetWidth = width;
            int targetHeight = height; 
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


        public Bitmap GetImageBitmapFromUrl(string url)
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