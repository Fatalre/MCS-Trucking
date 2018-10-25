using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using Android.Content;
using Android.Net;
using System;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_filtr_auto_do_ok : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_filtr_callendr_auto_do);

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

            Button button = FindViewById<Button>(Resource.Id.button_Activity_filtr_auto_do_OK);

            DatePicker datePicker = FindViewById<DatePicker>(Resource.Id.datePicker_filtr_data_auto_do);

            button.Click += delegate
            {
                string str = datePicker.DateTime.ToString();
                int pos = str.LastIndexOf(' ');
                str = str.Substring(0, pos);
                pos = str.LastIndexOf(' ');
                str = str.Replace(".", "-");

                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "filtr_auto_do.txt");
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