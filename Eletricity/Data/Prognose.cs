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
    public class Prognose
    {
        private readonly UploadBlob _uploadBlob;
        private readonly SqlConnecter _sqlConn;      
        private readonly SqlExecuteHelper _sqlHelper;
        private readonly ILogger<Spotprices> _logger;

        public Prognose(UploadBlob uploadBlob, SqlConnecter sqlConn, SqlExecuteHelper sqlHelper, ILogger<Spotprices> logger)
        {
            _uploadBlob = uploadBlob;
            _sqlConn = sqlConn;
            _sqlHelper = sqlHelper;
            _logger = logger;
        }

        public  void getPrognose()
        {
            try
            {
                //Get Prognose data from Henrik moller
                string url = $"https://raw.githubusercontent.com/solmoller/Spotprisprognose/main/prognose.json";
                HttpClient hourprice = new HttpClient();
                using (HttpResponseMessage response = hourprice.GetAsync(url).Result)
                using (HttpContent content = response.Content)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string json = content.ReadAsStringAsync().Result;

                         _uploadBlob.UploadBlobFile("prognose", json);

                        JObject tmp = JObject.Parse(json);
                        string jsonhourly = tmp["DK1"].ToString();

                        var settings = new JsonSerializerSettings
                        {
                            Converters = new[] { new RecordDataTableConverter() },
                        };

                        var table = JsonConvert.DeserializeObject<DataTable>(jsonhourly, settings);

                        InsertPrognose(table);
                    }
                }
            }
            catch (Exception ex)
            {

               _logger.LogError(ex.Message);
            }
        }

      
        
        public  void InsertPrognose(DataTable dt)
        {
            try
            {
                var conn = _sqlConn.SqlConn();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {

                    bulk.DestinationTableName = "dbo.PrognoseSpotprice";
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
