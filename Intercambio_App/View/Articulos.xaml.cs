using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class Articulos : Page
    {
        string CategoriaVar;
     //   List<Model.Articulos> ListaArticulito = new List<Model.Articulos>();
        List<Model.Articulos> ArticulosItems= new List<Model.Articulos>();

        public Articulos()
        {
            this.InitializeComponent();
            
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {

                    string Categor = String.Empty;
                    Categor = (string)e.Parameter;

                    //if (NavigationContext.QueryString.TryGetValue("videoId", out videoId))
                    //{
                    //    //Get The Video Uri and set it as a player source
                    //    var url = await YouTube.GetVideoUriAsync(videoId, YouTubeQuality.Quality480P);
                    //    player.Source = url.Uri;
                    //}

                    if (!string.IsNullOrWhiteSpace(Categor))
                    {
                        CategoriaVar = Categor;
                        CargarAzure();
                    }
                }
                else
                {
                    //  MessageBox.Show("You're not connected to Internet!");
                    // NavigationService.GoBack();
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            base.OnNavigatedTo(e);
        }


        public async void CargarAzure()
        {

            await App.MobileService.GetTable<Model.Articulos>().Where(citem => (citem.Categoria == CategoriaVar )).ToListAsync().ContinueWith(t =>
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

        private void Regresar(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.Principal));

        }

        private void Articulo_click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.Intercambio));
        }
    }
}
