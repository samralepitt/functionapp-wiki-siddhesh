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

namespace functionapp_wiki_siddhesh.GateFunctions
{
    public static class DeleteGate
    {
        [FunctionName("DeleteGate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "gates/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Delete Gate Triggered");

            var db = new sqldatabasewikisiddheshContext();
            var existingGate = await db.Gates.FindAsync(int.Parse(id));
            if (existingGate == null)
            {
                return new NotFoundResult();
            }
            //var deleteGate = await db.Gates.Include(p => p.Flights).FirstOrDefaultAsync(p => p.GateId == existingGate.GateId);
            try
            {
                var a = db.Gates.Remove(existingGate);
                await db.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                log.LogError($"Error deleting entity ");
                return new StatusCodeResult(500);
            }
            return new OkObjectResult(existingGate);
        }
    }
}
