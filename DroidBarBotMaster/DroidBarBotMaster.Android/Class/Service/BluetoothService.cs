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
        public Start_and_Connect context;



        public BluetoothService(Start_and_Connect context)
        {
            this.context = context;

           
            context.RunOnUiThread(() => 
            toastMessenger = new Toast(this.context));
        }


        #region Write, Read
        public async void ReadGetIngridients(int runTime, List<Container> containerList)
        {
            byte[] mmBuffer = new byte[1024];
            int numBytes; // bytes returned from read()

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (true)
            {
                try
                {
                    System.Console.WriteLine("reciving...");
                    if (timer.ElapsedMilliseconds >= runTime || containerList.Count > 5)
                    {
                        break;
                    }
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

        public async Task<bool> ReadOkMessageAsync(int timerTimeOut, int bufferAmount)
        {
            byte[] mmBuffer = new byte[bufferAmount];
            int numBytes; // bytes returned from read()

            Stopwatch timer = new Stopwatch();
            timer.Start();

            //text.Text = "Got it";
            while (true)
            {
                try
                {
                    numBytes = await mInStream.ReadAsync(mmBuffer, 0, mmBuffer.Length);
                    // Send the obtained bytes to the UI activity.
                    if (numBytes > 0)
                    {
                        string recivedMessage = ASCIIEncoding.ASCII.GetString(mmBuffer);
                        System.Console.WriteLine(recivedMessage);

                        recivedMessage = recivedMessage.Substring(0, 2);

                        if (recivedMessage.ToLower().Contains("ok"))
                        {
                            return true;
                        }
                    }

                }
                catch (System.IO.IOException e)
                {
                    System.Console.WriteLine("InputStream failure ERROR:1364");
                    System.Console.WriteLine(e.Message);
                    return false;
                    throw;
                }
                System.Console.WriteLine(timer.ElapsedMilliseconds.ToString());
                if (timer.ElapsedMilliseconds >= timerTimeOut)
                {
                    return false;
                }

            }
            return false;
        }
        public bool ReadOkMessage(int timerTimeOut, int bufferAmount)
        {
            byte[] mmBuffer = new byte[bufferAmount];
            int numBytes; // bytes returned from read()

            Stopwatch timer = new Stopwatch();
            timer.Start();

            Thread.Sleep(100);

            string msgRecived=""; 
            while (true)
            {
                try
                {
                    numBytes = mInStream.Read(mmBuffer, 0, mmBuffer.Length);
                    // Send the obtained bytes to the UI activity.
                    if (numBytes > 0)
                    {
                        string recivedMessage = ASCIIEncoding.ASCII.GetString(mmBuffer).Replace("\0",string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);

                        System.Console.WriteLine(recivedMessage);
                        msgRecived += recivedMessage;
                        //recivedMessage = recivedMessage.Substring(0, 2);

                        if (msgRecived.ToLower().Contains("ok"))
                        {
                            return true;
                        }
                    }

                }
                catch (System.IO.IOException e)
                {
                    System.Console.WriteLine("InputStream failure ERROR:1364");
                    System.Console.WriteLine(e.Message);
                    return false;
                    throw;
                }
                System.Console.WriteLine(timer.ElapsedMilliseconds.ToString());
                if (timer.ElapsedMilliseconds >= timerTimeOut)
                {
                    return false;
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
                ShowToastMessage("Writing: " + ASCIIEncoding.ASCII.GetString(bytes), ToastLength.Long);

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
                ShowToastMessage("Writing: " + ASCIIEncoding.ASCII.GetString(bytes), ToastLength.Long);
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

        public bool ConnectActivateBluetooth()
        {

            if (!DeviceHasBT())             return false;

            if (!EnableBTdevice().Result)   return false;

            if (!GetBondedDevices())        return false;

            if (!SocketConnect())           return false;

            if (InOutSocketInit())          return false;

            ShowToastMessage("CONNECTED TO BARBOT", ToastLength.Long);

            return true;
        }

        public bool DeviceHasBT()
        {
            mBluetoothAdapter = mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter == null)
            {
                ShowToastMessage("Error no BT found on device", ToastLength.Long);
                return false;
            }
            else
            {
                ShowToastMessage("Got adapter", ToastLength.Short);
            }
            return true;

        }

        public async Task<bool> EnableBTdevice()
        {
            int REQUEST_ENABLE_BT = 0;


            // Enable BT
            if (!mBluetoothAdapter.IsEnabled)
            {
                Intent enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
                Activity s = new Activity();

                // TODO: Check for cancelation

                //Thread BToutput = new Thread(() => { context.StartActivityForResult(enableBT, REQUEST_ENABLE_BT); });
                context.StartActivityForResult(enableBT, REQUEST_ENABLE_BT);

                context.adapterOnActivityResult(REQUEST_ENABLE_BT, Result.Canceled, enableBT);
                

                if (!mBluetoothAdapter.IsEnabled)
                {
                    ShowToastMessage("BT not enabled", ToastLength.Long);
                    
                    return false;
                }
            }
            ShowToastMessage("Enabled BT", ToastLength.Long);
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
                ShowToastMessage("Manual pair connect to BarBot please.", ToastLength.Long);
                return false;
            }
            else
            {
                ShowToastMessage("Found BarBot bond", ToastLength.Short);
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
                ShowToastMessage("Found BarBot bond", ToastLength.Short);
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
                    System.Console.WriteLine(r.Message);
                    return false;
                }
            }
            Thread.Sleep(1500);
            return true;
        }

        private bool InOutSocketInit()
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
                return false;
            }


            try
            {
                mOutStream = mSocket.OutputStream;
            }
            catch (System.IO.IOException e)
            {
                System.Console.WriteLine("OutputStream Socket fail to establish ERROR:4857");
                System.Console.WriteLine(e.Message);
                return false;
            }

            return true;
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

        public void ShowToastMessage(String message, ToastLength ts)
        {
            context.RunOnUiThread(() =>
            {
                toastMessenger = Toast.MakeText(context, message, ts);
                toastMessenger.Show();
            });
        }

    }
}
