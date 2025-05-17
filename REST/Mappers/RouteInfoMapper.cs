using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Dtos;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;

namespace REST.Mappers
{
    public static class RouteMapper
    {
        // public static IEnumerable<RouteDto> ToRouteDto(this RouteInfo route, DataContext context)
        // {
        //     var legs = context.Legs
        //         .Where(l => l.RouteInfoId == route.Id)
        //         .Include(l => l.Providers)
        //             .ThenInclude(p => p.Company)
        //         .ToList();

        //     var routeDtos = new List<RouteDto>();

        //     foreach (var leg in legs)
        //     {
        //         foreach (var provider in leg.Providers)
        //         {
        //             routeDtos.Add(new RouteDto
        //             {
        //                 RouteInfoIds = route.Id,
        //                 From = route.From.Name,
        //                 To = route.To.Name,
        //                 ProviderId = provider.Id,
        //                 CompanyName = provider.Company.Name,
        //                 Price = provider.Price,
        //                 Distance = route.Distance,
        //                 TravelTime = Math.Round((provider.FlightEnd - provider.FlightStart).TotalHours, 2)
        //             });
        //         }
        //     }

        //     return routeDtos;
        // }

    }
}