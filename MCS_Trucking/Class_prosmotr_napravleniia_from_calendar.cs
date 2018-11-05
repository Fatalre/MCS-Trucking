using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
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
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges =
        Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Class_prosmotr_napravleniia_from_calendar:Activity
    {
        bool Vhod = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_prosmotr_napravleniia_from_calendar);


            try
            {
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                string userToken = "";
                using (var reader = new StreamReader(backingFile1, true))
                {
                    string line1;
                    while ((line1 = reader.ReadLine()) != null)
                    {
                        userToken = line1;
                    }
                }

                Vhod = true;
                if (userToken == "")
                {
                    Vhod = false;
                }
            }
            catch (Exception)
            {

            }


            string[] id_napravleniia = Intent.GetStringArrayExtra("id_napr" ?? "id_napr");

            for (int i = 0; i < id_napravleniia.Length; i++)
            {
                if(id_napravleniia[i] != "0")
                {
                    RootObject1 napr_new = new RootObject1();

                M2: try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + id_napravleniia[i]);
                        httpWebRequest.Accept = "application/json";
                        httpWebRequest.Method = "GET";
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            napr_new = JsonConvert.DeserializeObject<RootObject1>(result);
                        }
                    }
                    catch (Exception)
                    {
                        if (napr_new.data == null)
                        {
                            goto M2;
                        }

                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Ошибка");
                        alert.SetMessage("Ошибка сервера. Попробуйте позже.");
                        alert.SetNeutralButton("OK", handllerNothingButton);
                        alert.Show();
                    }

                    Button myButton = new Button(this);
                    myButton.Text = napr_new.data.transportation.cityOfLoading + " - " + napr_new.data.transportation.deliveryCity;
                    myButton.TextAlignment = Android.Views.TextAlignment.Center;
                    myButton.SetTextColor(Color.ParseColor("#ff274284"));

                    if (napr_new.data.transportation.status.name == "Перевозка отменена")
                    {
                        myButton.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                    }
                    else if (napr_new.data.transportation.status.name == "Завершена")
                    {
                        myButton.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                    }
                    else if (napr_new.data.transportation.status.name == "Ожидания заявок")
                    {
                        myButton.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                    }
                    else if (napr_new.data.transportation.status.name == "Заявки рассмотрено")
                    {
                        myButton.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                    }
                    else
                    {
                        myButton.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                    }

                    myButton.SetTextSize(ComplexUnitType.Dip, 15);
                    myButton.Id = View.GenerateViewId();
                    myButton.Click += OnButtonClick;

                    TextView mytextView = new TextView(this);
                    mytextView.Text = "Дата постановки машины: "
                        + napr_new.data.transportation.carDeliveryDate + "\n" + "Лот: "
                        + napr_new.data.transportation.lot + "\n" + "Вес контейнера: "
                        + napr_new.data.transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                        + napr_new.data.transportation.containerType.name + "\n" + "Линия: "
                        + napr_new.data.transportation.line.name + "\n" + "Направление: "
                        + napr_new.data.transportation.transportationType.name + "\n" + "Статус: "
                        + napr_new.data.transportation.status.name;
                    mytextView.SetTextColor(Color.Black);
                    mytextView.Id = View.GenerateViewId();

                    TextView textView_id = new TextView(this);
                    textView_id.Text = napr_new.data.transportation.id.ToString();
                    textView_id.SetTextColor(Color.White);
                    textView_id.Id = View.GenerateViewId();

                    LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_prosmotr_napravleniia_from_calendar);
                    LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                        (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                    ll.AddView(myButton, lp);
                    ll.AddView(mytextView, lp);
                    ll.AddView(textView_id, lp);
                }
                else
                {

                }
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {
                //Действие при нажатие "ОК"
            }
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            Button myButton = (Button)sender;
            TextView textView_id = (TextView)FindViewById<TextView>(myButton.Id + 2);

            var Napravlenie = myButton.Text.ToString();
            var Id_napr = textView_id.Text.ToString();

            if (Vhod == true)
            {
                string vid = "Календарь";
                Intent intent_to_Prosmotr_napr = new Intent(this, typeof(Class_Prosmotr_napravleniia));
                intent_to_Prosmotr_napr.PutExtra("Id_napr", Id_napr);
                intent_to_Prosmotr_napr.PutExtra("Vid", vid);
                StartActivity(intent_to_Prosmotr_napr);
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Для просмотра направления Вам необходимо войти либо зарегистрироваться в системе.");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            void handllerNothingButton(object sender1, DialogClickEventArgs e1)
            {
                Intent Activity_Vhod = new Intent(this, typeof(Class_Vhod));
                StartActivity(Activity_Vhod);
            }

        }

        public class ContainerType1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Line1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class TransportationType1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Status1
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

        public class Transportation1
        {
            public int id { get; set; }
            public string cityOfLoading { get; set; }
            public string deliveryCity { get; set; }
            public string carDeliveryDate { get; set; }
            public int lot { get; set; }
            public int cargoWeightInContainer { get; set; }
            public ContainerType1 containerType { get; set; }
            public Line1 line { get; set; }
            public TransportationType1 transportationType { get; set; }
            public Status1 status { get; set; }
            public string updatedAt { get; set; }
            public string createdAt { get; set; }
            public int userId { get; set; }
            public List<App> apps { get; set; }
        }

        public class Data1
        {
            public Transportation1 transportation { get; set; }
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