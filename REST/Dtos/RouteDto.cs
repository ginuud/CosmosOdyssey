using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Dtos
{
    public class RouteDto
    {
        public Guid RouteInfoId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CompanyName { get; set; }
        public decimal Price { get; set; }
        public long Distance { get; set; }
        public double TravelTime { get; set; }
    }
}