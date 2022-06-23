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
                string Refresh_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0b2tlblR5cGUiOiJDdXN0b21lckFQSV9SZWZyZXNoIiwidG9rZW5pZCI6ImM4NDkwMWZjLWVlN2YtNGExOS05ZGRjLWQwOTFhY2NiMzVhYiIsIndlYkFwcCI6WyJDdXN0b21lckFwaSIsIkN1c3RvbWVyQXBwIl0sImp0aSI6ImM4NDkwMWZjLWVlN2YtNGExOS05ZGRjLWQwOTFhY2NiMzVhYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiUElEOjkyMDgtMjAwMi0yLTY1MTU5Mzg3NjUyOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2dpdmVubmFtZSI6IlRob21hcyBSeXR0ZXIgSmVuc2VuIiwibG9naW5UeXBlIjoiS2V5Q2FyZCIsInBpZCI6IjkyMDgtMjAwMi0yLTY1MTU5Mzg3NjUyOCIsInR5cCI6IlBPQ0VTIiwidXNlcklkIjoiNjM0NzgiLCJleHAiOjE2NzA0MDYwMzEsImlzcyI6IkVuZXJnaW5ldCIsInRva2VuTmFtZSI6InBvd2VyYmkiLCJhdWQiOiJFbmVyZ2luZXQifQ.OHvORKKuDZsK6x8PEbi13bC1ppK2-gaK6fdSg4YJX44";

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
