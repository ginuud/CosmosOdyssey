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
        public ICollection<Provider> Providers { get; set; } = new List<Provider>(); //Üks-mitmele seos Offeritega
        public PriceList PriceList { get; set; }
        public string PriceListId { get; set; } // FK

    }
}