using Eletricity.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eletricity.Data
{
    internal class Metering
    {

        public static async Task GetMetering(string body, string contentType, string token, string incrementalDate)
        {
            try
            {
                //Get Metering data
                string meteringurl = "https://api.eloverblik.dk/CustomerApi/api/MeterData/GetTimeSeries";
                string fromdate = incrementalDate; //DateTime.Now.AddDays(-730).ToString("yyyy-MM-dd");
                string todate = null;

                if (DateTime.Parse(incrementalDate).AddDays(700) > DateTime.Now)
                {
                    todate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    todate = DateTime.Parse(incrementalDate).AddDays(700).ToString("yyyy-MM-dd");
                }
                string Aggregation = "Hour";
                string apiurl = meteringurl + "/" + fromdate + "/" + todate + "/" + Aggregation;

                string resultData = await HttpAuthenticator.Auth(contentType, token, apiurl, body);

                UploadBlob.UploadBlobFile("Metering", resultData);
            }
            catch (Exception ex)
            {

               // log.LogInformation(ex.Message);
            }

        }

        public static async Task InsertMetering()
        {
            try
            {
                string jsonMetering = await GetBlobData.GetBlobFile("Metering");
                var datasetMetering = ReadDataFromJsonHelper.ReadDataFromJson(jsonMetering);

                if (datasetMetering.Tables.Count < 4)
                {
                    //Do nothing
                }
                else
                {
                    InsertData.InsertDataSQL(datasetMetering, "insertmetering");
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
                CultureInfo provider = CultureInfo.InvariantCulture;
                string query = "Select max(DataEnd) as MaxDate from [dm].[HourDataHistory]";
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
                                incrementalDate = (DateTime.Now).AddYears(-5).ToString("yyyy-MM-dd", provider);
                            }
                            else
                            {
                                DateTime date = (DateTime)reader["MaxDate"];
                                incrementalDate = date.ToString("yyyy-MM-dd", provider);
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
                // log.LogInformation(ex.Message);
            }

           

        }
    }
}
