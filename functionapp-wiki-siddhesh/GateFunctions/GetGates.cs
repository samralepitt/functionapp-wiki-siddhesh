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

namespace functionapp_wiki_siddhesh.GateFunctions
{
    public static class GetGate
    {
        [FunctionName("GetGates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "gates")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Cet Gates Triggered");
            var db = new sqldatabasewikisiddheshContext();
            var gates = await db.Gates.ToListAsync();

            return new OkObjectResult(gates);
        }
    }
}
