using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class PriceList
    {
        public string Id { get; set; }
        public DateTime ValidUntil { get; set; }
        public ICollection<Leg> Legs { get; set; } = new List<Leg>(); //Üks-mitmele seos Legidega
    }
}