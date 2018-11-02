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
using System.Globalization;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", ConfigurationChanges =
        Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_prosmotr_month_calendar : Activity
    {

        string cash = "";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_prosmotr_month_calendar);

            int month = Convert.ToInt32(Intent.GetStringExtra("month" ?? "month"));
            string[] date_transport = new string[500];
            int[] id_transport = new int[500];

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


            if (cash == "true")
            {
                int i = 0;
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

                for (i = 0; i < transport.data.transportations.Count; i++)
                {
                    if (month < 12)
                    {
                        if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01." + month.ToString()) &&
                            Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01." + month + 1.ToString()))
                        {
                            date_transport[i] = transport.data.transportations[i].carDeliveryDate;
                            id_transport[i] = transport.data.transportations[i].id;
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01." + month.ToString()) &&
                             Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.01." + Convert.ToString(DateTime.Now.Year + 1)))
                        {
                            date_transport[i] = transport.data.transportations[i].carDeliveryDate;
                            id_transport[i] = transport.data.transportations[i].id;
                        }
                    }

                }
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


                for (i=0; i < transport.data.transportations.Count; i++)
                {
                    if (month < 12)
                    {
                        if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01." + month.ToString()) &&
                            Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01." + month + 1.ToString()))
                        {
                            date_transport[i] = transport.data.transportations[i].carDeliveryDate;
                            id_transport[i] = transport.data.transportations[i].id;
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01." + month.ToString()) &&
                             Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01.01."+Convert.ToString(DateTime.Now.Year+1)))
                        {
                            date_transport[i] = transport.data.transportations[i].carDeliveryDate;
                            id_transport[i] = transport.data.transportations[i].id;
                        }
                    }

                }




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

            TextView TextView_den_week1 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView1);
            TextView TextView_den_week2 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView2);
            TextView TextView_den_week3 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView3);
            TextView TextView_den_week4 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView4);
            TextView TextView_den_week5 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView5);
            TextView TextView_den_week6 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView6);
            TextView TextView_den_week7 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView7);
            TextView TextView_den_week8 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView8);
            TextView TextView_den_week9 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView9);
            TextView TextView_den_week10 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView10);
            TextView TextView_den_week11 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView11);
            TextView TextView_den_week12 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView12);
            TextView TextView_den_week13 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView13);
            TextView TextView_den_week14 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView14);
            TextView TextView_den_week15 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView15);
            TextView TextView_den_week16 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView16);
            TextView TextView_den_week17 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView17);
            TextView TextView_den_week18 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView18);
            TextView TextView_den_week19 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView19);
            TextView TextView_den_week20 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView20);
            TextView TextView_den_week21 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView21);
            TextView TextView_den_week22 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView22);
            TextView TextView_den_week23 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView23);
            TextView TextView_den_week24 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView24);
            TextView TextView_den_week25 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView25);
            TextView TextView_den_week26 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView26);
            TextView TextView_den_week27 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView27);
            TextView TextView_den_week28 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView28);
            TextView TextView_den_week29 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView29);
            TextView TextView_den_week30 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView30);
            TextView TextView_den_week31 = FindViewById<TextView>(Resource.Id.Activity_prosmotr_calendar_TextView31);



            int year = DateTime.Now.Year;
            int days = DateTime.DaysInMonth(year, month);



            if (days == 31)
            {
                DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = formatInfo.Calendar;
                {

                    int[,] count_transport = new int[31,500];
                    int y = 0;
                    for (int i = 0; i < date_transport.Length; i++)
                    {
                        int pos = date_transport[i].LastIndexOf(' ');
                        date_transport[i] = date_transport[i].Substring(0, pos);
                        DateTime dateTime_dateTransport = Convert.ToDateTime(date_transport[i]);

                        DateTime date1 = new DateTime(DateTime.Now.Year, month, 1);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[0, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 2);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[1, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 3);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[2, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 4);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[3, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 5);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[4, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 6);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[5, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 7);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[6, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 8);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[7, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 9);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[8, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 10);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[9, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 11);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[10, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 12);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[11, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 13);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[12, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 14);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[13, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 15);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[14, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 16);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[15, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 17);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[16, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 18);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[17, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 19);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[18, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 20);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[19, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 21);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[20, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 22);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[21, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 23);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[22, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 24);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[23, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 25);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[24, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 26);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[25, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 27);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[26, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 28);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[27, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 29);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[28, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 30);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[29, y]++;
                        }

                        date1 = new DateTime(DateTime.Now.Year, month, 31);
                        if (dateTime_dateTransport == date1)
                        {
                            count_transport[30, y]++;
                        }

                        y++;

                    }




                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    var weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week29.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 29);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week30.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 30);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week30.Text = date.DayOfWeek.ToString() + ", 30" + "\n" + "Week: " + weeknumber.ToString();

                    TextView_den_week31.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 31);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    TextView_den_week31.Text = date.DayOfWeek.ToString() + ", 31" + "\n" + "Week: " + weeknumber.ToString();
                }//Кнопки видно


            }
            else if (days == 30)
            {
                {
                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    TextView_den_week1.Text = date.DayOfWeek.ToString();

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    TextView_den_week2.Text = date.DayOfWeek.ToString();

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    TextView_den_week3.Text = date.DayOfWeek.ToString();

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    TextView_den_week4.Text = date.DayOfWeek.ToString();

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    TextView_den_week5.Text = date.DayOfWeek.ToString();

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    TextView_den_week6.Text = date.DayOfWeek.ToString();

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    TextView_den_week7.Text = date.DayOfWeek.ToString();

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    TextView_den_week8.Text = date.DayOfWeek.ToString();

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    TextView_den_week9.Text = date.DayOfWeek.ToString();

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    TextView_den_week10.Text = date.DayOfWeek.ToString();

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    TextView_den_week11.Text = date.DayOfWeek.ToString();

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    TextView_den_week12.Text = date.DayOfWeek.ToString();

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    TextView_den_week13.Text = date.DayOfWeek.ToString();

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    TextView_den_week14.Text = date.DayOfWeek.ToString();

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    TextView_den_week15.Text = date.DayOfWeek.ToString();

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    TextView_den_week16.Text = date.DayOfWeek.ToString();

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    TextView_den_week17.Text = date.DayOfWeek.ToString();

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    TextView_den_week18.Text = date.DayOfWeek.ToString();

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    TextView_den_week19.Text = date.DayOfWeek.ToString();

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    TextView_den_week20.Text = date.DayOfWeek.ToString();

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    TextView_den_week21.Text = date.DayOfWeek.ToString();

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    TextView_den_week22.Text = date.DayOfWeek.ToString();

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    TextView_den_week23.Text = date.DayOfWeek.ToString();

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    TextView_den_week24.Text = date.DayOfWeek.ToString();

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    TextView_den_week25.Text = date.DayOfWeek.ToString();

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    TextView_den_week26.Text = date.DayOfWeek.ToString();

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    TextView_den_week27.Text = date.DayOfWeek.ToString();

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    TextView_den_week28.Text = date.DayOfWeek.ToString();

                    TextView_den_week29.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 29);
                    TextView_den_week29.Text = date.DayOfWeek.ToString();

                    TextView_den_week30.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 30);
                    TextView_den_week30.Text = date.DayOfWeek.ToString();
                }//Кнопки видно
            }
            else if (days == 28)
            {
                {
                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    TextView_den_week1.Text = date.DayOfWeek.ToString();

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    TextView_den_week2.Text = date.DayOfWeek.ToString();

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    TextView_den_week3.Text = date.DayOfWeek.ToString();

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    TextView_den_week4.Text = date.DayOfWeek.ToString();

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    TextView_den_week5.Text = date.DayOfWeek.ToString();

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    TextView_den_week6.Text = date.DayOfWeek.ToString();

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    TextView_den_week7.Text = date.DayOfWeek.ToString();

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    TextView_den_week8.Text = date.DayOfWeek.ToString();

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    TextView_den_week9.Text = date.DayOfWeek.ToString();

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    TextView_den_week10.Text = date.DayOfWeek.ToString();

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    TextView_den_week11.Text = date.DayOfWeek.ToString();

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    TextView_den_week12.Text = date.DayOfWeek.ToString();

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    TextView_den_week13.Text = date.DayOfWeek.ToString();

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    TextView_den_week14.Text = date.DayOfWeek.ToString();

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    TextView_den_week15.Text = date.DayOfWeek.ToString();

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    TextView_den_week16.Text = date.DayOfWeek.ToString();

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    TextView_den_week17.Text = date.DayOfWeek.ToString();

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    TextView_den_week18.Text = date.DayOfWeek.ToString();

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    TextView_den_week19.Text = date.DayOfWeek.ToString();

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    TextView_den_week20.Text = date.DayOfWeek.ToString();

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    TextView_den_week21.Text = date.DayOfWeek.ToString();

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    TextView_den_week22.Text = date.DayOfWeek.ToString();

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    TextView_den_week23.Text = date.DayOfWeek.ToString();

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    TextView_den_week24.Text = date.DayOfWeek.ToString();

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    TextView_den_week25.Text = date.DayOfWeek.ToString();

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    TextView_den_week26.Text = date.DayOfWeek.ToString();

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    TextView_den_week27.Text = date.DayOfWeek.ToString();

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    TextView_den_week28.Text = date.DayOfWeek.ToString();
                }//Кнопки видно
            }
            else if (days == 29)
            {
                {
                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    TextView_den_week1.Text = date.DayOfWeek.ToString();

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    TextView_den_week2.Text = date.DayOfWeek.ToString();

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    TextView_den_week3.Text = date.DayOfWeek.ToString();

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    TextView_den_week4.Text = date.DayOfWeek.ToString();

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    TextView_den_week5.Text = date.DayOfWeek.ToString();

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    TextView_den_week6.Text = date.DayOfWeek.ToString();

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    TextView_den_week7.Text = date.DayOfWeek.ToString();

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    TextView_den_week8.Text = date.DayOfWeek.ToString();

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    TextView_den_week9.Text = date.DayOfWeek.ToString();

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    TextView_den_week10.Text = date.DayOfWeek.ToString();

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    TextView_den_week11.Text = date.DayOfWeek.ToString();

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    TextView_den_week12.Text = date.DayOfWeek.ToString();

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    TextView_den_week13.Text = date.DayOfWeek.ToString();

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    TextView_den_week14.Text = date.DayOfWeek.ToString();

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    TextView_den_week15.Text = date.DayOfWeek.ToString();

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    TextView_den_week16.Text = date.DayOfWeek.ToString();

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    TextView_den_week17.Text = date.DayOfWeek.ToString();

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    TextView_den_week18.Text = date.DayOfWeek.ToString();

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    TextView_den_week19.Text = date.DayOfWeek.ToString();

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    TextView_den_week20.Text = date.DayOfWeek.ToString();

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    TextView_den_week21.Text = date.DayOfWeek.ToString();

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    TextView_den_week22.Text = date.DayOfWeek.ToString();

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    TextView_den_week23.Text = date.DayOfWeek.ToString();

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    TextView_den_week24.Text = date.DayOfWeek.ToString();

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    TextView_den_week25.Text = date.DayOfWeek.ToString();

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    TextView_den_week26.Text = date.DayOfWeek.ToString();

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    TextView_den_week27.Text = date.DayOfWeek.ToString();

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    TextView_den_week28.Text = date.DayOfWeek.ToString();

                    TextView_den_week29.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 29);
                    TextView_den_week29.Text = date.DayOfWeek.ToString();
                }//Кнопки видно
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