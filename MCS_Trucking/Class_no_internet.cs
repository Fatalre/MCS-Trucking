using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.Content;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_no_internet:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_no_internet);


            TextView textView_no_internet = FindViewById<TextView>(Resource.Id.textView_no_internet);
            textView_no_internet.SetTextSize(ComplexUnitType.Dip, 15);

            Button button_no_internet = FindViewById<Button>(Resource.Id.button_no_internet);

            button_no_internet.Click += delegate
            {
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
            };
        }
    }
}