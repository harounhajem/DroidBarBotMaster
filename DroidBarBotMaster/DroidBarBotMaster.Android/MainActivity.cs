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

namespace DroidBarBotMaster.Droid
{
	[Activity (Label = "DroidBarBotMaster.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
    {
        int count = 1;
        Toast toastMessenger;
        TextView text;
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
            Button btnDisConnect= FindViewById<Button>(Resource.Id.btnDisConnect);
            text =  FindViewById<TextView>(Resource.Id.textView1);
            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            btnDisConnect.Click += BtnDisConnect_Click;

            BtnConnect_Click(null, null);
        }

        public void setText(String sendtext)
        {
            RunOnUiThread(() => { text.Text = sendtext; });

        }

        private void BtnDisConnect_Click(object sender, EventArgs e)
        {
            myBTservice.cancelSocketServ();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            // Send message
            byte[] send = { 101 };
            Thread writeToSlave = new Thread(() => myBTservice.Write(send));

            // Read Message
            Thread readInputThread = new Thread(() => myBTservice.Read(text, this));

            writeToSlave.Start();
            readInputThread.Start();
        }

        MyBluetoothService myBTservice;

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
            myBTservice = new MyBluetoothService(this));
            th.Start();

            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);
            FindViewById<Button>(Resource.Id.btnDisConnect).Enabled = true;
            btnSend.Clickable = true;
            btnSend.Enabled = true;
        }
    }
}


