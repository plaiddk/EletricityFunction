using Eletricity.Helper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Http;
using static Eletricity.Helper.RecordDataTableConvert;

namespace Eletricity.Data
{
    public class Tariff
    {
        private readonly UploadBlob _uploadBlob;
        private readonly SqlConnecter _sqlConn;      
        private readonly SqlExecuteHelper _sqlHelper;
        private readonly ILogger<Spotprices> _logger;

        public Tariff(UploadBlob uploadBlob, SqlConnecter sqlConn, SqlExecuteHelper sqlHelper, ILogger<Spotprices> logger)
        {
            _uploadBlob = uploadBlob;
            _sqlConn = sqlConn;
            _sqlHelper = sqlHelper;
            _logger = logger;
        }

        public  void getTariffs()
        {
            try
            {
              
                //Get hourly prices
                string filter = @"{ ""GLN_Number"":""5790001100520""}";
                string url = @$"https://api.energidataservice.dk/dataset/DatahubPricelist?start=2022-01-01T00:00&end=2022-12-31T00:00&filter={filter}";
               
                HttpClient hourprice = new HttpClient();
                using (HttpResponseMessage response = hourprice.GetAsync(url).Result)
                using (HttpContent content = response.Content)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string json = content.ReadAsStringAsync().Result;

                         _uploadBlob.UploadBlobFile("tariffs", json);

                        JObject tmp = JObject.Parse(json);
                        string jsonhourly = tmp["records"].ToString();

                        var settings = new JsonSerializerSettings
                        {
                            Converters = new[] { new RecordDataTableConverter() },
                        };

                        var table = JsonConvert.DeserializeObject<DataTable>(jsonhourly, settings);

                        InsertTariffs(table);
                    }
                }
            }
            catch (Exception ex)
            {

               _logger.LogError(ex.Message);
            }
        }

      
        
        public  void InsertTariffs(DataTable dt)
        {
            try
            {
                var conn = _sqlConn.SqlConn();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {

                    bulk.DestinationTableName = "dbo.DatahubPricelist";
                    conn.Open();

                    bulk.WriteToServer(dt);
                    conn.Close();

                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
        }
    }
}
