using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Dtos;
using CosmosOdyssey.REST.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmosOdyssey.REST.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteFinderService _routeFinder;

        public RoutesController(IRouteFinderService routeFinder)
        {
            _routeFinder = routeFinder;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<RouteDto>>> SearchRoutes(
            [FromQuery] string fromPlanet,
            [FromQuery] string toPlanet)
        {
            try
            {
                if (string.IsNullOrEmpty(fromPlanet) || string.IsNullOrEmpty(toPlanet))
                    return BadRequest("Both fromPlanet and toPlanet are required.");

                var routes = await _routeFinder.FindAllRoutesAsync(fromPlanet, toPlanet);
                if (routes == null || !routes.Any())
                    return NotFound("No routes found for the given origin and destination.");
                return Ok(routes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}