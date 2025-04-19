using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Leg
    {
        public Guid Id { get; set; }
        public RouteInfo RouteInfo { get; set; }
        public Guid RouteInfoId { get; set; } //FK
        public ICollection<Provider> Providers { get; set; } = new List<Provider>(); //Üks-mitmele seos Offeritega // vb peaks kustutama
        public PriceList PriceList { get; set; }
        public Guid PriceListId { get; set; } // FK

    }
}