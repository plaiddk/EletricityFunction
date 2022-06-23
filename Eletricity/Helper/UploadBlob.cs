using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Eletricity.Helper
{
    internal class UploadBlob
    {

        public static async void UploadBlobFile(string name,string jsonData)
        {
            var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=saeletricity;AccountKey=xxxxxxxxxx;EndpointSuffix=core.windows.net";
            var blobStorageContainerName = "eletricityfiles";
            var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
            var blob = container.GetBlobClient($"{name}.json");

            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
                {
                    await blob.UploadAsync(ms, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {

                //log.LogInformation(ex.Message);
            }


        }
    }
}
