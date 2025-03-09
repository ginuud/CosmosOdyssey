using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Leg
    {
        public string Id { get; set; }
        public RouteInfo RouteInfo { get; set; }
        public string RouteInfoId { get; set; } //FK
        public List<Provider> Providers { get; set; } = new List<Provider>(); //Üks-mitmele seos Provideritega
        public string PricelistId { get; set; } // FK
        public Pricelist Pricelist { get; set; }
    }
}