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

namespace functionapp_wiki_siddhesh.flightFunctions
{
    public static class AddFlight
    {
        [FunctionName("Addflight")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "flights")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var db = new sqldatabasewikisiddheshContext();
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            var flight = JsonConvert.DeserializeObject<Flight>(requestbody);
            db.Flights.Add(flight);
            await db.SaveChangesAsync();
            string responseMessage = string.IsNullOrEmpty(flight.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{flight}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(flight);
        }
    }
}
