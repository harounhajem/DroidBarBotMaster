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

namespace DroidBarBotMaster.Droid
{
    [Activity(Label = "Start", Icon = "@drawable/icon", MainLauncher = true)]
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

        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
            {
                bluetoothService = new BluetoothService(this);

                barBot = new BarBot(bluetoothService);

                if (bluetoothService.ConnectActivateBluetooth())
                {
                    RunOnUiThread(() => SetContentView(Resource.Layout.Main));
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


