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

namespace DroidBarBotMaster.Droid
{

    // TODO: Dev activate:  MainLauncher = true
    [Activity(Label = "Start", Icon = "@drawable/icon")]
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

            FindViewById<Button>(Resource.Id.btnConnect).Click += BtnConnect_Click;

            TransporterClass.listContainer = new List<Container>();

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

                    Thread.Sleep(600);

                    var activity2 = new Intent(this, typeof(CocktailListview));

                    activity2.PutExtra("MyData", "Data from Start_and_Connect");

                    StartActivity(activity2);
                    
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


