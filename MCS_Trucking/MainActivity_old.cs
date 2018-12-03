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
    public class MainActivity_old : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, ViewTreeObserver.IOnScrollChangedListener
    {

        bool Vhod = false;
        SwipeRefreshLayout mySwipeRefreshLayout;
        int count = 0;
        int count_new = 0;
        string[] id_ID = new string[100000];
        string refresh_ = "";
        string vid = "Список";
        string[] tmp = new string[100000];

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Intent intent_service_transportationNew = new Intent(this, typeof(Service_notific_new_transportation));
            StartService(intent_service_transportationNew);

            Intent intent_service_new_version = new Intent(this, typeof(Service_new_version));
            StartService(intent_service_new_version);

            bool isConnected;
            var ping = new Ping();
            string host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            var options = new PingOptions();

            ScrollView scrollView = FindViewById<ScrollView>(Resource.Id.scrollView_ActivityStart);
            scrollView.ViewTreeObserver.AddOnScrollChangedListener(this);

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

            try
            {
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                string userToken = "";
                using (var reader = new StreamReader(backingFile1, true))
                {
                    string line1;
                    while ((line1 = reader.ReadLine()) != null)
                    {
                        userToken = line1;
                    }
                }

                Vhod = true;
                if (userToken == "")
                {
                    Vhod = false;
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh.txt");
                using (var reader = new StreamReader(backingFile1, true))
                {
                    string line1;
                    while ((line1 = reader.ReadLine()) != null)
                    {
                        refresh_ = line1;
                    }
                }
            }
            catch (Exception)
            {

            }

            mySwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout1);
            mySwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueDark, Android.Resource.Color.HoloGreenDark,
                Android.Resource.Color.HoloOrangeDark, Android.Resource.Color.HoloRedDark);
            mySwipeRefreshLayout.Refresh += MySwipeRefreshLayout_Refresh;
            mySwipeRefreshLayout.Click += MySwipeRefreshLayout_Refresh;

            if (refresh_ != "")
            {
                RootObject1 napr_new = new RootObject1();

                try
                {
                    string[] napr_infor = new string[100000];
                    int i = 0;
                    var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "napr_new.txt");
                    using (var reader = new StreamReader(backingFile1, true))
                    {
                        string line1;
                        while ((line1 = reader.ReadLine()) != null)
                        {
                            napr_infor[i] = line1;

                            napr_new = JsonConvert.DeserializeObject<RootObject1>(napr_infor[i]);

                            Button myButton = new Button(this);
                            myButton.Text = napr_new.data.transportation.cityOfLoading + " - " + napr_new.data.transportation.deliveryCity;
                            myButton.TextAlignment = Android.Views.TextAlignment.Center;
                            myButton.SetTextColor(Color.ParseColor("#ff274284"));

                            if (napr_new.data.transportation.status.name == "Перевозка отменена")
                            {
                                myButton.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                            }
                            else if (napr_new.data.transportation.status.name == "Завершена")
                            {
                                myButton.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                            }
                            else if (napr_new.data.transportation.status.name == "Ожидания заявок")
                            {
                                myButton.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                            }
                            else if (napr_new.data.transportation.status.name == "Заявки рассмотрено")
                            {
                                myButton.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                            }
                            else
                            {
                                myButton.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                            }

                            myButton.SetTextSize(ComplexUnitType.Dip, 15);
                            myButton.Id = View.GenerateViewId();
                            myButton.Click += OnButtonClick;

                            TextView mytextView = new TextView(this);
                            mytextView.Text = "Дата постановки машины: "
                                + napr_new.data.transportation.carDeliveryDate + "\n" + "Лот: "
                                + napr_new.data.transportation.lot + "\n" + "Вес контейнера: "
                                + napr_new.data.transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                                + napr_new.data.transportation.containerType.name + "\n" + "Линия: "
                                + napr_new.data.transportation.line.name + "\n" + "Направление: "
                                + napr_new.data.transportation.transportationType.name + "\n" + "Статус: "
                                + napr_new.data.transportation.status.name;
                            mytextView.SetTextColor(Color.Black);
                            mytextView.Id = View.GenerateViewId();

                            TextView textView_id = new TextView(this);
                            textView_id.Text = napr_new.data.transportation.id.ToString();
                            textView_id.SetTextColor(Color.White);
                            textView_id.Id = View.GenerateViewId();

                            LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_ActivityStart);
                            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                                (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                            ll.AddView(myButton, lp);
                            ll.AddView(mytextView, lp);
                            ll.AddView(textView_id, lp);

                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else
            {
                mySwipeRefreshLayout.Refreshing = true;
                mySwipeRefreshLayout.PerformClick();
            }

            Spinner spinner_vid = FindViewById<Spinner>(Resource.Id.spinner_ActivityStart);
            spinner_vid.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter_vid = ArrayAdapter.CreateFromResource(this, Resource.Array.vid, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner_vid.Adapter = adapter_vid;

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                Spinner spinner = (Spinner)sender;

                var po_chemu_vubranVAR = spinner.GetItemAtPosition(e.Position);

                vid = po_chemu_vubranVAR.ToString();
                if (vid == "Список")
                {

                }
                else
                {
                    Intent intent_vid_calendar = new Intent(this, typeof(Class_Vid_Calendar));
                    string str = "true";
                    var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh1.txt");
                    using (var writer = File.CreateText(backingFile1))
                    {
                        writer.WriteLine(str);
                    }
                    FinishAffinity();
                    StartActivity(intent_vid_calendar);
                }
            }
        }

        private void MySwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_ActivityStart);
            if (ll.ChildCount > 0)
            {
                ll.RemoveAllViews();
            }
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    
                    RootObject1 napr_new = new RootObject1();

                    M2: try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + id_ID[i]);
                        httpWebRequest.Accept = "application/json";
                        httpWebRequest.Method = "GET";
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            tmp[i] = result;

                            napr_new = JsonConvert.DeserializeObject<RootObject1>(result);
                        }
                    }
                    catch (Exception)
                    {
                        if (napr_new.data == null)
                        {
                            goto M2;
                        }

                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Ошибка");
                        alert.SetMessage("Ошибка сервера. Попробуйте позже.");
                        alert.SetNeutralButton("OK", handllerNothingButton);
                        alert.Show();
                    }

                    if (i < 5)
                    {
                        count_new++;
                        Button myButton = new Button(this);
                        myButton.Text = napr_new.data.transportation.cityOfLoading + " - " + napr_new.data.transportation.deliveryCity;
                        myButton.TextAlignment = Android.Views.TextAlignment.Center;
                        myButton.SetTextColor(Color.ParseColor("#ff274284"));

                        if (napr_new.data.transportation.status.name == "Перевозка отменена")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                        }
                        else if (napr_new.data.transportation.status.name == "Завершена")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                        }
                        else if (napr_new.data.transportation.status.name == "Ожидания заявок")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                        }
                        else if (napr_new.data.transportation.status.name == "Заявки рассмотрено")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                        }
                        else
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                        }

                        myButton.SetTextSize(ComplexUnitType.Dip, 15);
                        myButton.Id = View.GenerateViewId();
                        myButton.Click += OnButtonClick;

                        TextView mytextView = new TextView(this);
                        mytextView.Text = "Дата постановки машины: "
                            + napr_new.data.transportation.carDeliveryDate + "\n" + "Лот: "
                            + napr_new.data.transportation.lot + "\n" + "Вес контейнера: "
                            + napr_new.data.transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                            + napr_new.data.transportation.containerType.name + "\n" + "Линия: "
                            + napr_new.data.transportation.line.name + "\n" + "Направление: "
                            + napr_new.data.transportation.transportationType.name + "\n" + "Статус: "
                            + napr_new.data.transportation.status.name;
                        mytextView.SetTextColor(Color.Black);
                        mytextView.Id = View.GenerateViewId();

                        TextView textView_id = new TextView(this);
                        textView_id.Text = napr_new.data.transportation.id.ToString();
                        textView_id.SetTextColor(Color.White);
                        textView_id.Id = View.GenerateViewId();

                        LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_ActivityStart);
                        LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                            (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                        ll.AddView(myButton, lp);
                        ll.AddView(mytextView, lp);
                        ll.AddView(textView_id, lp);
                    }

                }

                string str = "false";
                var backingFile1 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                    writer.WriteLine(str);
                }

                var backingFile2 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "napr_new.txt");
                using (var writer = File.CreateText(backingFile2))
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        writer.WriteLine(tmp[i]);
                    }
                    
                }

                backingFile2 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "napr_new1.txt");
                using (var writer = File.CreateText(backingFile2))
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        writer.WriteLine(tmp[i]);
                    }

                }

                mySwipeRefreshLayout.Refreshing = false;
            });
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Другой поток
            string sort_po_chumu = "createdAt";
            string sort_voz_ub = "desc";

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Sortirovka_po_chemu.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        sort_po_chumu = line;
                    }
                }

                if (sort_po_chumu == "" || sort_po_chumu == null)
                {
                    sort_po_chumu = "createdAt";
                }
                else if (sort_po_chumu == "Дата добавления")
                {
                    sort_po_chumu = "createdAt";
                }
                else if (sort_po_chumu == "Дата постановки машины")
                {
                    sort_po_chumu = "carDeliveryDate";
                }
                else if (sort_po_chumu == "Лот")
                {
                    sort_po_chumu = "lot";
                }
                else if (sort_po_chumu == "Вес")
                {
                    sort_po_chumu = "cargoWeightInContainer";
                }
                else if (sort_po_chumu == "Город погрузки")
                {
                    sort_po_chumu = "cityOfLoading";
                }
                else if (sort_po_chumu == "Город доставки")
                {
                    sort_po_chumu = "deliveryCity";
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Sortirovka_voz_ub.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        sort_voz_ub = line;
                    }
                }

                if (sort_voz_ub == "" || sort_voz_ub == null)
                {
                    sort_voz_ub = "desc";
                }
                else if (sort_voz_ub == "По убыванию")
                {
                    sort_voz_ub = "desc";
                }
                else if (sort_voz_ub == "По возрастанию")
                {
                    sort_voz_ub = "asc";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/transportations";
            Sortirovka sortirovka = new Sortirovka()
            {
                orderBy = sort_po_chumu,
                sort = sort_voz_ub
            };
            RootObject jsonNew = new RootObject();

            try
            {
                var http1 = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                http1.ContentType = "application/json";
                http1.Method = "POST";

                string pasredContent = JsonConvert.SerializeObject(sortirovka);
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] bytes = encoding.GetBytes(pasredContent);

                Stream newStream = http1.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();


                var response = http1.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                jsonNew = JsonConvert.DeserializeObject<RootObject>(content);
            }
            catch (Exception)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Ошибка сервера. Попробуйте позже.");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            string Date_auto_s = "";
            string Date_auto_do = "";
            string lot_s = "0";
            string lot_do = "1000";
            string ves_s = "0";
            string ves_do = "1000";
            string[] lines = new string[32];
            string[] typesContainer = new string[32];

            bool Date_auto_s_count = true;
            bool Date_auto_do_count = true;
            bool lot_s_count = true;
            bool lot_do_count = true;
            bool ves_s_count = true;
            bool ves_do_count = true;
            bool lines_count = true;
            bool typeContainer_count = true;

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "filtr_auto_s.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        Date_auto_s = line;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "filtr_auto_do.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        Date_auto_do = line;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_s.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lot_s = line;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_do.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lot_do = line;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_s.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        ves_s = line;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_do.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        ves_do = line;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LinesFiltr.txt");
                string line = "";
                int l = 0;
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines[l] = line;
                        l++;
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "type_containerFiltr.txt");
                string line = "";
                int l = 0;
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        typesContainer[l] = line;
                        l++;
                    }
                }
            }
            catch (Exception)
            {

            }

            count = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "" || lines[i] == null)
                {
                    count++;
                }
            }
            if (count == lines.Length)
            {
                lines_count = false;
            }

            count = 0;
            for (int i = 0; i < typesContainer.Length; i++)
            {
                if (typesContainer[i] == "" || typesContainer[i] == null)
                {
                    count++;
                }
            }
            if (count == typesContainer.Length)
            {
                typeContainer_count = false;
            }

            if (lot_s == "")
            {
                lot_s_count = false;
                lot_s = "0";
            }

            if (lot_do == "")
            {
                lot_do_count = false;
                lot_do = "1000";
            }

            if (ves_s == "")
            {
                ves_s_count = false;
                ves_s = "0";
            }

            if (ves_do == "")
            {
                ves_do_count = false;
                ves_do = "1000";
            }

            if (Date_auto_s == "")
            {
                Date_auto_do_count = false;
                Date_auto_s = "01.01.1900";
            }

            if (Date_auto_do == "")
            {
                Date_auto_do_count = false;
                Date_auto_do = "01.01.3000";
            }

            count = 0;
            string[] Id_napr = new string[100000];
            try
            {
                for (int i = 0; i < jsonNew.data.transportations.Count; i++)
                {
                    for (int k = 0; k < lines.Length; k++)
                    {
                        if (jsonNew.data.transportations[i].line.name.ToUpper() == lines[k] || lines_count == false)
                        {
                            if (jsonNew.data.transportations[i].containerType.name.ToUpper() == typesContainer[k] || typeContainer_count == false)
                            {
                                if (Convert.ToInt32(jsonNew.data.transportations[i].lot) >= Convert.ToInt32(lot_s) || lot_s_count == false)
                                {
                                    if (Convert.ToInt32(jsonNew.data.transportations[i].lot) <= Convert.ToInt32(lot_do) || lot_do_count == false)
                                    {
                                        if (Convert.ToDouble(jsonNew.data.transportations[i].cargoWeightInContainer) >= Convert.ToDouble(ves_s) || ves_s_count == false)
                                        {
                                            if (Convert.ToDouble(jsonNew.data.transportations[i].cargoWeightInContainer) <= Convert.ToDouble(ves_do) || ves_do_count == false)
                                            {
                                                if (Convert.ToDateTime(Date_auto_s) <= Convert.ToDateTime(jsonNew.data.transportations[i].carDeliveryDate) || Date_auto_s_count == false)
                                                {
                                                    if (Convert.ToDateTime(Date_auto_do) >= Convert.ToDateTime(jsonNew.data.transportations[i].carDeliveryDate) || Date_auto_do_count == false)
                                                    {
                                                        Id_napr[count] = jsonNew.data.transportations[i].id.ToString();
                                                        count++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            int v = 1;
            string[] Id_naprNew = new string[Id_napr.Length];
            Id_naprNew[0] = Id_napr[0];
            for (int i = 1; i < Id_napr.Length; i++)
            {
                if (Id_napr[i - 1] != Id_napr[i])
                {
                    Id_naprNew[v] = Id_napr[i];
                    v++;
                }
            }



            count = 0;
            for (int i = 0; i < Id_naprNew.Length; i++)
            {
                if (Id_naprNew[i] == "" || Id_naprNew[i] == null)
                {

                }
                else
                {
                    count++;
                }
            }

            for (int i = 0; i <= count; i++)
            {
                id_ID[i] = Id_naprNew[i];
            }

        }

        void handllerNothingButton(object sender, DialogClickEventArgs e)
        {
            //Действие при нажатие "ОК"
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var backingFile6 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh.txt");
            using (var writer = File.CreateText(backingFile6))
            {

                writer.WriteLine("");

            }

            Intent intent_service_new_version = new Intent(this, typeof(Service_new_version));
            StopService(intent_service_new_version);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.filtr)
            {
                Intent intent_filter = new Intent(this, typeof(Class_filtr));
                StartActivity(intent_filter);

            }

            if (id == Resource.Id.sortirovka)
            {
                Intent intent_sortirovka = new Intent(this, typeof(Class_sortirovka));
                StartActivity(intent_sortirovka);
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Написать нам: info@mcs.od.ua", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            string Name = item.TitleCondensedFormatted.ToString();

            if (Vhod == false)
            {
                if (id == Resource.Id.Vhod_LichKab)
                {
                    Intent Activity_Vhod = new Intent(this, typeof(Class_Vhod));
                    Activity_Vhod.PutExtra("Vid", vid);
                    StartActivity(Activity_Vhod);

                }
            }
            else
            {
                if (id == Resource.Id.Vhod_LichKab)
                {

                    Intent Activity_Lich_Kab = new Intent(this, typeof(Class_LichKab));
                    StartActivity(Activity_Lich_Kab);

                }
            }

            if (id == Resource.Id.Moi_zayavki)
            {
                if (Vhod == true)
                {
                    Intent intent_moi_zayavku = new Intent(this, typeof(Class_moi_zayavki));
                    StartActivity(intent_moi_zayavku);
                }
                else
                {
                    Intent Activity_Vhod = new Intent(this, typeof(Class_Vhod));
                    Activity_Vhod.PutExtra("Vid", vid);
                    StartActivity(Activity_Vhod);
                }

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        private void OnButtonClick(object sender, System.EventArgs e)
        {
            Button myButton = (Button)sender;
            TextView textView_id = (TextView)FindViewById<TextView>(myButton.Id + 2);

            var Napravlenie = myButton.Text.ToString();
            var Id_napr = textView_id.Text.ToString();

            if (Vhod == true)
            {
                string vid = "Список";
                Intent intent_to_Prosmotr_napr = new Intent(this, typeof(Class_Prosmotr_napravleniia));
                intent_to_Prosmotr_napr.PutExtra("Id_napr", Id_napr);
                intent_to_Prosmotr_napr.PutExtra("Vid", vid);
                StartActivity(intent_to_Prosmotr_napr);
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Для просмотра направления Вам необходимо войти либо зарегистрироваться в системе.");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            void handllerNothingButton(object sender1, DialogClickEventArgs e1)
            {
                //Действие при нажатие "ОК"
            }

        }

        public void OnScrollChanged()
        {
            ScrollView scrollView = FindViewById<ScrollView>(Resource.Id.scrollView_ActivityStart);
            double scrollingSpace = scrollView.GetChildAt(0).Height - scrollView.Height;

            if (scrollingSpace <= scrollView.ScrollY)
            {
                RootObject1 napr_new = new RootObject1();
                int schetchik = 0;
                for (int i = count_new; i < count; i++)
                {

                    if (schetchik < 5)
                    {
                    M2: try
                        {
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://truck.mcs-bitrix.pp.ua/api/v1/transportation/" + id_ID[i]);
                            httpWebRequest.Accept = "application/json";
                            httpWebRequest.Method = "GET";
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();

                                napr_new = JsonConvert.DeserializeObject<RootObject1>(result);
                            }
                        }
                        catch (Exception)
                        {
                            if (napr_new.data == null)
                            {
                                goto M2;
                            }

                            //AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            //alert.SetTitle("Ошибка");
                            //alert.SetMessage("Ошибка сервера. Попробуйте позже.");
                            //alert.SetNeutralButton("OK", handllerNothingButton);
                            //alert.Show();
                        }

                        Button myButton = new Button(this);
                        myButton.Text = napr_new.data.transportation.cityOfLoading + " - " + napr_new.data.transportation.deliveryCity;
                        myButton.TextAlignment = Android.Views.TextAlignment.Center;
                        myButton.SetTextColor(Color.ParseColor("#ff274284"));

                        if (napr_new.data.transportation.status.name == "Перевозка отменена")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ffdc0000"));
                        }
                        else if (napr_new.data.transportation.status.name == "Завершена")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ff864d4d"));
                        }
                        else if (napr_new.data.transportation.status.name == "Ожидания заявок")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ff9bedb1"));
                        }
                        else if (napr_new.data.transportation.status.name == "Заявки рассмотрено")
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ff0f909c"));
                        }
                        else
                        {
                            myButton.SetBackgroundColor(Color.ParseColor("#ffd1d6d6"));
                        }

                        myButton.SetTextSize(ComplexUnitType.Dip, 15);
                        myButton.Id = View.GenerateViewId();
                        myButton.Click += OnButtonClick;

                        TextView mytextView = new TextView(this);
                        mytextView.Text = "Дата постановки машины: "
                            + napr_new.data.transportation.carDeliveryDate + "\n" + "Лот: "
                            + napr_new.data.transportation.lot + "\n" + "Вес контейнера: "
                            + napr_new.data.transportation.cargoWeightInContainer + "\n" + "Тип контейнера: "
                            + napr_new.data.transportation.containerType.name + "\n" + "Линия: "
                            + napr_new.data.transportation.line.name + "\n" + "Направление: "
                            + napr_new.data.transportation.transportationType.name + "\n" + "Статус: "
                            + napr_new.data.transportation.status.name;
                        mytextView.SetTextColor(Color.Black);
                        mytextView.Id = View.GenerateViewId();

                        TextView textView_id = new TextView(this);
                        textView_id.Text = napr_new.data.transportation.id.ToString();
                        textView_id.SetTextColor(Color.White);
                        textView_id.Id = View.GenerateViewId();

                        LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.linearLayout_ActivityStart);
                        LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams
                            (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);

                        ll.AddView(myButton, lp);
                        ll.AddView(mytextView, lp);
                        ll.AddView(textView_id, lp);
                    }
                    schetchik++;
                }
                count_new = count_new + 5;
                //Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
            }
            else
            {
                
            }
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






        public class ContainerType1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Line1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class TransportationType1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Status1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class App
        {
            public int lot { get; set; }
            public int price { get; set; }
            public string createdAt { get; set; }
        }

        public class Transportation1
        {
            public int id { get; set; }
            public string cityOfLoading { get; set; }
            public string deliveryCity { get; set; }
            public string carDeliveryDate { get; set; }
            public int lot { get; set; }
            public int cargoWeightInContainer { get; set; }
            public ContainerType1 containerType { get; set; }
            public Line1 line { get; set; }
            public TransportationType1 transportationType { get; set; }
            public Status1 status { get; set; }
            public string updatedAt { get; set; }
            public string createdAt { get; set; }
            public int userId { get; set; }
            public List<App> apps { get; set; }
        }

        public class Data1
        {
            public Transportation transportation { get; set; }
        }

        public class RootObject1
        {
            public bool success { get; set; }
            public int code { get; set; }
            public Data1 data { get; set; }
            public List<object> errors { get; set; }
        }

        public class Sortirovka
        {
            public string orderBy { get; set; }
            public string sort { get; set; }
        }
    }
}

