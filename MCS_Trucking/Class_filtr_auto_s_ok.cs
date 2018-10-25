using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using Android.Content;
using System;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_filtr_auto_s_ok:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_filtr_callendr_auto_s);

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

            Button button = FindViewById<Button>(Resource.Id.button_Activity_filtr_auto_s_OK);

            DatePicker datePicker = FindViewById<DatePicker>(Resource.Id.datePicker_filtr_data_auto_s);

            button.Click += delegate
            {
                string str = datePicker.DateTime.ToString();
                int pos = str.LastIndexOf(' ');
                str = str.Substring(0, pos);
                pos = str.LastIndexOf(' ');
                //str = str.Substring(0, pos);
                str=str.Replace(".","-");

                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "filtr_auto_s.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                        writer.WriteLine(str);
                }

                Intent intent_to_filtr = new Intent(this, typeof(Class_filtr));
                StartActivity(intent_to_filtr);
                
            };
        }
    }
}