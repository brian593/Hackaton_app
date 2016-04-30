using Intercambio_App.Model;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Intercambio_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        List<UserData> ObjUserDataList = new List<UserData>();
         bool resultado = false;
        string contraseña;
        UserData ObjUserData = new UserData();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += LoginPage_Loaded;

        }

        public async void LoginSelect(string UserName, string UserPass)
        {

            try
            {
                await App.MobileService.GetTable<UserData>().Where(citem =>( citem.UserName == UserName||citem.UPSEmail== UserName)).ToListAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        System.Diagnostics.Debug.WriteLine("Faulted");
                      //  Dispatcher.BeginInvoke(() => MessageBox.Show("Error de conexion !\n"));
                    }
                    else
                    {
                        System.Collections.Generic.List<UserData> UserItems = t.Result;
                        int count = UserItems.Count;
                        if (count > 0)
                        {

                            contraseña = UserItems[0].Password;
                            ObjUserData = UserItems[0] ;


                            // this.userexist = true;

                        }
                        //   Dispatcher.BeginInvoke(() => this._Passcode = record.user_pass);                              
                    }
                });

                Debug.WriteLine(contraseña);
                if (contraseña==PassWord.Password)
                {
                  
                    int Temp = 0;
                    foreach (var UserLogin in ObjUserDataList)
                    {
                        if (ObjUserData.UserName == UserLogin.UserName)
                        {
                            Temp = 1;
                        }
                    }

              //Grabar datos en la base
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



                    //Registro en una base local 
                    foreach (var UserLogin in ObjUserDataList)
                    {
                        if (UserName == UserLogin.UserName)
                        {
                            var Settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                            Settings.Values["CheckLogin"] = "Login sucess";

                            if (ISOFile.FileExists("CurrentLoginUserDetails"))
                            {
                                ISOFile.DeleteFile("CurrentLoginUserDetails");
                            }
                            using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("CurrentLoginUserDetails", FileMode.Create))
                            {
                                DataContractSerializer serializer = new DataContractSerializer(typeof(UserData));
                                serializer.WriteObject(fileStream, UserLogin);
                            }
                        }
                   }
                    //Fin de grabado 


                    Frame.Navigate(typeof(View.Principal));

                }

            }
            catch (System.Net.WebException e)
            {
              //  Dispatcher.BeginInvoke(() => MessageBox.Show("\nConnection failed!\n"));
            }
        }

   

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            //var Settings = IsolatedStorageSettings.ApplicationSettings;
            var Settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            //Check if user already login,so we need to direclty navigate to details page instead of showing login page when user launch the app.
            // if (Settings.Contains("CheckLogin"))
            if (Settings.Values.ContainsKey("CheckLogin"))
            {
              //  NavigationService.Navigate(new Uri("/Views/UserDetails.xaml", UriKind.Relative));
                Frame.Navigate(typeof(View.Principal));

            }
            else
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

        }
        public async void Login_Click(object sender, RoutedEventArgs e)
        {

            if (UserName.Text != "" && PassWord.Password != "")
            {
                LoginSelect(UserName.Text, PassWord.Password);
             
            }
            else
            {
                //    MessageBox.Show("Enter UserID/Password");
                MessageDialog dialog = new MessageDialog("Ingrese un usuario y contraseña", "Error");
                await dialog.ShowAsync();
            }
        }


        public void SignUp_Click(object sender, RoutedEventArgs e)
        {
            // NavigationService.Navigate(new Uri("/Views/SignUpPage.xaml", UriKind.Relative));
            Frame.Navigate(typeof(View.Registro));
        }
        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            UserName.BorderBrush = new SolidColorBrush(Colors.LightGray);
            PassWord.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
    }
}
