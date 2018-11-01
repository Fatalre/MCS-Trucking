using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Newtonsoft.Json;

namespace MCS_Trucking
{
    [Service]
    class Service_notific_new_transportation:Service
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
            int count_old = 0;
            int count_new = 0;
            int id_last_new = 0;

            M1: RootObject jsonNew = new RootObject();

            try
            {
                var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Count_old.txt");
                string line = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        count_old = Convert.ToInt32(line);
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                WebRequest reqGET = WebRequest.Create(@"https://truck.mcs-bitrix.pp.ua/api/v1/transportations");
                WebResponse resp = reqGET.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string json = sr.ReadToEnd();

                jsonNew = JsonConvert.DeserializeObject<RootObject>(json);

                count_new = jsonNew.data.transportations.Count;
                id_last_new = jsonNew.data.transportations[count_new-1].id;
            }
            catch (Exception ex)
            {
                ex.ToString();
                count_new = 0;
                count_old = 0;
            }

            if (count_new > count_old)
            {
                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Count_old.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine(count_new);
                }

                count_old = count_new;

                Bundle valuesSand = new Bundle();
                valuesSand.PutString("Id_napr", id_last_new.ToString());

                Intent newIntent = new Intent(this, typeof(Class_Prosmotr_napravleniia));
                newIntent.PutExtras(valuesSand);

                Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(Class_Prosmotr_napravleniia)));
                stackBuilder.AddNextIntent(newIntent);

                PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                    .SetAutoCancel(true)
                    .SetContentIntent(resultPendingIntent)
                    .SetContentTitle("Добавлена новая перевозка!")
                    .SetSmallIcon(Resource.Mipmap.ic_launcher_round)
                    .SetContentText(jsonNew.data.transportations[count_new-1].cityOfLoading + " - " 
                    + jsonNew.data.transportations[count_new-1].deliveryCity)
                    .SetVibrate(new long[] { 1000, 1000 })
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

                NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                notificationManager.Notify(BTC, builder.Build());
            }

            //Thread.Sleep(5000);
            Thread.Sleep(900000);
            goto M1;
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