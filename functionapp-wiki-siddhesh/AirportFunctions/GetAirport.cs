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

namespace functionapp_wiki_siddhesh.airportFunctions
{
    public static class GetAirport
    {
        [FunctionName("Getairports")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "airports")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var db = new sqldatabasewikisiddheshContext();
            var airports = await db.Airports.ToListAsync();

            string responseMessage = string.IsNullOrEmpty(airports.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{airports}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(airports);
        }
    }
}
