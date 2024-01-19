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

namespace functionapp_wiki_siddhesh.GateFunctions
{
    public static class GetGateById
    {
        [FunctionName("GetGateById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "gates/{id}")] HttpRequest req,
            ILogger log, String id)
        {
            log.LogInformation("Get Gates Triggered");
            var db = new sqldatabasewikisiddheshContext();
            var existingGate = await db.Gates.FindAsync(int.Parse(id));
            if (existingGate == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(existingGate);
        }
    }
}
