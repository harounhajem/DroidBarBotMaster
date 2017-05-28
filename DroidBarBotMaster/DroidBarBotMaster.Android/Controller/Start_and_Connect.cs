using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System.Linq;
using Java.Util;
using System.IO;
using DroidBarBotMaster.Droid.Class.Service;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using DroidBarBotMaster.Droid.Class.Model;
using DroidBarBotMaster.Droid.Class.Helper;
using DroidBarBotMaster.Droid.Controller;
using Android.Graphics;

namespace DroidBarBotMaster.Droid
{

    // TODO: Dev activate:  MainLauncher = true
    [Activity(Label = "BarBot", Icon = "@drawable/icon", MainLauncher = true)]
    public class Start_and_Connect : Activity
    {
        Toast toastMessenger;

        BluetoothService bluetoothService;

        BarBot barBot;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            toastMessenger = new Toast(this);

            SetContentView(Resource.Layout.Start_and_connect);



            Button btnConnect = FindViewById<Button>(Resource.Id.btnConnect);

            btnConnect.Click += BtnConnect_Click;

            //btnConnect.Alpha = 0f;

            //float yPos = 1450;
            //float movement = 33f;
            //btnConnect.SetY(yPos + movement);

            //int[] x = new int[2];
            //btnConnect.GetLocationInWindow(x);

            //int tp = btnConnect.Top;

            //btnConnect.Animate()
            //          .Alpha(1f)
            //          .Y(yPos - movement)
            //          .SetDuration(2200);

            //btnConnect.Animate().Start();



            TransporterClass.listContainer = new List<Container>();

            TransporterClass.Repository = Repository.GetSavedData();


            var font = Typeface.CreateFromAsset(Assets, "segoe.ttf");

            FindViewById<TextView>(Resource.Id.txtvMotto).Typeface = font;

        }

        protected override void OnStart()
        {

            // TODO: For dev att this as true

            //if (TransporterClass.Repository != null)
            //if (TransporterClass.Repository != null)
            //    {
            //    var newActivity = new Intent(this, typeof(CocktailListview));

            //    StartActivity(newActivity);

            //    this.Finish();

            //}
            base.OnStart(); 

            Button btnConnect = FindViewById<Button>(Resource.Id.btnConnect);


            Display ds = WindowManager.DefaultDisplay;
            Point size = new Point();

            ds.GetSize(size);

            btnConnect.Alpha = 0f;

            float yPos = (float)(size.Y * 0.77);
            float movement = 33f;
            btnConnect.SetY(yPos + movement);


            btnConnect.Animate()
                      .Alpha(1f)
                      .Y(yPos - movement)
                      .SetDuration(2200);

            btnConnect.Animate().Start();

        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            // TODO: Dev deactivate

            Thread th = new Thread(() =>
            {
                bluetoothService = new BluetoothService(this);

                TransporterClass.bluetoothService = bluetoothService;

                barBot = new BarBot(bluetoothService);

                TransporterClass.barBot = barBot;


                if (bluetoothService.ConnectActivateBluetooth())
                {

                    barBot.GetIngridients(5000, TransporterClass.listContainer);

                    Thread.Sleep(750);

                    var newActivity = new Intent(this, typeof(CocktailListview));

                    StartActivity(newActivity);


                    this.Finish();

                    OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                }
                else
                {
                    bluetoothService.ShowToastMessage("Could not connect", ToastLength.Long);
                }

            });

            th.Start();

        }

        internal void adapterOnActivityResult(int rEQUEST_ENABLE_BT, Result canceled, Intent enableBT)
        {
            OnActivityResult(rEQUEST_ENABLE_BT, canceled, enableBT);
        }
    }
}


