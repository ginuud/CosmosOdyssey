using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Dtos
{
    public class ProviderDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime FlightStart { get; set; }
        public DateTime FlightEnd { get; set; }
        public Guid LegId { get; set; }
    }
}