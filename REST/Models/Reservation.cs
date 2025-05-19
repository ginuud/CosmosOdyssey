using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<RouteInfo> Routes { get; set; } = new List<RouteInfo>();
        public List<Guid> RouteInfoIds { get; set; } = new List<Guid>();
        public decimal? TotalQuotedPrice { get; set; }
        public decimal? TotalQuotedTravelTime { get; set; }
        public List<string> TransportationCompanyNames { get; set; } = new List<string>();
    }
}