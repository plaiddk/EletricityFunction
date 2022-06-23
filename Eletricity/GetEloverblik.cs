using System;
using System.Threading.Tasks;
using Eletricity.Data;
using Eletricity.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Eletricity
{
    public class GetEloverblik
    {
        [FunctionName("GetEloverblik")]

#if RELEASE
               
         //public static async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif

        {
            try
            {

                //Get token access
                string token = await ElOverblikToken.GetToken();

                //general api settings
                string body = @"{
                         ""meteringPoints"": {
                                     ""meteringPoint"": [
                                               ""XXXXXXXXXXXXXXXXX""
                                                         ]
                                            }
                             }";

                string contentType = "application/json";

                await Prices.GetPrices(body, contentType, token);

                string incrementalDate = Metering.GetIncrementalDate();
                await Metering.GetMetering(body, contentType, token, incrementalDate);
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
