using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Helper
{
    internal class HttpAuthenticator
    {
        public static async Task<string> Auth(string contentType,string token, string url, string body)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                string resultContent = await result.Content.ReadAsStringAsync();

                return resultContent;
            }
            catch (Exception ex)
            {
                return null;
                //log.LogInformation(ex.Message);
            }

           
        }
    }
}
