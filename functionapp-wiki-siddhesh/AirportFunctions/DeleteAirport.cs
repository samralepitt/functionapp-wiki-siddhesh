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

namespace functionapp_wiki_siddhesh.airportFunctions
{
    public static class DeleteAirport
    {
        [FunctionName("Deleteairport")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "airports/{id}")] HttpRequest req,
            ILogger log, string id)
        {

            var db = new sqldatabasewikisiddheshContext();
            var existingairport = await db.Airports.FindAsync(int.Parse(id));
            if (existingairport == null)
            {
                return new NotFoundResult();
            }
            try
            {
                db.Airports.Remove(existingairport);
                await db.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                log.LogError($"Error deleting entity ");
                return new StatusCodeResult(500);
            }
 
            return new OkObjectResult(existingairport);
        }
    }
}
