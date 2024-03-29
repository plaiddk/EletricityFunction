﻿using Azure.Storage.Blobs;
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
    public class GetBlobData
    {
        private readonly BlobStorageSettings _BlobSettings;
        private readonly ILogger<GetBlobData> _logger;



        public GetBlobData(IOptions<BlobStorageSettings> blobSettings, ILogger<GetBlobData> logger)
        {

            _BlobSettings = blobSettings?.Value ?? throw new ArgumentNullException(nameof(blobSettings));
            _logger = logger;
        }

        public  async Task<string> GetBlobFile(string name)
        {
            try
            {

                var blobStorageConnectionString = $"DefaultEndpointsProtocol=https;AccountName={_BlobSettings.StorageName};;AccountKey={_BlobSettings.StorageKey};EndpointSuffix=core.windows.net";
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
                _logger.LogError(ex.Message);
                return string.Empty;
            }

            
        }
    }
}
