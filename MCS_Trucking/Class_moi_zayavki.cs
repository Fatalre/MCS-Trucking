using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_moi_zayavki : Activity
    {
        string Key1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_moi_Zayavki);

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

            LinearLayout linearLayout_skr = FindViewById<LinearLayout>(Resource.Id.linearLayout_Activity_moi_zayavki_dop);
            Button button_dop1 = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_dop1);
            Button button_dop2 = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_dop2);
            Button button_dop3 = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_dop3);

            Button button_Vse_zayavki = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_osn0);
            Button button_prinyatue_zayavki = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_osn1);
            Button button_ne_prinyatue_zayavki = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_osn2);
            Button button_ne_rassmotr_zayavki = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_osn3);
            Button button_zayavki_na_rassmotrenie = FindViewById<Button>(Resource.Id.button_Activity_moi_zayavki_osn4);


            button_Vse_zayavki.Click += delegate
            {
                Intent intent_moi_zayavki_os = new Intent(this, typeof(Class_prosmotr_napravleniia_os));
                intent_moi_zayavki_os.PutExtra("Key", button_Vse_zayavki.Text);
                StartActivity(intent_moi_zayavki_os);
            };

            button_zayavki_na_rassmotrenie.Click += delegate
            {
                Intent intent_moi_zayavki_os = new Intent(this, typeof(Class_prosmotr_napravleniia_os));
                intent_moi_zayavki_os.PutExtra("Key", button_zayavki_na_rassmotrenie.Text);
                StartActivity(intent_moi_zayavki_os);
            };

            button_prinyatue_zayavki.Click += delegate
            {
                if (linearLayout_skr.Visibility == ViewStates.Invisible)
                {
                    Key1 = button_prinyatue_zayavki.Text;
                    linearLayout_skr.Visibility = ViewStates.Visible;
                    button_Vse_zayavki.Visibility = ViewStates.Invisible;
                    button_ne_prinyatue_zayavki.Visibility = ViewStates.Invisible;
                    button_ne_rassmotr_zayavki.Visibility = ViewStates.Invisible;
                    button_zayavki_na_rassmotrenie.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Key1 = button_prinyatue_zayavki.Text;
                    linearLayout_skr.Visibility = ViewStates.Invisible;
                    button_Vse_zayavki.Visibility = ViewStates.Visible;
                    button_ne_prinyatue_zayavki.Visibility = ViewStates.Visible;
                    button_ne_rassmotr_zayavki.Visibility = ViewStates.Visible;
                    button_zayavki_na_rassmotrenie.Visibility = ViewStates.Visible;
                }
            };

            button_ne_prinyatue_zayavki.Click += delegate
            {
                if (linearLayout_skr.Visibility == ViewStates.Invisible)
                {
                    Key1 = button_ne_prinyatue_zayavki.Text;
                    linearLayout_skr.Visibility = ViewStates.Visible;
                    button_Vse_zayavki.Visibility = ViewStates.Invisible;
                    button_prinyatue_zayavki.Visibility = ViewStates.Invisible;
                    button_ne_rassmotr_zayavki.Visibility = ViewStates.Invisible;
                    button_zayavki_na_rassmotrenie.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Key1 = button_ne_prinyatue_zayavki.Text;
                    linearLayout_skr.Visibility = ViewStates.Invisible;
                    button_Vse_zayavki.Visibility = ViewStates.Visible;
                    button_prinyatue_zayavki.Visibility = ViewStates.Visible;
                    button_ne_rassmotr_zayavki.Visibility = ViewStates.Visible;
                    button_zayavki_na_rassmotrenie.Visibility = ViewStates.Visible;
                }
            };

            button_ne_rassmotr_zayavki.Click += delegate
            {
                if (linearLayout_skr.Visibility == ViewStates.Invisible)
                {
                    Key1 = button_ne_rassmotr_zayavki.Text;
                    linearLayout_skr.Visibility = ViewStates.Visible;
                    button_Vse_zayavki.Visibility = ViewStates.Invisible;
                    button_ne_prinyatue_zayavki.Visibility = ViewStates.Invisible;
                    button_prinyatue_zayavki.Visibility = ViewStates.Invisible;
                    button_zayavki_na_rassmotrenie.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Key1 = button_ne_rassmotr_zayavki.Text;
                    linearLayout_skr.Visibility = ViewStates.Invisible;
                    button_Vse_zayavki.Visibility = ViewStates.Visible;
                    button_prinyatue_zayavki.Visibility = ViewStates.Visible;
                    button_ne_prinyatue_zayavki.Visibility = ViewStates.Visible;
                    button_zayavki_na_rassmotrenie.Visibility = ViewStates.Visible;
                }
            };

            button_dop1.Click += delegate
            {
                Intent intent_moi_zayavki_os = new Intent(this, typeof(Class_prosmotr_napravleniia_os));
                intent_moi_zayavki_os.PutExtra("Key", Key1);
                intent_moi_zayavki_os.PutExtra("Key2", button_dop1.Text);
                StartActivity(intent_moi_zayavki_os);
            };

            button_dop2.Click += delegate
            {
                Intent intent_moi_zayavki_os = new Intent(this, typeof(Class_prosmotr_napravleniia_os));
                intent_moi_zayavki_os.PutExtra("Key", Key1);
                intent_moi_zayavki_os.PutExtra("Key2", button_dop2.Text);
                StartActivity(intent_moi_zayavki_os);
            };

            button_dop3.Click += delegate
            {
                Intent intent_moi_zayavki_os = new Intent(this, typeof(Class_prosmotr_napravleniia_os));
                intent_moi_zayavki_os.PutExtra("Key", Key1);
                intent_moi_zayavki_os.PutExtra("Key2", button_dop3.Text);
                StartActivity(intent_moi_zayavki_os);
            };
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            Button myButton = (Button)sender;
            TextView textView_id_napr = FindViewById<TextView>(myButton.Id + 1);

            Intent intent_prosmotr_napr = new Intent(this, typeof(Class_Prosmotr_napravleniia));
            intent_prosmotr_napr.PutExtra("Id_napr", textView_id_napr.Text);
            StartActivity(intent_prosmotr_napr);

        }
    }
}