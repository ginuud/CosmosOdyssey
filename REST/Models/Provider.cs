using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Provider
    {
        public Guid Id { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public decimal Price { get; set; }
        public DateTime FlightStart { get; set; }
        public DateTime FlightEnd { get; set; }
        public Leg Leg { get; set; }
        public Guid LegId { get; set; } //FK


    }
}