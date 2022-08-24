using System;
using System.Text;
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

        private readonly ELOverblikSettings _eLOverblikSettings;
        private readonly ElOverblikToken _elOverblikToken;
        private readonly Prices _prices;
        private readonly Metering _metering;
        private readonly Spotprices _spotPrices;
        private readonly Tariff _tariff;


        public GetEloverblik(IOptions<ELOverblikSettings> eloverblikAccess, ElOverblikToken eloverblikToken, Prices prices, Metering metering, Spotprices spotPrices, Tariff tariff)
        {

            _eLOverblikSettings = eloverblikAccess?.Value ?? throw new ArgumentNullException(nameof(eloverblikAccess));
            _elOverblikToken = eloverblikToken;
            _prices = prices;
            _metering = metering;
            _spotPrices = spotPrices;
            _tariff = tariff;
        }

        [FunctionName("GetEloverblik")]

#if RELEASE
               
         //public  async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif
        {

            try
            {

                //Get token access
                string token = await _elOverblikToken.GetToken();

                //general api settings
                StringBuilder builder = new StringBuilder(@"{
                         ""meteringPoints"": {
                                     ""meteringPoint"": [
                                               ""xxxx""
                                                         ]
                                            }
                             }");

                string body =  builder.Replace("xxxx", _eLOverblikSettings.MeteringKey).ToString();
               
             
                string contentType = "application/json";
                                              
                await _prices.GetPrices(body, contentType, token);

                string incrementalDate = _metering.GetIncrementalDate();
                await _metering.GetMetering(body, contentType, token, incrementalDate);
            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
            }

            return new OkObjectResult("Done");
        }






        [FunctionName("DataToSQL")]

#if RELEASE
               
         //public  async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        public  async Task<IActionResult> RunSql(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public async Task<IActionResult> RunSql(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif
        {
            try
            {
                await _prices.InsertPrice();
                await _metering.InsertMetering();
            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
            }

            return new OkObjectResult("Done");
        }



        [FunctionName("GetSpotPrices")]

#if RELEASE
               
         //public  async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
      public  Task<string> Spot(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
#else
        public Task<string> Spot(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)

#endif
        {
            try
            {
                var date = _spotPrices.GetIncrementalDate();
                _spotPrices.getSpotPrice(date);

                //Get tariffs
                _tariff.getTariffs();
            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
            }

            return Task.FromResult("done");
        }


    }
}
