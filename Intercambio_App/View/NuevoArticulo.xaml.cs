using Intercambio_App.Helpers;
using Intercambio_App.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Intercambio_App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NuevoArticulo : Page
    {
        private string Categoria;
    
        public NuevoArticulo()
        {
            this.InitializeComponent();
           }

        private StorageFile _storageFile;

        private async void btnTakeImage_OnClick(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI cameraCaptureUi = new CameraCaptureUI();

            cameraCaptureUi.PhotoSettings.AllowCropping = false;
            cameraCaptureUi.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.MediumXga;

            _storageFile =
               await cameraCaptureUi.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (_storageFile != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(await _storageFile.OpenReadAsync());
                CurrentImage.Source = bitmapImage;
            }

        }

        private async void btnUploadImage_OnClick(object sender, RoutedEventArgs e)
        {
            if (_storageFile != null)
            {
                if (!string.IsNullOrEmpty(Titulo.Text) && !string.IsNullOrEmpty("contenedor"))
                {
                    BlobStorageHelper blobStorageHelper = new BlobStorageHelper();

                    await blobStorageHelper.UploadImageAsync("contenedor", $"{Titulo.Text}.jpg", _storageFile);
                    //Ingreso a la base articulos 
                   Model.Articulos articulo = new Model.Articulos();

                    articulo.Categoria = Categoria;
                    articulo.Descripcion = Descripcion.Text;
                    articulo.Imagen = "https://elgaragestorage.blob.core.windows.net/contenedor/"+Titulo.Text+".jpg";
                    articulo.usuario = GlobalClass.Ususario;
                    articulo.Nombre = Titulo.Text;

                    await App.MobileService.GetTable<Model.Articulos>().InsertAsync(articulo);

                    MessageDialog messageDialog = new MessageDialog("La imagen se ha subido con éxito.");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Necesitas asignar un nombre a la imagen y al contenedor");
                    await messageDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Debes tener una imagen para subir al blob");
                await messageDialog.ShowAsync();
            }

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Categoria = ((RadioButton)sender).Content.ToString();

            
        }

        private void Regresar(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.Principal));
        }
    }
}
