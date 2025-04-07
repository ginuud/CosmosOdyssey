using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Provider> Providers { get; set; } = new List<Provider>();//Üks-mitmele seos Provideritega
    }
}