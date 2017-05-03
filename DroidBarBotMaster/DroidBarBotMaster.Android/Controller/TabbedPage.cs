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
using static Android.Widget.TabHost;

namespace DroidBarBotMaster.Droid.Controller
{
    [Activity(Label = "TabbedPage", MainLauncher = true)]
    public class TabbedPage : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.TabbedPage);

            xx = new LocalActivityManager(this, false);
            xx.DispatchCreate(savedInstanceState);
        }
        LocalActivityManager xx;
       

        protected override void OnStart()
        {
            base.OnStart();

            // create the TabHost that will contain the Tabs

            TabHost tabHost = FindViewById<TabHost>(Resource.Id.tabsHost);

            tabHost.Setup(xx);
            CreateTab(typeof(CocktailListview), "First Tab", "First Tab", tabHost);
            CreateTab(typeof(Start_and_Connect), "Sec Tab", "Sec Tab", tabHost);

        }

        private void CreateTab(Type activityType, string tag, string label, TabHost tabHost)
        {

            var intent = new Intent(this, activityType);
            intent.AddFlags(ActivityFlags.NewTask);

            var spec = tabHost.NewTabSpec(tag);
            spec.SetIndicator(label);
            spec.SetContent(intent);

            tabHost.AddTab(spec);
        }
    }
}