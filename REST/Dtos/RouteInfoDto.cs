using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;

namespace CosmosOdyssey.REST.Dtos
{
    public class RouteInfoDto
    {
        public Guid Id { get; set; }
        public Planet From { get; set; } = new Planet();
        public Planet To { get; set; } = new Planet();
        public long Distance { get; set; }
        public int? ReservationId { get; set; }

    }
}