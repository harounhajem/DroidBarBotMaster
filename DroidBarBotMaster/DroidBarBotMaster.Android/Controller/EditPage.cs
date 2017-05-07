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
        }

        private void btnSave(object sender, EventArgs e)
        {

            Container oldValue = selectedBottle;

            // 1. Hämta värderna

            EditText inputName = FindViewById<EditText>(Resource.Id.editTextDrinkName);

            if (String.IsNullOrEmpty(inputName.Text)) return;

            selectedBottle.Name = FindViewById<EditText>(Resource.Id.editTextDrinkName).Text;


            EditText inputValue = FindViewById<EditText>(Resource.Id.editTextAmount);

            if (String.IsNullOrEmpty(inputValue.Text)) return;

            selectedBottle.Amount = Int32.Parse(inputValue.Text);

            // 3. Skicka spara till maskinen

            Container oldBottle = bottles.Find(x => x == oldValue);

            TransporterClass.barBot.PostIngridients(selectedBottle, bottles.FindIndex(x => x == oldBottle));

            // 4. Updatera vyn & uppdatera Transporter

            oldBottle = selectedBottle;

            UpdateButtonSubtitles();
        }

        private void btnBottleClick(object sender, EventArgs e)
        {
            Button btnClicked = sender as Button;

            if (btnClicked == null) return;

            int bottlePosFromName = Int32.Parse(btnClicked.Text) - 1;

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
                selectedBottle = bottles[0];

                UpdateInputField(selectedBottle);
            }

        }

        private void UpdateButtonSubtitles()
        {
            FindViewById<TextView>(Resource.Id.bottletextName1).Text = bottles[0]?.Name;
            FindViewById<TextView>(Resource.Id.bottletextName2).Text = bottles[1]?.Name;
            FindViewById<TextView>(Resource.Id.bottletextName3).Text = bottles[2]?.Name;
            FindViewById<TextView>(Resource.Id.bottletextName4).Text = bottles[3]?.Name;
            FindViewById<TextView>(Resource.Id.bottletextName5).Text = bottles[4]?.Name;
            FindViewById<TextView>(Resource.Id.bottletextName6).Text = bottles[5]?.Name;
        }
    }
}