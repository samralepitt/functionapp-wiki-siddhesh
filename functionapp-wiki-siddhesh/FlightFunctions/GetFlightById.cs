using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using functionapp_wiki_siddhesh.Models;

namespace functionapp_wiki_siddhesh.FlightFunctions
{
    public static class GetFlightById
    {
        [FunctionName("GetFlightById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "flights/{id}")] HttpRequest req,
            ILogger log, String id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var db = new sqldatabasewikisiddheshContext();
            var existingflight = await db.Flights.FindAsync(int.Parse(id));
            if (existingflight == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(existingflight);
        }
    }
}
