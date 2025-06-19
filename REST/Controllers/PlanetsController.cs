using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;
using REST.Interfaces;

namespace REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanetsController : ControllerBase
    {
        private readonly IPlanetRepository repo;

        public PlanetsController(IPlanetRepository planetsRepo)
        {
            repo = planetsRepo;
        }


        [HttpGet("names")]
        public async Task<IActionResult> GetPlanetNames()
        {
            var planetNames = await repo.GetNames();

            if (planetNames == null || !planetNames.Any())
            {
                return NotFound("No planets found.");
            }

            return Ok(planetNames);
        }

    }
}
