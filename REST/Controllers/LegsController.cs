using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;

namespace REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegsController : ControllerBase
    {
        private readonly DataContext _context;

        public LegsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Legs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leg>>> GetLegs()
        {
            return await _context.Legs.ToListAsync();
        }

        // GET: api/Legs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leg>> GetLegById(Guid id)
        {
            var leg = await _context.Legs.FindAsync(id);

            if (leg == null)
            {
                return NotFound();
            }

            return leg;
        }


        // PUT: api/Legs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeg(Guid id, Leg leg)
        {
            if (id != leg.Id)
            {
                return BadRequest();
            }

            _context.Entry(leg).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LegExists(id))
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

        // POST: api/Legs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Leg>> PostLeg(Leg leg)
        {
            _context.Legs.Add(leg);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LegExists(leg.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLeg", new { id = leg.Id }, leg);
        }

        // DELETE: api/Legs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeg(Guid id)
        {
            var leg = await _context.Legs.FindAsync(id);
            if (leg == null)
            {
                return NotFound();
            }

            _context.Legs.Remove(leg);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LegExists(Guid id)
        {
            return _context.Legs.Any(e => e.Id == id);
        }
    }
}
