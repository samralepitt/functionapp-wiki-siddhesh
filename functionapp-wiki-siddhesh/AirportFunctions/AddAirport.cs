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

namespace functionapp_wiki_siddhesh.AirportFunctions
{
    public static class AddAirport
    {
        [FunctionName("AddAirport")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "airports")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var db = new sqldatabasewikisiddheshContext();
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            var airport = JsonConvert.DeserializeObject<Airport>(requestbody);
            db.Airports.Add(airport);
            await db.SaveChangesAsync();

            return new OkObjectResult(airport);
        }
    }
}
