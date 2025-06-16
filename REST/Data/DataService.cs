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
using REST.Models;

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

                    var routeInfoIdsToRemove = legsToRemove.Select(l => l.RouteInfoId).ToList();
                    var routeInfosToRemove = _context.RouteInfos.Where(ri => routeInfoIdsToRemove.Contains(ri.Id));

                    var fromPlanetIdsToRemove = routeInfosToRemove.Select(ri => ri.FromId).ToList();
                    var fromplanetsToRemove = _context.Planets.Where(p => fromPlanetIdsToRemove.Contains(p.Id));

                    var toPlanetIdsToRemove = routeInfosToRemove.Select(ri => ri.ToId).ToList();
                    var toPlanetsToRemove = _context.Planets.Where(p => toPlanetIdsToRemove.Contains(p.Id));

                    var routeSegmentsToRemove = _context.RouteSegments?.Where(rs => routeInfoIdsToRemove.Contains(rs.RouteInfoId));
                    var routeSegmentIdsToRemove = routeSegmentsToRemove?.Select(rs => rs.Id).ToList() ?? new List<int>();

                    var reservedRoutesToRemove = _context.ReservedRoutes?.Where(rr => rr.RouteSegments.Any(rs => routeSegmentIdsToRemove.Contains(rs.Id))) ?? Enumerable.Empty<ReservedRoute>().AsQueryable();
                    var reservationsToRemove = _context.Reservations?.Where(r => reservedRoutesToRemove.Select(rr => rr.Id).Contains(r.ReservedRouteId));

                    var providersToRemove = _context.Providers.Where(p => legsToRemove.SelectMany(l => l.Providers).Select(pr => pr.Id).Contains(p.Id));

                    var companyIdsToRemove = providersToRemove.Select(p => p.CompanyId).ToList();
                    var companiesToRemove = _context.Companies.Where(c => companyIdsToRemove.Contains(c.Id));

                    _context.Companies.RemoveRange(companiesToRemove);
                    _context.Providers.RemoveRange(providersToRemove);
                    _context.Reservations?.RemoveRange(reservationsToRemove ?? Enumerable.Empty<Reservation>());
                    _context.ReservedRoutes?.RemoveRange(reservedRoutesToRemove);
                    _context.RouteSegments?.RemoveRange(routeSegmentsToRemove ?? Enumerable.Empty<RouteSegment>());
                    _context.Planets.RemoveRange(fromplanetsToRemove);
                    _context.Planets.RemoveRange(toPlanetsToRemove);
                    _context.RouteInfos.RemoveRange(routeInfosToRemove);
                    _context.Legs.RemoveRange(legsToRemove);
                    _context.PriceLists.Remove(oldestPriceList);
                }
            }

            foreach (var leg in newPriceList.Legs)
            {
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

        public class JsonGuidConverter : JsonConverter<Guid>
        {
            public override Guid Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
                => Guid.Parse(reader.GetString());

            public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
                => writer.WriteStringValue(value.ToString());
        }
    }
}