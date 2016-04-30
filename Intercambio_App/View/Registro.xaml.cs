using Intercambio_App.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Intercambio_App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Registro : Page
    {
        IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        List<UserData> ObjUserDataList = new List<UserData>();
        public Registro()
        {
            this.InitializeComponent();
            this.Loaded += SignUpPage_Loaded;
          
        }
        public void Gender_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rtn = (RadioButton)sender;
        }
        private void SignUpPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ISOFile.FileExists("RegistrationDetails"))//loaded previous items into list
            {
                using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("RegistrationDetails", FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(List<UserData>));
                    ObjUserDataList = (List<UserData>)serializer.ReadObject(fileStream);

                }
            }
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
        private void pwd_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pbox = (PasswordBox)sender;
            pbox.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            //UserName Validation
            if (!Regex.IsMatch(TxtUserName.Text.Trim(), @"^[A-Za-z_][a-zA-Z0-9_\s]*$"))
            {
               // MessageBox.Show("Invalid UserName");
            }

            //Password length Validation
            else if (TxtPwd.Password.Length < 6)
            {
               // MessageBox.Show("Password length should be minimum of 6 characters!");
            }

            //Address Text Empty Validation
            else if (TxtAddr.Text == "")
            {
              //  MessageBox.Show("Please enter your address!");
            }

            //Gender Selection Empty Validation
         

            //Phone Number Length Validation
            else if (TxtPhNo.Text.Length != 10)
            {
              //  MessageBox.Show("Invalid PhonNo");
            }

            //EmailID validation
            else if (!Regex.IsMatch(TxtEmail.Text.Trim(), @"^([a-zA-Z_])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$"))
            {
               // MessageBox.Show("Invalid EmailId");
            }

            //After validation success ,store user detials in isolated storage
            else if (TxtUserName.Text != "" && TxtPwd.Password != "" && TxtAddr.Text != "" && TxtEmail.Text != "" &&  TxtPhNo.Text != "")
            {
                UserData ObjUserData = new UserData();
                ObjUserData.UserName = TxtUserName.Text;
                ObjUserData.Password = TxtPwd.Password;
                ObjUserData.Direccion = TxtAddr.Text;
                ObjUserData.UPSEmail = TxtEmail.Text;
                    int Temp = 0;
                foreach (var UserLogin in ObjUserDataList)
                {
                    if (ObjUserData.UserName == UserLogin.UserName)
                    {
                        Temp = 1;
                    }
                }
                //Checking existing user names in local DB
                if (Temp == 0)
                {
                    ObjUserDataList.Add(ObjUserData);
                    if (ISOFile.FileExists("RegistrationDetails"))
                    {
                        ISOFile.DeleteFile("RegistrationDetails");
                    }
                    using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("RegistrationDetails", FileMode.Create))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(List<UserData>));

                        serializer.WriteObject(fileStream, ObjUserDataList);

                    }
                    
                    await App.MobileService.GetTable<UserData>().InsertAsync(ObjUserData);
                    //   MessageBox.Show("Congrats! your have successfully Registered.");
                    //     NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.Relative));
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                   // MessageBox.Show("Sorry! user name is already existed.");
                }

            }
            else
            {
               // MessageBox.Show("Please enter all details");
            }
        }
    }
}
