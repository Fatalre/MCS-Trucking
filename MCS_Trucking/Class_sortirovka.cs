using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using Android.Content;
using System;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_sortirovka:Activity
    {
        string po_chemu_vubran = "";
        string ub_vozr = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_sortirovka);

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

            Spinner spinner_po_chemu = FindViewById<Spinner>(Resource.Id.spinner_Activity_sortirovka_po_chemu);
            spinner_po_chemu.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter_po_chemu = ArrayAdapter.CreateFromResource(this, Resource.Array.Po_chemu_sortirovka_array, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_po_chemu.Adapter = adapter_po_chemu;

            Spinner spinner_voz_ub = FindViewById<Spinner>(Resource.Id.spinner_Activity_sortirovka_ub_voz);
            spinner_voz_ub.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected1);
            var adapter_voz_ub = ArrayAdapter.CreateFromResource(this, Resource.Array.voz_ub_array, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_voz_ub.Adapter = adapter_voz_ub;

            Button button_sortirovka = FindViewById<Button>(Resource.Id.button_Activity_sortirovka_sortirovka);
            button_sortirovka.Click += delegate
            {
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Sortirovka_po_chemu.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine(po_chemu_vubran);
                }

                backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Sortirovka_voz_ub.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine(ub_vozr);
                }

                Intent intent_to_start = new Intent(this, typeof(MainActivity));
                Finish();
                StartActivity(intent_to_start);

            };

            void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
            {
                Spinner spinner = (Spinner)sender;

                var po_chemu_vubranVAR = spinner.GetItemAtPosition(e.Position);

                po_chemu_vubran = po_chemu_vubranVAR.ToString();
            }

            void spinner_ItemSelected1(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                Spinner spinner = (Spinner)sender;

                var ub_vozrVAR = spinner.GetItemAtPosition(e.Position);

                ub_vozr = ub_vozrVAR.ToString();
            }
        }
    }
}