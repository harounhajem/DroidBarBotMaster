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

namespace DroidBarBotMaster.Droid.Class.Helper
{
    public static class TransporterClass
    {
        public static List<Container> listContainer { get; set; }

        public static BluetoothService bluetoothService { get; set; }

        public static BarBot barBot { get; set; }

    }
}