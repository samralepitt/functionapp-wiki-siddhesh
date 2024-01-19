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
    public static class AddGate
    {
        [FunctionName("AddGate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "gates")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var db = new sqldatabasewikisiddheshContext();
            var requestbody = await new StreamReader(req.Body).ReadToEndAsync();
            var gate = JsonConvert.DeserializeObject<Gate>(requestbody);
            db.Gates.Add(gate);
            await db.SaveChangesAsync();
            string responseMessage = string.IsNullOrEmpty(gate.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{gate}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(gate);
        }
    }
}
