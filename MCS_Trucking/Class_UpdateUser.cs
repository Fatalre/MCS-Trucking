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
using Android.Net;

namespace MCS_Trucking
{
    [Activity(Label = "@string/app_name")]
    public class Class_UpdateUser : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Update_User);

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

            string FirstName = Intent.GetStringExtra("FirstName" ?? "FirstName");
            string LastName = Intent.GetStringExtra("LastName" ?? "LastName");
            string Email = Intent.GetStringExtra("Email" ?? "Email");
            string PhoneNumber = Intent.GetStringExtra("PhoneNumber" ?? "PhoneNumber");
            string Company = Intent.GetStringExtra("Company" ?? "Company");

            EditText editText_FirstName = FindViewById<EditText>(Resource.Id.editText_First_Name_ActivityUpdateUser);
            EditText editText_LastName = FindViewById<EditText>(Resource.Id.editText_Last_Name_ActivityUpdateUser);
            EditText editText_Email = FindViewById<EditText>(Resource.Id.editText_Email_ActivityUpdateUser);
            EditText editText_PhoneNumber = FindViewById<EditText>(Resource.Id.editText_PhoneNumber_ActivityUpdateUser);
            EditText editText_Company = FindViewById<EditText>(Resource.Id.editText_CompanyName_ActivityUpdateUser);

            editText_FirstName.Text = FirstName;
            editText_LastName.Text = LastName;
            editText_Email.Text = Email;
            editText_PhoneNumber.Text = PhoneNumber;
            editText_Company.Text = Company;

            string company_yes_no;

            if (editText_Company.Text == "")
            {
                company_yes_no = "0";
            }
            else
            {
                company_yes_no = "1";
            }

            if (editText_FirstName.Text.Length < 3 || editText_LastName.Text.Length < 3)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Поле 'Имя' и 'Фамилия' Должны содержать не менее 2 символов");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            if (editText_FirstName.Text == "" || editText_LastName.Text == "" 
                || editText_Email.Text == "" || editText_PhoneNumber.Text == "")
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Заполнены не все обязательные поля! Все поля, кроме поля 'Компания' являются обязательными!" +
                    " Заполните поля и повторите попытку");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            if (editText_PhoneNumber.Text.Length < 10 || editText_PhoneNumber.Text.Length > 15)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Номер телефона должен быть в пределах от 10 до 15 цифр");
                alert.SetNeutralButton("OK", handllerNothingButton);
                alert.Show();
            }

            void handllerNothingButton(object sender, DialogClickEventArgs e)
            {

                Intent Intent_Activity_Start = new Intent(this, typeof(MainActivity));
                StartActivity(Intent_Activity_Start);
            }

            Button button_UpdateUser = FindViewById<Button>(Resource.Id.button_UpdateUser_Activity_UpdateUser);
            button_UpdateUser.Click += delegate
            {
                string userToken = "";

                var user_update = new InfoToUpdate()
                {
                    isCompany = company_yes_no,
                    companyName = editText_Company.Text,
                    phoneNumber = editText_PhoneNumber.Text

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
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                string baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/session";

                var jsonNew = new RootObject();

                try
                {
                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.Method = "GET";
                    http.Headers.Add("userToken", userToken);
                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    jsonNew = JsonConvert.DeserializeObject<RootObject>(content);
                }
                catch (Exception)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла непредвиденная ошибка. Попробуйте повторить запрос позже.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }

                baseAddress = "https://truck.mcs-bitrix.pp.ua/api/v1/user/"+jsonNew.data.user.id.ToString();

                try
                {

                    var http = (HttpWebRequest)WebRequest.Create(new System.Uri(baseAddress));
                    http.Accept = "application/json";
                    http.Headers.Add("userToken", userToken);
                    http.ContentType = "application/json";
                    http.Method = "PUT";

                    string pasredContent = JsonConvert.SerializeObject(user_update);
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
                    alert.SetMessage("Данные успешно обновлены!");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
                catch (WebException)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка");
                    alert.SetMessage("Произошла непредвиденная ошибка. Попробуйте повторить запрос позже.");
                    alert.SetNeutralButton("OK", handllerNothingButton);
                    alert.Show();
                }
            };
        }
    }

    public class InfoToUpdate
    {
        public string isCompany { get; set; }
        public string companyName { get; set; }
        public string phoneNumber { get; set; }
    }

    public class Roles
    {
        public string carrier { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public int isCompany { get; set; }
        public string companyName { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public Roles roles { get; set; }
    }

    public class Data
    {
        public User user { get; set; }
    }

    public class RootObject
    {
        public bool success { get; set; }
        public int code { get; set; }
        public Data data { get; set; }
        public List<object> errors { get; set; }
    }
}