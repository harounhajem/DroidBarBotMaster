using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using static Android.Views.View;

namespace DroidBarBotMaster.Droid.Controller
{
    public class ListItemClick : IOnClickListener
    {
        public IntPtr Handle
        {
            get{ return Handle; }
        }

        public void Dispose()
        {
            
        }

        public void OnClick(View v)
        {
            float xx = v.GetY();
        }

        private void Initialize()
        {
        }
    }
}