using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Models;

namespace CosmosOdyssey.REST.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int ReservedRouteId { get; set; }
        public ReservedRoute ReservedRoute { get; set; }
        public decimal? TotalQuotedPrice { get; set; }
        public decimal? TotalQuotedTravelTime { get; set; }
        public List<string> TransportationCompanyNames { get; set; } = new List<string>();
    }
}