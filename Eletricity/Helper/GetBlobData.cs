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
    internal class GetBlobData
    {

        public static async Task<string> GetBlobFile(string name)
        {
            try
            {

                var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=saeletricity;AccountKey=xxxxxxxxxxxx;EndpointSuffix=core.windows.net";
                var blobStorageContainerName = "eletricityfiles";
                var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
                BlobClient blob = container.GetBlobClient($"{name}.json");

                using var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);
                stream.Position = 0;

                using var reader = new StreamReader(stream);
                string blobData = await reader.ReadToEndAsync();
                return blobData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            
        }
    }
}
