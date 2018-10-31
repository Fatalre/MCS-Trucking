using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Graphics;
using Android.Util;
using Android.Content;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_Prosmotr_napravleniia:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_prosmotr_napravleniia);

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

            string Id_napr = Intent.GetStringExtra("Id_napr" ?? "Id_napr");

            TextView textView_id_napr = FindViewById<TextView>(Resource.Id.textView_idnapr_activity_prosmotr_napr);
            textView_id_napr.Text = Id_napr;
            textView_id_napr.SetTextColor(Color.White);

            RootObject napr_new = new RootObject();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + Id_napr);
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
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Ошибка сервера. Попробуйте позже.");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {
                //Действие при нажатие "ОК"
            }


            TextView textView_Napravlenie = new TextView(this);
            textView_Napravlenie.Text = napr_new.data.transportation.cityOfLoading + " - " + napr_new.data.transportation.deliveryCity;
            textView_Napravlenie.SetTextColor(Color.ParseColor("#ff274284"));
            textView_Napravlenie.SetTextSize(ComplexUnitType.Dip, 20);
            textView_Napravlenie.TextAlignment = TextAlignment.Center;

            TextView textView_opisanie = new TextView(this);
            textView_opisanie.Text = "Дата постановки машины: "
                    + napr_new.data.transportation.carDeliveryDate + "\n" + "Лот: "
                    + napr_new.data.transportation.lot + "\n" + "Вес контейнера: "
                    + napr_new.data.transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                    + napr_new.data.transportation.containerType.name + "\n" + "Линия: "
                    + napr_new.data.transportation.line.name + "\n" + "Направление: "
                    + napr_new.data.transportation.transportationType.name + "\n" + "Статус: "
                    + napr_new.data.transportation.status.name;
            textView_opisanie.SetTextColor(Color.Black);

            TextView textView_status_transport = FindViewById<TextView>(Resource.Id.textView_status_perevozki);
            textView_status_transport.Text = napr_new.data.transportation.status.name;
            textView_status_transport.SetTextColor(Color.White);

            LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_Prosmotr_Napravleniia);
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

            Button button_podat_zayavku = new Button(this);
            button_podat_zayavku.Text = "Подать заявку";
            button_podat_zayavku.TextAlignment = TextAlignment.Center;
            button_podat_zayavku.Click += OnButtonClick;

            ll.AddView(textView_Napravlenie, lp);
            ll.AddView(textView_opisanie, lp);
            ll.AddView(button_podat_zayavku, lp);

            for (int i=0; i < napr_new.data.transportation.apps.Count; i++)
            {

                TextView textView_zayavki = new TextView(this);
                textView_zayavki.Text = "Заявка № " + (i+1).ToString() + ": "+"Лот: "+napr_new.data.transportation.apps[i].lot + " Цена: "+napr_new.data.transportation.apps[i].price;
                textView_zayavki.SetTextColor(Color.DarkGreen);
                textView_zayavki.TextAlignment = TextAlignment.Center;

                ll.AddView(textView_zayavki, lp);
            }

            Button button_nazad = new Button(this);
            button_nazad.Text = "Вернутся к просмотру направлений";
            button_nazad.TextAlignment = TextAlignment.Center;
            button_nazad.SetBackgroundColor(Color.Bisque);
            button_nazad.Click += OnButtonClick1;

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

            RootObject1 user_zayavki = new RootObject1();
            try
            {
                var httpWebRequest1 = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + Id_napr + "/app");
                httpWebRequest1.Headers.Add("userToken", userToken);
                httpWebRequest1.Accept = "application/json";
                httpWebRequest1.Method = "GET";
                var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    user_zayavki = JsonConvert.DeserializeObject<RootObject1>(result);
                }

                TextView textView_zayavki1 = new TextView(this);
                textView_zayavki1.Text = "Ваша заявка: " + "\n" + "Лот: " + user_zayavki.data.app.lot + " Цена: "
                    + user_zayavki.data.app.bet_price + " Комментарий: "+user_zayavki.data.app.comment;
                textView_zayavki1.SetTextColor(Color.DarkRed);
                textView_zayavki1.TextAlignment = TextAlignment.Center;

                ll.AddView(textView_zayavki1, lp);

                button_podat_zayavku.Text = "Изменить заявку";
                button_podat_zayavku.SetBackgroundColor(Color.AliceBlue);
                button_podat_zayavku.Click += OnButtonClick;
            }
            catch (Exception)
            {
                button_podat_zayavku.Text = "Подать заявку";
                button_podat_zayavku.SetBackgroundColor(Color.OrangeRed);
            }

            ll.AddView(button_nazad, lp);
        }

        private void OnButtonClick1(object sender, System.EventArgs e)
        {
            Intent intent_to_start = new Intent(this, typeof(MainActivity));
            Finish();
            StartActivity(intent_to_start);
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            TextView id_napr = (TextView)FindViewById<TextView>(Resource.Id.textView_idnapr_activity_prosmotr_napr);
            TextView textView = FindViewById<TextView>(Resource.Id.textView_status_perevozki);
            Button myButton = (Button)sender;
            if (myButton.Text == "Изменить заявку")
            {
                Intent intent_izm_zayavky = new Intent(this, typeof(Class_izm_zayavku));
                intent_izm_zayavky.PutExtra("Id_napr", id_napr.Text.ToString());
                intent_izm_zayavky.PutExtra("Status_transport", textView.Text);
                Finish();
                StartActivity(intent_izm_zayavky);
                
            }
            else
            {
                Intent intent_podat_zayavku = new Intent(this, typeof(Class_podat_zayavku));
                intent_podat_zayavku.PutExtra("Id_napr",id_napr.Text.ToString());
                Finish();
                StartActivity(intent_podat_zayavku);
            }
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




        public class App1
        {
            public int id { get; set; }
            public int transportation_id { get; set; }
            public int user_id { get; set; }
            public int lot { get; set; }
            public int bet_price { get; set; }
            public string comment { get; set; }
            public object accepted { get; set; }
            public int lot_accepted { get; set; }
            public object assessment { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Data1
        {
            public App1 app { get; set; }
        }

        public class RootObject1
        {
            public bool success { get; set; }
            public int code { get; set; }
            public Data1 data { get; set; }
            public List<object> errors { get; set; }
        }
    }
}