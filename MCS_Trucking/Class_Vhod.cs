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
    public class Class_Vhod:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Vhod);

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


            Button Vhod = (Button)FindViewById<Button>(Resource.Id.button_Activity_Vhod_Vhod);

            Vhod.Click += delegate
            {
                var zapros_na_vhod = new Zapros()
                {
                    email = FindViewById<EditText>(Resource.Id.editText_Activity_Vhod_Email).Text.ToString(),
                    password = FindViewById<EditText>(Resource.Id.editText_Activity_Vhod_Password).Text.ToString()
                };

                var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/session";

                RootObject jsonNew;

                try
                {
                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string pasredContent = JsonConvert.SerializeObject(zapros_na_vhod);
                    UTF8Encoding encoding = new UTF8Encoding();
                    Byte[] bytes = encoding.GetBytes(pasredContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();


                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    jsonNew = JsonConvert.DeserializeObject<RootObject>(content);

                    
                    var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                    using (var writer = File.CreateText(backingFile))
                    {
                        writer.WriteLine(jsonNew.data.userToken.ToString());
                    }


                    Intent intent_to_first_Activity = new Intent(this, typeof(MainActivity_old));
                    Finish();
                    StartActivity(intent_to_first_Activity);
                }
                catch(Exception)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка входа");
                    alert.SetMessage("Введен неверный email или пароль. Проверьте данные и повторите попытку.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                void handllerNothingButton(object sender, DialogClickEventArgs e)
                {
                    //Действие при нажатие "ОК"
                }

            };

            Button button_Reg = (Button)FindViewById<Button>(Resource.Id.button_Activity_Vhod_Reg);

            button_Reg.Click += delegate
            {
                Intent Activity_Reg = new Intent(this, typeof(Class_Registration));
                StartActivity(Activity_Reg);
                
            };

            Button button_restore_password = FindViewById<Button>(Resource.Id.button_Activity_Vhod_Restore_Password);

            button_restore_password.Click += delegate
            {
                Intent Intent_activity_restore_password = new Intent(this, typeof(Class_restore_password));
                StartActivity(Intent_activity_restore_password);
                
            };
            
        }

        public class Zapros
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class Data
        {
            public string userToken { get; set; }
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