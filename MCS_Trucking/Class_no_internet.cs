using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.Content;
using Android.Net;

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
                try
                {
                    var cm = (ConnectivityManager)GetSystemService(Application.ConnectivityService);
                    var isConnected = cm.ActiveNetworkInfo.IsConnected;
                }
                catch (Exception)
                {
                    Intent intent_to_start = new Intent(this, typeof(MainActivity));
                    FinishAffinity();
                    StartActivity(intent_to_start);
                }
            };
        }
    }
}