using Eletricity.Helper;
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
    internal class Spotprices
    {
        public static void getSpotPrice(string date)
        {
            try
            {
                //Get hourly prices
                string url = $"https://api.energidataservice.dk/datastore_search_sql?sql=SELECT * from \"elspotprices\" where \"PriceArea\"='DK1' and \"HourDK\">'{date}'";
                HttpClient hourprice = new HttpClient();
                using (HttpResponseMessage response = hourprice.GetAsync(url).Result)
                using (HttpContent content = response.Content)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string json = content.ReadAsStringAsync().Result;

                        UploadBlob.UploadBlobFile("HourlySpotPrices", json);

                        JObject tmp = JObject.Parse(json);
                        string jsonhourly = tmp["result"].ToString();

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

                //log.LogInformation(ex.Message);
            }
        }

        public static string GetIncrementalDate()

        {
            try
            {
                var delete = SqlExecuteHelper.Execute("delete from [dm].[SpotpricesHistory] where SpotPriceDKK is null");

                CultureInfo provider = CultureInfo.InvariantCulture;
                string query = "Select max([HourUTC]) as MaxDate from [dm].[SpotpricesHistory] where SpotPriceDKK is not null";
                string incrementalDate = null;

                var conn = SqlConnecter.SqlConn();
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
                return null;
                //log.LogInformation(ex.Message);
            }

           

        }
        
        public static void InsertSpotPrice(DataTable dt)
        {
            try
            {
                var conn = SqlConnecter.SqlConn();
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

                //log.LogInformation(ex.Message);
            }
        }
    }
}
