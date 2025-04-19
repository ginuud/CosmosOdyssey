using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Dtos
{
    public class RouteInfoDto
    {
        public Guid Id { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public long Distance { get; set; }
        public string FromName { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public List<LegDto> Legs { get; set; } = new List<LegDto>();

    }
}