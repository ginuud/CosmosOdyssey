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
        public List<RouteDto> Routes { get; set; } = new List<RouteDto>();
        public List<Guid> RouteInfoIds { get; set; } = new List<Guid>();
        public decimal? TotalQuotedPrice { get; set; }
        public decimal? TotalQuotedTravelTime { get; set; }
        public string? CompanyName { get; set; }

    }
}