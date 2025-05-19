using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<RouteInfoDto> Routes { get; set; } = new List<RouteInfoDto>();
        public decimal? TotalQuotedPrice { get; set; }
        public decimal? TotalQuotedTravelTime { get; set; }
        public List<string> TransportationCompanyNames { get; set; } = new List<string>();

    }
}