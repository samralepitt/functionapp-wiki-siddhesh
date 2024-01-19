using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using functionapp_wiki_siddhesh.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace functionapp_wiki_siddhesh.flightFunctions
{
    public static class GetFlight
    {
        [FunctionName("Getflights")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "flights")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var db = new sqldatabasewikisiddheshContext();
            var flights = await db.Flights.ToListAsync();

            return new OkObjectResult(flights);
        }
    }
}
