using System;
using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using Android.Util;
using Android.Content;
using Path = System.IO.Path;
using Android.Net;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_filtr : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_filtr);

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

            string Date_auto_s = "Дата поставновки машины с";
            string Date_auto_do = "Дата поставновки машины до";
            string lot_s = "";
            string lot_do = "";
            string ves_s = "";
            string ves_do = "";

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

            EditText editText_lot_s = FindViewById<EditText>(Resource.Id.editText_Lot_S_Filtr);
            EditText editText_lot_do = FindViewById<EditText>(Resource.Id.editText_Lot_Do_Filtr);
            EditText editText_ves_s = FindViewById<EditText>(Resource.Id.editText_Ves_S_Filtr);
            EditText editText_ves_do = FindViewById<EditText>(Resource.Id.editText_Ves_Do_Filtr);

            editText_lot_s.Text = lot_s;
            editText_lot_do.Text = lot_do;
            editText_ves_s.Text = ves_s;
            editText_ves_do.Text = ves_do;

            TextView textView_auto_date_do = FindViewById<TextView>(Resource.Id.textView_Date_Auto_do);
            textView_auto_date_do.Text = Date_auto_do;
            textView_auto_date_do.SetTextSize(ComplexUnitType.Dip, 18);
            textView_auto_date_do.Click += delegate
            {
                Intent intent_auto_do = new Intent(this, typeof(Class_filtr_auto_do_ok));
                StartActivity(intent_auto_do);
                
            };


            TextView textView_auto_date_s = FindViewById<TextView>(Resource.Id.textView_Date_Auto_s);
            textView_auto_date_s.Text = Date_auto_s;
            textView_auto_date_s.SetTextSize(ComplexUnitType.Dip, 18);
            textView_auto_date_s.Click += delegate
            {
                Intent intent_auto_s = new Intent(this, typeof(Class_filtr_auto_s_ok));
                StartActivity(intent_auto_s);
                
            };


            Button button_Line = FindViewById<Button>(Resource.Id.button_Line_Filter);

            button_Line.Click += delegate
            {

                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_s.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine(editText_lot_s.Text);
                }

                var backingFile1 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_do.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                    writer.WriteLine(editText_lot_do.Text);
                }

                var backingFile2 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_s.txt");
                using (var writer = File.CreateText(backingFile2))
                {
                    writer.WriteLine(editText_ves_s.Text);
                }

                var backingFile3 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_do.txt");
                using (var writer = File.CreateText(backingFile3))
                {
                    writer.WriteLine(editText_ves_do.Text);
                }

                Intent intent_filtr_line = new Intent(this, typeof(Class_filtr_line));
                StartActivity(intent_filtr_line);
                
            };

            Button button_type_container = FindViewById<Button>(Resource.Id.button_Container_Filter);

            button_type_container.Click += delegate
            {

                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_s.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine(editText_lot_s.Text);
                }

                var backingFile1 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_do.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                    writer.WriteLine(editText_lot_do.Text);
                }

                var backingFile2 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_s.txt");
                using (var writer = File.CreateText(backingFile2))
                {
                    writer.WriteLine(editText_ves_s.Text);
                }

                var backingFile3 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_do.txt");
                using (var writer = File.CreateText(backingFile3))
                {
                    writer.WriteLine(editText_ves_do.Text);
                }

                Intent intent_type_container = new Intent(this, typeof(Class_filtr_type_container));
                StartActivity(intent_type_container);
                
            };

            Button button_filtr_Ok = FindViewById<Button>(Resource.Id.button_Filtr_OK);

            button_filtr_Ok.Click += delegate
            {
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_s.txt");
                using (var writer = File.CreateText(backingFile))
                {
                   writer.WriteLine(editText_lot_s.Text);
                }

                var backingFile1 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_do.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                    writer.WriteLine(editText_lot_do.Text);
                }

                var backingFile2 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_s.txt");
                using (var writer = File.CreateText(backingFile2))
                {
                    writer.WriteLine(editText_ves_s.Text);
                }

                var backingFile3 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_do.txt");
                using (var writer = File.CreateText(backingFile3))
                {
                    writer.WriteLine(editText_ves_do.Text);
                }

                Intent intent_start = new Intent(this, typeof(MainActivity));
                StartActivity(intent_start);
            };

            Button button_filtr_cancel = FindViewById<Button>(Resource.Id.button_Filtr_cancel);
            button_filtr_cancel.Click += delegate
            {
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_s.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine("");
                }

                var backingFile1 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Lot_do.txt");
                using (var writer = File.CreateText(backingFile1))
                {
                    writer.WriteLine("");
                }

                var backingFile2 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_s.txt");
                using (var writer = File.CreateText(backingFile2))
                {
                    writer.WriteLine("");
                }

                var backingFile3 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Ves_do.txt");
                using (var writer = File.CreateText(backingFile3))
                {
                    writer.WriteLine("");
                }

                var backingFile4 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "filtr_auto_s.txt");
                using (var writer = File.CreateText(backingFile4))
                {
                    writer.WriteLine("");
                }

                var backingFile5 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "filtr_auto_do.txt");
                using (var writer = File.CreateText(backingFile5))
                {
                    writer.WriteLine("");
                }

                string[] Temp1 = new string[30];
                var backingFile6 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LinesFiltr.txt");
                using (var writer = File.CreateText(backingFile6))
                {
                    for(int i = 0; i < Temp1.Length; i++)
                    {
                        Temp1[i] = "";
                        writer.WriteLine(Temp1[i]);
                    }
                }

                string[] Temp2 = new string[30];
                var backingFile7 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "type_containerFiltr.txt");
                using (var writer = File.CreateText(backingFile7))
                {
                    for (int i = 0; i < Temp2.Length; i++)
                    {
                        Temp2[i] = "";
                        writer.WriteLine(Temp2[i]);
                    }
                }

                var backingFile8 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "type_containerFiltrCheck.txt");
                using (var writer = File.CreateText(backingFile8))
                {
                    for (int i = 0; i < Temp2.Length; i++)
                    {
                        Temp2[i] = "False";
                        writer.WriteLine(Temp2[i]);
                    }
                }

                var backingFile9 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LinesFiltrCheck.txt");
                using (var writer = File.CreateText(backingFile9))
                {
                    for (int i = 0; i < Temp1.Length; i++)
                    {
                        Temp1[i] = "False";
                        writer.WriteLine(Temp1[i]);
                    }
                }

                Intent intent_start = new Intent(this, typeof(MainActivity));
                StartActivity(intent_start);
            };
        }
    }
}