using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;
using REST.Interfaces;

namespace REST.Data.Repos
{
    public class PriceListRepo(DataContext context) : IPriceListRepository
    {
        private readonly DataContext _context = context;

        public async Task<PriceList> GetLatest()
        {
            try
            {
                var dbPriceList = await _context.PriceLists
                    .OrderByDescending(pl => pl.ValidUntil).FirstOrDefaultAsync();

                return dbPriceList;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving latest price list from database: {ex.Message}");
                throw;
            }
        }




    }
}