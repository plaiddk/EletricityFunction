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
using System.Threading.Tasks;

namespace Eletricity.Data
{
    public class Metering
    {
        private readonly UploadBlob _uploadBlob;
        private readonly GetBlobData _getBlob;
        private readonly InsertData _insertData;
        private readonly SqlConnecter _sqlConnecter;
        private readonly ILogger<Metering> _logger;


        public Metering(UploadBlob uploadBlob, GetBlobData getBlob, InsertData insertData, SqlConnecter sqlConnecter, ILogger<Metering> logger)
        {
            _uploadBlob = uploadBlob;
            _getBlob = getBlob;
            _insertData = insertData;
            _sqlConnecter = sqlConnecter;
            _logger = logger;
        }

        public  async Task GetMetering(string body, string contentType, string token, string incrementalDate)
        {
            try
            {
                //Get Metering data
                string meteringurl = "https://api.eloverblik.dk/CustomerApi/api/MeterData/GetTimeSeries";
                string fromdate = incrementalDate; //DateTime.Now.AddDays(-730).ToString("yyyy-MM-dd");
                string todate = null;

                //CANNOT EXCEED 730 DAYS - 700 IS FINE FOR NOW
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

                _uploadBlob.UploadBlobFile("Metering", resultData);
            }
            catch (Exception ex)
            {

               _logger.LogInformation(ex.Message);
            }

        }

        public  async Task InsertMetering()
        {
            try
            {
                string jsonMetering = await _getBlob.GetBlobFile("Metering");
                var datasetMetering = ReadDataFromJsonHelper.ReadDataFromJson(jsonMetering);

                if (datasetMetering.Tables.Count < 4)
                {
                    //Do nothing
                }
                else
                {
                    _insertData.InsertDataSQL(datasetMetering, "insertmetering");
                }
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.Message);
            }

        }

        public  string GetIncrementalDate()

        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                string query = "Select max(DataEnd) as MaxDate from [dm].[HourDataHistory]";
                string incrementalDate = null;

                var conn = _sqlConnecter.SqlConn();
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
                _logger.LogInformation(ex.Message);
                return null;
               
            }

           

        }
    }
}
