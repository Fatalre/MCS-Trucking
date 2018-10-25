using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Android.Content;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Text;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_restore_password:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Restore_Password);

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

            Button button_restore_password = FindViewById<Button>(Resource.Id.button_Activity_Restore_paswword);


            button_restore_password.Click += delegate
            {

                var restore_pass = new RestorePassword()
                {
                    email = FindViewById<EditText>(Resource.Id.editText_Activity_Restore_paswword).Text.ToString()
                };

                try
                {
                    var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/user/password/restore";

                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.ContentType = "application/json";
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string pasredContent = JsonConvert.SerializeObject(restore_pass);
                    UTF8Encoding encoding = new UTF8Encoding();
                    Byte[] bytes = encoding.GetBytes(pasredContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();


                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();

                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Восстановление пароля");
                    alert.SetMessage("Инструкции для восстановления пароля были отправлены на указанный e-mail: "+
                        FindViewById<EditText>(Resource.Id.editText_Activity_Restore_paswword).Text.ToString()+"\nНе забывайте проверять папку 'Спам'!!!!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
                catch (Exception)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла ошибка. Возможно данного e-mail нет в базе. Проверьте правильность ввода и попробуйте еще раз.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                void handllerNothingButton(object sender, DialogClickEventArgs e)
                {
                    //Действие при нажатие "ОК"
                }
            };
        }

        public class RestorePassword
        {
            public string email { get; set; }
        }
    }
}