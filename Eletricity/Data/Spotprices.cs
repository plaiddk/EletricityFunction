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
    public class Spotprices
    {
        private readonly UploadBlob _uploadBlob;
        private readonly SqlConnecter _sqlConn;      
        private readonly SqlExecuteHelper _sqlHelper;
        private readonly ILogger<Spotprices> _logger;

        public Spotprices(UploadBlob uploadBlob, SqlConnecter sqlConn, SqlExecuteHelper sqlHelper, ILogger<Spotprices> logger)
        {
            _uploadBlob = uploadBlob;
            _sqlConn = sqlConn;
            _sqlHelper = sqlHelper;
            _logger = logger;
        }

        public  void getSpotPrice(string date)
        {
            try
            {
                //Get hourly prices
                //OLD API string url = $"https://api.energidataservice.dk/datastore_search_sql?sql=SELECT * from \"elspotprices\" where \"PriceArea\"='DK1' and \"HourDK\">'{date}'";
                string url = $"https://api.energidataservice.dk/dataset/Elspotprices?start={date}&filter=%7B%22PriceArea%22:%22DK1%22%7D";
                HttpClient hourprice = new HttpClient();
                using (HttpResponseMessage response = hourprice.GetAsync(url).Result)
                using (HttpContent content = response.Content)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string json = content.ReadAsStringAsync().Result;

                        _uploadBlob.UploadBlobFile("HourlySpotPrices", json);

                        JObject tmp = JObject.Parse(json);
                        string jsonhourly = tmp["records"].ToString();

                        var settings = new JsonSerializerSettings
                        {
                            Converters = new[] { new RecordDataTableConverter() },
                        };

                        var table = JsonConvert.DeserializeObject<DataTable>(jsonhourly, settings);

                        InsertSpotPrice(table);
                    }
                }
            }
            catch (Exception ex)
            {

               _logger.LogError(ex.Message);
            }
        }

        public  string GetIncrementalDate()

        {
            try
            {
                //LOGIC WORKS FOR NOW - COME BACK LATER
                var delete = _sqlHelper.Execute("delete from [dm].[SpotpricesHistory] where SpotPriceDKK is null");

                CultureInfo provider = CultureInfo.InvariantCulture;
                string query = "Select max([HourUTC]) as MaxDate from [dm].[SpotpricesHistory] where SpotPriceDKK is not null";
                string incrementalDate = null;

                var conn = _sqlConn.SqlConn();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull("MaxDate"))
                            {
                                incrementalDate = (DateTime.Now).AddYears(-5).ToString("yyyy-MM-dd HH:mm:ss", provider);
                            }
                            else
                            {
                                DateTime date = (DateTime)reader["MaxDate"];
                                incrementalDate = date.ToString("yyyy-MM-dd HH:mm:ss", provider);
                            }

                        }
                    }
                    conn.Close();

                }
                return incrementalDate;
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex.Message);
                return null;
                
            }

           

        }
        
        public  void InsertSpotPrice(DataTable dt)
        {
            try
            {
                var conn = _sqlConn.SqlConn();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {

                    bulk.DestinationTableName = "dbo.SpotPrices";
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
