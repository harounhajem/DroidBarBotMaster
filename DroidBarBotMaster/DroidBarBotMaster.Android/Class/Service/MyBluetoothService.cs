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
    public class MyBluetoothService
    {
        private BluetoothAdapter mBluetoothAdapter;
        private BluetoothDevice mbarBotDevice;
        private BluetoothSocket mSocket;
        private System.IO.Stream mInStream;
        private System.IO.Stream mOutStream;
        private Toast toastMessenger;
        private Context context;



        public MyBluetoothService(Context context)
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
        public async void Read(TextView text, MainActivity main)
        {
            byte[] mmBuffer = new byte[1024];
            int numBytes; // bytes returned from read()

            // Keep listening to the InputStream until an exception occurs.

            List<Container> containerList = new List<Container>();

            for (int i = 0; i < 6; i++)
            {
                containerList.Add(new Container());
            }
            int listCount;
            Container[] contArray = new Container[6];

            Stopwatch timer = new Stopwatch();

            timer.Start();
            //text.Text = "Got it";
            while (true)
            {

                try
                {
                    //if (timer.ElapsedMilliseconds >= 6500)
                    //{
                    //    int t = 45;
                    //    break;
                    //}
                    // Read from the InputStream.
                    numBytes = await mInStream.ReadAsync(mmBuffer, 0, mmBuffer.Length);
                    // Send the obtained bytes to the UI activity.
                    if (numBytes > 0)
                    {
                        string recivedMessage = ASCIIEncoding.ASCII.GetString(mmBuffer);
                        System.Console.WriteLine(recivedMessage);
                        DeSerialize.DeSerializeArray(recivedMessage, containerList, this);

                        //listCount = 0;
                        //foreach (var item in containerList)
                        //{
                        //    if (item.Name != null)
                        //    {
                        //        listCount++;
                        //    }
                        //}


                        //if (listCount >= 6)
                        //{
                        //    String arrivedText = "";
                        //    foreach (var item in containerList)
                        //    {
                        //        arrivedText += item.Name + " " + item.Amount.ToString() + "\n";
                        //    }

                        //    //main
                        //    main.setText(arrivedText);

                        //    break;
                        //}
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
                System.Console.WriteLine("--SEND ASYNC--");
                System.Console.WriteLine(ASCIIEncoding.ASCII.GetString(bytes));

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
                System.Console.WriteLine("--SEND NO ASYNC--");
                System.Console.WriteLine(ASCIIEncoding.ASCII.GetString(bytes));

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
                toastMessenger = Toast.MakeText(context, "Error no BT found on device", ToastLength.Long);
                toastMessenger.Show();
                return false;
            }
            else
            {
                toastMessenger = Toast.MakeText(context, "Got adapter", ToastLength.Long);
                toastMessenger.Show();
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

                // TODO: Check for cancelaion

                //OnActivityResult(REQUEST_ENABLE_BT, Result.Canceled, enableBT);
                //if (REQUEST_ENABLE_BT == 0)
                //{
                //    toastMessenger = Toast.MakeText(this, "BT not enabled", ToastLength.Long);
                //    toastMessenger.Show();
                //    return;
                //}
            }
            toastMessenger = Toast.MakeText(context, "Enabled BT", ToastLength.Long);
            toastMessenger.Show();
            return true;
        }

        public bool GetBondedDevices()
        {
            // Get Bonded Devices
            mbarBotDevice = (from x in mBluetoothAdapter.BondedDevices
                             where x.Name.ToLower() == ("BarBotPi3").ToLower()
                             select x).FirstOrDefault();

            if (mbarBotDevice == null)
            {
                toastMessenger = Toast.MakeText(context, "Manual pair connect to BarBot please.", ToastLength.Long);
                toastMessenger.Show();
                return false;
            }
            else
            {
                toastMessenger = Toast.MakeText(context, "Found BarBot bond", ToastLength.Long);
                toastMessenger.Show();
                return true;
            }
        }

        public bool SocketConnect()
        {
            BluetoothSocket tmpSocket;
            // Set up socket
            try
            {
                UUID id = UUID.FromString("34B1CF4D-1069-4AD6-89B6-E161D79BE4D8");
                tmpSocket = mbarBotDevice.CreateInsecureRfcommSocketToServiceRecord(id);
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine("Socket's listen() method failed");
                toastMessenger = Toast.MakeText(context, "Found BarBot bond", ToastLength.Long);
                toastMessenger.Show();
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
            toastMessenger = Toast.MakeText(context, message, ToastLength.Long);
            toastMessenger.Show();
        }

    }
}
