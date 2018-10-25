using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;
using Android.Content;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_filtr_line : Activity
    {
        private string[] lines = new string[] { "ADMIRAL","APL", "ARKAS", "CMA CGM", "COSCO", "EVERGREEN",
            "HAPAG-LLOYD", "HMM", "MAERSK", "MOL", "MSC", "NYK", "OOCL", "SEAGO", "TURKON", "UASC",
            "YANG MING", "ZIM", "TAVRIA", "HANJIN", "PIL", "WHL", "ANL", "ECU-LINE", "CHINA SHIPPING",
            "FPS", "SHIPCO", "CSAV", "BSS", "ONE", "HDASCO", "Other" };
        int[] lop = new int[32];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_filtr_line);

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

            string[] CheckLines = new string[lines.Length];

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LinesFiltrCheck.txt");
                int l = 0;
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        CheckLines[l] = line;
                        l++;
                    }
                }
            }
            catch (Exception)
            {

            }



            RootObject jsonNew = new RootObject();
            try
            {
                WebRequest reqGET = WebRequest.Create(@"https://truck.mcs-bitrix.pp.ua/api/v1/transportations");
                WebResponse resp = reqGET.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string json = sr.ReadToEnd();

                jsonNew = JsonConvert.DeserializeObject<RootObject>(json);
            }
            catch (Exception)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Ошибка сервера. Попробуйте позже.");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {
                //Действие при нажатие "ОК"
            }



            LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_ActivityFiltrLine);
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

            for (int i=0; i < lines.Length; i++)
            {
                CheckBox checkBox = new CheckBox(this);
                checkBox.Text = lines[i];
                checkBox.Enabled = false;
                checkBox.Id = View.GenerateViewId();

                lop[i] = checkBox.Id;
                ll.AddView(checkBox, lp);
            }

            for (int i=0; i < lines.Length; i++)
            {
                CheckBox checkBox_true = FindViewById<CheckBox>(lop[i]);
                for (int k=0; k < jsonNew.data.transportations.Count; k++)
                {
                    if (checkBox_true.Text == jsonNew.data.transportations[k].line.name)
                    {
                        checkBox_true.Enabled = true;
                    }
                }
            }

            Button button_Confirm = new Button(this);
            button_Confirm.Text = "Запомнить выбор";
            button_Confirm.TextAlignment = Android.Views.TextAlignment.Center;
            button_Confirm.SetTextColor(Color.ParseColor("#ff274284"));
            button_Confirm.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
            button_Confirm.Click += OnButtonClick;
            ll.AddView(button_Confirm, lp);

            int p = 0;
            for (int i=lop[0]; i < lop[lines.Length-1]; i++)
            {
                if(CheckLines[p] == "True")
                {
                    FindViewById<CheckBox>(i).Checked = true;
                }
                else
                {
                    FindViewById<CheckBox>(i).Checked = false;
                }
                p++;
            }
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            string[] linesChek = new string[lines.Length];
            string[] ChekLines = new string[lines.Length];
            int k = 0;
            int p = 0;
            for (int i=lop[0]; i <= lop[lines.Length-1]; i++)
            {
                CheckBox checkBoxNew = FindViewById<CheckBox>(i);
                if (checkBoxNew.Checked)
                {
                    linesChek[k] = checkBoxNew.Text.ToString();
                    k++;
                    ChekLines[p] = "True";
                }
                else
                {
                    ChekLines[p] = "False";
                }

                p++;
            }

            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LinesFiltr.txt");
            using (var writer = File.CreateText(backingFile))
            {
                for(int i = 0; i < linesChek.Length; i++)
                {
                    writer.WriteLine(linesChek[i]);
                }
            }

            var backingFile1 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LinesFiltrCheck.txt");
            using (var writer = File.CreateText(backingFile1))
            {
                for (int i = 0; i < linesChek.Length; i++)
                {
                    writer.WriteLine(ChekLines[i]);
                }
            }

            Intent Intent_filt = new Intent(this, typeof(Class_filtr));
            StartActivity(Intent_filt);
            
        }



        public class ContainerType
        {
            public object id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Line
        {
            public object id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class TransportationType
        {
            public object id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Status
        {
            public object id { get; set; }
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