using Azure.Storage.Blobs;
using Eletricity.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Eletricity.Helper
{
    public class UploadBlob
    {
        private readonly BlobStorageSettings _BlobSettings;
        private readonly ILogger<UploadBlob> _logger;


        public UploadBlob(IOptions<BlobStorageSettings> blobSettings, ILogger<UploadBlob> logger)
        {

            _BlobSettings = blobSettings?.Value ?? throw new ArgumentNullException(nameof(blobSettings));
            _logger = logger;
          
        }

        public  async void UploadBlobFile(string name,string jsonData)
        {
            
            var blobStorageConnectionString = $"DefaultEndpointsProtocol=https;AccountName={_BlobSettings.StorageName};AccountKey={_BlobSettings.StorageKey};EndpointSuffix=core.windows.net";
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

                _logger.LogError(ex.Message);
            }


        }
    }
}
