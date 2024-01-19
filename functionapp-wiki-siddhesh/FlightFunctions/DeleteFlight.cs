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
using Microsoft.EntityFrameworkCore;

namespace functionapp_wiki_siddhesh.flightFunctions
{
    public static class DeleteFlight
    {
        [FunctionName("Deleteflight")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "flights/{id}")] HttpRequest req,
            ILogger log, string id)
        {

            var db = new sqldatabasewikisiddheshContext();
            var existingflight = await db.Flights.FindAsync(int.Parse(id));
            if (existingflight == null)
            {
                return new NotFoundResult();
            }

            db.Flights.Remove(existingflight);
            await db.SaveChangesAsync();
            string responseMessage = string.IsNullOrEmpty(existingflight.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{existingflight}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(existingflight);
        }
    }
}
