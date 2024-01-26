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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace functionapp_wiki_siddhesh.FlightFunctions
{
    public static class FlightFunction
    {
        [FunctionName("FlightFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "delete", "put", Route = "flights/{id?}")] HttpRequest req,
            ILogger log, string id)
        {
            Guid flightId;
            try
            {
                var db = new sqldatabasewikisiddheshContext();
                if (req.Method == "GET")
                {
                    if (String.IsNullOrEmpty(id))               //get all records if no id specified in Path
                    {
                        var flights = db.Flights.Include(x => x.Gate).Include(x => x.PartnerAirportNavigation).ToList();
                        return new OkObjectResult(flights);
                    }
                    else
                    {
                        //Guid in Path Parameter
                        if (!Guid.TryParse(id, out flightId)) { return new BadRequestObjectResult("Invalid flightID in URL - Bad Parse"); }
                        var checkExist = db.Flights.Any(g => g.FlightId == flightId);
                        if (checkExist)                         // get record by id
                        {
                            if (req.Method == "GET")
                            {
                                var flight = db.Flights.Where(g => g.FlightId == flightId);
                                return new OkObjectResult(flight);
                            }
                        }
                    }
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Flight>(requestBody);
                //Guid in reqBody
                if (!Guid.TryParse(data.FlightId.ToString(), out flightId)) { return new BadRequestObjectResult("Invalid flightID in request"); }
                var doesExist = db.Flights.Any(g => g.FlightId == flightId);

                if (req.Method == "POST")
                {
                    if (doesExist)
                    {
                        return new BadRequestObjectResult($"Entry Exists for {flightId}");
                    }
                    db.Flights.Add(data);
                    await db.SaveChangesAsync();
                    return new OkObjectResult(data);
                }
                if (doesExist)
                {
                    if (req.Method == "DELETE")
                    {

                        db.Flights.Remove(data);
                        await db.SaveChangesAsync();
                        return new OkObjectResult(data);
                    }
                    if (req.Method == "PUT")
                    {
                        var checkFlightExist = db.Flights.Any(m => (m.FlightId == data.FlightId) && (m.FlightNumber == data.FlightNumber));
                        if (checkFlightExist)
                        {
                            return new BadRequestObjectResult($"Duplicate Entry for {flightId}");
                        }
                        db.Flights.Update(data);
                        await db.SaveChangesAsync();
                        return new OkObjectResult(data);
                    }
                }
            }
            catch(Exception ex)
            {
                log.LogError($"{ex}");
                return new BadRequestObjectResult("Bad Request");
            }
            return new BadRequestObjectResult("Could not find Entry or Bad Request");
        }
    }
}
