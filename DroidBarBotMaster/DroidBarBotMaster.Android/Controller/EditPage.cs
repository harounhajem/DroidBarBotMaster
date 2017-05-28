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
using DroidBarBotMaster.Droid.Class.Service;

namespace DroidBarBotMaster.Droid.Controller
{
    [Activity(Label = "EditPage")]
    public class EditPage : Activity
    {
        List<Container> bottles;

        Container selectedBottle;

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditPage);

            bottles = TransporterClass.listContainer;

            FindViewById<Button>(Resource.Id.btnedit1).Click += btnBottleClick;
            FindViewById<Button>(Resource.Id.btnedit2).Click += btnBottleClick;
            FindViewById<Button>(Resource.Id.btnedit3).Click += btnBottleClick;
            FindViewById<Button>(Resource.Id.btnedit4).Click += btnBottleClick;
            FindViewById<Button>(Resource.Id.btnedit5).Click += btnBottleClick;
            FindViewById<Button>(Resource.Id.btnedit6).Click += btnBottleClick;

            FindViewById<Button>(Resource.Id.editsave).Click += btnSave;


            PopulateViewWithData();
            AnimateButton(FindViewById<Button>(Resource.Id.editsave));
        }

        private void AnimateButton(Button btn)
        {
            Button btnConnect = btn;
            btnConnect.Alpha = 0f;
            btnConnect.Animate()
                      .Alpha(1f)
                      .SetDuration(1200);
            btnConnect.Animate().Start();

        }

        private void btnSave(object sender, EventArgs e)
        {

            Container oldSelectedBottle = selectedBottle;

            // 1. Hämta värderna

            EditText inputName = FindViewById<EditText>(Resource.Id.editTextDrinkName);

            if (String.IsNullOrEmpty(inputName.Text)) return;

            selectedBottle.Name = FindViewById<EditText>(Resource.Id.editTextDrinkName).Text;


            EditText inputValue = FindViewById<EditText>(Resource.Id.editTextAmount);

            if (String.IsNullOrEmpty(inputValue.Text)) return;

            selectedBottle.Amount = Int32.Parse(inputValue.Text);

            // 3. Skicka spara till maskinen

            //int bottlePos = bottles.FindIndex(x => x.Name == oldSelectedBottle.n);


            TransporterClass.barBot.PostIngridients(selectedBottle, bottlePosFromName);



            // 4. Updatera vyn & uppdatera Transporter

            bottles[bottlePosFromName].Name = selectedBottle.Name;
            bottles[bottlePosFromName].Amount = selectedBottle.Amount;

            UpdateButtonSubtitles();

            RepopulateList();
        }

        private void RepopulateList()
        {
            //throw new NotImplementedException();
        }

        int bottlePosFromName;
        private void btnBottleClick(object sender, EventArgs e)
        {
            Button btnClicked = sender as Button;

            if (btnClicked == null) return;

            bottlePosFromName = Int32.Parse(btnClicked.Text) - 1;

            UpdateInputField(bottles[bottlePosFromName]);

        }

        private void UpdateInputField(Container clickedBottle)
        {
            if (clickedBottle == null) return;

            FindViewById<EditText>(Resource.Id.editTextDrinkName).Text = clickedBottle.Name;

            FindViewById<EditText>(Resource.Id.editTextAmount).Text = clickedBottle.Amount.ToString();
        }

        private void PopulateViewWithData()
        {
            if (bottles == null) return;

            UpdateButtonSubtitles();

            if (bottles[0] != null)
            {
                selectedBottle = new Container();

                selectedBottle.Name = bottles[0].Name;

                selectedBottle.Amount = bottles[0].Amount;

                UpdateInputField(selectedBottle);
            }

        }

        private void UpdateButtonSubtitles()
        {
            int count = bottles.Count;
            FindViewById<TextView>(Resource.Id.bottletextName1).Text = count > 0 && bottles[0] != null ? bottles[0].Name : "";
            FindViewById<TextView>(Resource.Id.bottletextName2).Text = count > 1 && bottles[1] != null ? bottles[1].Name : "";
            FindViewById<TextView>(Resource.Id.bottletextName3).Text = count > 2 && bottles[2] != null ? bottles[2].Name : "";
            FindViewById<TextView>(Resource.Id.bottletextName4).Text = count > 3 && bottles[3] != null ? bottles[3].Name : "";
            FindViewById<TextView>(Resource.Id.bottletextName5).Text = count > 4 && bottles[4] != null ? bottles[4].Name : "";
            FindViewById<TextView>(Resource.Id.bottletextName6).Text = count > 5 && bottles[5] != null ? bottles[5].Name : "";
        }
    }
}