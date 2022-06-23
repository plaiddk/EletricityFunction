using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Eletricity.Helper;
using Azure.Storage.Blobs;
using System;

namespace Eletricity
{
    internal class Prices
    {
        public static async Task GetPrices(string body,string contentType,string token)
        {
            try
            {
                //Get prices
                string priceurl = "https://api.eloverblik.dk/CustomerApi/api/MeteringPoints/MeteringPoint/GetCharges";
                string resultData = await HttpAuthenticator.Auth(contentType, token, priceurl, body);

                UploadBlob.UploadBlobFile("Prices", resultData);
            }
            catch (Exception ex)
            {

               // log.LogInformation(ex.Message);
            }

        }

        public static async Task InsertPrice()
        {
            try
            {
                string jsonPrices = await GetBlobData.GetBlobFile("Prices");
                var datasetPrices = ReadDataFromJsonHelper.ReadDataFromJson(jsonPrices);
                InsertData.InsertDataSQL(datasetPrices, "insertprices");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
           
        }
    }
}
