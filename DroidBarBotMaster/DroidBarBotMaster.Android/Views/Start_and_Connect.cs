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
using System.Threading;
using DroidBarBotMaster.Droid.Class.Service;

namespace DroidBarBotMaster.Droid
{
    [Activity(Label = "Start_and_Connect", MainLauncher = false)]
    public class Start_and_Connect : Activity
    {
        BluetoothService bluetoothService;
        BarBot barBot;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Start_and_connect);


            //btn.click += delegate { StartActivity(typeof(MainActivity)); };
        }

        public void xx()
        {

            Thread th = new Thread(() =>
            {
                bluetoothService = new BluetoothService(this);
                barBot = new BarBot(bluetoothService);
            });
            th.Start();
        }
    }
}