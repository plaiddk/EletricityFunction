using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Eletricity.Helper;
using Azure.Storage.Blobs;
using System;
using Microsoft.Extensions.Logging;

namespace Eletricity
{
    public  class Prices
    {
        private readonly UploadBlob _uploadBlob;
        private readonly GetBlobData _getBlob;
        private readonly InsertData _insertData;
        private readonly ILogger<Prices> _logger;

        public Prices(UploadBlob uploadBlob, GetBlobData getBlob, InsertData insertData, ILogger<Prices> logger)
        {
            _uploadBlob = uploadBlob;
            _getBlob = getBlob;
            _insertData = insertData;
            _logger = logger;
         
        }
        public  async Task GetPrices(string body,string contentType,string token)
        {
            try
            { 
                //Get prices
                string priceurl = "https://api.eloverblik.dk/CustomerApi/api/MeteringPoints/MeteringPoint/GetCharges";
                string resultData = await HttpAuthenticator.Auth(contentType, token, priceurl, body);

                _uploadBlob.UploadBlobFile("Prices", resultData);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

        }

        public  async Task InsertPrice()
        {
            try
            {
                //ERROR HANDLING
                string jsonPrices = await _getBlob.GetBlobFile("Prices");
                var datasetPrices = ReadDataFromJsonHelper.ReadDataFromJson(jsonPrices);
                _insertData.InsertDataSQL(datasetPrices, "insertprices");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
           
        }
    }
}
