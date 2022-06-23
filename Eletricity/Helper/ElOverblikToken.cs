using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity
{
    internal class ElOverblikToken
    {
        public static async Task<string> GetToken()
        {
            try
            {
                //Get Token            
                string Token_url = "https://api.eloverblik.dk/CustomerApi/api/Token";
                string Refresh_token = "xxxxxxxxxxxxxxxxxxxx";

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Refresh_token);
                HttpResponseMessage res = await client.GetAsync(Token_url);
                res.EnsureSuccessStatusCode();
                var bodycontent = await res.Content.ReadAsStringAsync();

                JObject tmp = JObject.Parse(bodycontent);
                string token = tmp["result"].ToString();
                return token;
            }
            catch (Exception ex)
            {
                return null;
                //log.LogInformation(ex.Message);
            }

           
        }
    }
}
