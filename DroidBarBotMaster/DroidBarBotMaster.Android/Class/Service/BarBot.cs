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
using DroidBarBotMaster.Droid.Class.Helper;
using System.Threading;
using System.Threading.Tasks;
using DroidBarBotMaster.Droid.Class.Helper;

namespace DroidBarBotMaster.Droid.Class.Service
{
    public class BarBot
    {
        BluetoothService btService;

        private enum CommandsToBarBot
        {
            GetIngridients = 101,    // e
            PostIngridients = 100,   // d
            SendCocktailOrder = 102  // f

        }

        public BarBot(BluetoothService btService)
        {
            this.btService = btService;
        }

        public void PostIngridients(Container container, int position)
        {
            btService.WriteAsync(new byte[] { Convert.ToByte(CommandsToBarBot.PostIngridients) });

            Thread.Sleep(100); // Wait for sync, BT problem

            btService.WriteAsync(Encoding.ASCII.GetBytes("$" + position + "#" + container.Name + "&" + container.Amount + "@"));

        }

        public async void SendCocktailOrder()
        {
            // 1. Skicka commando
            btService.Write(new byte[] { Convert.ToByte(CommandsToBarBot.SendCocktailOrder) });

            // 2. Skicka meddelandet
            String msg = "$3;1158;2;2073;1;1307;4;452;5;1271;@";
            Random rand = new Random(DateTime.Now.Millisecond);
            string cmdToSend = $"$1;{rand.Next(1, 7).ToString()};2;{rand.Next(1, 7).ToString()};3;{rand.Next(1, 7).ToString()};4;{rand.Next(1, 7).ToString()};5;{rand.Next(1, 7).ToString()};6;{rand.Next(1, 7).ToString()};@";
            btService.Write(Encoding.ASCII.GetBytes(cmdToSend));


            ///--------------- NOT WORKING -------------////////
            /// TODO: Fix that read input.stream code blocking in btService
            /// 
            ///// 3. Vänta på svar att meddelandet är accepterat
            //bool ans = btService.ReadOkMessage(2000, 10);

            //if (ans)
            //{
            //    Console.WriteLine("Order confirmed");
            //}

            //// 4. 
            ///--------------- NOT WORKING -------------////////


        }
        internal void SendCocktailOrder(Drink drink)
        {
            // TODO: Send drink

            Dictionary<int, int> filteredDrinkIngridients = FilterAndFormatDrinkOrder(drink);

            // 3. Send it

            // 3.1 Skicka commando

            const char startBTMessage = '$',
                       endBTMessage = '@',
                       seperator = ';';

            string bluetoothMessage = startBTMessage.ToString(); 

            foreach (var item in filteredDrinkIngridients)
            {
                bluetoothMessage += item.Key.ToString() + seperator + item.Value + seperator;
            }

            bluetoothMessage += endBTMessage.ToString();

            if (bluetoothMessage.Length < 5) return;

            btService.Write(new byte[] { Convert.ToByte(CommandsToBarBot.SendCocktailOrder) });

            btService.Write(Encoding.ASCII.GetBytes(bluetoothMessage));




            ///--------------- NOT WORKING -------------////////
            /// TODO: Fix that read input.stream code blocking in btService
            /// 
            ///// 3. Vänta på svar att meddelandet är accepterat
            //bool ans = btService.ReadOkMessage(2000, 10);

            //if (ans)
            //{
            //    Console.WriteLine("Order confirmed");
            //}

            //// 4. 
            ///--------------- NOT WORKING -------------////////
        }

        private Dictionary<int, int> FilterAndFormatDrinkOrder(Drink drink)
        {

            List<Container> listContainer = TransporterClass.listContainer;

            SortedDictionary<string, int> listDrinkIngridients = drink.GetStrIngridientsMeasurementDictionary();

            Dictionary<int, int> filteredDrinkOrder = FilterDrinkOrder(listContainer, listDrinkIngridients);
           
            return filteredDrinkOrder;
        }

        private Dictionary<int, int> FilterDrinkOrder(List<Container> listContainer, SortedDictionary<string, int> listDrinkIngridients)
        {
            Dictionary<int, int> filteredDrinkOrder = new Dictionary<int, int>();

            for (int i = 0; i < listContainer.Count; i++)
            {

                foreach (var drinkItem in listDrinkIngridients)
                {
                    if (drinkItem.Key.ToLower() == listContainer[i].Name.ToLower())
                    {

                        if (!filteredDrinkOrder.ContainsKey(i))
                        {
                            filteredDrinkOrder.Add(i, drinkItem.Value);
                        }
                        else
                        {
                            int addedValue = filteredDrinkOrder[i] + drinkItem.Value;

                            filteredDrinkOrder.Add(i, addedValue);
                        }
                    }
                }
            }
            return filteredDrinkOrder;
        }

        public void GetIngridients(int runTime, List<Container> listContainer)
        {
            Thread writeToSlave = new Thread(() => btService.Write(new byte[] { Convert.ToByte(CommandsToBarBot.GetIngridients) }));


            Thread readInputThread = new Thread(() => btService.ReadGetIngridients(runTime, listContainer));

            writeToSlave.Start();

            readInputThread.Start();

        }
    }
}