using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.IO;
using System.Threading.Tasks;

namespace Api.Service
{
    public class AzureStorageService : IFileStorageService
    {
        private IConfiguration _config;
        public AzureStorageService(IConfiguration config)
        {
            this._config = config;
        }
        public async Task Upload(Stream stream, string blobName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._config.GetConnectionString("azureBlogConnection"));


            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("tailoryfycontainer");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.UploadFromStreamAsync(stream);
        }
    }
}
