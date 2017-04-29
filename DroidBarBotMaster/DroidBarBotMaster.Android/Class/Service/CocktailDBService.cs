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
using Newtonsoft.Json;
using DroidBarBotMaster.Droid.Class.Model;
using System.Net;
using System.IO;

namespace DroidBarBotMaster.Droid.Class.Service
{
    public static class CocktailDBService
    {
        public static DrinkMultiple HttpGet(string stringGet, HttpGetRequests enumRequest)
        {

            string html = string.Empty;

            string requestApi = "empty";

            switch (enumRequest)
            {
                case HttpGetRequests.CocktailByIngridient:
                    requestApi = "filter";
                    break;
                case HttpGetRequests.CocktailByID:
                    requestApi = "lookup";
                    break;
                case HttpGetRequests.IngridientInfo:
                    requestApi = "search";
                    break;
                default:
                    break;
            }

            string url = $"http://www.thecocktaildb.com/api/json/v1/1/{requestApi}.php?i={stringGet}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Accept = "application/json";

            DrinkMultiple drinks = new DrinkMultiple();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
                drinks = JsonConvert.DeserializeObject<DrinkMultiple>(html);
            }

            return drinks;
        }
    }

    public enum HttpGetRequests
    {
        CocktailByIngridient,
        CocktailByID,
        IngridientInfo

    }
}