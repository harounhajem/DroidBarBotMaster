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
using DroidBarBotMaster.Droid.Class.Service;
using System.Threading;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    public static class DeSerialize
    {
        public static bool DeSerializer(String serialMessage, ICollection<Container> container, MyBluetoothService btService)
        {
            if (container == null || serialMessage == null)
                return false;

            String[] splitMessage = serialMessage.Split('#');

            int s;
            if (!Int32.TryParse(splitMessage[1], out s) || splitMessage.Length != 12)
            {
                Console.WriteLine("ERROR ERROR MESSAGE CORRUPT, CALL AGAIN!");
                byte[] send = { 101 };
                btService.Write(send);

                //Random rn = new Random();
                //Thread.Sleep(rn.Next(0, 800));
                return false;
            }





            for (int i = 0; i < splitMessage.Length; i++)
            {
                if (i % 2 == 0)
                {
                    int newAmount;
                    Int32.TryParse(splitMessage[i + 1], out newAmount);
                    container.Add(new Container()
                    {
                        Name = splitMessage[i],
                        Amount = newAmount
                    });

                }
            }




            return true;


        }

        public static bool DeSerializeArray(String serialMessage, Container[] array, MyBluetoothService btService)
        {
            String[] splitMessage = serialMessage.Split('#');
            int s;

            for (int i = 0; i < splitMessage.Length; i++)
            {


                if (i + 2 >= splitMessage.Length)
                {
                    break;
                }
                bool ans = Int32.TryParse(splitMessage[i+2], out s);

                if (splitMessage[i].Length < 1 && ans)
                {

                    int number = Int32.Parse(splitMessage[i]);

                    if (array[number] == null)
                    {
                        array[number] = new Container()
                        {
                            Name = splitMessage[i + 1],
                            Amount = Int32.Parse(splitMessage[i+2])
                        };
                    }
                }

            }

            if (array.Contains(null))
            {

                byte[] send = { 101 };
                btService.Write(send);
                return false;
            }

            return true;
        }
    }
}