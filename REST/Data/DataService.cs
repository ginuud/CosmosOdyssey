using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Globalization;

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

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new JsonGuidConverter() }
            };

            var newPriceList = JsonSerializer.Deserialize<PriceList>(jsonString, options);

            // _context.ChangeTracker.Clear();
            // await _context.Providers.ExecuteDeleteAsync();
            // await _context.Legs.ExecuteDeleteAsync();
            // await _context.RouteInfos.ExecuteDeleteAsync();
            // await _context.PriceLists.ExecuteDeleteAsync();
            // await _context.Companies.ExecuteDeleteAsync();
            // await _context.Planets.ExecuteDeleteAsync();

            var existingPriceList = await _context.PriceLists.FindAsync(newPriceList.Id);
            if (existingPriceList != null)
            {
                return;
            }

            var priceListCount = await _context.PriceLists.CountAsync();
            if (priceListCount >= 15)
            {
                var oldestPriceList = await _context.PriceLists.OrderBy(pl => pl.ValidUntil).FirstOrDefaultAsync();
                if (oldestPriceList != null)
                {
                    var legsToRemove = _context.Legs.Where(l => l.PriceListId == oldestPriceList.Id);
                    var routeInfosToRemove = _context.RouteInfos.Where(ri => legsToRemove.Select(l => l.RouteInfoId).Contains(ri.Id));
                    var providersToRemove = _context.Providers.Where(p => legsToRemove.SelectMany(l => l.Providers).Select(pr => pr.Id).Contains(p.Id));

                    _context.Providers.RemoveRange(providersToRemove);
                    _context.RouteInfos.RemoveRange(routeInfosToRemove);
                    _context.Legs.RemoveRange(legsToRemove);
                    _context.PriceLists.Remove(oldestPriceList);
                }
            }

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
        // public class JsonDateTimeConverter : JsonConverter<DateTime>
        // {
        //     public override DateTime Read(
        //         ref Utf8JsonReader reader,
        //         Type typeToConvert,
        //         JsonSerializerOptions options)
        //     {
        //         return DateTime.Parse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        //     }

        //     public override void Write(
        //         Utf8JsonWriter writer,
        //         DateTime value,
        //         JsonSerializerOptions options)
        //     {
        //         writer.WriteStringValue(value.ToString("O"));
        //     }
        // }
        public class JsonGuidConverter : JsonConverter<Guid>
        {
            public override Guid Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
                => Guid.Parse(reader.GetString());

            public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
                => writer.WriteStringValue(value.ToString());
        }
    }
}