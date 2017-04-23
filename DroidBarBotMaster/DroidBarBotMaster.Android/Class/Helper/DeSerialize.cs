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

        public static bool DeSerializeArray(String serialMessage, ICollection<Container> container, BluetoothService btService)
        {



            serialMessage = serialMessage.Replace("\0", String.Empty);

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


            if (_index > container.Count - 1)
            {
                container.Add(new Container
                {
                    Name = nameMessage,
                    Amount = _amount
                });

                Console.WriteLine($"Added :{nameMessage}");

                btService.Write(ASCIIEncoding.ASCII.GetBytes("$" + indexPos + "#" + nameMessage + "&" + numberMessage + "@"));
                Thread.Sleep(100);

                return true;
            }
            else
            {
                return false;

            }
        }
    }
}