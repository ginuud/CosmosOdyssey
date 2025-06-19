using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Data;
using Microsoft.EntityFrameworkCore;
using REST.Interfaces;

namespace REST.Data.Repos
{
    public class PlanetRepo(DataContext context) : IPlanetRepository
    {
        private readonly DataContext _context = context;

        public async Task<List<string>> GetNames()
        {
            try
            {
                var dbPlanets = await _context.Planets
                    .Select(p => p.Name).Distinct().ToListAsync();
                return dbPlanets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving planet names from database: {ex.Message}");
                throw;
            }
        }
    }
}