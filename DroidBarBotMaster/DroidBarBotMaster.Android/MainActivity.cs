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

            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            //InitExample();

            // Send Message



            //byte[] sennd = { 101 };
            //Thread writeToSlave = new Thread(() => myBTservice.Write(sennd));
            //writeToSlave.Start();

            //// Read Message
            //Thread readInputThread = new Thread(() => myBTservice.Read());
            //readInputThread.Start();

        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            byte[] sennd = { 101 };
            Thread writeToSlave = new Thread(() => myBTservice.Write(sennd));
            writeToSlave.Start();

            // Read Message
            Thread readInputThread = new Thread(() => myBTservice.Read());
            readInputThread.Start();
        }

        MyBluetoothService myBTservice;

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
            myBTservice = new MyBluetoothService(this));
            th.Start();

            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);
            btnSend.Clickable = true;
            btnSend.Enabled = true;
        }

        private async void InitExample()
        {

            // Has the device BT?
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (mBluetoothAdapter == null)
            {
                toastMessenger = Toast.MakeText(this, "Error no BT found on device", ToastLength.Long);
                toastMessenger.Show();
                return;
            }
            else
            {
                toastMessenger = Toast.MakeText(this, "Got adapter", ToastLength.Long);
                toastMessenger.Show();
            }

            int REQUEST_ENABLE_BT = 0;


            // Enable BT
            if (!mBluetoothAdapter.IsEnabled)
            {
                Intent enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBT, REQUEST_ENABLE_BT);

                // TODO: Check for cancelaion

                //OnActivityResult(REQUEST_ENABLE_BT, Result.Canceled, enableBT);
                //if (REQUEST_ENABLE_BT == 0)
                //{
                //    toastMessenger = Toast.MakeText(this, "BT not enabled", ToastLength.Long);
                //    toastMessenger.Show();
                //    return;
                //}
            }
            toastMessenger = Toast.MakeText(this, "Enabled BT", ToastLength.Long);
            toastMessenger.Show();



            // Get Bonded Devices
            var barBotDevice = (from x in mBluetoothAdapter.BondedDevices
                                where x.Name.ToLower() == ("BarBot").ToLower()
                                select x).FirstOrDefault();

            if (barBotDevice == null)
            {
                toastMessenger = Toast.MakeText(this, "Manual connect to BarBot please.", ToastLength.Long);
                toastMessenger.Show();
                return;
            }
            else
            {
                toastMessenger = Toast.MakeText(this, "Found BarBot bond", ToastLength.Long);
                toastMessenger.Show();
            }


            // Set up socket
            BluetoothSocket mmSocket;
            BluetoothSocket tmpSocket;

            try
            {
                UUID id = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
                tmpSocket = barBotDevice.CreateInsecureRfcommSocketToServiceRecord(id);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Socket's listen() method failed");
                toastMessenger = Toast.MakeText(this, "Found BarBot bond", ToastLength.Long);
                toastMessenger.Show();
                return;
            }

            mmSocket = tmpSocket;


            // Drive on a diffrent Socket
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                mmSocket.Connect();
            }
            catch (Exception e)
            {
                try
                {
                    mmSocket.Close();
                    Console.WriteLine("Closing connection");

                }
                catch (Exception r)
                {
                    Console.WriteLine("Could not close the client socket");
                    return;
                }
            }

            Thread.Sleep(1500);








        }

    }
}


