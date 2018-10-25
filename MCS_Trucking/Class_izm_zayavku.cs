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
using Android.Net;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_izm_zayavku:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_izm_zayavku);


            try
            {
                var cm = (ConnectivityManager)GetSystemService(Application.ConnectivityService);
                var isConnected = cm.ActiveNetworkInfo.IsConnected;
            }
            catch (Exception)
            {
                Intent intent_no_internet = new Intent(this, typeof(Class_no_internet));
                Finish();
                StartActivity(intent_no_internet);
            }

            string Id_napr = Intent.GetStringExtra("Id_napr" ?? "Id_napr");

            EditText editText_lot = FindViewById<EditText>(Resource.Id.editText_lot_Activity_izm_zayavku);
            EditText editText_betPrice = FindViewById<EditText>(Resource.Id.editText_betPrice_Activity_izm_zayavku);
            EditText editText_comment = FindViewById<EditText>(Resource.Id.editText_comment_Activity_izm_zayavku);

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

                editText_lot.Text = user_zayavki.data.app.lot.ToString();
                editText_betPrice.Text = user_zayavki.data.app.bet_price.ToString();
                editText_comment.Text = user_zayavki.data.app.comment;

            }
            catch (Exception)
            {
                AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                alert1.SetTitle("Ошибка");
                alert1.SetMessage("Произошла непредвиденая ошибка. Попробуйте позже");
                alert1.SetNeutralButton("OK", handllerNothingButton);
                alert1.Show();
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {
                Intent intent_to_prosmotr_napr = new Intent(this, typeof(Class_Prosmotr_napravleniia));
                intent_to_prosmotr_napr.PutExtra("Id_napr", Id_napr);
                Finish();
                StartActivity(intent_to_prosmotr_napr);
            }

            void handllerNothingButton1(object sender, DialogClickEventArgs e)
            {

            }

            Button button_update_zayavku = FindViewById<Button>(Resource.Id.button_izmZayavku_Activity_izm_zayavku);

            button_update_zayavku.Click += delegate
            {

                if(editText_lot.Text == "")
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Поле 'Лот' не может быть пустым");
                    alert1.SetNeutralButton("OK", handllerNothingButton1);
                    alert1.Show();
                }
                if(editText_betPrice.Text == "")
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Поле 'Ваша ставка' не может быть пустой");
                    alert1.SetNeutralButton("OK", handllerNothingButton1);
                    alert1.Show();
                }
                if(Convert.ToInt32(editText_lot.Text) > user_zayavki.data.app.lot)
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Невозможно указать большее значение лот, чем есть в перевозке");
                    alert1.SetNeutralButton("OK", handllerNothingButton1);
                    alert1.Show();
                }
                if(Convert.ToInt32(editText_betPrice.Text) > 9999)
                {
                    AlertDialog.Builder alert1 = new AlertDialog.Builder(this);
                    alert1.SetTitle("Ошибка");
                    alert1.SetMessage("Цена не может превышать 9999 ");
                    alert1.SetNeutralButton("OK", handllerNothingButton1);
                    alert1.Show();
                }

                IzmZayavku izmZayavku = new IzmZayavku()
                {
                    lot = editText_lot.Text,
                    comment = editText_comment.Text,
                    betPrice = editText_betPrice.Text
                };

                string baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/app/" + user_zayavki.data.app.id;

                try
                {
                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.ContentType = "application/json";
                    http.Accept = "application/json";
                    http.Headers.Add("userToken", userToken);
                    http.Method = "PUT";

                    string pasredContent = JsonConvert.SerializeObject(izmZayavku);
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
                    alert.SetMessage("Заявка обновлена успешно!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
                catch (WebException)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла непредвиденная ошибка. Попробуйте повторить запрос позже.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

            };

            Button button_delete_zayavku = FindViewById<Button>(Resource.Id.button_deleteZayavku_Activity_izm_zayavku);

            button_delete_zayavku.Click += delegate
            {
                string baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/app/" + user_zayavki.data.app.id;

                try
                {
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(baseAddress);
                    httpWebRequest1.Headers.Add("userToken", userToken);
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Method = "DELETE";
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }

                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Успешно");
                    alert.SetMessage("Заявка удалена успешно!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
                catch (WebException)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла непредвиденная ошибка. Попробуйте повторить запрос позже.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
            };

        }

        public class IzmZayavku
        {
            public string lot { get; set; }
            public string comment { get; set; }
            public string betPrice { get; set; }
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