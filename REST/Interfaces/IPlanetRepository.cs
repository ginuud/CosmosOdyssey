using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Interfaces
{
    public interface IPlanetRepository
    {
        Task<List<string>> GetNames();
    }
}