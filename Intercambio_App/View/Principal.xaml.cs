using Intercambio_App.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Principal : Page
    {
        IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        UserData ObjUserData = new UserData();

        public Principal()
        {
            this.InitializeComponent();
            List<ListData> ObjListDataList = new List<ListData>();

            if (ISOFile.FileExists("CurrentLoginUserDetails"))//read current user login details  
            {
                using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("CurrentLoginUserDetails", FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(UserData));
                    ObjUserData = (UserData)serializer.ReadObject(fileStream);

                }
                StckUserDetailsUI.DataContext = ObjUserData;//Bind current login details to UI
            }
            GlobalClass.Ususario = TxtUserName.Text;

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var Settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Settings.Values.Remove("CheckLogin");
            Frame.Navigate(typeof(MainPage));

        }

        private void TxtPhoneNo_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(NuevoArticulo));
        }

        private void Tecnologia_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(Articulos), "Tecnologia");

        }
    }
}
