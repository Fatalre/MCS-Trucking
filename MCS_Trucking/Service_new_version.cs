using System;
using System.Net;
using System.Reflection;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;

namespace MCS_Trucking
{
    [Service]
    class Service_new_version:Service
    {

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        private static readonly int BTC = 9999;

        public override void OnCreate()
        {
            base.OnCreate();
            Thread mythread = new Thread(Testik);
            mythread.IsBackground = true;
            mythread.Start();
        }

        public void Testik()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            WebClient http = new WebClient();

            Version latestVersion = version;

            try
            {
                latestVersion = new Version(http.DownloadString("http://citg.at.ua/MCS_Trucking/version.txt"));
            }
            catch (Exception)
            {

            }

            if (latestVersion != version)
            {
                Thread.Sleep(5000);

                Bundle valuesSand = new Bundle();

                Intent newIntent = new Intent(this, typeof(Class_new_version));
                newIntent.PutExtras(valuesSand);

                Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(Class_new_version)));
                stackBuilder.AddNextIntent(newIntent);

                PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                    .SetAutoCancel(true)
                    .SetContentIntent(resultPendingIntent)
                    .SetContentTitle("Доступно обнвление программы!")
                    .SetSmallIcon(Resource.Mipmap.ic_launcher_round)
                    .SetContentText("Нажмите, что бы загрузить")
                    .SetVibrate(new long[] { 1000, 1000 })
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

                NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                notificationManager.Notify(BTC, builder.Build());
            }
        }
    }
}