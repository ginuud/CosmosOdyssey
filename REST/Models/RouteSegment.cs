using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;

namespace REST.Models
{
    public class RouteSegment
    {
        public int Id { get; set; }
        public int ReservedRouteId { get; set; }
        public ReservedRoute ReservedRoute { get; set; }
        public Guid RouteInfoId { get; set; }
        public RouteInfo RouteInfo { get; set; }
        public int SegmentOrder { get; set; }
    }
}