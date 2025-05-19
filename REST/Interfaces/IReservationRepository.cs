using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Dtos;

namespace REST.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateAsync(Reservation reservationModel);
        Task<List<RouteInfo>> GetRouteInfosByIdsAsync(List<Guid> routeInfoIds);

        Task<Reservation?> GetByIdAsync(int id);
    }
}