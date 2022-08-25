using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Eletricity
{
    public static class IsAlive
    {
        [FunctionName("IsAlive")]
        public static  ActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = $"{DateTime.Now}: ELOVERBLIK API ----- ITS ALIVE -----";

            return new OkObjectResult(responseMessage);
        }
    }
}
