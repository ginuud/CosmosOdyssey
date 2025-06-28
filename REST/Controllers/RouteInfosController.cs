using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Dtos;
using REST.Mappers;
using CosmosOdyssey.REST.Interfaces;

namespace REST.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RouteInfosController : ControllerBase
    {
        private readonly DataContext _context;

        public RouteInfosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/RouteInfos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteInfo>>> GetRouteInfos()
        {
            return await _context.RouteInfos
         .Include(r => r.From)
         .Include(r => r.To)
         .ToListAsync();
        }


        // GET: api/RouteInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouteInfo>> GetRouteInfo(Guid id)
        {
            var routeInfo = await _context.RouteInfos.FindAsync(id);

            if (routeInfo == null)
            {
                return NotFound();
            }

            return routeInfo;
        }

        // PUT: api/RouteInfos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRouteInfo(Guid id, RouteInfo routeInfo)
        {
            if (id != routeInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(routeInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteInfoExists(id))
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



        // POST: api/RouteInfos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RouteInfo>> PostRouteInfo(RouteInfo routeInfo)
        {
            _context.RouteInfos.Add(routeInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RouteInfoExists(routeInfo.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRouteInfo", new { id = routeInfo.Id }, routeInfo);
        }

        // DELETE: api/RouteInfos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRouteInfo(Guid id)
        {
            var routeInfo = await _context.RouteInfos.FindAsync(id);
            if (routeInfo == null)
            {
                return NotFound();
            }

            _context.RouteInfos.Remove(routeInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RouteInfoExists(Guid id)
        {
            return _context.RouteInfos.Any(e => e.Id == id);
        }
    }
}
