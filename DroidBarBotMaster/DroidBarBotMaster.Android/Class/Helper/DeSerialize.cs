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

        public static bool DeSerializeArray(String serialMessage, ICollection<Container> container, MyBluetoothService btService)
        {

            int splitBeginIndex = serialMessage.IndexOf('$');
            if (splitBeginIndex == -1)
                return false;

            int splitBeginName = serialMessage.IndexOf('#', splitBeginIndex);
            if (splitBeginName == -1)
                return false;

            int splitEndNameBeginNumber = serialMessage.IndexOf('&', splitBeginName);
            if (splitEndNameBeginNumber == -1)
                return false;

            int splitEndNumber = serialMessage.IndexOf('@', splitEndNameBeginNumber);
            if (splitEndNumber == -1)
                return false;

            if (splitBeginIndex == splitEndNumber)
            {
                return false;
            }




            string indexPos = serialMessage.Substring(splitBeginIndex + 1, splitBeginName - splitBeginIndex - 1);
            string nameMessage = serialMessage.Substring(splitBeginName + 1, splitEndNameBeginNumber - splitBeginName - 1);
            string numberMessage = serialMessage.Substring(splitEndNameBeginNumber + 1, splitEndNumber - splitEndNameBeginNumber - 1);


            int _amount = 0, _index = 0;
            try
            {
                _index = Int32.Parse(indexPos);
                _amount = Int32.Parse(numberMessage);

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR NUMBER PARSE FAIL");
                return false;
            }





            if (container.ElementAt(_index).Name == nameMessage)
            {
                btService.WriteAsync(ASCIIEncoding.ASCII.GetBytes("$" + indexPos + "#" + nameMessage + "&" + numberMessage + "@"));
                return false;
            }





            container.ElementAt(_index).Name = nameMessage;
            container.ElementAt(_index).Amount = _amount;

            btService.WriteAsync(ASCIIEncoding.ASCII.GetBytes("$" + indexPos + "#" + nameMessage + "&" + numberMessage + "@"));




            return true;
        }
    }
}