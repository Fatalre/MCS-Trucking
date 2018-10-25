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
    public class Class_New_Password : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_New_Password);

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

            EditText editText_Old_password = FindViewById<EditText>(Resource.Id.editText_OldPassword_Activity_New_Password);
            EditText editText_New_password = FindViewById<EditText>(Resource.Id.editText_NewPassword_Activity_New_Password);

            Button button_new_password = FindViewById<Button>(Resource.Id.button_NewPassword_Activity_New_Password);

            button_new_password.Click += delegate
            {
                if (editText_New_password == editText_Old_password)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Новый пароль не может быть таким же, как старый");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                if (editText_New_password.Text.Length < 8)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Длина нового пароля должна быть длинее 8 символов");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                void handllerNothingButton(object sender, DialogClickEventArgs e)
                {

                }

                void handllerNothingButton1(object sender, DialogClickEventArgs e)
                {
                    Intent Intent_Activity_Start = new Intent(this, typeof(MainActivity));
                    StartActivity(Intent_Activity_Start);
                }

                string userToken = "";

                NewPassword newPassword = new NewPassword()
                {
                    oldPassword = editText_Old_password.Text,
                    newPassword = editText_New_password.Text
                };

                try
                {
                    var backingFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                    using (var reader = new StreamReader(backingFile, true))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            userToken = line;
                        }
                    }
                }
                catch (Exception)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла непредвиденная ошибка. Попробуйте повторить запрос позже.");
                    alert.SetNeutralButton("OK", handllerNothingButton1);
                    alert.Show();
                }

                var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/user/password";

                try
                {

                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.Headers.Add("userToken", userToken);
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "PUT";

                    string pasredContent = JsonConvert.SerializeObject(newPassword);
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
                    alert.SetTitle("Успешно");
                    alert.SetMessage("Пароль успешно изменен!");
                    alert.SetNeutralButton("OK", handllerNothingButton1);
                    alert.Show();
                }
                catch (WebException)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Введен неправильный старый пароль. Повторите попытку.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
            };
        }

        public class NewPassword
        {
            public string oldPassword { get; set; }
            public string newPassword { get; set; }
        }
    }
}