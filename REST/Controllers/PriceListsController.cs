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
    public class PriceListsController : ControllerBase
    {
        private readonly IPriceListRepository repo;

        public PriceListsController(IPriceListRepository priceListsRepo)
        {
            repo = priceListsRepo;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<PriceList>> GetLatestPriceList()
        {
            var priceList = await repo.GetLatest();

            if (priceList == null)
            {
                return NotFound();
            }

            return Ok(priceList);
        }
    }
}
