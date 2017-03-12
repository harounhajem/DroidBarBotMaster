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
using DroidBarBotMaster.Droid.Class.Model;
using System.Threading;

namespace DroidBarBotMaster.Droid.Class.Service
{
    public class BarBot
    {
        BluetoothService btService;

        enum Commands
        {   
            GetIngridients = 101,    // e
            PostIngridients = 100,   // d
            SendCocktailOrder = 102  // f

        }

        public BarBot(BluetoothService btService)
        {
            this.btService = btService;
        }

        public void UpdateIngridients(Container container, int position)
        {
           

        }

        public void PostIngridients(Container container, int position)
        {
            btService.WriteAsync(new byte[] {Convert.ToByte(Commands.PostIngridients)});

            // MockData
            //container.Name = "Walle Edstam";
            //container.Amount = 12345;
            //position = 1;

            Thread.Sleep(100);
            btService.WriteAsync(Encoding.ASCII.GetBytes("$" + position + "#" + container.Name + "&" + container.Amount + "@"));

        }

        public void SendCocktailOrder()
        {
            // Skicka någon typ av lista

        }

        public void GetIngridients()
        {
            Thread writeToSlave = new Thread(() =>
            btService.Write(new byte[] { Convert.ToByte(Commands.GetIngridients)}));
            // Read Message
            Thread readInputThread = new Thread(() => btService.ReadGetIngridients());

            writeToSlave.Start();
            readInputThread.Start();

        }


    }
}