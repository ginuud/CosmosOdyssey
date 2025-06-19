using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;

namespace REST.Interfaces
{
    public interface IPriceListRepository
    {
        Task<PriceList> GetLatest();
    }
}