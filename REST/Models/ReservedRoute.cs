using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;

namespace REST.Models
{
    public class ReservedRoute
    {
        public int Id { get; set; }
        public ICollection<RouteSegment> RouteSegments { get; set; } = new List<RouteSegment>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}