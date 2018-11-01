using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
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
using System.Text;
using System.ComponentModel;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges =
    Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Class_Vid_Calendar: Activity
    {

        string vid = "Календарь";

        int[] month_count = new int[12];

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_vid_calendar);


            Spinner spinner_vid = FindViewById<Spinner>(Resource.Id.spinner_Activity_Vid_Calendar);
            spinner_vid.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter_vid = ArrayAdapter.CreateFromResource(this, Resource.Array.vid1, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_vid.Adapter = adapter_vid;

            RootObject transport = new RootObject();


            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportations/");
                httpWebRequest.Method = "GET";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    transport = JsonConvert.DeserializeObject<RootObject>(result);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            for (int i = 0; i > month_count.Length; i++)
            {
                month_count[i] = 0;
            }

            for (int i = 0; i < transport.data.transportations.Count; i++)
            {
                if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.01") &&
                    Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.02"))
                {
                    month_count[0]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.02") &&
                    Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.03"))
                {
                    month_count[1]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.03") &&
                            Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.04"))
                {
                    month_count[2]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.04") &&
                 Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.05"))
                {
                    month_count[3]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.05") &&
                            Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.06"))
                {
                    month_count[4]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.06") &&
                 Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.07"))
                {
                    month_count[5]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.07") &&
                 Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.08"))
                {
                    month_count[6]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.08") &&
                            Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.09"))
                {
                    month_count[7]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.09") &&
                 Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.10"))
                {
                    month_count[8]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.10") &&
                            Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.11"))
                {
                    month_count[9]++;
                }
           else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.11") &&
                 Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) <= Convert.ToDateTime("01.12"))
                {
                    month_count[10]++;
                }
                else
                {
                    month_count[11]++;
                }
            }

            Button textView_Vid_Calendar1 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar1);
            Button textView_Vid_Calendar2 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar2);
            Button textView_Vid_Calendar3 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar3);
            Button textView_Vid_Calendar4 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar4);
            Button textView_Vid_Calendar5 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar5);
            Button textView_Vid_Calendar6 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar6);
            Button textView_Vid_Calendar7 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar7);
            Button textView_Vid_Calendar8 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar8);
            Button textView_Vid_Calendar9 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar9);
            Button textView_Vid_Calendar10 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar10);
            Button textView_Vid_Calendar11 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar11);
            Button textView_Vid_Calendar12 = FindViewById<Button>(Resource.Id.textView_Activity_Vid_Calendar12);


            textView_Vid_Calendar1.Click += OnButtonClick;
            textView_Vid_Calendar2.Click += OnButtonClick;
            textView_Vid_Calendar3.Click += OnButtonClick;
            textView_Vid_Calendar4.Click += OnButtonClick;
            textView_Vid_Calendar5.Click += OnButtonClick;
            textView_Vid_Calendar6.Click += OnButtonClick;
            textView_Vid_Calendar7.Click += OnButtonClick;
            textView_Vid_Calendar8.Click += OnButtonClick;
            textView_Vid_Calendar9.Click += OnButtonClick;
            textView_Vid_Calendar10.Click += OnButtonClick;
            textView_Vid_Calendar11.Click += OnButtonClick;
            textView_Vid_Calendar12.Click += OnButtonClick;


            textView_Vid_Calendar1.Text = "Январь \n(" + month_count[0].ToString() + ")";
            textView_Vid_Calendar2.Text = "Февраль \n(" + month_count[1].ToString() + ")";
            textView_Vid_Calendar3.Text = "Март \n(" + month_count[2].ToString() + ")";
            textView_Vid_Calendar4.Text = "Апрель \n(" + month_count[3].ToString() + ")";
            textView_Vid_Calendar5.Text = "Май \n(" + month_count[4].ToString() + ")";
            textView_Vid_Calendar6.Text = "Июнь \n(" + month_count[5].ToString() + ")";
            textView_Vid_Calendar7.Text = "Июль \n(" + month_count[6].ToString() + ")";
            textView_Vid_Calendar8.Text = "Август \n(" + month_count[7].ToString() + ")";
            textView_Vid_Calendar9.Text = "Сентябрь \n(" + month_count[8].ToString() + ")";
            textView_Vid_Calendar10.Text = "Октябрь \n(" + month_count[9].ToString() + ")";
            textView_Vid_Calendar11.Text = "Ноябрь \n(" + month_count[10].ToString() + ")";
            textView_Vid_Calendar12.Text = "Декабрь \n(" + month_count[11].ToString() + ")";

            void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                Spinner spinner = (Spinner)sender;

                var po_chemu_vubranVAR = spinner.GetItemAtPosition(e.Position);

                vid = po_chemu_vubranVAR.ToString();
                if (vid == "Список")
                {
                    Intent intent_vid_spisok = new Intent(this, typeof(MainActivity_old));
                    Finish();
                    StartActivity(intent_vid_spisok);
                }
                else
                {

                }
            }
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            Button myButton = (Button)sender;
        }

        public class ContainerType
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Line
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class TransportationType
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Transportation
        {
            public int id { get; set; }
            public string cityOfLoading { get; set; }
            public string deliveryCity { get; set; }
            public string carDeliveryDate { get; set; }
            public int lot { get; set; }
            public int cargoWeightInContainer { get; set; }
            public ContainerType containerType { get; set; }
            public Line line { get; set; }
            public TransportationType transportationType { get; set; }
            public Status status { get; set; }
            public string updatedAt { get; set; }
            public string createdAt { get; set; }
            public int userId { get; set; }
        }

        public class Data
        {
            public List<Transportation> transportations { get; set; }
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