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
using System.IO;

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

        public static bool DeSerializeArray(String serialMessage, ICollection<Container> container, MyBluetoothService btService, Stream stream)
        {


            // Counter
            //serialMessage = serialMessage.TrimEnd('\');

            int splitBeginIndex = serialMessage.IndexOf('$');
            if (splitBeginIndex == -1)
                return false;

            int splitBeginName = serialMessage.IndexOf('#', splitBeginIndex);
            if (splitBeginName == -1)
                return false;

            int splitBeginNumber = serialMessage.IndexOf('&', splitBeginName);
            if (splitBeginNumber == -1)
                return false;

            int splitEndNumber = serialMessage.IndexOf('@', splitBeginNumber);
            if (splitEndNumber == -1)
                return false;

            if (splitBeginIndex == splitEndNumber)
            {
                return false;
            }


            // Cutter
            splitBeginIndex += 1;
            int indexLength = splitBeginName - splitBeginIndex;
            string indexPos = serialMessage.Substring(splitBeginIndex, indexLength);

            splitBeginName += 1;
            int nameLength = splitBeginNumber - splitBeginName;
            string nameMessage = serialMessage.Substring(splitBeginName, nameLength);

            splitBeginNumber += 1;
            int numberLength = splitEndNumber - splitBeginNumber;
            string numberMessage = serialMessage.Substring(splitBeginNumber, numberLength);



            // Double Check
            int _amount = 0, _index = 0;
            try
            {
                _index = Int32.Parse(indexPos);
                _amount = Int32.Parse(numberMessage);


            }
            catch (Exception e)
            {
                Console.WriteLine("\n\nERROR NUMBER PARSE FAIL" + e.Message + "\n\n");
                return false;
            }




            //if (_index < container.Count)
            //{

            //    btService.Write(ASCIIEncoding.ASCII.GetBytes("$" + container.Count + "#" + container.ElementAt(container.Count).Name + "&" + container.ElementAt(container.Count).Amount + "@"));
            //    stream.Flush();///---> Flush!
            //    return false;

            //}

            if (_index > container.Count - 1)
            {
                container.Add(new Container
                {
                    Name = nameMessage,
                    Amount = _amount
                });

                    btService.Write(ASCIIEncoding.ASCII.GetBytes("$" + indexPos + "#" + nameMessage + "&" + numberMessage + "@"));
                    Thread.Sleep(100);

                return true;
            }
            else
            {
                return false;

            }

            //container.ElementAt(_index).Name = nameMessage;
            //container.ElementAt(_index).Amount = _amount;





        }
    }
}