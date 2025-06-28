using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CosmosOdyssey.REST.Dtos
{
    public class CreateReservationDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be over 30 over characters")]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public List<Guid>? RouteInfoIds { get; set; } = new List<Guid>();
        public decimal? TotalQuotedPrice { get; set; }
        public decimal? TotalQuotedTravelTime { get; set; }
        public List<string>? TransportationCompanyNames { get; set; }

    }
}