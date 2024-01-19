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
    public static class PutFlight
    {
        [FunctionName("Putflight")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "flights/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request!!!");


            var db = new sqldatabasewikisiddheshContext();

            log.LogInformation($"{id}");
            var existingflight = await db.Flights.FindAsync(int.Parse(id));

            if (existingflight == null)
            {
                return new OkObjectResult(existingflight);
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedflight = JsonConvert.DeserializeObject<Flight>(requestBody);

            db.Entry(existingflight).CurrentValues.SetValues(updatedflight);
            await db.SaveChangesAsync();
            string responseMessage = string.IsNullOrEmpty(existingflight.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{existingflight}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(existingflight);
        }
    }
}
