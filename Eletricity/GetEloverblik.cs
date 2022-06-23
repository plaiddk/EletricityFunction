using System;
using System.Threading.Tasks;
using Eletricity.Configuration;
using Eletricity.Data;
using Eletricity.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eletricity
{
    
    public class GetEloverblik
    {

        private readonly ConnectionSettings _connectionSettings;
        private static ELoverblikAccess _eloverblikAccess;
  

        public GetEloverblik(IOptions<ConnectionSettings> connectionStrings, IOptions<ELoverblikAccess> eloverblikAccess)
        {
          
            _connectionSettings = connectionStrings?.Value ?? throw new ArgumentNullException(nameof(connectionStrings));
            _eloverblikAccess = eloverblikAccess?.Value ?? throw new ArgumentNullException(nameof(eloverblikAccess));
         
        }

        [FunctionName("GetEloverblik")]

#if RELEASE
               
         //public static async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif
        {            
            var d = _eloverblikAccess.MeteringToken;
            var s = _connectionSettings.SQLPassword;
           
            try
            {
                ///HER ER MIN FEJL : JEG G�TTER P� NOGET MED ET INTERFACE SKAL LAVES?
                ///DEN BROKKER SIG OVER STATIC KLASSER - MEN JEG FORST�R MIG IKKE HELT P� DEPENDENCY INJECTION JEG HAR LAVET �VERST
                ///VILLE TROR DEN SKULLE ARVE EN VOID FRA ET INTERFACE? ELLER M�SKE DER ER EN SMARTERE M�DE
                //Get token access
                string token = await ElOverblikToken.GetToken();  

                //general api settings
                string body = @"{
                         ""meteringPoints"": {
                                     ""meteringPoint"": [
                                               ""X""
                                                         ]
                                            }
                             }";
                body.Replace("X", _eloverblikAccess.MeteringKey);
                string contentType = "application/json";

              //  await Prices.GetPrices(body, contentType, token);

                string incrementalDate = Metering.GetIncrementalDate();
                //await Metering.GetMetering(body, contentType, token, incrementalDate);
            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
            }

            return new OkObjectResult("Done");
        }

        [FunctionName("DataToSQL")]

#if RELEASE
               
         //public static async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        public static async Task<IActionResult> RunSql(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public static async Task<IActionResult> RunSql(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif
        {
            try
            {
                await Prices.InsertPrice();
                await Metering.InsertMetering();
            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
            }

            return new OkObjectResult("Done");
        }



        [FunctionName("GetSpotPrices")]

#if RELEASE
               
         //public static async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
      public static Task<string> Spot(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public static Task<string> Spot(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif
        {
            try
            {
                var date = Spotprices.GetIncrementalDate();
                Spotprices.getSpotPrice(date);
            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
            }

            return Task.FromResult("done"); 
        }

       
    }
}
