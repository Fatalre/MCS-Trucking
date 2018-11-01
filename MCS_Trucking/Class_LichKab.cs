using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Util;
using Android.Content;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_LichKab:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Lich_kab);

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

            var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
            string userToken="";
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    userToken = line;
                }
            }

            try
            {
                var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/session";
                var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                http.Method = "GET";
                http.Headers.Add("userToken", userToken);
                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                var jsonNew = JsonConvert.DeserializeObject<RootObject>(content);

                TextView textView_FirstName = (TextView)FindViewById<TextView>(Resource.Id.textView_FirstName_Activity_LichKab);
                TextView textView_LastName = (TextView)FindViewById<TextView>(Resource.Id.textView_LastNAme_Activity_LichKab);
                TextView textView_Email = (TextView)FindViewById<TextView>(Resource.Id.textView_Email_Activity_LichKab);
                TextView textView_PhoneNumber = (TextView)FindViewById<TextView>(Resource.Id.textView_PhoneNumber_Activity_LichKab);
                TextView textView_Company = (TextView)FindViewById<TextView>(Resource.Id.textView_Company_Activity_LichKab);

                textView_FirstName.Text = jsonNew.data.user.firstName;
                textView_FirstName.SetTextSize(ComplexUnitType.Dip, 18);

                textView_LastName.Text = jsonNew.data.user.lastName;
                textView_LastName.SetTextSize(ComplexUnitType.Dip, 18);

                textView_Email.Text = jsonNew.data.user.email;
                textView_Email.SetTextSize(ComplexUnitType.Dip, 18);

                textView_PhoneNumber.Text = jsonNew.data.user.phoneNumber;
                textView_PhoneNumber.SetTextSize(ComplexUnitType.Dip, 18);

                textView_Company.Text = jsonNew.data.user.companyName;
                textView_Company.SetTextSize(ComplexUnitType.Dip, 18);

            }
            catch (Exception)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Произошла неожиданная ошибка, попробуйте позже");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {
                backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine("");
                }
                Intent To_First_Activity = new Intent(this, typeof(MainActivity_old));
                StartActivity(To_First_Activity);
            }

            Button button_Exit = (Button)FindViewById<Button>(Resource.Id.button_Exit_Activity_LichKab);

            button_Exit.Click += delegate
            {
                try
                {
                    var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/session";
                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.Method = "DELETE";
                    http.Headers.Add("userToken", userToken);
                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    var jsonNew = JsonConvert.DeserializeObject<RootObjectDelete>(content);

                    backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                    using (var writer = File.CreateText(backingFile))
                    {
                        writer.WriteLine("");
                    }
                    Intent To_First_Activity = new Intent(this, typeof(MainActivity_old));
                    StartActivity(To_First_Activity);
                }
                catch (Exception)
                {
                    backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                    using (var writer = File.CreateText(backingFile))
                    {
                        writer.WriteLine("");
                    }
                    Intent To_First_Activity = new Intent(this, typeof(MainActivity_old));
                    StartActivity(To_First_Activity);
                }

            };

            Button button_Update_User = (Button)FindViewById<Button>(Resource.Id.button_UpdateUser_Activity_LichKab);

            button_Update_User.Click += delegate
            {
                TextView textView_FirstName = (TextView)FindViewById<TextView>(Resource.Id.textView_FirstName_Activity_LichKab);
                TextView textView_LastName = (TextView)FindViewById<TextView>(Resource.Id.textView_LastNAme_Activity_LichKab);
                TextView textView_Email = (TextView)FindViewById<TextView>(Resource.Id.textView_Email_Activity_LichKab);
                TextView textView_PhoneNumber = (TextView)FindViewById<TextView>(Resource.Id.textView_PhoneNumber_Activity_LichKab);
                TextView textView_Company = (TextView)FindViewById<TextView>(Resource.Id.textView_Company_Activity_LichKab);

                Intent Intent_Activity_UpdateUSer = new Intent(this, typeof(Class_UpdateUser));

                Intent_Activity_UpdateUSer.PutExtra("FirstName", textView_FirstName.Text);
                Intent_Activity_UpdateUSer.PutExtra("LastName", textView_LastName.Text);
                Intent_Activity_UpdateUSer.PutExtra("Email", textView_Email.Text);
                Intent_Activity_UpdateUSer.PutExtra("PhoneNumber", textView_PhoneNumber.Text);
                Intent_Activity_UpdateUSer.PutExtra("Company", textView_Company.Text);

                StartActivity(Intent_Activity_UpdateUSer);
                
            };

            Button button_New_Password = FindViewById<Button>(Resource.Id.button_NewPassword_Activity_LichKab);

            button_New_Password.Click += delegate
            {
                Intent Intent_Activity_New_Password = new Intent(this, typeof(Class_New_Password));
                StartActivity(Intent_Activity_New_Password);
                
            };

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

        public class RootObjectDelete
        {
            public bool success { get; set; }
            public int code { get; set; }
            public List<object> data { get; set; }
            public List<object> errors { get; set; }
        }
    }
}