using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Content;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Text;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_podat_zayavku:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_podat_zayavku);

            bool isConnected;
            var ping = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            var options = new PingOptions();

            try
            {
                var reply = ping.Send(host, timeout, buffer, options);
                var temp = reply.Status;
                temp = IPStatus.Success;
                isConnected = true;
            }
            catch (Exception ex) when (ex is PingException || ex is Exception)
            {
                isConnected = false;
            }

            if (isConnected == false)
            {
                Intent intent_no_internet = new Intent(this, typeof(Class_no_internet));
                StartActivity(intent_no_internet);
            }

            string id_napr = Intent.GetStringExtra("Id_napr" ?? "Id_napr");

            string userToken = "";
            try
            {
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                using (var reader = new StreamReader(backingFile1, true))
                {
                    string line1;
                    while ((line1 = reader.ReadLine()) != null)
                    {
                        userToken = line1;
                    }
                }
            }
            catch (Exception)
            {

            }

            RootObject napr_new = new RootObject();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + id_napr);
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "GET";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    napr_new = JsonConvert.DeserializeObject<RootObject>(result);
                }
            }
            catch (Exception)
            {

            }

            Button button_podat_zayavku = FindViewById<Button>(Resource.Id.button_podatZayavku_Activity_podat_zayavk);

            button_podat_zayavku.Click += delegate
            {
                string lot = FindViewById<EditText>(Resource.Id.editText_lot_Activity_podat_zayavku).Text.ToString();
                string betPrice = FindViewById<EditText>(Resource.Id.editText_betPrice_Activity_podat_zayavku).Text.ToString();
                string comment = FindViewById<EditText>(Resource.Id.editText_comment_Activity_podat_zayavku).Text.ToString();

                if (lot == "")
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Поле 'Лот' не может быть пустым!");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                if (betPrice == "")
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Поле 'Лот' не может быть пустым!");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }
                if (Convert.ToInt32(lot) > napr_new.data.transportation.lot)
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Невозможно указать большее значение лот, чем есть в перевозке!");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }
                if (Convert.ToInt32(betPrice) > 9999)
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Цена не может превышать 9999 ");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                PodatZayavku podatZayavku = new PodatZayavku()
                {
                    lot = lot,
                    betPrice = betPrice,
                    comment = comment
                };

                string baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + id_napr + "/app";

                try
                {
                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.Headers.Add("userToken", userToken);
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string pasredContent = JsonConvert.SerializeObject(podatZayavku);
                    UTF8Encoding encoding = new UTF8Encoding();
                    Byte[] bytes = encoding.GetBytes(pasredContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();


                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();

                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Успешно");
                    alert.SetMessage("Заявка подана успешно!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
                catch (WebException)
                {
                    //AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    //alert.SetTitle("Ошибка");
                    //alert.SetMessage("Произошла непредвиденная ошибка. Попробуйте повторить запрос позже.");
                    //alert.SetNeutralButton("OK", handllerNothingButton1);
                    //alert.Show();
                }

                void handllerNothingButton(object sender, DialogClickEventArgs e)
                {
                    Intent intent_to_prosmotr_napr = new Intent(this, typeof(Class_Prosmotr_napravleniia));
                    intent_to_prosmotr_napr.PutExtra("Id_napr", id_napr);
                    Finish();
                    StartActivity(intent_to_prosmotr_napr);
                }
            };
        }

        public class PodatZayavku
        {
            public string lot { get; set; }
            public string betPrice { get; set; }
            public string comment { get; set; }
        }

        public class ContainerType
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Line
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class TransportationType
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class App
        {
            public int lot { get; set; }
            public int price { get; set; }
            public string createdAt { get; set; }
        }

        public class Transportation
        {
            public int id { get; set; }
            public string cityOfLoading { get; set; }
            public string deliveryCity { get; set; }
            public string carDeliveryDate { get; set; }
            public int lot { get; set; }
            public int cargoWeightInContainer { get; set; }
            public ContainerType containerType { get; set; }
            public Line line { get; set; }
            public TransportationType transportationType { get; set; }
            public Status status { get; set; }
            public string updatedAt { get; set; }
            public string createdAt { get; set; }
            public int userId { get; set; }
            public List<App> apps { get; set; }
        }

        public class Data
        {
            public Transportation transportation { get; set; }
        }

        public class RootObject
        {
            public bool success { get; set; }
            public int code { get; set; }
            public Data data { get; set; }
            public List<object> errors { get; set; }
        }
    }
}