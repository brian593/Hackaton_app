using Intercambio_App.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Intercambio : Page
    {
        List<Model.Articulos> ArticulosItems = new List<Model.Articulos>();

        public Intercambio()
        {
            this.InitializeComponent();
            CargarAzure();
        }

        private void Regresar(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.Principal));

        }
        public async void CargarAzure()
        {

            await App.MobileService.GetTable<Model.Articulos>().Where(citem => (citem.usuario == GlobalClass.Ususario)).ToListAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    System.Diagnostics.Debug.WriteLine("Faulted");
                    //  Dispatcher.BeginInvoke(() => MessageBox.Show("Error de conexion !\n"));
                }
                else
                {
                    ArticulosItems = t.Result;

                }
            });
            ListaArticulos.ItemsSource = ArticulosItems;

        }
    }
}
