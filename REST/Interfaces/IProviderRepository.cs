using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;

// vb kustuta
namespace REST.Interfaces
{
    public interface IProviderRepository
    {
        Task<List<Provider>> GetProvidersAsync();
        Task<Provider?> GetProviderByIdAsync(Guid id);
        Task<bool> ProviderExists(Guid id);

    }
}