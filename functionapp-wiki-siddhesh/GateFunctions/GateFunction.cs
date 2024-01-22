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
using System.Linq;

namespace functionapp_wiki_siddhesh.GateFunctions
{
    public static class GateFunction
    {
        [FunctionName("GateFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "delete", "put", Route = "gates/{id?}")] HttpRequest req,
            ILogger log, string id)
        {
            Guid gateId;
            try
            {


                var db = new sqldatabasewikisiddheshContext();
                if (req.Method == "GET")
                {
                    if (String.IsNullOrEmpty(id))               //get all records if no id specified in Path
                    {
                        var gates = db.Gates.ToList();
                        return new OkObjectResult(gates);
                    }
                    else
                    {
                        //Guid in Path Parameter
                        if (!Guid.TryParse(id, out gateId)) { return new BadRequestObjectResult("Invalid gateID in URL - Bad Parse"); }
                        var checkExist = db.Gates.Any(g => g.GateId == gateId);
                        if (checkExist)                         // get record by id
                        {
                            if (req.Method == "GET")
                            {
                                var gate = db.Gates.Where(g => g.GateId == gateId);
                                return new OkObjectResult(gate);
                            }
                        }
                    }
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Gate>(requestBody);
                //Guid in reqBody
                if (!Guid.TryParse(data.GateId.ToString(), out gateId)) { return new BadRequestObjectResult("Invalid gateID in request"); }
                var doesExist = db.Gates.Any(g => g.GateId == gateId);

                if (req.Method == "POST")
                {
                    if (doesExist)
                    {
                        return new BadRequestObjectResult($"Entry Exists for {gateId}");
                    }
                    db.Gates.Add(data);
                    await db.SaveChangesAsync();
                    return new OkObjectResult(data);
                }
                if (doesExist)
                {
                    if (req.Method == "DELETE")
                    {
                        var hasDependency = db.Flights.Any(m => m.Gate == gateId);
                        if (hasDependency)
                        {
                            return new BadRequestObjectResult($"Cannot delete {gateId} ID. Dependency exists.");
                        }
                        db.Gates.Remove(data);
                        await db.SaveChangesAsync();
                        return new OkObjectResult(data);
                    }
                    if (req.Method == "PUT")
                    {
                        var checkGateExist = db.Gates.Any(m => (m.GateId == data.GateId) && (m.GateName == data.GateName));
                        if (checkGateExist)
                        {
                            return new BadRequestObjectResult($"Duplicate Entry for {gateId}");
                        }
                        db.Gates.Update(data);
                        await db.SaveChangesAsync();
                        return new OkObjectResult(data);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"{ex}");
                return new BadRequestObjectResult("Bad Request");
            }
            return new BadRequestObjectResult("Could not find Entry or Bad Request");
        }
    }
}
