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
    public class ProvidersController : ControllerBase
    {
        private readonly DataContext _context;

        public ProvidersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProviders()
        {
            return await _context.Providers.ToListAsync();
        }

        // GET: api/Providers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProviderById(Guid id)
        {
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                return NotFound();
            }

            return provider;
        }

        // // PUT: api/Providers/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProvider(Guid id, Provider provider)
        // {
        //     if (id != provider.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(provider).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ProviderExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Providers
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Provider>> PostProvider(Provider provider)
        // {
        //     _context.Providers.Add(provider);
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateException)
        //     {
        //         if (ProviderExists(provider.Id))
        //         {
        //             return Conflict();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return CreatedAtAction("GetProvider", new { id = provider.Id }, provider);
        // }

        // // DELETE: api/Providers/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteProvider(Guid id)
        // {
        //     var provider = await _context.Providers.FindAsync(id);
        //     if (provider == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Providers.Remove(provider);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ProviderExists(Guid id)
        {
            return _context.Providers.Any(e => e.Id == id);
        }
    }
}
