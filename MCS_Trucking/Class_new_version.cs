﻿using System;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Net;
using Android;
using Android.Content;
using System.Net.NetworkInformation;
using System.Threading;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_new_version : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_new_version);

            bool isConnected;
            var ping = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            var options = new PingOptions();

            if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Android.Content.PM.Permission.Granted
                 && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Android.Content.PM.Permission.Granted)
            {
                var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                RequestPermissions(permissions, 1);
            }

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

            TextView textView_new = FindViewById<TextView>(Resource.Id.textView_new_version_activity_new_version);

            WebClient http = new WebClient();
            string new_version = http.DownloadString("http://citg.at.ua/MCS_Trucking/new_version.txt");

            textView_new.Text = new_version;

            Button button_new_version = FindViewById<Button>(Resource.Id.button_download_new_version_activity_new_version);

            button_new_version.Click += delegate
            {
                Android.Net.Uri uri = Android.Net.Uri.Parse("http://citg.at.ua/MCS_Trucking/MCS_Trucking_new_version.apk.txt");

                try
                {
                    DownloadManager.Request request = new DownloadManager.Request(uri);
                    request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, "MCS_Trucking_new_version.apk");
                    request.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
                    request.AllowScanningByMediaScanner();
                    DownloadManager manager = (DownloadManager)GetSystemService(DownloadService);
                    manager.Enqueue(request);

                    Thread.Sleep(15000);

                    var tmp = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                    var str = System.IO.Path.Combine(tmp, "new_version.apk");
                    var open_file = Android.Net.Uri.Parse(str);

                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(open_file, "application/vnd.android.package-archive");
                    intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

                    StartActivity(intent);
                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
            };
        }

        private void Completed (object sender, EventArgs e)
        {

        }
    }
}