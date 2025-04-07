using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Models;

namespace CosmosOdyssey.REST.Data
{
    public interface IDataService
    {
        Task SyncTravelPricesAsync();
    }
    public class DataService : IDataService
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://cosmosodyssey.azurewebsites.net/api/v1.0/TravelPrices";

        public DataService(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task SyncTravelPricesAsync()
        {
            var response = await _httpClient.GetAsync(ApiUrl);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var newPriceList = JsonSerializer.Deserialize<PriceList>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _context.ChangeTracker.Clear();
            await _context.Providers.ExecuteDeleteAsync();
            await _context.Legs.ExecuteDeleteAsync();
            await _context.RouteInfos.ExecuteDeleteAsync();
            await _context.PriceLists.ExecuteDeleteAsync();
            await _context.Companies.ExecuteDeleteAsync();
            await _context.Planets.ExecuteDeleteAsync();

            foreach (var leg in newPriceList.Legs)
            {
                leg.RouteInfo.From = await GetOrAddPlanet(leg.RouteInfo.From);
                leg.RouteInfo.To = await GetOrAddPlanet(leg.RouteInfo.To);

                _context.RouteInfos.Add(leg.RouteInfo);

                foreach (var provider in leg.Providers)
                {
                    provider.Company = await GetOrAddCompany(provider.Company);
                }
            }

            await _context.PriceLists.AddAsync(newPriceList);
            await _context.SaveChangesAsync();
        }

        private async Task<Company> GetOrAddCompany(Company company)
        {
            var existing = await _context.Companies.FindAsync(company.Id);
            if (existing != null)
            {
                return existing;
            }
            else
            {
                await _context.Companies.AddAsync(company);
                return company;
            }
        }

        private async Task<Planet> GetOrAddPlanet(Planet planet)
        {
            var existing = await _context.Planets.FindAsync(planet.Id);
            if (existing != null)
            {
                return existing;
            }
            else
            {
                await _context.Planets.AddAsync(planet);
                return planet;
            }
        }
    }
}