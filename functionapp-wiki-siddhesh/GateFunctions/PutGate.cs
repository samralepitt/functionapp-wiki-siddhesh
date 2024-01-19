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
    public static class PutGate
    {
        [FunctionName("PutGate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "gates/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Put Gate Triggered");


            var db = new sqldatabasewikisiddheshContext();
            var existingGate = await db.Gates.FindAsync(int.Parse(id));

            if (existingGate == null)
            {
                return new OkObjectResult(existingGate);
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedGate = JsonConvert.DeserializeObject<Gate>(requestBody);

            db.Entry(existingGate).CurrentValues.SetValues(updatedGate);
            await db.SaveChangesAsync();
            string responseMessage = string.IsNullOrEmpty(existingGate.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{existingGate}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(existingGate);
        }
    }
}
