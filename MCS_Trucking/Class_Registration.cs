using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Android.Content;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Text;
using System.Net.NetworkInformation;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Class_Registration:Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Registration);

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

            EditText editText_FirstName = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_FirstName);
            EditText editText_LastName = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_LastName);
            EditText editText_Password = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_Password);
            EditText editText_ReapetPassword = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_ReapetPassword);
            EditText editText_Email = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_Email);
            EditText editText_Phone = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_Phone);
            EditText editText_Company = (EditText)FindViewById<EditText>(Resource.Id.editText_Activity_Reg_Company);

            Button button_Reg = (Button)FindViewById<Button>(Resource.Id.button_Activity_Reg_Reg);

            button_Reg.Click += delegate
            {
                string company_yes_no;

                if (editText_Company.Text == "")
                {
                    company_yes_no = "0";
                }
                else
                {
                    company_yes_no = "1";
                }

                if (editText_Password.Text != editText_ReapetPassword.Text)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Введенные пароли отличаются. Проверьте правильность ввода и повторите попытку!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                if(editText_Password.Text.Length < 8)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Пароль должен содержать больше 8 символов!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                void handllerNothingButton(object sender, DialogClickEventArgs e)
                {
                    Intent intent_to_first_Activity = new Intent(this, typeof(MainActivity_old));
                    Finish();
                    StartActivity(intent_to_first_Activity);
                }

                if (editText_FirstName.Text.Length < 3 || editText_LastName.Text.Length < 3)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Поле 'Имя' и 'Фамилия' Должны содержать не менее 2 символов");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                if (editText_FirstName.Text == "" || editText_LastName.Text == "" || editText_Password.Text == "" || 
                editText_ReapetPassword.Text=="" || editText_Email.Text == "" || editText_Phone.Text == "")
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Заполнены не все обязательные поля! Все поля, кроме поля 'Компания' являются обязательными!" +
                        " Заполните поля и повторите попытку");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                if (editText_Phone.Text.Length < 10 || editText_Phone.Text.Length > 15)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Номер телефона должен быть в пределах от 10 до 15 цифр");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                var user_Reg = new USerReg()
                {
                    firstName = editText_FirstName.Text,
                    lastName = editText_LastName.Text,
                    email = editText_Email.Text,
                    password = editText_Password.Text,
                    isCompany = company_yes_no,
                    companyName = editText_Company.Text,
                    phoneNumber = editText_Phone.Text
                    
                };

                var baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/user";

                try
                {

                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string pasredContent = JsonConvert.SerializeObject(user_Reg);
                    UTF8Encoding encoding = new UTF8Encoding();
                    Byte[] bytes = encoding.GetBytes(pasredContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();


                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                }
                catch (WebException)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка регистрации");
                    alert.SetMessage("Ошибка регистрации. Данные email уже зарегистрирован, либо данные введины неверно"+
                        "\n"+"Правильный ввод e-mail: example@example.com"+
                        "Проверьте данные и повторите попытку");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/session";

                RootObject jsonNew;

                try
                {
                    var zapros_na_vhod = new Zapros()
                    {
                        email = editText_Email.Text,
                        password = editText_Password.Text
                    };

                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    string pasredContent = JsonConvert.SerializeObject(zapros_na_vhod);
                    UTF8Encoding encoding = new UTF8Encoding();
                    Byte[] bytes = encoding.GetBytes(pasredContent);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();


                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    jsonNew = JsonConvert.DeserializeObject<RootObject>(content);


                    var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userToken.txt");
                    using (var writer = File.CreateText(backingFile))
                    {
                        writer.WriteLine(jsonNew.data.userToken.ToString());
                    }

                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Регистрация успешна");
                    alert.SetMessage("Регистрация успешно завершена");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
                catch (Exception)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла непредвинденная ошибка. Попробуйте повторить запрос позже");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

            };

        }

        public class USerReg
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string isCompany { get; set; }
            public string companyName { get; set; }
            public string phoneNumber { get; set; }
        }

        public class Zapros
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class Data
        {
            public string userToken { get; set; }
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