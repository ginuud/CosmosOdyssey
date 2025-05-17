using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Dtos
{
    public class RouteDto
    {

        public List<Guid> RouteInfoIds { get; set; } = new List<Guid>();
        public string From { get; set; }
        public string To { get; set; }
        public List<Guid> ProviderIds { get; set; } = new List<Guid>();
        public List<string> CompanyNames { get; set; } = new List<string>();
        public decimal Price { get; set; }
        public long Distance { get; set; }
        public double TravelTime { get; set; }
    }
}