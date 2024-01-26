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
using System.Linq;

namespace functionapp_wiki_siddhesh.AirportFunctions
{
    public static class AirportFunction
    {
        [FunctionName("AirportFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "delete", "put", Route = "airports/{id?}")] HttpRequest req,
            ILogger log, string id)
        {
            Guid airportId;
            try
            {
                var db = new sqldatabasewikisiddheshContext();
                if (req.Method == "GET")
                {
                    if (String.IsNullOrEmpty(id))               //get all records if no id specified in Path
                    {
                        var airports = db.Airports.ToList();
                        return new OkObjectResult(airports);
                    }
                    else
                    {
                        //Guid in Path Parameter
                        if (!Guid.TryParse(id, out airportId)) { return new BadRequestObjectResult("Invalid airportID in URL - Bad Parse"); }
                        var checkExist = db.Airports.Any(g => g.AirportId == airportId);
                        if (checkExist)                         // get record by id
                        {
                            if (req.Method == "GET")
                            {
                                var airport = db.Airports.Where(g => g.AirportId == airportId);
                                return new OkObjectResult(airport);
                            }
                        }
                    }
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Airport>(requestBody);
                //Guid in reqBody
                if (!Guid.TryParse(data.AirportId.ToString(), out airportId)) { return new BadRequestObjectResult("Invalid airportID in request"); }
                var doesExist = db.Airports.Any(g => g.AirportId == airportId);

                if (req.Method == "POST")
                {
                    if (doesExist)
                    {
                        return new BadRequestObjectResult($"Entry Exists for {airportId}");
                    }
                    db.Airports.Add(data);
                    await db.SaveChangesAsync();
                    return new OkObjectResult(data);
                }
                if (doesExist)
                {
                    if (req.Method == "DELETE")
                    {
                        var hasDependency = db.Flights.Any(m => m.PartnerAirport == airportId);
                        if (hasDependency)
                        {
                            return new BadRequestObjectResult($"Cannot delete {airportId} ID. Dependency exists.");
                        }
                        db.Airports.Remove(data);
                        await db.SaveChangesAsync();
                        return new OkObjectResult(data);
                    }
                    if (req.Method == "PUT")
                    {
                        var checkAirportExist = db.Airports.Any(m => (m.AirportId == data.AirportId) && (m.AirportName == data.AirportName));
                        if (checkAirportExist)
                        {
                            return new BadRequestObjectResult($"Duplicate Entry for {airportId}");
                        }
                        db.Airports.Update(data);
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