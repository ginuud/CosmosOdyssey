using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Dtos
{
    public class LegDto
    {
        public Guid Id { get; set; }
        public Guid PriceListId { get; set; }
        public List<ProviderDto> Providers { get; set; } = new List<ProviderDto>();
    }
}