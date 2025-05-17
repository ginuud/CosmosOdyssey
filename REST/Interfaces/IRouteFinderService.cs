using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Dtos;

namespace CosmosOdyssey.REST.Interfaces
{
    public interface IRouteFinderService
    {
        Task<List<RouteDto>> FindAllRoutesAsync(string fromPlanet, string toPlanet);
    }
}