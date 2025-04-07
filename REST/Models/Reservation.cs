using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public Enum Routes { get; set; }
        public decimal? TotalQuotedPrice { get; set; }
        public DateTime? TotalQuotedTravelTime { get; set; }
        public string? TransportationCompanyNames { get; set; }
    }
}