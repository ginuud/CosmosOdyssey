using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.REST.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Provider> Providers { get; set; } = new List<Provider>();//Üks-mitmele seos Provideritega
    }
}