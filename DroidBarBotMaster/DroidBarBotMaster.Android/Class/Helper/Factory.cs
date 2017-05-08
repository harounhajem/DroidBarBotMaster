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
using static Android.Support.V4.View.ViewPager;
using Android.Graphics;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    static class Factory
    {
        public static LinearLayout ProduceDetailIngridientsForLinview(Context context, string ingridients, string measurment)
        {

            LinearLayout linearLayout = ProduceLinearLayout(context);

            TextView textViewIngridients = ProduceTextViewIngridients(context, ingridients);

            TextView textViewMeasurement = ProduceTextViewMeasurment(context, measurment);

            linearLayout.AddView(textViewMeasurement, 0);

            linearLayout.AddView(textViewIngridients, 0);

            return linearLayout;
        }

        private static TextView ProduceTextViewMeasurment(Context context, string measurment)
        {
            TextView tw = new TextView(context);

            Android.Widget.LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(-1, -2, 1f);

            tw.LayoutParameters = layoutParams;

            tw.SetPadding(10, 0, 0, 0);

            tw.SetTypeface(Typeface.SansSerif, TypefaceStyle.Normal);

            tw.SetTextColor(Android.Graphics.Color.LightGray);

           tw.SetTextSize(Android.Util.ComplexUnitType.Sp, 13);

            tw.Text = measurment;

            tw.Id = 487899546;
            return tw;


        }

        private static TextView ProduceTextViewIngridients(Context context, string textIngridients)
        {
            TextView textView = new TextView(context);

            Android.Widget.LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(-1, -2, 1f);


            textView.LayoutParameters = layoutParams;

            textView.SetPadding(70, 0, 0, 0);

            textView.SetTypeface(Typeface.SansSerif, TypefaceStyle.Normal);

            textView.SetTextColor(Android.Graphics.Color.LightGray);

            textView.TextAlignment = TextAlignment.TextEnd;

            textView.Text = textIngridients;

            return textView;

        }

        private static LinearLayout ProduceLinearLayout(Context context)
        {
            // LinearLayout

            LinearLayout linearLayout = new LinearLayout(context);

            linearLayout.Orientation = Orientation.Horizontal;


            Android.Widget.LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(-1, -2, 1f);

            linearLayout.LayoutParameters = layoutParams;


            linearLayout.SetGravity(GravityFlags.Center);

            return linearLayout;
        }

    }
}