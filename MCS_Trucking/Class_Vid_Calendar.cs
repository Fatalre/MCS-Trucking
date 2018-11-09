using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Content;
using System.Net.NetworkInformation;
using System.Globalization;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges =
    Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_Vid_Calendar : Activity
    {

        string vid = "Календарь";
        string cash = "";
        int[] month_count = new int[12];

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_vid_calendar);


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


            Spinner spinner_vid = FindViewById<Spinner>(Resource.Id.spinner_Activity_Vid_Calendar);
            spinner_vid.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter_vid = ArrayAdapter.CreateFromResource(this, Resource.Array.vid1, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_vid.Adapter = adapter_vid;

            Button button_Vid_Calendar1 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar1);
            Button button_Vid_Calendar2 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar2);
            Button button_Vid_Calendar3 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar3);
            Button button_Vid_Calendar4 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar4);
            Button button_Vid_Calendar5 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar5);
            Button button_Vid_Calendar6 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar6);
            Button button_Vid_Calendar7 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar7);
            Button button_Vid_Calendar8 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar8);
            Button button_Vid_Calendar9 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar9);
            Button button_Vid_Calendar10 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar10);
            Button button_Vid_Calendar11 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar11);
            Button button_Vid_Calendar12 = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar12);


            button_Vid_Calendar1.Click += OnButtonClick;
            button_Vid_Calendar2.Click += OnButtonClick;
            button_Vid_Calendar3.Click += OnButtonClick;
            button_Vid_Calendar4.Click += OnButtonClick;
            button_Vid_Calendar5.Click += OnButtonClick;
            button_Vid_Calendar6.Click += OnButtonClick;
            button_Vid_Calendar7.Click += OnButtonClick;
            button_Vid_Calendar8.Click += OnButtonClick;
            button_Vid_Calendar9.Click += OnButtonClick;
            button_Vid_Calendar10.Click += OnButtonClick;
            button_Vid_Calendar11.Click += OnButtonClick;
            button_Vid_Calendar12.Click += OnButtonClick;

            RootObject transport = new RootObject();

            try
            {
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh1.txt");
                using (var reader = new StreamReader(backingFile1, true))
                {
                    string line1;
                    while ((line1 = reader.ReadLine()) != null)
                    {
                        cash = line1;
                    }
                }
            }
            catch (Exception)
            {

            }


            H1: if (cash == "true")
            {
                int i = 0;
                try
                {
                    string napr = "";
                    var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "napr_new1.txt");
                    using (var reader = new StreamReader(backingFile1, true))
                    {
                        string line1;
                        while ((line1 = reader.ReadLine()) != null)
                        {
                            napr = line1;
                            transport = JsonConvert.DeserializeObject<RootObject>(napr);
                        }
                    }
                }
                catch (Exception)
                {
                    cash = "";
                    goto H1;
                }

                for (i = 0; i < month_count.Length; i++)
                {
                    month_count[i] = 0;
                }
                try
                {
                    for (i = 0; i < transport.data.transportations.Count; i++)
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
                }
                catch (Exception)
                {
                    cash = "";
                    goto H1;
                }
                


                button_Vid_Calendar1.Text = "Январь \n(" + month_count[0].ToString() + ")";
                button_Vid_Calendar2.Text = "Февраль \n(" + month_count[1].ToString() + ")";
                button_Vid_Calendar3.Text = "Март \n(" + month_count[2].ToString() + ")";
                button_Vid_Calendar4.Text = "Апрель \n(" + month_count[3].ToString() + ")";
                button_Vid_Calendar5.Text = "Май \n(" + month_count[4].ToString() + ")";
                button_Vid_Calendar6.Text = "Июнь \n(" + month_count[5].ToString() + ")";
                button_Vid_Calendar7.Text = "Июль \n(" + month_count[6].ToString() + ")";
                button_Vid_Calendar8.Text = "Август \n(" + month_count[7].ToString() + ")";
                button_Vid_Calendar9.Text = "Сентябрь \n(" + month_count[8].ToString() + ")";
                button_Vid_Calendar10.Text = "Октябрь \n(" + month_count[9].ToString() + ")";
                button_Vid_Calendar11.Text = "Ноябрь \n(" + month_count[10].ToString() + ")";
                button_Vid_Calendar12.Text = "Декабрь \n(" + month_count[11].ToString() + ")";

            }
            else
            {
                string tmp = "";
                int i = 0;

            M1: try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportations/");
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        tmp = result;
                        transport = JsonConvert.DeserializeObject<RootObject>(result);
                    }
                }
                catch (Exception ex)
                {
                    if (transport.data == null)
                    {
                        goto M1;
                    }
                    ex.ToString();
                }

                for (i = 0; i < month_count.Length; i++)
                {
                    month_count[i] = 0;
                }

                for (i = 0; i < transport.data.transportations.Count; i++)
                {
                    if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.01") &&
                        Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.02"))
                    {
                        month_count[0]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.02") &&
                             Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.03"))
                    {
                        month_count[1]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.03") &&
                                     Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.04"))
                    {
                        month_count[2]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.04") &&
                          Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.05"))
                    {
                        month_count[3]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.05") &&
                                     Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.06"))
                    {
                        month_count[4]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.06") &&
                          Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.07"))
                    {
                        month_count[5]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.07") &&
                          Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.08"))
                    {
                        month_count[6]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.08") &&
                                     Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.09"))
                    {
                        month_count[7]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.09") &&
                          Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.10"))
                    {
                        month_count[8]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.10") &&
                                     Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.11"))
                    {
                        month_count[9]++;
                    }
                    else if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01.11") &&
                          Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.12"))
                    {
                        month_count[10]++;
                    }
                    else
                    {
                        month_count[11]++;
                    }
                }


                button_Vid_Calendar1.Text = "Январь \n(" + month_count[0].ToString() + ")";
                button_Vid_Calendar2.Text = "Февраль \n(" + month_count[1].ToString() + ")";
                button_Vid_Calendar3.Text = "Март \n(" + month_count[2].ToString() + ")";
                button_Vid_Calendar4.Text = "Апрель \n(" + month_count[3].ToString() + ")";
                button_Vid_Calendar5.Text = "Май \n(" + month_count[4].ToString() + ")";
                button_Vid_Calendar6.Text = "Июнь \n(" + month_count[5].ToString() + ")";
                button_Vid_Calendar7.Text = "Июль \n(" + month_count[6].ToString() + ")";
                button_Vid_Calendar8.Text = "Август \n(" + month_count[7].ToString() + ")";
                button_Vid_Calendar9.Text = "Сентябрь \n(" + month_count[8].ToString() + ")";
                button_Vid_Calendar10.Text = "Октябрь \n(" + month_count[9].ToString() + ")";
                button_Vid_Calendar11.Text = "Ноябрь \n(" + month_count[10].ToString() + ")";
                button_Vid_Calendar12.Text = "Декабрь \n(" + month_count[11].ToString() + ")";

                string str = "true";
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh1.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                    writer.WriteLine(str);
                }

                var backingFile2 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "napr_new1.txt");
                using (var writer = File.CreateText(backingFile2))
                {

                    writer.WriteLine(tmp);

                }
            }

            void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                Spinner spinner = (Spinner)sender;

                var po_chemu_vubranVAR = spinner.GetItemAtPosition(e.Position);

                vid = po_chemu_vubranVAR.ToString();
                if (vid == "Список")
                {
                    string str = "false";
                    var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh.txt");
                    using (var writer = File.CreateText(backingFile1))
                    {
                        writer.WriteLine(str);
                    }
                    Intent intent_vid_spisok = new Intent(this, typeof(MainActivity_old));
                    StartActivity(intent_vid_spisok);
                }
                else
                {

                }
            }

            Button button_moi_zayavki = FindViewById<Button>(Resource.Id.button_Activity_Vid_Calendar13);
            button_moi_zayavki.Click += delegate
            {
                Intent intent_moi_zayavku = new Intent(this, typeof(Class_moi_zayavki));
                StartActivity(intent_moi_zayavku);
            };

        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            int month;

            Button myButton = (Button)sender;

            if (myButton.Text.Contains("Январь"))
            {
                month = 1;
            }
            else if (myButton.Text.Contains("Февраль"))
            {
                month = 2;
            }
            else if (myButton.Text.Contains("Март"))
            {
                month = 3;
            }
            else if (myButton.Text.Contains("Апрель"))
            {
                month = 4;
            }
            else if (myButton.Text.Contains("Май"))
            {
                month = 5;
            }
            else if (myButton.Text.Contains("Июнь"))
            {
                month = 6;
            }
            else if (myButton.Text.Contains("Июль"))
            {
                month = 7;
            }
            else if (myButton.Text.Contains("Август"))
            {
                month = 8;
            }
            else if (myButton.Text.Contains("Сентябрь"))
            {
                month = 9;
            }
            else if (myButton.Text.Contains("Октябрь"))
            {
                month = 10;
            }
            else if (myButton.Text.Contains("Ноябрь"))
            {
                month = 11;
            }
            else
            {
                month = 12;
            }

            Intent intent_prosmotr_month = new Intent(this, typeof(Class_prosmotr_month_calendar));
            intent_prosmotr_month.PutExtra("month", month.ToString());
            StartActivity(intent_prosmotr_month);

        }

        protected override void OnRestart()
        {
            base.OnRestart();
            string str = "";
            var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh1.txt");
            using (var writer = File.CreateText(backingFile1))
            {
                writer.WriteLine(str);
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            string str = "";
            var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh1.txt");
            using (var writer = File.CreateText(backingFile1))
            {
                writer.WriteLine(str);
            }
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