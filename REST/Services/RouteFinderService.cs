using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Interfaces;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Dtos;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Models;

namespace REST.Services
{
    public class RouteFinderService : IRouteFinderService
    {
        private readonly DataContext _context;
        public RouteFinderService(DataContext context)
        {
            _context = context;
        }

        private List<List<Leg>> FindRoutes(string origin, string destination, List<Leg> allLegs)
        {
            var results = new List<List<Leg>>();
            int maxTransfers = 4;

            void Dfs(string currentPlanet, List<Leg> path, HashSet<string> visitedPlanets)
            {
                if (path.Count > maxTransfers + 1)
                    return;

                if (currentPlanet == destination)
                {
                    results.Add(new List<Leg>(path));
                    return;
                }

                foreach (var leg in allLegs)
                {
                    var nextPlanetName = leg.RouteInfo.To.Name;
                    if (leg.RouteInfo.From.Name == currentPlanet &&
                        !path.Any(p => p.Id == leg.Id) &&
                        !visitedPlanets.Contains(nextPlanetName))
                    {
                        path.Add(leg);
                        visitedPlanets.Add(nextPlanetName);
                        Dfs(nextPlanetName, path, visitedPlanets);
                        path.RemoveAt(path.Count - 1);
                        visitedPlanets.Remove(nextPlanetName);
                    }
                }
            }

            var initialVisited = new HashSet<string> { origin };
            Dfs(origin, new List<Leg>(), initialVisited);
            return results;
        }

        private List<List<Provider>> GetValidProviderSequences(List<Leg> path)
        {
            var results = new List<List<Provider>>();

            void Dfs(int index, List<Provider> currentSequence)
            {
                if (index == path.Count)
                {
                    results.Add(new List<Provider>(currentSequence));
                    return;
                }

                var currentLeg = path[index];

                foreach (var provider in currentLeg.Providers.OrderBy(p => p.FlightStart))
                {
                    if (index == 0)
                    {
                        currentSequence.Add(provider);
                        Dfs(index + 1, currentSequence);
                        currentSequence.RemoveAt(currentSequence.Count - 1);
                    }
                    else
                    {
                        var previousFlightEnd = currentSequence.Last().FlightEnd;
                        var timeGap = provider.FlightStart - previousFlightEnd;

                        if (provider.FlightStart > previousFlightEnd &&
                            timeGap.TotalHours <= 24)
                        {
                            currentSequence.Add(provider);
                            Dfs(index + 1, currentSequence);
                            currentSequence.RemoveAt(currentSequence.Count - 1);
                        }
                    }
                }
            }

            Dfs(0, new List<Provider>());
            return results;
        }


        public async Task<List<RouteDto>> FindAllRoutesAsync(string origin, string destination)
        {
            var allLegs = await _context.Legs
                .Include(l => l.RouteInfo)
                    .ThenInclude(r => r.From)
                .Include(l => l.RouteInfo)
                    .ThenInclude(r => r.To)
                .Include(l => l.Providers)
                    .ThenInclude(p => p.Company)
                .ToListAsync();

            var legPaths = FindRoutes(origin, destination, allLegs);



            var routeDtos = new List<RouteDto>();

            foreach (var path in legPaths)
            {
                var validProviderSequences = GetValidProviderSequences(path);

                foreach (var providerSequence in validProviderSequences)
                {
                    routeDtos.Add(new RouteDto
                    {
                        RouteInfoIds = path.Select(l => l.RouteInfoId).ToList(),
                        From = path.First().RouteInfo.From.Name ?? "Unknown",
                        To = path.Last().RouteInfo.To.Name ?? "Unknown",
                        ProviderIds = providerSequence.Select(p => p.Id).ToList(),
                        CompanyNames = providerSequence.Select(p => p.Company?.Name ?? "Unknown").ToList(),
                        Price = providerSequence.Sum(p => p.Price),
                        Distance = path.Sum(l => l.RouteInfo.Distance),
                        TravelTime = Math.Round((providerSequence.Last().FlightEnd - providerSequence.First().FlightStart).TotalHours, 2)
                    });
                    if (routeDtos.Count >= 100)
                        return routeDtos;
                }
            }
            return routeDtos;
        }
    }

}