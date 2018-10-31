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
    public class Class_prosmotr_napravleniia_os : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_prosmotr_napravleniia_os);

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
                AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                alert1.SetTitle("Ошибка");
                alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                alert1.SetNeutralButton("OK", handllerNothingButton);
                alert1.Show();
            }

            string key = Intent.GetStringExtra("Key" ?? "Key");
            string key2 = Intent.GetStringExtra("Key2" ?? "Key2");

            if (key2 == "Ожидается перевозка")
            {
                key2 = "Заявки рассмотрено";
            }
            else if (key2 == "Перевозка завершена")
            {
                key2 = "Завершена";
            }
            else if (key2 == "Перевозка отменена")
            {
                key2 = "Перевозка отменена";
            }

            int count = 0;

            if (key == "Все заявки")
            {
                RootObject user = new RootObject();

                M1: try
                {
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/session");
                    httpWebRequest1.Headers.Add("userToken", userToken);
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Method = "GET";
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user = JsonConvert.DeserializeObject<RootObject>(result);
                    }

                }
                catch
                {
                    if(user.data.user == null)
                    {
                        goto M1;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                RootObject1 user_app = new RootObject1();

                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/user/" + user.data.user.id.ToString() + "/apps");
                    httpWebRequest.Headers.Add("userToken", userToken);
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user_app = JsonConvert.DeserializeObject<RootObject1>(result);
                    }

                }
                catch
                {
                    if (user_app.data.apps == null)
                    {
                        goto M1;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_moi_zayavki);
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                    (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                if (user_app.data.apps.Count != 0)
                {
                    for (int i = 0; i < user_app.data.apps.Count; i++)
                    {
                        Button button_napr = new Button(this);
                        button_napr.Text = user_app.data.apps[i].transportation.cityOfLoading + " - " + user_app.data.apps[i].transportation.deliveryCity;
                        button_napr.TextAlignment = Android.Views.TextAlignment.Center;
                        button_napr.SetTextColor(Color.ParseColor("#ff274284"));

                        if (user_app.data.apps[i].transportation.status.name == "Перевозка отменена")
                        {
                            button_napr.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                        }
                        else if (user_app.data.apps[i].transportation.status.name == "Завершена")
                        {
                            button_napr.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                        }
                        else if (user_app.data.apps[i].transportation.status.name == "Ожидания заявок")//null на рассмотрение
                        {
                            button_napr.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                        }
                        else if (user_app.data.apps[i].transportation.status.name == "Заявки рассмотрено")
                        {
                            button_napr.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                        }
                        else
                        {
                            button_napr.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                        }

                        button_napr.SetTextSize(ComplexUnitType.Dip, 19);
                        button_napr.Id = View.GenerateViewId();
                        button_napr.Click += OnButtonClick;

                        TextView textView_opis_napr = new TextView(this);
                        textView_opis_napr.Text = "Дата постановки машины: "
                        + user_app.data.apps[i].transportation.carDeliveryDate + "\n" + "Лот: "
                        + user_app.data.apps[i].transportation.lot + "\n" + "Вес контейнера: "
                        + user_app.data.apps[i].transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                        + user_app.data.apps[i].transportation.containerType.name + "\n" + "Линия: "
                        + user_app.data.apps[i].transportation.line.name + "\n" + "Направление: "
                        + user_app.data.apps[i].transportation.transportationType.name + "\n" + "Статус: "
                        + user_app.data.apps[i].transportation.status.name;

                        textView_opis_napr.TextAlignment = TextAlignment.ViewStart;
                        textView_opis_napr.SetTextColor(Color.Black);
                        textView_opis_napr.SetTextSize(ComplexUnitType.Dip, 15);

                        TextView textView_apps_user = new TextView(this);
                        textView_apps_user.Text = "\nВаша заявка: " + "\n" + "Лот: " + user_app.data.apps[i].lot
                            + "\n" + "Ваша ставка: " + user_app.data.apps[i].betPrice
                            + "\n" + "Ваш комментарий: " + user_app.data.apps[i].comment;

                        textView_apps_user.TextAlignment = TextAlignment.ViewStart;
                        textView_apps_user.SetTextColor(Color.OrangeRed);
                        textView_apps_user.SetTextSize(ComplexUnitType.Dip, 15);

                        TextView textView_id_napr = new TextView(this);
                        textView_id_napr.Text = user_app.data.apps[i].transportationId.ToString();
                        textView_id_napr.SetTextColor(Color.White);
                        textView_id_napr.Id = View.GenerateViewId();

                        ll.AddView(button_napr, lp);
                        ll.AddView(textView_opis_napr, lp);
                        ll.AddView(textView_apps_user, lp);
                        ll.AddView(textView_id_napr, lp);
                    }

                }
                else
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "У Вас пока нет ни одной заявки на превозку по этому фильтру";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }
            }
            else if (key == "Заявки на рассмотрение")
            {
                RootObject user = new RootObject();

                M2:  try
                {
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/session");
                    httpWebRequest1.Headers.Add("userToken", userToken);
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Method = "GET";
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user = JsonConvert.DeserializeObject<RootObject>(result);
                    }

                }
                catch
                {
                    if (user.data.user == null)
                    {
                        goto M2;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                RootObject1 user_app = new RootObject1();

                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/user/" + user.data.user.id.ToString() + "/apps");
                    httpWebRequest.Headers.Add("userToken", userToken);
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user_app = JsonConvert.DeserializeObject<RootObject1>(result);
                    }

                }
                catch
                {
                    if(user_app.data.apps == null)
                    {
                        goto M2;
                    }
                    
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_moi_zayavki);
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                    (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                if (user_app.data.apps.Count != 0)
                {
                    for (int i = 0; i < user_app.data.apps.Count; i++)
                    {
                        if(user_app.data.apps[i].accepted == null && user_app.data.apps[i].transportation.status.name == "Ожидания заявок")
                        {
                            Button button_napr = new Button(this);
                            button_napr.Text = user_app.data.apps[i].transportation.cityOfLoading + " - " + user_app.data.apps[i].transportation.deliveryCity;
                            button_napr.TextAlignment = Android.Views.TextAlignment.Center;
                            button_napr.SetTextColor(Color.ParseColor("#ff274284"));

                            if (user_app.data.apps[i].transportation.status.name == "Перевозка отменена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Завершена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Ожидания заявок")//null на рассмотрение
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Заявки рассмотрено")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                            }
                            else
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                            }

                            button_napr.SetTextSize(ComplexUnitType.Dip, 19);
                            button_napr.Id = View.GenerateViewId();
                            button_napr.Click += OnButtonClick;

                            TextView textView_opis_napr = new TextView(this);
                            textView_opis_napr.Text = "Дата постановки машины: "
                            + user_app.data.apps[i].transportation.carDeliveryDate + "\n" + "Лот: "
                            + user_app.data.apps[i].transportation.lot + "\n" + "Вес контейнера: "
                            + user_app.data.apps[i].transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                            + user_app.data.apps[i].transportation.containerType.name + "\n" + "Линия: "
                            + user_app.data.apps[i].transportation.line.name + "\n" + "Направление: "
                            + user_app.data.apps[i].transportation.transportationType.name + "\n" + "Статус: "
                            + user_app.data.apps[i].transportation.status.name;

                            textView_opis_napr.TextAlignment = TextAlignment.ViewStart;
                            textView_opis_napr.SetTextColor(Color.Black);
                            textView_opis_napr.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_apps_user = new TextView(this);
                            textView_apps_user.Text = "\nВаша заявка: " + "\n" + "Лот: " + user_app.data.apps[i].lot
                                + "\n" + "Ваша ставка: " + user_app.data.apps[i].betPrice
                                + "\n" + "Ваш комментарий: " + user_app.data.apps[i].comment;

                            textView_apps_user.TextAlignment = TextAlignment.ViewStart;
                            textView_apps_user.SetTextColor(Color.OrangeRed);
                            textView_apps_user.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_id_napr = new TextView(this);
                            textView_id_napr.Text = user_app.data.apps[i].transportationId.ToString();
                            textView_id_napr.SetTextColor(Color.White);
                            textView_id_napr.Id = View.GenerateViewId();

                            ll.AddView(button_napr, lp);
                            ll.AddView(textView_opis_napr, lp);
                            ll.AddView(textView_apps_user, lp);
                            ll.AddView(textView_id_napr, lp);

                            count++;
                        }
                    }

                }
                else
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "У Вас пока нет ни одной заявки на превозку по этому фильтру";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }

                if (count == 0)
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "В данной категории у Вас нет заявок";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }
            }
            else if (key == "Не рассмотренные заявки")
            {
                RootObject user = new RootObject();

                M3:  try
                {
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/session");
                    httpWebRequest1.Headers.Add("userToken", userToken);
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Method = "GET";
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user = JsonConvert.DeserializeObject<RootObject>(result);
                    }

                }
                catch
                {
                    if (user.data.user == null)
                    {
                        goto M3;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                RootObject1 user_app = new RootObject1();

                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/user/" + user.data.user.id.ToString() + "/apps");
                    httpWebRequest.Headers.Add("userToken", userToken);
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user_app = JsonConvert.DeserializeObject<RootObject1>(result);
                    }

                }
                catch
                {
                    if (user_app.data.apps == null)
                    {
                        goto M3;
                    }
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_moi_zayavki);
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                    (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                if (user_app.data.apps.Count != 0)
                {
                    for (int i = 0; i < user_app.data.apps.Count; i++)
                    {
                        if (user_app.data.apps[i].accepted == null && user_app.data.apps[i].transportation.status.name == key2)
                        {
                            Button button_napr = new Button(this);
                            button_napr.Text = user_app.data.apps[i].transportation.cityOfLoading + " - " + user_app.data.apps[i].transportation.deliveryCity;
                            button_napr.TextAlignment = Android.Views.TextAlignment.Center;
                            button_napr.SetTextColor(Color.ParseColor("#ff274284"));

                            if (user_app.data.apps[i].transportation.status.name == "Перевозка отменена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Завершена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Ожидания заявок")//null на рассмотрение
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Заявки рассмотрено")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                            }
                            else
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                            }

                            button_napr.SetTextSize(ComplexUnitType.Dip, 19);
                            button_napr.Id = View.GenerateViewId();
                            button_napr.Click += OnButtonClick;

                            TextView textView_opis_napr = new TextView(this);
                            textView_opis_napr.Text = "Дата постановки машины: "
                            + user_app.data.apps[i].transportation.carDeliveryDate + "\n" + "Лот: "
                            + user_app.data.apps[i].transportation.lot + "\n" + "Вес контейнера: "
                            + user_app.data.apps[i].transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                            + user_app.data.apps[i].transportation.containerType.name + "\n" + "Линия: "
                            + user_app.data.apps[i].transportation.line.name + "\n" + "Направление: "
                            + user_app.data.apps[i].transportation.transportationType.name + "\n" + "Статус: "
                            + user_app.data.apps[i].transportation.status.name;

                            textView_opis_napr.TextAlignment = TextAlignment.ViewStart;
                            textView_opis_napr.SetTextColor(Color.Black);
                            textView_opis_napr.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_apps_user = new TextView(this);
                            textView_apps_user.Text = "\nВаша заявка: " + "\n" + "Лот: " + user_app.data.apps[i].lot
                                + "\n" + "Ваша ставка: " + user_app.data.apps[i].betPrice
                                + "\n" + "Ваш комментарий: " + user_app.data.apps[i].comment;

                            textView_apps_user.TextAlignment = TextAlignment.ViewStart;
                            textView_apps_user.SetTextColor(Color.OrangeRed);
                            textView_apps_user.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_id_napr = new TextView(this);
                            textView_id_napr.Text = user_app.data.apps[i].transportationId.ToString();
                            textView_id_napr.SetTextColor(Color.White);
                            textView_id_napr.Id = View.GenerateViewId();

                            ll.AddView(button_napr, lp);
                            ll.AddView(textView_opis_napr, lp);
                            ll.AddView(textView_apps_user, lp);
                            ll.AddView(textView_id_napr, lp);

                            count++;
                        }
                    }

                }
                else
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "У Вас пока нет ни одной заявки на превозку по этому фильтру";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }

                if (count == 0)
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "В данной категории у Вас нет заявок";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }
            }
            else if (key == "Не принятые заявки")
            {
                RootObject user = new RootObject();

                M4:  try
                {
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/session");
                    httpWebRequest1.Headers.Add("userToken", userToken);
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Method = "GET";
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user = JsonConvert.DeserializeObject<RootObject>(result);
                    }

                }
                catch
                {
                    if (user.data.user == null)
                    {
                        goto M4;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                RootObject1 user_app = new RootObject1();

                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/user/" + user.data.user.id.ToString() + "/apps");
                    httpWebRequest.Headers.Add("userToken", userToken);
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user_app = JsonConvert.DeserializeObject<RootObject1>(result);
                    }

                }
                catch
                {
                    if(user_app.data.apps == null)
                    {
                        goto M4;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_moi_zayavki);
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                    (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                if (user_app.data.apps.Count != 0)
                {
                    for (int i = 0; i < user_app.data.apps.Count; i++)
                    {
                        if (Convert.ToInt32(user_app.data.apps[i].accepted) == 0 && user_app.data.apps[i].transportation.status.name == key2)
                        {
                            Button button_napr = new Button(this);
                            button_napr.Text = user_app.data.apps[i].transportation.cityOfLoading + " - " + user_app.data.apps[i].transportation.deliveryCity;
                            button_napr.TextAlignment = Android.Views.TextAlignment.Center;
                            button_napr.SetTextColor(Color.ParseColor("#ff274284"));

                            if (user_app.data.apps[i].transportation.status.name == "Перевозка отменена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Завершена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Ожидания заявок")//null на рассмотрение
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Заявки рассмотрено")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                            }
                            else
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                            }

                            button_napr.SetTextSize(ComplexUnitType.Dip, 19);
                            button_napr.Id = View.GenerateViewId();
                            button_napr.Click += OnButtonClick;

                            TextView textView_opis_napr = new TextView(this);
                            textView_opis_napr.Text = "Дата постановки машины: "
                            + user_app.data.apps[i].transportation.carDeliveryDate + "\n" + "Лот: "
                            + user_app.data.apps[i].transportation.lot + "\n" + "Вес контейнера: "
                            + user_app.data.apps[i].transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                            + user_app.data.apps[i].transportation.containerType.name + "\n" + "Линия: "
                            + user_app.data.apps[i].transportation.line.name + "\n" + "Направление: "
                            + user_app.data.apps[i].transportation.transportationType.name + "\n" + "Статус: "
                            + user_app.data.apps[i].transportation.status.name;

                            textView_opis_napr.TextAlignment = TextAlignment.ViewStart;
                            textView_opis_napr.SetTextColor(Color.Black);
                            textView_opis_napr.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_apps_user = new TextView(this);
                            textView_apps_user.Text = "\nВаша заявка: " + "\n" + "Лот: " + user_app.data.apps[i].lot
                                + "\n" + "Ваша ставка: " + user_app.data.apps[i].betPrice
                                + "\n" + "Ваш комментарий: " + user_app.data.apps[i].comment;

                            textView_apps_user.TextAlignment = TextAlignment.ViewStart;
                            textView_apps_user.SetTextColor(Color.OrangeRed);
                            textView_apps_user.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_id_napr = new TextView(this);
                            textView_id_napr.Text = user_app.data.apps[i].transportationId.ToString();
                            textView_id_napr.SetTextColor(Color.White);
                            textView_id_napr.Id = View.GenerateViewId();

                            ll.AddView(button_napr, lp);
                            ll.AddView(textView_opis_napr, lp);
                            ll.AddView(textView_apps_user, lp);
                            ll.AddView(textView_id_napr, lp);

                            count++;
                        }
                    }

                }
                else
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "У Вас пока нет ни одной заявки на превозку по этому фильтру";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }

                if (count == 0)
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "В данной категории у Вас нет заявок";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }
            }
            else if (key == "Принятые заявки")
            {
                RootObject user = new RootObject();

               M5: try
                {
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/session");
                    httpWebRequest1.Headers.Add("userToken", userToken);
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Method = "GET";
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user = JsonConvert.DeserializeObject<RootObject>(result);
                    }

                }
                catch
                {
                    if (user.data.user == null)
                    {
                        goto M5;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                RootObject1 user_app = new RootObject1();

                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/user/" + user.data.user.id.ToString() + "/apps");
                    httpWebRequest.Headers.Add("userToken", userToken);
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        user_app = JsonConvert.DeserializeObject<RootObject1>(result);
                    }

                }
                catch
                {
                    if(user_app.data.apps == null)
                    {
                        goto M5;
                    }

                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                    alert1.SetNeutralButton("OK", handllerNothingButton);
                    alert1.Show();
                }

                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_Activity_moi_zayavki);
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                    (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                if (user_app.data.apps.Count != 0)
                {
                    for (int i = 0; i < user_app.data.apps.Count; i++)
                    {
                        if (Convert.ToInt32(user_app.data.apps[i].accepted) == 1 && user_app.data.apps[i].transportation.status.name == key2)
                        {
                            Button button_napr = new Button(this);
                            button_napr.Text = user_app.data.apps[i].transportation.cityOfLoading + " - " + user_app.data.apps[i].transportation.deliveryCity;
                            button_napr.TextAlignment = Android.Views.TextAlignment.Center;
                            button_napr.SetTextColor(Color.ParseColor("#ff274284"));

                            if (user_app.data.apps[i].transportation.status.name == "Перевозка отменена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Завершена")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Ожидания заявок")//null на рассмотрение
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                            }
                            else if (user_app.data.apps[i].transportation.status.name == "Заявки рассмотрено")
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                            }
                            else
                            {
                                button_napr.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                            }

                            button_napr.SetTextSize(ComplexUnitType.Dip, 19);
                            button_napr.Id = View.GenerateViewId();
                            button_napr.Click += OnButtonClick;

                            TextView textView_opis_napr = new TextView(this);
                            textView_opis_napr.Text = "Дата постановки машины: "
                            + user_app.data.apps[i].transportation.carDeliveryDate + "\n" + "Лот: "
                            + user_app.data.apps[i].transportation.lot + "\n" + "Лотов подтверждено: " 
                            + user_app.data.apps[i].lotAccepted.ToString() + "\n" + "Вес контейнера: "
                            + user_app.data.apps[i].transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                            + user_app.data.apps[i].transportation.containerType.name + "\n" + "Линия: "
                            + user_app.data.apps[i].transportation.line.name + "\n" + "Направление: "
                            + user_app.data.apps[i].transportation.transportationType.name + "\n" + "Статус: "
                            + user_app.data.apps[i].transportation.status.name;

                            textView_opis_napr.TextAlignment = TextAlignment.ViewStart;
                            textView_opis_napr.SetTextColor(Color.Black);
                            textView_opis_napr.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_apps_user = new TextView(this);
                            textView_apps_user.Text = "\nВаша заявка: " + "\n" + "Лот: " + user_app.data.apps[i].lot
                                + "\n" + "Ваша ставка: " + user_app.data.apps[i].betPrice
                                + "\n" + "Ваш комментарий: " + user_app.data.apps[i].comment;

                            textView_apps_user.TextAlignment = TextAlignment.ViewStart;
                            textView_apps_user.SetTextColor(Color.OrangeRed);
                            textView_apps_user.SetTextSize(ComplexUnitType.Dip, 15);

                            TextView textView_id_napr = new TextView(this);
                            textView_id_napr.Text = user_app.data.apps[i].transportationId.ToString();
                            textView_id_napr.SetTextColor(Color.White);
                            textView_id_napr.Id = View.GenerateViewId();

                            ll.AddView(button_napr, lp);
                            ll.AddView(textView_opis_napr, lp);
                            ll.AddView(textView_apps_user, lp);
                            ll.AddView(textView_id_napr, lp);

                            count++;
                        }
                    }

                }
                else
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "У Вас пока нет ни одной заявки на превозку по этому фильтру";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }

                if (count == 0)
                {
                    TextView textView_no_app = new TextView(this);
                    textView_no_app.Text = "В данной категории у Вас нет заявок";
                    textView_no_app.TextAlignment = TextAlignment.Center;
                    textView_no_app.SetTextColor(Color.DarkRed);
                    textView_no_app.SetTextSize(ComplexUnitType.Dip, 20);

                    ll.AddView(textView_no_app, lp);
                }
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {
                Intent intent_to_main = new Intent(this, typeof(MainActivity));
                Finish();
                StartActivity(intent_to_main);
            }


        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            Button myButton = (Button)sender;
            TextView textView_id_napr = FindViewById<TextView>(myButton.Id + 1);

            Intent intent_prosmotr_napr = new Intent(this, typeof(Class_Prosmotr_napravleniia));
            intent_prosmotr_napr.PutExtra("Id_napr", textView_id_napr.Text);
            StartActivity(intent_prosmotr_napr);

        }


        public class Roles
        {
            public string carrier { get; set; }
        }

        public class User
        {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string phoneNumber { get; set; }
            public object rememberToken { get; set; }
            public int isCompany { get; set; }
            public string companyName { get; set; }
            public string createdAt { get; set; }
            public string updatedAt { get; set; }
            public Roles roles { get; set; }
        }

        public class Data
        {
            public User user { get; set; }
        }

        public class RootObject
        {
            public bool success { get; set; }
            public int code { get; set; }
            public Data data { get; set; }
            public List<object> errors { get; set; }
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
        }

        public class App1
        {
            public int id { get; set; }
            public int userId { get; set; }
            public int transportationId { get; set; }
            public int lot { get; set; }
            public int betPrice { get; set; }
            public string comment { get; set; }
            public object accepted { get; set; }
            public int lotAccepted { get; set; }
            public string createdAt { get; set; }
            public string updatedAt { get; set; }
            public Transportation1 transportation { get; set; }
        }

        public class Data1
        {
            public List<App1> apps { get; set; }
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