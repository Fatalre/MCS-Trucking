using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Graphics;
using Android.Content;
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
                        N1:  try
                        {
                            if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= Convert.ToDateTime("01." + month.ToString()) &&
                                    Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < Convert.ToDateTime("01." + month + 1.ToString()))
                            {
                                date_transport[i] = transport.data.transportations[i].carDeliveryDate;
                                id_transport[i] = transport.data.transportations[i].id;
                            }
                        }
                        catch (Exception)
                        {
                            goto N1;
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

                DateTime dateTime = new DateTime(DateTime.Now.Year,month,1);

                DateTime dateTime1 = new DateTime();
                if (month != 12)
                {
                    dateTime1 = new DateTime(DateTime.Now.Year, month + 1, 1);
                }

                DateTime dateTime2 = new DateTime(DateTime.Now.Year + 1, 1, 1);

                for (i=0; i < transport.data.transportations.Count; i++)
                {
                    if (month < 12)
                    {
                        if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= dateTime && Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < dateTime1)
                        {
                            date_transport[i] = transport.data.transportations[i].carDeliveryDate;
                            id_transport[i] = transport.data.transportations[i].id;
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) >= dateTime && Convert.ToDateTime(transport.data.transportations[i].carDeliveryDate) < dateTime2)
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

            int[] id1 = new int[100];
            int[] id2 = new int[100];
            int[] id3 = new int[100];
            int[] id4 = new int[100];
            int[] id5 = new int[100];
            int[] id6 = new int[100];
            int[] id7 = new int[100];
            int[] id8 = new int[100];
            int[] id9 = new int[100];
            int[] id10 = new int[100];
            int[] id11 = new int[100];
            int[] id12 = new int[100];
            int[] id13 = new int[100];
            int[] id14 = new int[100];
            int[] id15 = new int[100];
            int[] id16 = new int[100];
            int[] id17 = new int[100];
            int[] id18 = new int[100];
            int[] id19 = new int[100];
            int[] id20 = new int[100];
            int[] id21 = new int[100];
            int[] id22 = new int[100];
            int[] id23 = new int[100];
            int[] id24 = new int[100];
            int[] id25 = new int[100];
            int[] id26 = new int[100];
            int[] id27 = new int[100];
            int[] id28 = new int[100];
            int[] id29 = new int[100];
            int[] id30 = new int[100];
            int[] id31 = new int[100];
            string[] idSTR = new string[id1.Length];

            if (days == 31)
            {
                DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = formatInfo.Calendar;
                {

                    int[] count_transport = new int[31];

                    int[] schetchik = new int[100];
                    for (int i = 0; i < date_transport.Length; i++)
                    {
                        if (date_transport[i] != null)
                        {
                            int pos = date_transport[i].LastIndexOf(' ');
                            date_transport[i] = date_transport[i].Substring(0, pos);
                            DateTime dateTime_dateTransport = Convert.ToDateTime(date_transport[i]);

                            DateTime date1 = new DateTime(DateTime.Now.Year, month, 1);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[0]++;
                                id1[schetchik[0]] = id_transport[i];
                                schetchik[0]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 2);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[1]++;
                                id2[schetchik[1]] = id_transport[i];
                                schetchik[1]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 3);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[2]++;
                                id3[schetchik[2]] = id_transport[i];
                                schetchik[2]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 4);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[3]++;
                                id4[schetchik[3]] = id_transport[i];
                                schetchik[3]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 5);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[4]++;
                                id5[schetchik[4]] = id_transport[i];
                                schetchik[4]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 6);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[5]++;
                                id6[schetchik[5]] = id_transport[i];
                                schetchik[5]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 7);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[6]++;
                                id7[schetchik[6]] = id_transport[i];
                                schetchik[6]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 8);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[7]++;
                                id8[schetchik[7]] = id_transport[i];
                                schetchik[7]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 9);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[8]++;
                                id9[schetchik[8]] = id_transport[i];
                                schetchik[8]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 10);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[9]++;
                                id10[schetchik[9]] = id_transport[i];
                                schetchik[9]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 11);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[10]++;
                                id11[schetchik[10]] = id_transport[i];
                                schetchik[10]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 12);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[11]++;
                                id12[schetchik[11]] = id_transport[i];
                                schetchik[11]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 13);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[12]++;
                                id13[schetchik[12]] = id_transport[i];
                                schetchik[12]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 14);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[13]++;
                                id14[schetchik[13]] = id_transport[i];
                                schetchik[13]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 15);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[14]++;
                                id15[schetchik[14]] = id_transport[i];
                                schetchik[14]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 16);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[15]++;
                                id16[schetchik[15]] = id_transport[i];
                                schetchik[15]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 17);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[16]++;
                                id17[schetchik[16]] = id_transport[i];
                                schetchik[16]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 18);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[17]++;
                                id18[schetchik[17]] = id_transport[i];
                                schetchik[17]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 19);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[18]++;
                                id19[schetchik[18]] = id_transport[i];
                                schetchik[18]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 20);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[19]++;
                                id20[schetchik[19]] = id_transport[i];
                                schetchik[19]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 21);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[20]++;
                                id21[schetchik[20]] = id_transport[i];
                                schetchik[20]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 22);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[21]++;
                                id22[schetchik[21]] = id_transport[i];
                                schetchik[21]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 23);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[22]++;
                                id23[schetchik[22]] = id_transport[i];
                                schetchik[22]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 24);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[23]++;
                                id24[schetchik[23]] = id_transport[i];
                                schetchik[23]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 25);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[24]++;
                                id25[schetchik[24]] = id_transport[i];
                                schetchik[24]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 26);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[25]++;
                                id26[schetchik[25]] = id_transport[i];
                                schetchik[25]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 27);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[26]++;
                                id27[schetchik[26]] = id_transport[i];
                                schetchik[26]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 28);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[27]++;
                                id28[schetchik[27]] = id_transport[i];
                                schetchik[27]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 29);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[28]++;
                                id29[schetchik[28]] = id_transport[i];
                                schetchik[28]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 30);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[29]++;
                                id30[schetchik[29]] = id_transport[i];
                                schetchik[29]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 31);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[30]++;
                                id31[schetchik[30]] = id_transport[i];
                                schetchik[30]++;
                            }
                        }
                    }




                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    var weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[0] == 0)
                    {
                        TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[0].ToString();
                    }

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[1] == 0)
                    {
                        TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[1].ToString();
                    }

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[2] == 0)
                    {
                        TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[2].ToString();
                    }

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[3] == 0)
                    {
                        TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[3].ToString();
                    }

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[4] == 0)
                    {
                        TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[4].ToString();
                    }

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[5] == 0)
                    {
                        TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[5].ToString();
                    }

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[6] == 0)
                    {
                        TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[6].ToString();
                    }

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[7] == 0)
                    {
                        TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[7].ToString();
                    }

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[8] == 0)
                    {
                        TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[8].ToString();
                    }

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[9] == 0)
                    {
                        TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[9].ToString();
                    }

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[10] == 0)
                    {
                        TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[10].ToString();
                    }

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[11] == 0)
                    {
                        TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[11].ToString();
                    }

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[12] == 0)
                    {
                        TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[12].ToString();
                    }

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[13] == 0)
                    {
                        TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[13].ToString();
                    }

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[14] == 0)
                    {
                        TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[14].ToString();
                    }

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[15] == 0)
                    {
                        TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[15].ToString();
                    }

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[16] == 0)
                    {
                        TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[16].ToString();
                    }

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[17] == 0)
                    {
                        TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[17].ToString();
                    }

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[18] == 0)
                    {
                        TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[18].ToString();
                    }

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[19] == 0)
                    {
                        TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[19].ToString();
                    }

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[20] == 0)
                    {
                        TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[20].ToString();
                    }

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[21] == 0)
                    {
                        TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[21].ToString();
                    }

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[22] == 0)
                    {
                        TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[22].ToString();
                    }

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[23] == 0)
                    {
                        TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[23].ToString();
                    }

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[24] == 0)
                    {
                        TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[24].ToString();
                    }

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[25] == 0)
                    {
                        TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[25].ToString();
                    }

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[26] == 0)
                    {
                        TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[26].ToString();
                    }

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[27] == 0)
                    {
                        TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[27].ToString();
                    }

                    TextView_den_week29.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 29);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[28] == 0)
                    {
                        TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[28].ToString();
                    }

                    TextView_den_week30.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 30);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[29] == 0)
                    {
                        TextView_den_week30.Text = date.DayOfWeek.ToString() + ", 30" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week30.Text = date.DayOfWeek.ToString() + ", 30" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[29].ToString();
                    }

                    TextView_den_week31.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 31);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[30] == 0)
                    {
                        TextView_den_week31.Text = date.DayOfWeek.ToString() + ", 31" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week31.Text = date.DayOfWeek.ToString() + ", 31" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[30].ToString();
                    }
                }//Кнопки видно


            }
            else if (days == 30)
            {
                DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = formatInfo.Calendar;
                {

                    int[] count_transport = new int[31];

                    int[] schetchik = new int[100];
                    for (int i = 0; i < date_transport.Length; i++)
                    {
                        if (date_transport[i] != null)
                        {
                            int pos = date_transport[i].LastIndexOf(' ');
                            date_transport[i] = date_transport[i].Substring(0, pos);
                            DateTime dateTime_dateTransport = Convert.ToDateTime(date_transport[i]);

                            DateTime date1 = new DateTime(DateTime.Now.Year, month, 1);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[0]++;
                                id1[schetchik[0]] = id_transport[i];
                                schetchik[0]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 2);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[1]++;
                                id2[schetchik[1]] = id_transport[i];
                                schetchik[1]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 3);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[2]++;
                                id3[schetchik[2]] = id_transport[i];
                                schetchik[2]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 4);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[3]++;
                                id4[schetchik[3]] = id_transport[i];
                                schetchik[3]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 5);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[4]++;
                                id5[schetchik[4]] = id_transport[i];
                                schetchik[4]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 6);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[5]++;
                                id6[schetchik[5]] = id_transport[i];
                                schetchik[5]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 7);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[6]++;
                                id7[schetchik[6]] = id_transport[i];
                                schetchik[6]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 8);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[7]++;
                                id8[schetchik[7]] = id_transport[i];
                                schetchik[7]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 9);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[8]++;
                                id9[schetchik[8]] = id_transport[i];
                                schetchik[8]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 10);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[9]++;
                                id10[schetchik[9]] = id_transport[i];
                                schetchik[9]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 11);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[10]++;
                                id11[schetchik[10]] = id_transport[i];
                                schetchik[10]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 12);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[11]++;
                                id12[schetchik[11]] = id_transport[i];
                                schetchik[11]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 13);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[12]++;
                                id13[schetchik[12]] = id_transport[i];
                                schetchik[12]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 14);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[13]++;
                                id14[schetchik[13]] = id_transport[i];
                                schetchik[13]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 15);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[14]++;
                                id15[schetchik[14]] = id_transport[i];
                                schetchik[14]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 16);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[15]++;
                                id16[schetchik[15]] = id_transport[i];
                                schetchik[15]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 17);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[16]++;
                                id17[schetchik[16]] = id_transport[i];
                                schetchik[16]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 18);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[17]++;
                                id18[schetchik[17]] = id_transport[i];
                                schetchik[17]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 19);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[18]++;
                                id19[schetchik[18]] = id_transport[i];
                                schetchik[18]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 20);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[19]++;
                                id20[schetchik[19]] = id_transport[i];
                                schetchik[19]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 21);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[20]++;
                                id21[schetchik[20]] = id_transport[i];
                                schetchik[20]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 22);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[21]++;
                                id22[schetchik[21]] = id_transport[i];
                                schetchik[21]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 23);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[22]++;
                                id23[schetchik[22]] = id_transport[i];
                                schetchik[22]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 24);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[23]++;
                                id24[schetchik[23]] = id_transport[i];
                                schetchik[23]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 25);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[24]++;
                                id25[schetchik[24]] = id_transport[i];
                                schetchik[24]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 26);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[25]++;
                                id26[schetchik[25]] = id_transport[i];
                                schetchik[25]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 27);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[26]++;
                                id27[schetchik[26]] = id_transport[i];
                                schetchik[26]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 28);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[27]++;
                                id28[schetchik[27]] = id_transport[i];
                                schetchik[27]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 29);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[28]++;
                                id29[schetchik[28]] = id_transport[i];
                                schetchik[28]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 30);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[29]++;
                                id30[schetchik[29]] = id_transport[i];
                                schetchik[29]++;
                            }
                        }
                    }




                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    var weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[0] == 0)
                    {
                        TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[0].ToString();
                    }

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[1] == 0)
                    {
                        TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[1].ToString();
                    }

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[2] == 0)
                    {
                        TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[2].ToString();
                    }

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[3] == 0)
                    {
                        TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[3].ToString();
                    }

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[4] == 0)
                    {
                        TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[4].ToString();
                    }

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[5] == 0)
                    {
                        TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[5].ToString();
                    }

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[6] == 0)
                    {
                        TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[6].ToString();
                    }

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[7] == 0)
                    {
                        TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[7].ToString();
                    }

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[8] == 0)
                    {
                        TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[8].ToString();
                    }

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[9] == 0)
                    {
                        TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[9].ToString();
                    }

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[10] == 0)
                    {
                        TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[10].ToString();
                    }

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[11] == 0)
                    {
                        TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[11].ToString();
                    }

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[12] == 0)
                    {
                        TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[12].ToString();
                    }

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[13] == 0)
                    {
                        TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[13].ToString();
                    }

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[14] == 0)
                    {
                        TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[14].ToString();
                    }

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[15] == 0)
                    {
                        TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[15].ToString();
                    }

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[16] == 0)
                    {
                        TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[16].ToString();
                    }

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[17] == 0)
                    {
                        TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[17].ToString();
                    }

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[18] == 0)
                    {
                        TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[18].ToString();
                    }

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[19] == 0)
                    {
                        TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[19].ToString();
                    }

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[20] == 0)
                    {
                        TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[20].ToString();
                    }

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[21] == 0)
                    {
                        TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[21].ToString();
                    }

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[22] == 0)
                    {
                        TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[22].ToString();
                    }

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[23] == 0)
                    {
                        TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[23].ToString();
                    }

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[24] == 0)
                    {
                        TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[24].ToString();
                    }

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[25] == 0)
                    {
                        TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[25].ToString();
                    }

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[26] == 0)
                    {
                        TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[26].ToString();
                    }

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[27] == 0)
                    {
                        TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[27].ToString();
                    }

                    TextView_den_week29.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 29);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[28] == 0)
                    {
                        TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[28].ToString();
                    }

                    TextView_den_week30.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 30);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[29] == 0)
                    {
                        TextView_den_week30.Text = date.DayOfWeek.ToString() + ", 30" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week30.Text = date.DayOfWeek.ToString() + ", 30" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[29].ToString();
                    }
                }//Кнопки видно

                TextView_den_week31.SetBackgroundColor(Color.White);
            }
            else if (days == 28)
            {
                {
                    DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = formatInfo.Calendar;
                    {

                        int[] count_transport = new int[31];

                        int[] schetchik = new int[100];
                        for (int i = 0; i < date_transport.Length; i++)
                        {
                            if (date_transport[i] != null)
                            {
                                int pos = date_transport[i].LastIndexOf(' ');
                                date_transport[i] = date_transport[i].Substring(0, pos);
                                DateTime dateTime_dateTransport = Convert.ToDateTime(date_transport[i]);

                                DateTime date1 = new DateTime(DateTime.Now.Year, month, 1);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[0]++;
                                    id1[schetchik[0]] = id_transport[i];
                                    schetchik[0]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 2);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[1]++;
                                    id2[schetchik[1]] = id_transport[i];
                                    schetchik[1]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 3);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[2]++;
                                    id3[schetchik[2]] = id_transport[i];
                                    schetchik[2]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 4);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[3]++;
                                    id4[schetchik[3]] = id_transport[i];
                                    schetchik[3]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 5);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[4]++;
                                    id5[schetchik[4]] = id_transport[i];
                                    schetchik[4]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 6);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[5]++;
                                    id6[schetchik[5]] = id_transport[i];
                                    schetchik[5]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 7);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[6]++;
                                    id7[schetchik[6]] = id_transport[i];
                                    schetchik[6]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 8);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[7]++;
                                    id8[schetchik[7]] = id_transport[i];
                                    schetchik[7]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 9);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[8]++;
                                    id9[schetchik[8]] = id_transport[i];
                                    schetchik[8]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 10);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[9]++;
                                    id10[schetchik[9]] = id_transport[i];
                                    schetchik[9]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 11);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[10]++;
                                    id11[schetchik[10]] = id_transport[i];
                                    schetchik[10]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 12);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[11]++;
                                    id12[schetchik[11]] = id_transport[i];
                                    schetchik[11]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 13);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[12]++;
                                    id13[schetchik[12]] = id_transport[i];
                                    schetchik[12]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 14);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[13]++;
                                    id14[schetchik[13]] = id_transport[i];
                                    schetchik[13]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 15);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[14]++;
                                    id15[schetchik[14]] = id_transport[i];
                                    schetchik[14]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 16);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[15]++;
                                    id16[schetchik[15]] = id_transport[i];
                                    schetchik[15]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 17);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[16]++;
                                    id17[schetchik[16]] = id_transport[i];
                                    schetchik[16]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 18);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[17]++;
                                    id18[schetchik[17]] = id_transport[i];
                                    schetchik[17]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 19);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[18]++;
                                    id19[schetchik[18]] = id_transport[i];
                                    schetchik[18]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 20);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[19]++;
                                    id20[schetchik[19]] = id_transport[i];
                                    schetchik[19]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 21);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[20]++;
                                    id21[schetchik[20]] = id_transport[i];
                                    schetchik[20]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 22);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[21]++;
                                    id22[schetchik[21]] = id_transport[i];
                                    schetchik[21]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 23);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[22]++;
                                    id23[schetchik[22]] = id_transport[i];
                                    schetchik[22]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 24);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[23]++;
                                    id24[schetchik[23]] = id_transport[i];
                                    schetchik[23]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 25);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[24]++;
                                    id25[schetchik[24]] = id_transport[i];
                                    schetchik[24]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 26);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[25]++;
                                    id26[schetchik[25]] = id_transport[i];
                                    schetchik[25]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 27);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[26]++;
                                    id27[schetchik[26]] = id_transport[i];
                                    schetchik[26]++;
                                }

                                date1 = new DateTime(DateTime.Now.Year, month, 28);
                                if (dateTime_dateTransport == date1)
                                {
                                    count_transport[27]++;
                                    id28[schetchik[27]] = id_transport[i];
                                    schetchik[27]++;
                                }
                            }
                        }




                        TextView_den_week1.Visibility = ViewStates.Visible;
                        DateTime date = new DateTime(year, month, 1);
                        var weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[0] == 0)
                        {
                            TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[0].ToString();
                        }

                        TextView_den_week2.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 2);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[1] == 0)
                        {
                            TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[1].ToString();
                        }

                        TextView_den_week3.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 3);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[2] == 0)
                        {
                            TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[2].ToString();
                        }

                        TextView_den_week4.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 4);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[3] == 0)
                        {
                            TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[3].ToString();
                        }

                        TextView_den_week5.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 5);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[4] == 0)
                        {
                            TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[4].ToString();
                        }

                        TextView_den_week6.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 6);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[5] == 0)
                        {
                            TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[5].ToString();
                        }

                        TextView_den_week7.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 7);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[6] == 0)
                        {
                            TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[6].ToString();
                        }

                        TextView_den_week8.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 8);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[7] == 0)
                        {
                            TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[7].ToString();
                        }

                        TextView_den_week9.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 9);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[8] == 0)
                        {
                            TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[8].ToString();
                        }

                        TextView_den_week10.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 10);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[9] == 0)
                        {
                            TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[9].ToString();
                        }

                        TextView_den_week11.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 11);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[10] == 0)
                        {
                            TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[10].ToString();
                        }

                        TextView_den_week12.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 12);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[11] == 0)
                        {
                            TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[11].ToString();
                        }

                        TextView_den_week13.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 13);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[12] == 0)
                        {
                            TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[12].ToString();
                        }

                        TextView_den_week14.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 14);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[13] == 0)
                        {
                            TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[13].ToString();
                        }

                        TextView_den_week15.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 15);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[14] == 0)
                        {
                            TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[14].ToString();
                        }

                        TextView_den_week16.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 16);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[15] == 0)
                        {
                            TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[15].ToString();
                        }

                        TextView_den_week17.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 17);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[16] == 0)
                        {
                            TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[16].ToString();
                        }

                        TextView_den_week18.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 18);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[17] == 0)
                        {
                            TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[17].ToString();
                        }

                        TextView_den_week19.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 19);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[18] == 0)
                        {
                            TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[18].ToString();
                        }

                        TextView_den_week20.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 20);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[19] == 0)
                        {
                            TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[19].ToString();
                        }

                        TextView_den_week21.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 21);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[20] == 0)
                        {
                            TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[20].ToString();
                        }

                        TextView_den_week22.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 22);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[21] == 0)
                        {
                            TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[21].ToString();
                        }

                        TextView_den_week23.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 23);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[22] == 0)
                        {
                            TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[22].ToString();
                        }

                        TextView_den_week24.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 24);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[23] == 0)
                        {
                            TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[23].ToString();
                        }

                        TextView_den_week25.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 25);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[24] == 0)
                        {
                            TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[24].ToString();
                        }

                        TextView_den_week26.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 26);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[25] == 0)
                        {
                            TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[25].ToString();
                        }

                        TextView_den_week27.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 27);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[26] == 0)
                        {
                            TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[26].ToString();
                        }

                        TextView_den_week28.Visibility = ViewStates.Visible;
                        date = new DateTime(year, month, 28);
                        weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                        if (count_transport[27] == 0)
                        {
                            TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString();
                        }
                        else
                        {
                            TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[27].ToString();
                        }
                    }//Кнопки видно
                    TextView_den_week31.SetBackgroundColor(Color.White);
                    TextView_den_week30.SetBackgroundColor(Color.White);
                    TextView_den_week29.SetBackgroundColor(Color.White);
                }
            }
            else if (days == 29)
            {
                DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = formatInfo.Calendar;
                {

                    int[] count_transport = new int[31];

                    int[] schetchik = new int[100];
                    for (int i = 0; i < date_transport.Length; i++)
                    {
                        if (date_transport[i] != null)
                        {
                            int pos = date_transport[i].LastIndexOf(' ');
                            date_transport[i] = date_transport[i].Substring(0, pos);
                            DateTime dateTime_dateTransport = Convert.ToDateTime(date_transport[i]);

                            DateTime date1 = new DateTime(DateTime.Now.Year, month, 1);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[0]++;
                                id1[schetchik[0]] = id_transport[i];
                                schetchik[0]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 2);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[1]++;
                                id2[schetchik[1]] = id_transport[i];
                                schetchik[1]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 3);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[2]++;
                                id3[schetchik[2]] = id_transport[i];
                                schetchik[2]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 4);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[3]++;
                                id4[schetchik[3]] = id_transport[i];
                                schetchik[3]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 5);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[4]++;
                                id5[schetchik[4]] = id_transport[i];
                                schetchik[4]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 6);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[5]++;
                                id6[schetchik[5]] = id_transport[i];
                                schetchik[5]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 7);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[6]++;
                                id7[schetchik[6]] = id_transport[i];
                                schetchik[6]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 8);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[7]++;
                                id8[schetchik[7]] = id_transport[i];
                                schetchik[7]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 9);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[8]++;
                                id9[schetchik[8]] = id_transport[i];
                                schetchik[8]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 10);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[9]++;
                                id10[schetchik[9]] = id_transport[i];
                                schetchik[9]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 11);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[10]++;
                                id11[schetchik[10]] = id_transport[i];
                                schetchik[10]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 12);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[11]++;
                                id12[schetchik[11]] = id_transport[i];
                                schetchik[11]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 13);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[12]++;
                                id13[schetchik[12]] = id_transport[i];
                                schetchik[12]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 14);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[13]++;
                                id14[schetchik[13]] = id_transport[i];
                                schetchik[13]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 15);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[14]++;
                                id15[schetchik[14]] = id_transport[i];
                                schetchik[14]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 16);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[15]++;
                                id16[schetchik[15]] = id_transport[i];
                                schetchik[15]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 17);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[16]++;
                                id17[schetchik[16]] = id_transport[i];
                                schetchik[16]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 18);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[17]++;
                                id18[schetchik[17]] = id_transport[i];
                                schetchik[17]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 19);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[18]++;
                                id19[schetchik[18]] = id_transport[i];
                                schetchik[18]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 20);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[19]++;
                                id20[schetchik[19]] = id_transport[i];
                                schetchik[19]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 21);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[20]++;
                                id21[schetchik[20]] = id_transport[i];
                                schetchik[20]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 22);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[21]++;
                                id22[schetchik[21]] = id_transport[i];
                                schetchik[21]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 23);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[22]++;
                                id23[schetchik[22]] = id_transport[i];
                                schetchik[22]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 24);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[23]++;
                                id24[schetchik[23]] = id_transport[i];
                                schetchik[23]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 25);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[24]++;
                                id25[schetchik[24]] = id_transport[i];
                                schetchik[24]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 26);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[25]++;
                                id26[schetchik[25]] = id_transport[i];
                                schetchik[25]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 27);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[26]++;
                                id27[schetchik[26]] = id_transport[i];
                                schetchik[26]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 28);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[27]++;
                                id28[schetchik[27]] = id_transport[i];
                                schetchik[27]++;
                            }

                            date1 = new DateTime(DateTime.Now.Year, month, 29);
                            if (dateTime_dateTransport == date1)
                            {
                                count_transport[28]++;
                                id29[schetchik[28]] = id_transport[i];
                                schetchik[28]++;
                            }
                        }
                    }




                    TextView_den_week1.Visibility = ViewStates.Visible;
                    DateTime date = new DateTime(year, month, 1);
                    var weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[0] == 0)
                    {
                        TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week1.Text = date.DayOfWeek.ToString() + ", 1" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[0].ToString();
                    }

                    TextView_den_week2.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 2);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[1] == 0)
                    {
                        TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week2.Text = date.DayOfWeek.ToString() + ", 2" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[1].ToString();
                    }

                    TextView_den_week3.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 3);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[2] == 0)
                    {
                        TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week3.Text = date.DayOfWeek.ToString() + ", 3" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[2].ToString();
                    }

                    TextView_den_week4.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 4);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[3] == 0)
                    {
                        TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week4.Text = date.DayOfWeek.ToString() + ", 4" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[3].ToString();
                    }

                    TextView_den_week5.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 5);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[4] == 0)
                    {
                        TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week5.Text = date.DayOfWeek.ToString() + ", 5" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[4].ToString();
                    }

                    TextView_den_week6.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 6);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[5] == 0)
                    {
                        TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week6.Text = date.DayOfWeek.ToString() + ", 6" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[5].ToString();
                    }

                    TextView_den_week7.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 7);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[6] == 0)
                    {
                        TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week7.Text = date.DayOfWeek.ToString() + ", 7" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[6].ToString();
                    }

                    TextView_den_week8.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 8);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[7] == 0)
                    {
                        TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week8.Text = date.DayOfWeek.ToString() + ", 8" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[7].ToString();
                    }

                    TextView_den_week9.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 9);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[8] == 0)
                    {
                        TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week9.Text = date.DayOfWeek.ToString() + ", 9" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[8].ToString();
                    }

                    TextView_den_week10.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 10);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[9] == 0)
                    {
                        TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week10.Text = date.DayOfWeek.ToString() + ", 10" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[9].ToString();
                    }

                    TextView_den_week11.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 11);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[10] == 0)
                    {
                        TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week11.Text = date.DayOfWeek.ToString() + ", 11" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[10].ToString();
                    }

                    TextView_den_week12.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 12);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[11] == 0)
                    {
                        TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week12.Text = date.DayOfWeek.ToString() + ", 12" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[11].ToString();
                    }

                    TextView_den_week13.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 13);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[12] == 0)
                    {
                        TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week13.Text = date.DayOfWeek.ToString() + ", 13" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[12].ToString();
                    }

                    TextView_den_week14.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 14);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[13] == 0)
                    {
                        TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week14.Text = date.DayOfWeek.ToString() + ", 14" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[13].ToString();
                    }

                    TextView_den_week15.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 15);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[14] == 0)
                    {
                        TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week15.Text = date.DayOfWeek.ToString() + ", 15" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[14].ToString();
                    }

                    TextView_den_week16.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 16);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[15] == 0)
                    {
                        TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week16.Text = date.DayOfWeek.ToString() + ", 16" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[15].ToString();
                    }

                    TextView_den_week17.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 17);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[16] == 0)
                    {
                        TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week17.Text = date.DayOfWeek.ToString() + ", 17" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[16].ToString();
                    }

                    TextView_den_week18.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 18);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[17] == 0)
                    {
                        TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week18.Text = date.DayOfWeek.ToString() + ", 18" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[17].ToString();
                    }

                    TextView_den_week19.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 19);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[18] == 0)
                    {
                        TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week19.Text = date.DayOfWeek.ToString() + ", 19" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[18].ToString();
                    }

                    TextView_den_week20.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 20);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[19] == 0)
                    {
                        TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week20.Text = date.DayOfWeek.ToString() + ", 20" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[19].ToString();
                    }

                    TextView_den_week21.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 21);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[20] == 0)
                    {
                        TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week21.Text = date.DayOfWeek.ToString() + ", 21" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[20].ToString();
                    }

                    TextView_den_week22.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 22);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[21] == 0)
                    {
                        TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week22.Text = date.DayOfWeek.ToString() + ", 22" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[21].ToString();
                    }

                    TextView_den_week23.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 23);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[22] == 0)
                    {
                        TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week23.Text = date.DayOfWeek.ToString() + ", 23" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[22].ToString();
                    }

                    TextView_den_week24.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 24);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[23] == 0)
                    {
                        TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week24.Text = date.DayOfWeek.ToString() + ", 24" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[23].ToString();
                    }

                    TextView_den_week25.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 25);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[24] == 0)
                    {
                        TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week25.Text = date.DayOfWeek.ToString() + ", 25" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[24].ToString();
                    }

                    TextView_den_week26.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 26);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[25] == 0)
                    {
                        TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week26.Text = date.DayOfWeek.ToString() + ", 26" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[25].ToString();
                    }

                    TextView_den_week27.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 27);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[26] == 0)
                    {
                        TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week27.Text = date.DayOfWeek.ToString() + ", 27" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[26].ToString();
                    }

                    TextView_den_week28.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 28);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[27] == 0)
                    {
                        TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week28.Text = date.DayOfWeek.ToString() + ", 28" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[27].ToString();
                    }

                    TextView_den_week29.Visibility = ViewStates.Visible;
                    date = new DateTime(year, month, 29);
                    weeknumber = cal.GetWeekOfYear(date, formatInfo.CalendarWeekRule, formatInfo.FirstDayOfWeek);
                    if (count_transport[28] == 0)
                    {
                        TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Неделя: " + weeknumber.ToString();
                    }
                    else
                    {
                        TextView_den_week29.Text = date.DayOfWeek.ToString() + ", 29" + "\n" + "Неделя: " + weeknumber.ToString() + "\n" + "Перевозок: " + count_transport[28].ToString();
                    }
                }//Кнопки видно
                TextView_den_week31.SetBackgroundColor(Color.White);
                TextView_den_week30.SetBackgroundColor(Color.White);
            }

            TextView_den_week1.Click += delegate
            {
                if (TextView_den_week1.Text == "" || TextView_den_week1.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week1.Text.Contains("Перевозок"))
                    {
                        for(int i=0; i < id1.Length; i++)
                        {
                            idSTR[i] = id1[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week2.Click += delegate
            {
                if (TextView_den_week2.Text == "" || TextView_den_week2.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week2.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id2.Length; i++)
                        {
                            idSTR[i] = id2[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week3.Click += delegate
            {
                if (TextView_den_week3.Text == "" || TextView_den_week3.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week3.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id3.Length; i++)
                        {
                            idSTR[i] = id3[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week4.Click += delegate
            {
                if (TextView_den_week4.Text == "" || TextView_den_week4.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week4.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id4.Length; i++)
                        {
                            idSTR[i] = id4[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week5.Click += delegate
            {
                if (TextView_den_week5.Text == "" || TextView_den_week5.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week5.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id5.Length; i++)
                        {
                            idSTR[i] = id5[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week6.Click += delegate
            {
                if (TextView_den_week6.Text == "" || TextView_den_week6.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week6.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id6.Length; i++)
                        {
                            idSTR[i] = id6[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week7.Click += delegate
            {
                if (TextView_den_week7.Text == "" || TextView_den_week7.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week7.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id7.Length; i++)
                        {
                            idSTR[i] = id7[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week8.Click += delegate
            {
                if (TextView_den_week8.Text == "" || TextView_den_week8.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week8.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id8.Length; i++)
                        {
                            idSTR[i] = id8[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week9.Click += delegate
            {
                if (TextView_den_week9.Text == "" || TextView_den_week9.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week9.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id9.Length; i++)
                        {
                            idSTR[i] = id9[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week10.Click += delegate
            {
                if (TextView_den_week10.Text == "" || TextView_den_week10.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week10.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id10.Length; i++)
                        {
                            idSTR[i] = id10[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week11.Click += delegate
            {
                if (TextView_den_week11.Text == "" || TextView_den_week11.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week11.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id11.Length; i++)
                        {
                            idSTR[i] = id11[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week12.Click += delegate
            {
                if (TextView_den_week12.Text == "" || TextView_den_week12.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week12.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id12.Length; i++)
                        {
                            idSTR[i] = id12[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week13.Click += delegate
            {
                if (TextView_den_week13.Text == "" || TextView_den_week13.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week13.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id13.Length; i++)
                        {
                            idSTR[i] = id13[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week14.Click += delegate
            {
                if (TextView_den_week14.Text == "" || TextView_den_week14.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week14.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id14.Length; i++)
                        {
                            idSTR[i] = id14[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week15.Click += delegate
            {
                if (TextView_den_week15.Text == "" || TextView_den_week15.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week15.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id15.Length; i++)
                        {
                            idSTR[i] = id15[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week16.Click += delegate
            {
                if (TextView_den_week16.Text == "" || TextView_den_week16.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week16.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id16.Length; i++)
                        {
                            idSTR[i] = id16[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week17.Click += delegate
            {
                if (TextView_den_week17.Text == "" || TextView_den_week17.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week17.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id17.Length; i++)
                        {
                            idSTR[i] = id17[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week18.Click += delegate
            {
                if (TextView_den_week18.Text == "" || TextView_den_week18.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week18.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id18.Length; i++)
                        {
                            idSTR[i] = id18[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week19.Click += delegate
            {
                if (TextView_den_week19.Text == "" || TextView_den_week19.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week19.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id19.Length; i++)
                        {
                            idSTR[i] = id19[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week20.Click += delegate
            {
                if (TextView_den_week20.Text == "" || TextView_den_week20.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week20.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id20.Length; i++)
                        {
                            idSTR[i] = id20[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week21.Click += delegate
            {
                if (TextView_den_week21.Text == "" || TextView_den_week21.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week21.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id21.Length; i++)
                        {
                            idSTR[i] = id21[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week22.Click += delegate
            {
                if (TextView_den_week22.Text == "" || TextView_den_week22.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week22.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id22.Length; i++)
                        {
                            idSTR[i] = id22[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week23.Click += delegate
            {
                if (TextView_den_week23.Text == "" || TextView_den_week23.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week23.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id23.Length; i++)
                        {
                            idSTR[i] = id23[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week24.Click += delegate
            {
                if (TextView_den_week24.Text == "" || TextView_den_week24.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week24.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id24.Length; i++)
                        {
                            idSTR[i] = id24[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week25.Click += delegate
            {
                if (TextView_den_week25.Text == "" || TextView_den_week25.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week25.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id25.Length; i++)
                        {
                            idSTR[i] = id25[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week26.Click += delegate
            {
                if (TextView_den_week26.Text == "" || TextView_den_week26.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week26.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id26.Length; i++)
                        {
                            idSTR[i] = id26[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week27.Click += delegate
            {
                if (TextView_den_week27.Text == "" || TextView_den_week27.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week27.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id27.Length; i++)
                        {
                            idSTR[i] = id27[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week28.Click += delegate
            {
                if (TextView_den_week28.Text == "" || TextView_den_week28.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week28.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id28.Length; i++)
                        {
                            idSTR[i] = id28[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week29.Click += delegate
            {
                if (TextView_den_week29.Text == "" || TextView_den_week29.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week29.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id29.Length; i++)
                        {
                            idSTR[i] = id29[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week30.Click += delegate
            {
                if (TextView_den_week30.Text == "" || TextView_den_week30.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week30.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id30.Length; i++)
                        {
                            idSTR[i] = id30[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
            TextView_den_week31.Click += delegate
            {
                if (TextView_den_week31.Text == "" || TextView_den_week31.Text == null)
                {

                }
                else
                {
                    if (TextView_den_week31.Text.Contains("Перевозок"))
                    {
                        for (int i = 0; i < id31.Length; i++)
                        {
                            idSTR[i] = id31[i].ToString();
                        }
                        Intent intent_prosmotr_napravleniia_from_calendar = new Intent(this, typeof(Class_prosmotr_napravleniia_from_calendar));
                        intent_prosmotr_napravleniia_from_calendar.PutExtra("id_napr", idSTR);
                        StartActivity(intent_prosmotr_napravleniia_from_calendar);
                    }
                    else
                    {
                        Toast.MakeText(this, "В этот день нет перевозок", ToastLength.Long).Show();
                    }
                }
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var backingFile6 = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "refresh1.txt");
            using (var writer = File.CreateText(backingFile6))
            {

                writer.WriteLine("");

            }

            Intent intent_service_new_version = new Intent(this, typeof(Service_new_version));
            StopService(intent_service_new_version);
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