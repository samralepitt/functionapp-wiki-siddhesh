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

namespace functionapp_wiki_siddhesh.airportFunctions
{
    public static class PutAirport
    {
        [FunctionName("Putairport")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "airports/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request!!!");


            var db = new sqldatabasewikisiddheshContext();

            log.LogInformation($"{id}");
            var existingairport = await db.Airports.FindAsync(int.Parse(id));

            if (existingairport == null)
            {
                return new OkObjectResult(existingairport);
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedairport = JsonConvert.DeserializeObject<Airport>(requestBody);

            db.Entry(existingairport).CurrentValues.SetValues(updatedairport);
            await db.SaveChangesAsync();
            string responseMessage = string.IsNullOrEmpty(existingairport.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{existingairport}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(existingairport);
        }
    }
}
