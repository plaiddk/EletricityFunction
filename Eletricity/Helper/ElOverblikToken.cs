using Eletricity.Configuration;
using Eletricity.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity
{
  

    public class ElOverblikToken
    {     
        private readonly ELOverblikSettings _eloverblikSettings;
        private readonly ILogger<ElOverblikToken> _logger;


        public  ElOverblikToken(IOptions<ELOverblikSettings> eloverblikAccess, ILogger<ElOverblikToken> logger)
        {

            _eloverblikSettings = eloverblikAccess?.Value ?? throw new ArgumentNullException(nameof(eloverblikAccess));
            _logger = logger;
        }


        public async Task<string>GetToken()
        {
          
                try
                {
                    //Get Token            
                    string Token_url = "https://api.eloverblik.dk/CustomerApi/api/Token";
                    string Refresh_token = _eloverblikSettings.MeteringToken;

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
                _logger.LogError(ex.Message);
                return null;
                    
                }


            

        }
    }
}
