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
using DroidBarBotMaster.Droid.Class.Helper;

namespace DroidBarBotMaster.Droid
{
    [Activity(Label = "MainActivity", MainLauncher = false)]
    public class MainActivity : Activity
    {
        Toast toastMessenger;
        TextView text;
        BluetoothService btService;
        BarBot barBot;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            // Get our button from the layout resource,
            // and attach an event to it,

            Button btnConnect = FindViewById<Button>(Resource.Id.btnConnect);
            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);
            Button btnDisConnect = FindViewById<Button>(Resource.Id.btnDisConnect);
            FindViewById<Button>(Resource.Id.btnUpdateBottle).Click += btnUpdateBottle;
            FindViewById<Button>(Resource.Id.btnOrderDrink).Click += btnOrderDrink;
            text = FindViewById<TextView>(Resource.Id.textView1);

            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            btnDisConnect.Click += BtnDisConnect_Click;

            text.Text = "";
            BtnConnect_Click(this, null);
        }

        protected override void OnStart()
        {
            Thread.Sleep(600);

            if (TransporterClass.listContainer != null)
            {
                foreach (var item in TransporterClass.listContainer)
                {
                    setText(item.Name);
                }

            }
            base.OnStart();
        }

        private void btnOrderDrink(object sender, EventArgs e)
        {
            barBot.SendCocktailOrder();
        }

        private void btnUpdateBottle(object sender, EventArgs e)
        {
            barBot.PostIngridients(new Droid.Class.Model.Container(), 1);
        }

        public void setText(String sendtext)
        {
            RunOnUiThread(() => { text.Text += sendtext + "\n"; });

        }

        private void BtnDisConnect_Click(object sender, EventArgs e)
        {
            btService.cancelSocketServ();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            // GetIngridients
            //barBot.GetIngridients();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {

            btService = TransporterClass.bluetoothService;
            barBot = TransporterClass.barBot;

            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);
            btnSend.Clickable = true;
            btnSend.Enabled = true;
            FindViewById<Button>(Resource.Id.btnDisConnect).Enabled = true;
            FindViewById<Button>(Resource.Id.btnUpdateBottle).Enabled = true;
            FindViewById<Button>(Resource.Id.btnOrderDrink).Enabled = true;
        }

        internal void adapterOnActivityResult(int rEQUEST_ENABLE_BT, Result canceled, Intent enableBT)
        {
            OnActivityResult(rEQUEST_ENABLE_BT, canceled, enableBT);
        }
    }
}