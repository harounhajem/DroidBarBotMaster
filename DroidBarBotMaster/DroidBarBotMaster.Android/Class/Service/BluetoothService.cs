using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Widget;
using Java.Util;
using Android.Bluetooth;
using System.Threading.Tasks;
using System.Threading;
using DroidBarBotMaster.Droid.Class.Helper;
using DroidBarBotMaster.Droid.Class.Model;
using System.Diagnostics;

namespace DroidBarBotMaster.Droid.Class.Service
{
    public class BluetoothService
    {
        private BluetoothAdapter mBluetoothAdapter;
        private BluetoothDevice mbarBotDevice;
        private BluetoothSocket mSocket;
        private System.IO.Stream mInStream;
        private System.IO.Stream mOutStream;
        private Toast toastMessenger;
        private MainActivity context;



        public BluetoothService(MainActivity context)
        {
            this.context = context;
            toastMessenger = new Toast(this.context);

            DeviceHasBT();
            EnableBTdevice();
            GetBondedDevices();
            SocketConnect();
            InOutSocketInit();
            ShowToastMessage("CONNECTED TO BARBOT");
        }


        #region Write, Read
        public async void ReadGetIngridients()
        {
            byte[] mmBuffer = new byte[1024];
            int numBytes; // bytes returned from read()

            // Keep listening to the InputStream until an exception occurs.

            List<Container> containerList = new List<Container>();


            Stopwatch timer = new Stopwatch();
            timer.Start();

            //text.Text = "Got it";
            while (true)
            {
                if (timer.ElapsedMilliseconds > 10000) 
                {
                    break;
                }
                try
                {
                    numBytes = await mInStream.ReadAsync(mmBuffer, 0, mmBuffer.Length);
                    // Send the obtained bytes to the UI activity.
                    if (numBytes > 0)
                    {
                        string recivedMessage = ASCIIEncoding.ASCII.GetString(mmBuffer);
                        System.Console.WriteLine(recivedMessage);
                        DeSerialize.DeSerializeArray(recivedMessage, containerList, this);
                    }

                }
                catch (System.IO.IOException e)
                {
                    System.Console.WriteLine("InputStream failure ERROR:4084");
                    System.Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        //Call this from the main activity to send data to the remote device.
        public async void WriteAsync(byte[] bytes)
        {
            try
            {
                await mOutStream.WriteAsync(bytes, 0, bytes.Length);
                System.Console.WriteLine("--SEND, ASYNC :  " + ASCIIEncoding.ASCII.GetString(bytes));
                ShowToastMessage("Writing: " + ASCIIEncoding.ASCII.GetString(bytes));


            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("Could not SEND Error:6548");
                System.Console.WriteLine(e.Message);

                throw;
            }
        }

        public void Write(byte[] bytes)
        {
            try
            {
                mOutStream.Write(bytes, 0, bytes.Length);
                System.Console.WriteLine("--SEND, NO ASYNC :  "+ ASCIIEncoding.ASCII.GetString(bytes));
                ShowToastMessage("Writing: " + ASCIIEncoding.ASCII.GetString(bytes));
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("Could not SEND Error:6548");
                System.Console.WriteLine(e.Message);

                throw;
            }
        }


        #endregion

        #region Initialize Bluetooth sequence
        public bool DeviceHasBT()
        {
            mBluetoothAdapter = mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter == null)
            {
                ShowToastMessage("Error no BT found on device");
                return false;
            }
            else
            {
                ShowToastMessage("Got adapter");
            }
            return true;

        }

        public bool EnableBTdevice()
        {
            int REQUEST_ENABLE_BT = 0;


            // Enable BT
            if (!mBluetoothAdapter.IsEnabled)
            {
                Intent enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
                Activity s = new Activity();
                s.StartActivityForResult(enableBT, REQUEST_ENABLE_BT);

                // TODO: Check for cancelation

                //OnActivityResult(REQUEST_ENABLE_BT, Result.Canceled, enableBT);
                //if (REQUEST_ENABLE_BT == 0)
                //{
                //    toastMessenger = Toast.MakeText(this, "BT not enabled", ToastLength.Long);
                //    toastMessenger.Show();
                //    return;
                //}
            }
            ShowToastMessage("Enabled BT");
            return true;
        }

        public bool GetBondedDevices()
        {
            // Get Bonded Devices
            mbarBotDevice = (from x in mBluetoothAdapter.BondedDevices
                             where x.Name.ToLower() == ("BarBot").ToLower()
                             select x).FirstOrDefault();

            if (mbarBotDevice == null)
            {
                ShowToastMessage("Manual pair connect to BarBot please.");
                return false;
            }
            else
            {
                ShowToastMessage("Found BarBot bond");
                return true;
            }
        }

        public bool SocketConnect()
        {
            BluetoothSocket tmpSocket;
            // Set up socket
            try
            {
                UUID id = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
                tmpSocket = mbarBotDevice.CreateInsecureRfcommSocketToServiceRecord(id);
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine("Socket's listen() method failed");
                ShowToastMessage("Found BarBot bond");
                return false;
            }

            mSocket = tmpSocket;


            // Drive on a diffrent Socket
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                mSocket.Connect();
            }
            catch (Exception e)
            {
                try
                {
                    System.Console.WriteLine("\n\nClosing connection:\n");
                    System.Console.WriteLine(e.Message);
                    mSocket.Close();

                }
                catch (Exception r)
                {
                    System.Console.WriteLine("Could not close the client socket");
                    return false;
                }
            }
            Thread.Sleep(1500);
            return true;
        }

        private void InOutSocketInit()
        {

            // Get the input and output streams; using temp objects because
            // member streams are .
            try
            {
                mInStream = mSocket.InputStream;
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("InputStream Socket fail to establish ERROR:4856");
                System.Console.WriteLine(e.Message);
                return;
            }


            try
            {
                mOutStream = mSocket.OutputStream;
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("OutputStream Socket fail to establish ERROR:4857");
                System.Console.WriteLine(e.Message);
                return;
            }

        }

        // Call this method from the main activity to shut down the connection.
        public void cancelSocketServ()
        {
            try
            {
                mSocket.Close();
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine("Could not cancel socket");
                throw;
            }
        }

        #endregion

        private void ShowToastMessage(String message)
        {
            context.RunOnUiThread(() =>
            {
                toastMessenger = Toast.MakeText(context, message, ToastLength.Long);
                toastMessenger.Show();
            });
        }

    }
}
