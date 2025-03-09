using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Provider
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public decimal Price { get; set; }
        public DateTime FlightStart { get; set; }
        public string LegId { get; set; } //FK
        public Leg Leg { get; set; }
        public DateTime FlightEnd { get; set; }
    }
}