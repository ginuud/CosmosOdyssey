using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;

namespace REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceListsController : ControllerBase
    {
        private readonly DataContext _context;

        public PriceListsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PriceLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceList>>> GetPriceLists()
        {
            return await _context.PriceLists.ToListAsync();
        }

        // GET: api/PriceLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceList>> GetPriceList(Guid id)
        {
            var priceList = await _context.PriceLists.FindAsync(id);

            if (priceList == null)
            {
                return NotFound();
            }

            return priceList;
        }

        // PUT: api/PriceLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriceList(Guid id, PriceList priceList)
        {
            if (id != priceList.Id)
            {
                return BadRequest();
            }

            _context.Entry(priceList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PriceLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PriceList>> PostPriceList(PriceList priceList)
        {
            _context.PriceLists.Add(priceList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PriceListExists(priceList.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPriceList", new { id = priceList.Id }, priceList);
        }

        // DELETE: api/PriceLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceList(Guid id)
        {
            var priceList = await _context.PriceLists.FindAsync(id);
            if (priceList == null)
            {
                return NotFound();
            }

            _context.PriceLists.Remove(priceList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PriceListExists(Guid id)
        {
            return _context.PriceLists.Any(e => e.Id == id);
        }
    }
}
