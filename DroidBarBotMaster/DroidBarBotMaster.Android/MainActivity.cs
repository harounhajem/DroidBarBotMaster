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
    [Activity(Label = "DroidBarBotMaster.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Toast toastMessenger;
        TextView text;
        BluetoothService btService;
        BarBot barBot;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            toastMessenger = new Toast(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            // Get our button from the layout resource,
            // and attach an event to it
            Button btnConnect = FindViewById<Button>(Resource.Id.btnConnect);
            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);
            Button btnDisConnect = FindViewById<Button>(Resource.Id.btnDisConnect);
            FindViewById<Button>(Resource.Id.btnSendCommand).Click += btnSendCommand;
            text = FindViewById<TextView>(Resource.Id.textView1);

            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            btnDisConnect.Click += BtnDisConnect_Click;

            BtnConnect_Click(null, null);
        }

        private void btnSendCommand(object sender, EventArgs e)
        {
            barBot.PostIngridients(new Droid.Class.Model.Container(), 1);
        }

        public void setText(String sendtext)
        {
            RunOnUiThread(() => { text.Text = sendtext; });

        }

        private void BtnDisConnect_Click(object sender, EventArgs e)
        {
            btService.cancelSocketServ();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            // GetIngridients
            barBot.GetIngridients();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
            {
                btService = new BluetoothService(this);
                barBot = new BarBot(btService);
            });
            th.Start();

            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);
            FindViewById<Button>(Resource.Id.btnDisConnect).Enabled = true;
            btnSend.Clickable = true;
            btnSend.Enabled = true;
            FindViewById<Button>(Resource.Id.btnSendCommand).Enabled = true;
        }
    }
}


