using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class RouteInfo
    {
        public Guid Id { get; set; }
        public Planet From { get; set; }
        public Guid FromId { get; set; }
        public Planet To { get; set; }
        public Guid ToId { get; set; }
        public long Distance { get; set; }
    }
}