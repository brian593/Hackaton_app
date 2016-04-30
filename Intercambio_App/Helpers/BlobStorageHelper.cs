using System;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Intercambio_App.Helpers
{
    public class BlobStorageHelper
    {

        private readonly CloudStorageAccount _storageAccount;
        private string _accountName = "elgaragestorage";
        string _key = "yNmIpQh1m7n+TKUvwZ+0g/AN948+LFZo6IFzOtI5L2q+QrXGnhfJGigPzK9IM0C5SanLBqFRmVticLiEuoVfBQ==";


        public BlobStorageHelper()
        {
            StorageCredentials storageCredentials = new StorageCredentials(_accountName, _key);
            _storageAccount = new CloudStorageAccount(storageCredentials, true);
        }

        public async Task UploadImageAsync(string containerName, string fileName, StorageFile file)
        {
            CloudBlobClient cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);


            await cloudBlockBlob.UploadFromFileAsync(file);
        }

    }
}
